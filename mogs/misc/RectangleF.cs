using Microsoft.Xna.Framework;
using System;

public struct RectangleF
{
    /// <summary>
    /// A custom struct based on Monogame / XNA Rectangle. This instead enables rectangles to be positioned via float instead of int.
    /// This was created in order for smoother animation for the nine-slice panel.
    /// </summary>

    public Vector2 Position { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public RectangleF(Vector2 position, float width, float height)
    {
        Position = position;
        Width = width;
        Height = height;
    }

    public RectangleF(float x, float y, float width, float height)
    {
        Position = new Vector2(x, y);
        Width = width;
        Height = height;
    }

    public float X
    {
        get => Position.X;
        set => Position = new Vector2(value, Y);
    }

    public float Y
    {
        get => Position.Y;
        set => Position = new Vector2(X, value);
    }

    public float Left => Position.X;
    public float Right => Position.X + Width;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Height;
    public Vector2 Center => new Vector2(Position.X + Width / 2, Position.Y + Height / 2);

    public void Offset(Vector2 amount)
    {
        Position += amount;
    }

    public Rectangle ToRectangle()
    {
        return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
    }

    public static implicit operator RectangleF(Rectangle rectangle)
    {
        return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }
}