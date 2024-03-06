using Microsoft.Xna.Framework;

public class Slide : UITransition
{
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 currentPosition;

    public Slide(UIElement element, float duration, Vector2 startPosition)
    {
        UIElement = element;
        Duration = duration;
        this.startPosition = startPosition;
    }

    public override void StartTransitionIn()
    {
        targetPosition = UIElement.BasePosition;
        currentPosition = targetPosition + startPosition;
        UIElement.AdjustPos(startPosition);
        ElapsedTime = 0;
        ActiveState = State.TransitionIn;
    }

    public override void StartTransitionOut()
    {
        targetPosition = startPosition;
        currentPosition = UIElement.BasePosition;
        ElapsedTime = 0;
        ActiveState = State.TransitionOut;
    }

    public override void Update(GameTime gameTime)
    {
        switch (ActiveState)
        {
            case State.TransitionIn:
                AnimateTransition(gameTime, targetPosition);
                break;
            case State.TransitionOut:
                AnimateTransition(gameTime, startPosition);
                break;
        }
    }

    private void AnimateTransition(GameTime gameTime, Vector2 targetPos)
    {
        ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (ElapsedTime < Duration)
        {
            float progress = ElapsedTime / Duration;
            Vector2 newPosition = Vector2.Lerp(currentPosition, targetPos, progress);
            UIElement.AdjustPos(newPosition - UIElement.Position);
        }
        else
        {
            UIElement.AdjustPos(targetPos - UIElement.Position);
            ActiveState = State.Idle;
            AnimationComplete?.Invoke();
        }
    }
}

