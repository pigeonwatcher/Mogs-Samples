using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StatusIcon
{
    public Rectangle Sprite { get; set; }
    public Color Color { get; set; }
    public int Index { get; set; }
    public Vector2 Position { get; set; }

    private Vector2[] frames = new Vector2[] { new Vector2(0, -1), new Vector2(0, 1) };
    private float frameRate = 0.5f;

    public bool isComplete;
    public float elapsedTime;
    private float timer;
    private int frameIndex;

    public void SetIconType(UI icon)
    {
        switch (icon)
        {
            case UI.Heart:
                frames = new Vector2[] { new Vector2(0, -1), new Vector2(0, 1) };
                break;
            case UI.Fish:
                frames = new Vector2[] { new Vector2(0, -1), new Vector2(0, 1) };
                break;
            default:
                frames = new Vector2[] { };
                break;
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (timer >= frameRate)
        {
            if (isComplete == true) return;

            if (frameIndex == frames.Length - 1)
            {
                isComplete = true;
            }

            Vector2 currentDirection = frames[frameIndex];
            Position += currentDirection;

            frameIndex = (frameIndex + 1) % frames.Length; 
            timer = 0f;
        }
    }
}
