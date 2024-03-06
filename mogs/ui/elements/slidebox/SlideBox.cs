using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

public class SlideBox : UIElement
{
    public delegate void UpdateItem(UIElement[] items, int index, int actualIndex);
    public delegate void DisposeItems();

    public int SlideTotal { get; private set; }

    private enum State
    {
        Idle,
        SlideRight,
        SlideLeft,
        Return,
    }

    private State state;

    private int slideIndex;
    private int slideDirection;
    private int slideThreshold;

    private int boxMaxPos;
    private int boxMinPos;
    private Point? swipePos = null;

    private RectangleF box;
    
    private UIElement[] items;
    private Text slideText;

    private int itemAmount;
    private int itemsPerRow;
    private int itemsPerSlide = 4;
    private UpdateItem updateItem;
    private DisposeItems disposeitems;

    private const int swipeThreshold = 1;

    public void Setup(Rectangle box, int itemAmount, int itemsPerRow, int itemsPerSlide, UpdateItem updateItem = null, DisposeItems disposeItems = null)
    {
        this.box = box;
        base.SetPos(this.box.Position);

        slideThreshold = (int)(this.box.X + (this.box.Width / 2));
        boxMaxPos = (int)(BasePosition.X + (this.box.Width));
        boxMinPos = (int)(BasePosition.X - (this.box.Width));

        this.itemAmount = itemAmount;
        this.itemsPerRow = itemsPerRow;
        this.itemsPerSlide = itemsPerSlide;
        items = new UIElement[itemsPerSlide * 3];

        this.updateItem = updateItem;
        this.disposeitems = disposeItems;
    }

    public void Refresh()
    {
        SlideTotal = (int)Math.Floor((double)(float)itemAmount / itemsPerSlide);

        for (int i = 0; i < items.Length; i++)
        {
            Vector2 pos = ItemPosition(i);
            Vector2 slidePos = SlidePosition(i, pos);
            items[i].SetPos(slidePos);
        }

        UpdateItems();
        UpdateSlideText();
    }

    public void Dispose()
    {
        disposeitems?.Invoke();
    }

    public void UpdateItems()
    {
        if (slideIndex > SlideTotal)
        {
            slideIndex = 0;
        }
        else if (slideIndex < 0) 
        { 
            slideIndex = SlideTotal;
        }

        for (int i = 0; i < items.Length; i++)
        {
            int itemIndex = (i - itemsPerSlide) + (itemsPerSlide * slideIndex);

            if (!items[i].Active || !items[i].Enabled) 
            {
                items[i].SetActive(true);
                items[i].SetEnabled(true);
            }
        
            if (slideIndex == 0)
            {
                int start = itemAmount - (itemAmount % itemsPerSlide);
                int lastItemIndex = start + i;

                if (i < itemsPerSlide && lastItemIndex > itemAmount)
                {
                    items[i].SetActive(false);
                    items[i].SetEnabled(false);
                    continue;
                }

                if (i < itemsPerSlide)
                {
                    updateItem?.Invoke(items, i, lastItemIndex);
                }
                else
                {
                    updateItem?.Invoke(items, i, itemIndex);
                }
            }
            else if (slideIndex == SlideTotal)
            {
                if (itemIndex > (itemAmount) && i < (itemsPerSlide * 2))
                {
                    items[i].SetActive(false);
                    items[i].SetEnabled(false);
                    continue;
                }

                if(i >= (itemsPerSlide * 2)) 
                {
                    int index = i % itemsPerSlide;
                    updateItem?.Invoke(items, i, index);
                }
                else
                {
                    updateItem?.Invoke(items, i, itemIndex);
                }
            }
            else if(slideIndex == SlideTotal - 1)
            {
                if (itemIndex > itemAmount)
                {
                    items[i].SetActive(false);
                    items[i].SetEnabled(false);
                    continue;
                }

                updateItem?.Invoke(items, i, itemIndex);
            }
            else
            {
                updateItem?.Invoke(items, i, itemIndex);
            }
        }
    }

