using Microsoft.Xna.Framework;

public partial class GameObject
{
    public virtual void SetRectTransform(Rectangle rectTransform)
    {
        this.rectTransform = rectTransform;
    }

    public virtual void SetRectTransform(Vector2 position, int width, int height)
    {
        rectTransform = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    protected void SetSize(int width, int height)
    {
        rectTransform.Width = width;
        rectTransform.Height = height;
    }

    protected void SetSize(Rectangle size)
    {
        rectTransform.Width = size.Width;
        rectTransform.Height = size.Height;
    }
}
