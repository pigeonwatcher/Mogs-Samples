using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public partial class UIElement
{
    public virtual void Update(GameTime gameTime)
    {
        if (Active)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(gameTime);
            }

            Transition?.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Draw(spriteBatch);
            }
        }
    }

    public virtual void OnTouch(IInputService inputSystem)
    {
        if (Enabled)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].OnTouch(inputSystem);
            }
        }
    }

    public virtual void OnRelease(IInputService inputSystem)
    {
        if (Enabled)
        {
            for (int i = 0; i < children.Count; i++)
            {
                children[i].OnRelease(inputSystem);
            }
        }
    }
}
