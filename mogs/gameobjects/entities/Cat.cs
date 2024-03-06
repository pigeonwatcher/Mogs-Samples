using System;
using System.Diagnostics;

public class Cat : Entity
{
    public CatData CatData { get; set; }

    public Action<Cat> BeingPet { get; set; }
    public Action<Cat> SpriteRectChanged { get; set; }
    public Action<Cat> ColorChanged { get; set; }
    public Action<Cat> PositionChanged { get; set; }
    public Action<Cat> AccessoryChanged { get; set; }

    public virtual void Pet()
    {
        Debug.WriteLine("~Purrrrrrrr~");

        BeingPet?.Invoke(this);
    }
}