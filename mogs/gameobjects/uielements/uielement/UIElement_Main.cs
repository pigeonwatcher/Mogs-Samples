using System.Collections.Generic;

public partial class UIElement : GameObject
{
    /// <summary>
    /// UIElement is used for buttons, panels, the inventory display, etc. 
    /// What seperates UElement from its parent GameObject class is it ability to have children.
    /// This functionality allows child elements to be more easily managed through their parent class. 
    /// </summary>

    public UITransition Transition { get; protected set; }

    private List<UIElement> children = new List<UIElement>();

    public UIElement()
    {
        SetLayer(Layer.UI);
    }

    public virtual void AddChild<T>(T child) where T : UIElement
    {
        child.SetLayer(LayerDepth + Layer.UI_CHILDOFFSET);
        children.Add(child);
    }

    public T GetChild<T>(string name = null) where T : UIElement
    {
        foreach (var child in children)
        {
            if (child is T && name == null)
            {
                return (T)child;
            }
            else if (child is T && name != null && child.Name == name)
            {
                return (T)child;
            }
        }
        return default(T);
    }

    public void ClearChildren()
    {
        children.Clear();
    }

    public virtual void Activate()
    {
        SetActive(true);

        if (Transition != null)
        {
            Transition.AnimationComplete = OnActivated;
            Transition?.StartTransitionIn();
        }
        else
        {
            OnActivated();
        }
    }

    public virtual void Deactivate()
    {
        SetEnabled(false);

        if (Transition != null)
        {
            Transition.AnimationComplete = OnDeactivated;
            Transition?.StartTransitionOut();
        }
        else
        {
            OnDeactivated();
        }
    }

    protected virtual void OnActivated()
    {
        SetEnabled(true);

        if (Position != BasePosition)
        {
            ResetPos();
        }
    }

    protected virtual void OnDeactivated()
    {
        SetActive(false);

        if (Position != BasePosition)
        {
            ResetPos();
        }
    }

    public void SetTransition(UITransition transition)
    {
        Transition = transition;
    }
}