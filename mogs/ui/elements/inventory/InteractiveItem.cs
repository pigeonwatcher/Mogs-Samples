using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InteractiveItem
{
    public ItemID Item { get; private set; }
    public int Index { get; private set; }
    public Rectangle Sprite { get; private set; }
    public Vector2 Position { get; private set; }
    public Rectangle Bounds { get; private set; }

    private Vector2 initPosition;

    public InteractiveItem(ItemID item, int index, Rectangle sprite, Vector2 position)
    {
        Item = item;
        Index = index;
        Sprite = sprite;
        Position = position;
        Bounds = BoundsUtilities.Calculate(position, sprite);

        initPosition = position;
    }

    public void Set(Vector2 position)
    {
        Position = position;
        Bounds = BoundsUtilities.Calculate(position, Sprite);
    }

    public void Move(Vector2 position)
    {
        position.X = position.X / (int)(Grid.ScaleMatrix.M22) - Sprite.Width;
        position.Y = position.Y / (int)(Grid.ScaleMatrix.M22) - Sprite.Height;
        Position = position;
    }

    public void Adjust(Vector2 adjustment)
    {
        Position += adjustment;
    }

    public void Reset()
    {
        Position = initPosition;
        Bounds = BoundsUtilities.Calculate(initPosition, Sprite);
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D itemAtlas, float layerDepth) 
    {
        spriteBatch.Draw(itemAtlas, Position, Sprite, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layerDepth);
    }
}
