using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class ElementList : UIElement
{
    private enum State
    {
        Idle,
        StretchTop,
        StretchBottom,
    }

    public List<InteractiveElement> Elements { get; private set; } = new List<InteractiveElement>();

    private ListDirection direction;

    private int swipeThreshold = 1;
    private Point? currentSwipePosition = null;

    private State state = State.Idle;
    private bool isStationary;
    private int topLeftLimit; // For first card. Should be close to 0.
    private int topLeftAbsoluteLimit; // For stretch. Should be higher than topLeftLimit.
    private int bottomRightLimit; // For last card. Should also be close to 0.
    private int bottomRightAbsoluteLimit; // For stretch. Should be lower than bottomRightLimit.
    private int spacing = 0;
    private int inactiveLimit = 0;

    public ElementList(ListDirection _direction, bool isStationary = false, int topLeftLimit = 0, int topLeftAbsoluteLimit = 0, int bottomRightLimit = 0, int bottomRightAbsoluteLimit = 0, int inactiveLimit = 0, int spacing = 0)
    {
        direction = _direction;

        this.isStationary = isStationary;
        this.topLeftLimit = topLeftLimit;
        this.topLeftAbsoluteLimit = topLeftAbsoluteLimit;
        this.bottomRightLimit = bottomRightLimit;
        this.bottomRightAbsoluteLimit = bottomRightAbsoluteLimit;
        this.inactiveLimit = inactiveLimit;
        this.spacing = spacing;
    }

    public override void AddChild<T>(T child)
    {
        if (child is InteractiveElement card)
        {
            Elements.Add(card);
            UpdateCardPositions();
        }

        base.AddChild(child);
    }

    public void AddClickEvents(Action[] events)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (Elements[i] != null)
            {
                Elements[i].SetClickEvent(events[i]);
            }
        }
    }

    public void ToggleStationary(bool isStationary)
    {
        this.isStationary = isStationary;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (state == State.StretchTop && currentSwipePosition == null)
        {
            if (GetDirection(Elements[0].Position) <= topLeftLimit)
            {
                state = State.Idle;
            }

            MoveCards(-1f * Grid.SwipeSensitivity);
        }

        if (state == State.StretchBottom && currentSwipePosition == null)
        {
            if (GetDirection(Elements[^1].Position) >= bottomRightLimit)
            {
                state = State.Idle;
            }

            MoveCards(1f * Grid.SwipeSensitivity);
        }
    }

    public override void OnTouch(IInputService inputSystem)
    {
        if (!isStationary)
        {
            if (currentSwipePosition == null)
            {
                currentSwipePosition = inputSystem.InitialTouchPosition;
            }

            int swipeDistance = 0;

            if (direction == ListDirection.Vertical)
            {
                swipeDistance = inputSystem.TouchPosition.Y - currentSwipePosition.Value.Y;
            }
            else if (direction == ListDirection.Horizontal)
            {
                swipeDistance = inputSystem.TouchPosition.X - currentSwipePosition.Value.X;
            }

            if (Math.Abs(swipeDistance) > swipeThreshold)
            {
                currentSwipePosition = inputSystem.TouchPosition;
                MoveCards(swipeDistance * Grid.SwipeSensitivity);
            }
        }

        base.OnTouch(inputSystem);
    }

    public override void OnRelease(IInputService inputSystem)
    {
        currentSwipePosition = null;

        base.OnRelease(inputSystem);
    }

    private void UpdateCardPositions()
    {
        if (Elements.Count <= 1) return;

        if (direction == ListDirection.Vertical)
        {
            Rectangle rect = Elements[0].RectTransform;
            Vector2 startPos = new Vector2(RectTransform.X, topLeftLimit);

            for (int i = 0; i < Elements.Count; i++)
            {
                Vector2 pos = new Vector2(startPos.X, startPos.Y + ((rect.Height + spacing) * i));
                Elements[i].SetPos(pos);
                Elements[i].GetChild<Text>().ParentPositionUpdated();
            }

            MoveCards(0);
        }
        else if (direction == ListDirection.Horizontal)
        {
            Rectangle rect = Elements[0].RectTransform;
            Vector2 startPos = new Vector2(topLeftLimit, RectTransform.Y);

            for (int i = 0; i < Elements.Count; i++)
            {
                Vector2 pos = new Vector2(startPos.X + ((rect.Width + spacing) * i), startPos.Y);
                Elements[i].SetPos(pos);
                Elements[i].GetChild<Text>().ParentPositionUpdated();
            }

            MoveCards(0);
        }
    }

    public void MoveCards(float distance)
    {
        float moveAmount = CalculateMoveAmount(distance);

        foreach (InteractiveElement card in Elements)
        {
            if (direction == ListDirection.Vertical)
            {
                card.AdjustPos(new Vector2(0, moveAmount));
            }
            else if (direction == ListDirection.Horizontal)
            {
                card.AdjustPos(new Vector2(moveAmount, 0));
            }
            UpdateCardAppearance(card);
        }
    }

    private void UpdateCardAppearance(InteractiveElement card)
    {
        if (GetDirection(card.Position) < inactiveLimit)
        {
            card.SetInactive(true);
        }
        else
        {
            card.SetInactive(false);
        }
    }

    private float CalculateMoveAmount(float proposedMoveAmount)
    {
        float firstCardNewPosition = GetDirection(Elements[0].Position) + proposedMoveAmount;
        float lastCardNewPosition = GetDirection(Elements[^1].Position) + proposedMoveAmount;

        if (state == State.Idle)
        {
            if (firstCardNewPosition > topLeftLimit)
            {
                state = State.StretchTop;
                return 0;
            }
            else if (lastCardNewPosition < bottomRightLimit)
            {
                state = State.StretchBottom;
                return 0;
            }
        }

        if (firstCardNewPosition > topLeftAbsoluteLimit)
        {
            return topLeftAbsoluteLimit - GetDirection(Elements[0].Position);
        }
        if (lastCardNewPosition < bottomRightAbsoluteLimit)
        {
            return bottomRightAbsoluteLimit - GetDirection(Elements[^1].Position);
        }

        return proposedMoveAmount;
    }

    private float GetDirection(Vector2 position)
    {
        if (direction == ListDirection.Horizontal)
        {
            return position.X;
        }
        else if (direction == ListDirection.Vertical)
        {
            return position.Y;
        }
        else
        {
            return 0;
        }
    }
}

public enum ListDirection
{
    Vertical,
    Horizontal,
    None
}