using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public partial class InputService : IInputService
{
#if DESKTOP
    private MouseState currentMouseState;
    private MouseState previousMouseState;

    public void Update()
    {
        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();

        TouchPosition = MousePosition();

        if (IsMouseLeftButtonDown())
        {
            if (InitialTouchPosition == null)
            {
                InitialTouchPosition = TouchPosition;
            }

            OnTouch?.Invoke(this);
        }

        if (IsMouseLeftButtonRelease())
        {
            InitialTouchPosition = null;
            OnRelease?.Invoke(this);
        }
    }

    private Point MousePosition()
    {
        return currentMouseState.Position;
    }

    private bool IsMouseLeftButtonDown()
    {
        return currentMouseState.LeftButton == ButtonState.Pressed;
    }

    private bool IsMouseLeftButtonRelease()
    {
        return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
    }
#endif
}

