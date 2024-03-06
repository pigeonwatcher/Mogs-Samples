using Microsoft.Xna.Framework;
using System;

public abstract class UITransition
{
    public enum State
    {
        Idle,
        TransitionIn,
        TransitionOut,
    }

    public Action AnimationComplete { get; set; }
    protected UIElement UIElement { get; set; }
    protected float Duration { get; set; }
    protected float ElapsedTime { get; set; }
    protected State ActiveState { get; set; } = State.Idle;

    public abstract void StartTransitionIn();
    public abstract void StartTransitionOut();
    public abstract void Update(GameTime gameTime);
}
