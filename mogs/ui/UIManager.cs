using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

public class UIManager : ITouchService
{
    private List<UIElement> elements = new List<UIElement>();
    private List<UIElement> modalElements = new List<UIElement>();

    private GreyScreen greyScreen => Resources.GreyScreen;

    public void SetConfig(UIConfig config)
    {
        Debug.WriteLine("CLEARING SCENE UI...");
        DeactivateAll();
        ClearAll();

        elements = config.UIElements;
    }

    public T GetUIElement<T>(string name = null) where T : UIElement
    {
        foreach (UIElement element in elements)
        {
            if (element is T && (element.Name == name || name == null))
            {
                return (T)element;
            }
        }

        return default;
    }

    public void ActivateUIElement<T>(string name = null) where T : UIElement
    {
        foreach (UIElement element in elements)
        {
            if (element is T && (element.Name == name || name == null))
            {
                element.Activate();
                return;
            }
        }
    }

    public void DeactivateUIElement<T>(string name = null) where T : UIElement
    {
        foreach (UIElement element in elements)
        {
            if (element is T && (element.Name == name || name == null))
            {
                element.Deactivate();
                return;
            }
        }
    }

    public void ActivateAll(string exceptElementName = null)
    {
        foreach (UIElement element in elements)
        {
            if (exceptElementName == null || element.Name != exceptElementName)
            {
                element.Activate();
            }
        }
    }

    public void ActivateAll<T>(string exceptElementName = null) where T : UIElement
    {
        foreach (UIElement element in elements)
        {
            if (element is not T || (element.Name == exceptElementName && exceptElementName != null))
            {
                element.Activate();
            }
        }
    }

    public void DeactivateAll(string exceptElementName = null)
    {
        foreach (UIElement element in elements)
        {
            if (exceptElementName == null || element.Name != exceptElementName)
            {
                element.Deactivate();
            }
        }
    }

    public void DeactivateAll<T>(string exceptElementName = null) where T : UIElement
    {
        foreach (UIElement element in elements)
        {
            if (element is not T || (element.Name == exceptElementName && exceptElementName != null))
            {
                element.Deactivate();
            }
        }
    }

    public void SetModal(UIElement _element, bool toAdd)
    {
        if (toAdd)
        {
            modalElements.Add(_element);
        }
        else
        {
            if (modalElements.Contains(_element))
            {
                modalElements.Remove(_element);
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (modalElements.Count > 0)
        {
            if (modalElements[^1] is IScreen)
            {
                modalElements[^1].Draw(spriteBatch);
                return;
            }
            else
            {
                greyScreen.Draw(spriteBatch, modalElements[^1].LayerDepth);
                modalElements[^1].Draw(spriteBatch);
            }
        }

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].Draw(spriteBatch);
        }
    }

    public void OnTouch(IInputService input)
    {
        if (modalElements.Count > 0)
        {
            modalElements[^1].OnTouch(input);
            return;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].OnTouch(input);
        }
    }

    public void OnRelease(IInputService input)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].OnRelease(input);
        }
    }

    public void ClearAll()
    {
        elements.Clear();
        modalElements.Clear();
    }
}
