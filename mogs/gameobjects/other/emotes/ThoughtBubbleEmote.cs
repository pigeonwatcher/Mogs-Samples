using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ThoughtBubbleEmote : Emote
{
    public State LoadingState { get; private set; }

    public enum State
    {
        Loading,
        Loaded,
        Unloading,
        Unloaded,
    }

    private ThoughtBubbleSection[] sections = new ThoughtBubbleSection[3];

    private ItemID foodItem;
    private Rectangle SpriteRectItem;
    private Vector2 itemPos;

    private float timer;

    public ThoughtBubbleEmote(Cat cat)
    {
        Duration = 5;
        Frames = new Vector2[] { new Vector2(-1, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(1, 0) };
        FrameRate = 0.5f;

        ThoughtBubbleSection thoughtBubbleSmall = new ThoughtBubbleSection()
        {
            SpriteRect = Sprite.EmojiData[Emoji.ThoughtBubbleSmall],
            Position = new Vector2(cat.Position.X + 3, cat.Position.Y - 5),
            Index = 1,
        };
        ThoughtBubbleSection thoughtBubbleMedium = new ThoughtBubbleSection()
        {
            SpriteRect = Sprite.EmojiData[Emoji.ThoughtBubbleMedium],
            Position = new Vector2(cat.Position.X + 8, cat.Position.Y - 11),
            Index = 1,
        };
        ThoughtBubbleSection thoughtBubbleMain = new ThoughtBubbleSection()
        {
            SpriteRect = Sprite.EmojiData[Emoji.ThoughtBubbleMain],
            Position = new Vector2(cat.Position.X - 1, cat.Position.Y - 31),
            Index = 0,
        };

        sections[0] = thoughtBubbleSmall;
        sections[1] = thoughtBubbleMedium;
        sections[2] = thoughtBubbleMain;

        foodItem = ItemID.StrawberryCake;
        SpriteRectItem = Sprite.ItemData[foodItem];
        itemPos = new Vector2(thoughtBubbleMain.Position.X + 4, thoughtBubbleMain.Position.Y + 3);

        SetActive(true);
    }

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);

        if (isActive)
        {
            LoadingState = State.Loading;
        }
        else
        {
            LoadingState = State.Unloading;
        }
    }

    public override void Update(GameTime gameTime)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (!Active && LoadingState != State.Unloading) { return; }

        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (LoadingState == State.Loading)
        {
            for (int i = 0; i < sections.Length; i++)
            {
                if (!sections[i].IsLoaded)
                {
                    if (timer >= FrameRate)
                    {
                        sections[i].IsLoaded = true;
                    }

                    break;
                }
            }

            if (sections[^1].IsLoaded)
            {
                LoadingState = State.Loaded;
            }
        }
        else if (LoadingState == State.Unloading)
        {
            for (int i = sections.Length - 1; i >= 0; i--)
            {
                if (sections[i].IsLoaded)
                {
                    if (timer >= FrameRate)
                    {
                        sections[i].IsLoaded = false;
                    }

                    break;
                }
            }

            if (!sections[0].IsLoaded)
            {
                LoadingState = State.Unloaded;
            }
        }

        if (timer >= FrameRate)
        {
            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].IsLoaded)
                {
                    Vector2 direction = Frames[sections[i].Index];
                    sections[i].Position += direction;
                    sections[i].Index = (sections[i].Index + 1) % Frames.Length;

                    if (i == 2)
                    {
                        itemPos += direction;
                    }

                }
            }

            timer = 0f; // Reset Timer.
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].IsLoaded)
                spriteBatch.Draw(Resources.EmojiAtlas, sections[i].Position, sections[i].SpriteRect, ColorUtils.LightOffWhite, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.EMOJI_LAYER);
        }

        if (sections[^1].IsLoaded)
            spriteBatch.Draw(Resources.ItemAtlas, itemPos, SpriteRectItem, ColorUtils.LightOffWhite, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.EMOJI_LAYER + Layer.UI_CHILDOFFSET);
    }
}

public struct ThoughtBubbleSection
{
    public Rectangle SpriteRect { get; set; }
    public Vector2 Position { get; set; }
    public int Index { get; set; }
    public bool IsLoaded { get; set; }
}