using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Emote : GameObject
{
    public float ElapsedTime { get; protected set; }
    public float Duration { get; protected set; }

    protected Vector2[] Frames;
    protected float FrameRate;

    private float timer;
    private int frameIndex;

    public virtual void Update(GameTime gameTime)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (timer >= FrameRate)
        {
            Vector2 currentDirection = Frames[frameIndex];
            AdjustPos(currentDirection);

            frameIndex = (frameIndex + 1) % Frames.Length; 

            timer = 0f; 
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Resources.EmojiAtlas, Position, SpriteRect, ColorUtils.LightOffWhite, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.EMOJI_LAYER);
    }

    public void ResetTimer()
    {
        ElapsedTime = 0;
    }
}