using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class StatusMeter : UIElement
{
    private readonly PlayerCatService playerCatService;
    private readonly Texture2D uiAtlas;
    private readonly Rectangle heartSprite;
    private readonly Rectangle fishSprite;

    private List<StatusIcon> icons = new List<StatusIcon>();

    private Color fullHeart = Color.OrangeRed;
    private Color emptyHeart = Color.White;
    private Color fish = Color.LightBlue;

    private int currentIcon;
    private float frameRate = 0.5f;
    private StatusIcon[] animatedIcons = new StatusIcon[5];

    private float elapsedTime;
    private float timer;
    private int frameIndex;

    public StatusMeter(PlayerCatService _playerCatService, Texture2D _uiAtlas)
    {
        playerCatService = _playerCatService;
        playerCatService.HungerChanged += UpdateMeter;
        playerCatService.HappinessChanged += UpdateMeter;
        uiAtlas = _uiAtlas;

        heartSprite = Sprite.UIData[UI.Heart];
        fishSprite = Sprite.UIData[UI.Fish];

        UpdateMeter();
    }

    private void UpdateMeter(PlayerCatData playerCatData = null)
    {
        icons.Clear();

        int happiness = playerCatService.GetHappiness();
        int hunger = playerCatService.GetHunger();

        int maxHearts = 5;
        int totalWidth = (heartSprite.Width * maxHearts) + (maxHearts - 1);
        Vector2 basePosition = Grid.Position(Anchor.TopCenter, Pivot.TopCenter, totalWidth, heartSprite.Height);

        for (int i = 0; i < maxHearts; i++)
        {
            Vector2 position = new Vector2(basePosition.X + (i * (heartSprite.Width + 1)), basePosition.Y + 2);
            StatusIcon heart = new StatusIcon { Index = i, Position = position, Sprite = heartSprite, Color = i < happiness ? fullHeart : emptyHeart };
            heart.SetIconType(UI.Heart);
            icons.Add(heart);
        }

        int startIndex = icons.Count - hunger;
        for (int i = startIndex; i < icons.Count; i++)
        {
            icons[i].Sprite = fishSprite;
            icons[i].Color = fish;
            icons[i].SetIconType(UI.Fish);
        }
    }

    public override void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (timer >= frameRate)
        {
            timer = 0f; // Reset Timer.
            animatedIcons[currentIcon] = icons[currentIcon];

            currentIcon = (currentIcon + 1) % icons.Count;
        }

        for (int i = 0; i < animatedIcons.Length; i++)
        {
            if (animatedIcons[i]?.isComplete == true)
            {
                animatedIcons[i].isComplete = false;
                animatedIcons[i] = null;

                if (i == 2)
                {
                    timer = -1f;
                }
                continue;
            }

            animatedIcons[i]?.Update(gameTime);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Active) return;

        for (int i = 0; i < icons.Count; i++)
        {
            spriteBatch.Draw(uiAtlas, icons[i].Position, icons[i].Sprite, icons[i].Color, 0, Vector2.Zero, 1, SpriteEffects.None, LayerDepth);
        }
    }
}
