using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public partial class GameObject
{
    public virtual void SetAtlas(Texture2D atlas)
    {
        Atlas = atlas;
    }

    public virtual void SetSpriteRect(Rectangle spriteRect)
    {
        SpriteRect = spriteRect;
    }

    public virtual void SetColor(Color color)
    {
        Color = color;
    }
}
