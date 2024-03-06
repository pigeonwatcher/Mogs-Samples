using Microsoft.Xna.Framework;
using System;

public class Entity : GameObject
{
    public Color Color { get; protected set; } = Color.White;
    public Vector4[] Palette { get; protected set; } = new Vector4[4];
    public Matrix MatrixPos { get; protected set; }
    public Rectangle Bounds { get; protected set; }

    public event Action<Rectangle> VisualChanged;

    public override void SetSpriteRect(Rectangle spriteRect)
    {
        base.SetSpriteRect(spriteRect);
        VisualChanged?.Invoke(spriteRect);
    }

    public override void SetPos(Vector2 pos)
    {
        base.SetPos(pos);
        MatrixPos = Matrix.CreateTranslation(new Vector3(Position, 0)) * Grid.ScaleMatrix;
    }

    public override void AdjustPos(float x, float y)
    {
        base.AdjustPos(x, y);
        MatrixPos = Matrix.CreateTranslation(new Vector3(Position, 0)) * Grid.ScaleMatrix;
    }

    public override void AdjustPos(Vector2 adjustPos)
    {
        base.AdjustPos(adjustPos);
        MatrixPos = Matrix.CreateTranslation(new Vector3(Position, 0)) * Grid.ScaleMatrix;
    }

    public override void ResetPos()
    {
        base.ResetPos();
        MatrixPos = Matrix.CreateTranslation(new Vector3(Position, 0)) * Grid.ScaleMatrix;
    }

    public virtual void SetPalette(Color[] palette)
    {
        Palette[0] = palette[0].ToVector4();
        Palette[1] = palette[1].ToVector4();
    }
}
