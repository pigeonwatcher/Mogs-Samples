using Microsoft.Xna.Framework;

public partial class GameObject
{
    // SetPos: Sets Absolute Position.
    // MovePos: Changes Absolute Position.
    // AdjustPos: Changes Current Position.
    // ResetPos: Resets Current Position to Absolute Position.

    public virtual void SetPos(Vector2 pos)
    {
        Position = pos;
        BasePosition = pos;

        rectTransform.X = (int)pos.X;
        rectTransform.Y = (int)pos.Y;
    }

    public virtual void SetPos(float x, float y)
    {
        Vector2 pos = new Vector2(x, y);
        Position = pos;
        BasePosition = pos;

        rectTransform.X = (int)pos.X;
        rectTransform.Y = (int)pos.Y;
    }

    public virtual void MovePos(Vector2 _movePos)
    {
        Vector2 movePos = Position + _movePos;
        SetPos(movePos);
    }

    public virtual void MovePos(float? x, float? y)
    {
        float moveX = x != null ? Position.X + (float)x : Position.X;
        float moveY = y != null ? Position.Y + (float)y : Position.Y;
        Vector2 movePos = new Vector2(moveX, moveY);
        SetPos(movePos);
    }

    public virtual void AdjustPos(Vector2 adjustPos)
    {
        Position += adjustPos;
    }

    public virtual void AdjustPos(float x, float y)
    {
        Position += new Vector2(x, y);
    }

    public virtual void ResetPos()
    {
        Position = BasePosition;
    }
}
