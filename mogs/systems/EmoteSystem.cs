using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmoteSystem : IEntitySystem, ITouchSystem, IUpdateSystem, IDrawSystem
{
    private readonly CatInteractionSystem catInteractionSystem;

    public bool Active { get; private set; }
    private CatEmoteData[] catEmoteData = new CatEmoteData[MAX_ENTITIES];

    private float itemDragTimer;
    private float emoteTimer;

    private const int ITEM_DRAG_ANNOY = 10;
    private const int EMOTE_INTERVAL = 10; // every 10 seconds a emoji appears.
    private const int MAX_ENTITIES = 2;

    private const int PLAYER_CAT_INDEX = 0;
    private const int CAT_PAL_INDEX = 1;

    public EmoteSystem(CatInteractionSystem _catInteractionSystem)
    {
        catInteractionSystem = _catInteractionSystem;
    }

    public void SetActive(bool isActive)
    {
        Active = isActive;

        if (isActive)
        {
            foreach (var cat in catEmoteData)
            {
                if(cat.Cat != null)
                    cat.Cat.BeingPet += OnPet;
            }

            catInteractionSystem.CatTapped += OnTapped; 
        }
        else
        {
            foreach (var cat in catEmoteData)
            {
                if (cat.Cat != null)
                    cat.Cat.BeingPet -= OnPet;
            }

            catInteractionSystem.CatTapped -= OnTapped;
        }
    }

    public void RegisterEntity(Entity entity)
    {
        if (entity is Cat cat)
        {
            int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

            catEmoteData[index].Cat = cat;
            catEmoteData[index].Cat.BeingPet += OnPet;
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entity is Cat cat)
        {
            int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

            catEmoteData[index].Cat.BeingPet -= OnPet;
            catEmoteData[index].Cat = null;
        }
    }

    public void Update(GameTime gameTime)
    {
        if(!Active) { return; }

        emoteTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (catEmoteData[0].Cat != null && catEmoteData[0].ActiveEmote == null && emoteTimer > EMOTE_INTERVAL)
        {
            ActivateEmoji();

            emoteTimer = 0;
        }

        for(int i = 0; i < catEmoteData.Length; i++)
        {
            if (catEmoteData[i].ActiveEmote != null && 
                catEmoteData[i].ActiveEmote.ElapsedTime > catEmoteData[i].ActiveEmote.Duration &&
                catEmoteData[i].ActiveEmote is not ThoughtBubbleEmote)
            {
                catEmoteData[i].ActiveEmote = null;
                catEmoteData[i].TapCounter = 0;

                emoteTimer = 0; // So another emote does not appear immediately after one has finished.
            }
            else if (catEmoteData[i].ActiveEmote != null &&
                     catEmoteData[i].ActiveEmote.ElapsedTime > catEmoteData[i].ActiveEmote.Duration &&
                     catEmoteData[i].ActiveEmote is ThoughtBubbleEmote thoughtBubble)
            {
                if(thoughtBubble.Active)
                {
                    thoughtBubble.SetActive(false);
                }
                else if (thoughtBubble.LoadingState == ThoughtBubbleEmote.State.Unloaded)
                {
                    catEmoteData[i].ActiveEmote = null;
                    emoteTimer = 0;
                }
            }

                catEmoteData[i].ActiveEmote?.Update(gameTime);
        }

        if(Game1.IsItemDragging)
        {
            itemDragTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(itemDragTimer > ITEM_DRAG_ANNOY)
            {
                OnLongDrag();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Active) { return; }

        for (int i = 0; i < catEmoteData.Length; i++)
        {
            catEmoteData[i].ActiveEmote?.Draw(spriteBatch);
        }
    }

    public void OnTouch(IInputService input)
    {

    }

    public void OnRelease(IInputService input)
    {
        for(int i = 0; i < catEmoteData.Length; i++)
        {
            catEmoteData[i].PetCounter = 0;
        }

        itemDragTimer = 0;
    }

    private void ActivateEmoji()
    {
        PlayerCat playerCat = (PlayerCat)catEmoteData[0].Cat;

        if(catEmoteData[0].LastEmote != typeof(ThoughtBubbleEmote))
        {
            catEmoteData[0].ActiveEmote = new ThoughtBubbleEmote(playerCat);
            catEmoteData[0].LastEmote = typeof(ThoughtBubbleEmote);
        }
        else if (playerCat.Data.Hunger == PlayerCatData.MaxHunger) 
        {
            catEmoteData[0].ActiveEmote = new SadEmote(playerCat);
            catEmoteData[0].LastEmote = typeof(SadEmote);
        }
        else if(playerCat.Data.Happiness == PlayerCatData.MaxHappiness && playerCat.Data.Hunger == 0)
        {
            catEmoteData[0].ActiveEmote = new HappyEmote(playerCat);
            catEmoteData[0].LastEmote = typeof(HappyEmote);
        }
    }

    private void OnPet(Cat cat)
    {
        int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

        if (catEmoteData[index].ActiveEmote != null && catEmoteData[index].ActiveEmote is HappyEmote)
        {
            catEmoteData[index].ActiveEmote.ResetTimer();
            return;
        }

        catEmoteData[index].PetCounter++;

        if (catEmoteData[index].ActiveEmote == null && catEmoteData[index].PetCounter > 20)
        {
            catEmoteData[index].ActiveEmote = new HappyEmote(cat);
            catEmoteData[index].LastEmote = typeof(HappyEmote);
        }
    }

    private void OnTapped(Cat cat)
    {
        int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

        if (catEmoteData[index].ActiveEmote != null && catEmoteData[index].ActiveEmote is AngryEmote)
        {
            catEmoteData[index].ActiveEmote.ResetTimer();
            return;
        }

        catEmoteData[index].TapCounter++;

        if (catEmoteData[index].ActiveEmote == null && catEmoteData[index].TapCounter > 15)
        {
            catEmoteData[index].ActiveEmote = new AngryEmote(cat);
            catEmoteData[index].LastEmote = typeof(AngryEmote);
        }
    }

    private void OnLongDrag()
    {
        for(int i = 0; i < catEmoteData.Length; i++)
        {
            if(catEmoteData[i].Cat != null)
            {
                if (catEmoteData[i].ActiveEmote != null && catEmoteData[i].ActiveEmote is AnnoyedEmote)
                {
                    catEmoteData[i].ActiveEmote.ResetTimer();
                    return;
                }
                else if(catEmoteData[i].ActiveEmote == null)
                {
                    catEmoteData[i].ActiveEmote = new AnnoyedEmote(catEmoteData[i].Cat);
                    catEmoteData[i].LastEmote = typeof(AnnoyedEmote);
                }
            }
        }
    }
}

public struct CatEmoteData
{
    public Cat Cat { get; set; }
    public Emote ActiveEmote { get; set; }
    public Type LastEmote { get; set; }
    public int PetCounter { get; set; }
    public int TapCounter { get; set; }
}