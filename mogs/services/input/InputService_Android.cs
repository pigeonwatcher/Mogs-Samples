using Microsoft.Xna.Framework.Input.Touch;

public partial class InputService : IInputService
{
#if ANDROID
    public void Update()
    {
        foreach (var touch in TouchPanel.GetState())
        {
            TouchPosition = touch.Position.ToPoint();

            if (touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved)
            {
                if (InitialTouchPosition == null)
                {
                    InitialTouchPosition = TouchPosition;
                }

                OnTouch?.Invoke(this);
            }

            if (touch.State == TouchLocationState.Released)
            {
                InitialTouchPosition = null;
                OnRelease?.Invoke(this);
            }
        }
    }
#endif
}

