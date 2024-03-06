using Microsoft.Xna.Framework;

public partial class UIElement
{
    protected Rectangle ParentRectTransform;

    public virtual void SetParentRectTransform(Rectangle parentRectTransform)
    {
        ParentRectTransform = parentRectTransform;
    }

    public virtual void SetRectTransform(Rectangle _rectTransform)
    {
        rectTransform = _rectTransform;
    }

    public virtual void SetRectTransform(Vector2 position, int width, int height)
    {
        rectTransform = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    protected void SetDimensions(int width, int height)
    {
        rectTransform.Width = width;
        rectTransform.Height = height;
    }

    protected void SetDimensions(Rectangle dimensions)
    {
        rectTransform.Width = dimensions.Width;
        rectTransform.Height = dimensions.Height;
    }
}
