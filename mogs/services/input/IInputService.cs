using Microsoft.Xna.Framework;
using System;

public interface IInputService : IService
{
    Point TouchPosition { get; }
    Point? InitialTouchPosition { get; }

    event Action<IInputService> OnTouch;
    event Action<IInputService> OnRelease;

    void Update();
}
