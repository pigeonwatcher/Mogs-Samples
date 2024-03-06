using Microsoft.Xna.Framework;
using System;

public partial class InputService : IInputService
{
    public Point TouchPosition { get; private set; }
    public Point? InitialTouchPosition { get; private set; }

    public event Action<IInputService> OnTouch;
    public event Action<IInputService> OnRelease;
}
