using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public partial class GameObject
{
    /// <summary>
    /// The base GameObject for classes such as UIElement and Entity. Contains core data and logic for display. 
    /// Due to being a large class, it has been seperated into partical classes for better readability.
    /// </summary>

    public Guid ID { get; private set; }
    public string Name { get; private set; }
    public Texture2D Atlas { get; private set; }
    public Rectangle SpriteRect { get; private set; }
    public Color Color { get; private set; } = Color.White;
    public Vector2 BasePosition { get; private set; }
    public Vector2 Position { get; private set; }
    public Rectangle RectTransform => rectTransform;
    public float LayerDepth { get; private set; }
    public bool Active { get; private set; }
    public bool Enabled { get; private set; }

    protected Rectangle rectTransform;

    public GameObject()
    {
        ID = Guid.NewGuid();
    }

    public void SetName(string name)
    {
        Name = name;
    }
}