    public override void AddChild<T>(T child)
    {
        if (child is Text _slideText)
        {
            slideText = _slideText;
        }
        else if (child is InteractiveElement item)
        {
            for(int i = 0; i < items.Length; i++) 
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    break;
                }
            }
        }

        base.AddChild(child);
    }

    public override void AdjustPos(Vector2 adjustPos)
    {
        box.Position += adjustPos;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].AdjustPos(adjustPos);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if(!Active) { return; }

        if (swipePos == null)
        {
            switch (state)
            {
                case State.SlideRight:
                    OnSlideRight();
                    break;
                case State.SlideLeft:
                    OnSlideLeft();
                    break;
                case State.Return:
                    OnSlideReturn();
                    break;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].Update(gameTime);
            }
        }
    }

    public override void OnTouch(IInputService inputSystem)
    {
        if (!Enabled) { return; }

        if (swipePos == null)
        {
            swipePos = inputSystem.InitialTouchPosition;
        }

        Vector2 swipeDistance = new Vector2(inputSystem.TouchPosition.X - swipePos.Value.X, 0);

        if (Math.Abs(swipeDistance.X) > swipeThreshold)
        {
            swipePos = inputSystem.TouchPosition;

            MoveElements(swipeDistance * Grid.SwipeSensitivity);
        }

        if(state == State.Idle)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    items[i].OnTouch(inputSystem);
                }
            }
        }
    }

    public override void OnRelease(IInputService inputSystem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].OnRelease(inputSystem);
            }
        }

        swipePos = null;
    }

    private void MoveElements(Vector2 distance)
    {
        float adjustPos = CalculateMoveAmount(distance.X);
        AdjustPos(new Vector2(adjustPos, 0));
    }

    private float CalculateMoveAmount(float proposedMoveAmount)
    {
        if (box.X > slideThreshold)
        {
            state = State.SlideLeft;
            slideDirection = 1;

            if (box.X > boxMaxPos)
            {
                return boxMaxPos - box.X;
            }
        }
        else if (box.X < -slideThreshold)
        {
            state = State.SlideRight;
            slideDirection = -1;

            if (box.X < boxMinPos)
            {
                return boxMinPos - box.X;
            }
        }
        else if (box.X > -slideThreshold && box.X < 0)
        {
            state = State.Return;
        }
        else if (box.X < slideThreshold && box.X > 0)
        {
            state = State.Return;
        }

        if (state != State.SlideLeft && state != State.SlideRight)
        {
            return proposedMoveAmount;
        }

        if (state == State.SlideLeft || state == State.SlideRight)
        {
            float totalMovement = box.X + proposedMoveAmount;

            if (totalMovement > -slideThreshold && totalMovement < 0)
            {
                state = State.Return;
            }
            if (totalMovement < slideThreshold && totalMovement > 0)
            {
                state = State.Return;
            }

            if (totalMovement < boxMaxPos && totalMovement > boxMinPos)
            {
                // So you can move back after reaching max.
                return proposedMoveAmount;
            }
        }

        if (swipePos == null)
        {
            return proposedMoveAmount;
        }

        return 0;
    }

    private Vector2 ItemPosition(int itemIndex)
    {
        // Get First Item Pos.
        int xMargin = 2;
        int yMargin = 2;
        int itemWidth = items[0].RectTransform.Width;
        int itemHeight = items[0].RectTransform.Height;
        int totalWidth = (itemWidth * itemsPerRow) + xMargin * (itemsPerRow - 1);
        int rowsPerSlide = itemsPerSlide / itemsPerRow;
        int totalHeight = (itemHeight * rowsPerSlide) + yMargin * (rowsPerSlide - 1);

        Vector2 startPos = Grid.Position(Anchor.Center, totalWidth, totalHeight);

        // Wrap item index if it exceeds itemsPerSlide
        itemIndex = itemIndex % itemsPerSlide;

        // Calculate row and column
        int row = (itemIndex / itemsPerRow) % rowsPerSlide;
        int column = itemIndex % itemsPerRow;

        // Calculate position
        int columnPos = (itemWidth + xMargin) * column;
        int rowPos = (itemHeight + yMargin) * row;

        Vector2 position = new Vector2(startPos.X + columnPos, startPos.Y + rowPos);

        return position;
    }



    private Vector2 SlidePosition(int itemIndex, Vector2 itemPosition)
    {
        int slide = itemIndex / itemsPerSlide;
        int slideOffset = (int)box.Width * ((slide - 1) - slideIndex);

        Vector2 position = new Vector2(itemPosition.X + slideOffset, itemPosition.Y);
        return position;
    }

    private void OnSlideRight()
    {
        if (box.X <= boxMinPos)
        {
            state = State.Idle;
            box.X = BasePosition.X;

            slideIndex++;
            ResetPos();
            UpdateItems();
            UpdateSlideText();
            return;
        }

        MoveElements(new Vector2(1 * slideDirection, 0));
    }

    private void OnSlideLeft()
    {
        if (box.X >= boxMaxPos)
        {
            state = State.Idle;
            box.X = BasePosition.X;

            slideIndex--;
            ResetPos();
            UpdateItems();
            UpdateSlideText();
            return;
        }

        MoveElements(new Vector2(1 * slideDirection, 0));
    }

    private void OnSlideReturn()
    {
        if (box.X < 2 && box.X > -2)
        {
            box.X = (int)BasePosition.X;
            state = State.Idle;
            ResetPos();
        }
        else if (box.X < 0)
        {
            MoveElements(new Vector2(1, 0));  // Moving right
        }
        else if (box.X > 0)
        {
            MoveElements(new Vector2(-1, 0)); // Moving left
        }
    }

    private void UpdateSlideText()
    {
        slideText.SetText($"{slideIndex + 1} / {SlideTotal + 1}", Alignment.Center, Alignment.Bottom, 0, -4);
    }
}
