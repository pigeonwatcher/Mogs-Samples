using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class InteractiveElement : UIElement
{
    public Rectangle Bounds { get; private set; }
    public Action OnClick { get; private set; }
    public Color BaseColor { get; private set; } = Color.White;
    public Color ClickColor { get; private set; } = Color.LightGray;

    private NineSliceRenderer nineSlice;

    private bool hasMoved;
    private bool isInitialTouch;
    private bool isInactive;
    private Color disabledColor = Color.DimGray * 0.6f;

    public InteractiveElement(Texture2D atlas, Rectangle sprite)
    {
        SetAtlas(atlas);
        SetSpriteRect(sprite);
    }

    public void Setup(float xUnits, float yUnits)
    {
        nineSlice = new NineSliceRenderer(SpriteRect, xUnits, yUnits);
        SetSize(nineSlice.Width, nineSlice.Height);
        nineSlice.SetColor(ColorUtils.LightOffWhite);
    }

    public void SetClickEvent(Action onClick)
    {
        OnClick = onClick;
    }

    public void SetColor(Color color)
    {
        nineSlice.SetColor(color);
    }

    public void SetInactive(bool isInactive)
    {
        this.isInactive = isInactive;

        if (isInactive)
        {
            nineSlice.SetColor(disabledColor);
        }
        else
        {
            nineSlice.SetColor(BaseColor);
        }
    }

    public override void SetPos(Vector2 position)
    {
        base.SetPos(position);
        nineSlice.SetPosition(position);
        Bounds = BoundsUtilities.Calculate(Position, RectTransform);
    }

    public override void AdjustPos(Vector2 adjustPosition)
    {
        base.AdjustPos(adjustPosition);

        nineSlice.AdjustPosition(adjustPosition);
        Bounds = BoundsUtilities.Calculate(Position, RectTransform);

        if (isInitialTouch)
        {
            hasMoved = true;
        }
    }

    public override void ResetPos()
    {
        base.ResetPos();

        nineSlice.ResetPosition();
        Bounds = BoundsUtilities.Calculate(Position, RectTransform);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
        {
            nineSlice.Draw(spriteBatch, LayerDepth);
            base.Draw(spriteBatch);
        }
    }

    public override void OnTouch(IInputService input)
    {
        if (Enabled && !isInactive)
        {
            if (input.InitialTouchPosition != null && Bounds.Contains((Point)input.InitialTouchPosition))
            {
                isInitialTouch = true;
            }

            if (Bounds.Contains(input.TouchPosition))
            {
                UpdateTouch(true);
            }
            else
            {
                isInitialTouch = false;
                UpdateTouch(false);
            }
        }
    }

    public override void OnRelease(IInputService input)
    {
        if (Enabled)
        {
            if (isInitialTouch && !hasMoved && !isInactive)
            {
                OnClick?.Invoke();
            }
        }
        hasMoved = false;
        isInitialTouch = false;
        UpdateTouch(false);
    }

    private void UpdateTouch(bool isTouched)
    {
        if (!isTouched)
        {
            nineSlice.SetColor(BaseColor);
        }
        else
        {
            nineSlice.SetColor(ClickColor);
        }
    }
}
