using Microsoft.Xna.Framework;

public class NineSlice
{
    public Rectangle SpriteRect { get; private set; }
    public RectangleF RectTransform => rectTransform;
    public Vector2 Position => rectTransform.Position;
    public Vector2 Scale { get; private set; }

    private RectangleF rectTransform;
    private Vector2 basePosition;

    public NineSlice(Rectangle spriteRect, RectangleF _rectTransform)
    {
        SpriteRect = spriteRect;
        rectTransform = _rectTransform;

        Rectangle sprite = Sprite.UIData[UI.PanelTypeOne];
        Vector2 scale = new Vector2(rectTransform.Width / sprite.Width, rectTransform.Height / sprite.Height);
        Scale = scale * 3;
    }

    public void SetPosition(Vector2 position)
    {
        rectTransform.Position = position;
        basePosition = position;
    }

    public void MovePosition(Vector2 movePos)
    {
        Vector2 adjustment = movePos - Position;
        rectTransform.Offset(adjustment);
    }

    public void AdjustPosition(Vector2 adjustment)
    {
        rectTransform.Offset(adjustment);
    }

    public void ResetPosition()
    {
        rectTransform.Position = basePosition;
    }
}
