using Microsoft.Xna.Framework;

public static class Grid
{
    public static int ScreenWidth { get; private set; }
    public static int ScreenHeight { get; private set; }
    public static int Scale { get; private set; }
    public static Matrix ScaleMatrix { get; private set; }
    public static float SwipeSensitivity { get; private set; }

    private static int widthAdjustment;
    private static int heightAdjustment;

    public static void Initialise(int actualScreenWidth, int actualScreenHeight)
    {
        int scale = actualScreenWidth / 81;

        int gameScreenWidth = 81 * scale;
        int gameScreenHeight = 144 * scale;

        widthAdjustment = ((actualScreenWidth - gameScreenWidth) / scale) / 2;
        heightAdjustment = ((actualScreenHeight - gameScreenHeight) / scale) / 2;

        ScaleMatrix = Matrix.CreateScale(scale, scale, 1);
        ScreenWidth = gameScreenWidth / scale;
        ScreenHeight = gameScreenHeight / scale;

        SwipeSensitivity = 1 / (float)scale;
    }

    public static Vector2 Position(Anchor anchor, Pivot pivot, int spriteWidth, int spriteHeight)
    {
        Vector2 anchorXY = AnchorXY(anchor);
        Vector2 pivotXY = PivotXY(pivot, spriteWidth, spriteHeight);

        Vector2 position = anchorXY - pivotXY;
        position.X += widthAdjustment;
        position.Y += heightAdjustment;

        return position;
    }

    public static Vector2 Position(Anchor anchor, int spriteWidth, int spriteHeight)
    {
        Pivot pivot = AnchorPivot(anchor);
        Vector2 anchorXY = AnchorXY(anchor);
        Vector2 pivotXY = PivotXY(pivot, spriteWidth, spriteHeight);

        Vector2 position = anchorXY - pivotXY;
        position.X += widthAdjustment;
        position.Y += heightAdjustment;

        return position;
    }

    public static Vector2 Position(Anchor anchor, Pivot pivot, Rectangle sprite)
    {
        Vector2 anchorXY = AnchorXY(anchor);
        Vector2 pivotXY = PivotXY(pivot, sprite.Width, sprite.Height);

        Vector2 position = anchorXY - pivotXY;
        position.X += widthAdjustment;
        position.Y += heightAdjustment;

        return position;
    }

    public static Vector2 Position(Anchor anchor, Pivot pivot, int childWidth, int childHeight, int parentWidth, int parentHeight, Vector2 parentPosition)
    {
        Vector2 anchorXY = AnchorXY(anchor, parentWidth, parentHeight);
        Vector2 pivotXY = PivotXY(pivot, childWidth, childHeight);

        Vector2 position = parentPosition + anchorXY - pivotXY;
        // Don't apply adjustment as adjustment is already applied to parent.

        return position;
    }

    public static Vector2 Position(Anchor anchor, Pivot pivot, Rectangle child, Rectangle parent)
    {
        Vector2 parentPosition = parent.Location.ToVector2();

        Vector2 anchorXY = AnchorXY(anchor, parent.Width, parent.Height);
        Vector2 pivotXY = PivotXY(pivot, child.Width, child.Height);

        Vector2 position = parentPosition + anchorXY - pivotXY;

        return position;
    }

    public static int EvenWidthSpacing(int itemWidth, int itemsNum, int parentWidth = 0, int margin = 0)
    {
        if (parentWidth == 0) { parentWidth = ScreenWidth; }

        parentWidth -= margin * 2;

        int totalWidth = itemWidth * itemsNum;
        int remainingWidth = parentWidth - totalWidth;
        int evenWidthSpacing = itemWidth + (remainingWidth / (itemsNum - 1));

        return evenWidthSpacing;
    }

    public static int EvenHeightSpacing(int itemHeight, int itemsNum, int parentHeight = 0, int margin = 0)
    {
        if (parentHeight == 0) { parentHeight = ScreenHeight; }

        parentHeight -= margin * 2;

        int totalHeight = itemHeight * itemsNum;
        int remainingHeight = parentHeight - totalHeight;
        int evenHeightSpacing = itemHeight + (remainingHeight / (itemsNum - 1));

        return evenHeightSpacing;
    }

    private static Vector2 AnchorXY(Anchor anchor, int parentWidth = 0, int parentHeight = 0)
    {
        if (parentWidth == 0 || parentHeight == 0)
        {
            parentWidth = ScreenWidth;
            parentHeight = ScreenHeight;
        }

        switch (anchor)
        {
            case Anchor.TopLeft:
                return new Vector2(0, 0);
            case Anchor.TopCenter:
                return new Vector2(parentWidth / 2, 0);
            case Anchor.Center:
                return new Vector2(parentWidth / 2, parentHeight / 2);
            case Anchor.BottomCenter:
                return new Vector2(parentWidth / 2, parentHeight);
            case Anchor.BottomLeft:
                return new Vector2(0, parentHeight);
            case Anchor.BottomRight:
                return new Vector2(parentWidth, parentHeight);
            default:
                return Vector2.Zero;
        }
    }

    private static Vector2 PivotXY(Pivot pivot, int spriteWidth, int spriteHeight)
    {
        switch (pivot)
        {
            case Pivot.TopLeft:
                return new Vector2(0, 0);
            case Pivot.TopCenter:
                return new Vector2(spriteWidth / 2, 0);
            case Pivot.Center:
                return new Vector2(spriteWidth / 2, spriteHeight / 2);
            case Pivot.BottomCenter:
                return new Vector2(spriteWidth / 2, spriteHeight);
            case Pivot.BottomLeft:
                return new Vector2(0, spriteHeight);
            case Pivot.BottomRight:
                return new Vector2(spriteWidth, spriteHeight);
            default:
                return Vector2.Zero;
        }
    }

    private static Pivot AnchorPivot(Anchor anchor)
    {
        switch (anchor)
        {
            case Anchor.TopLeft:
                return Pivot.TopLeft;
            case Anchor.TopCenter:
                return Pivot.TopCenter;
            case Anchor.TopRight:
                return Pivot.TopRight;
            case Anchor.CenterLeft:
                return Pivot.CenterLeft;
            case Anchor.Center:
                return Pivot.Center;
            case Anchor.CenterRight:
                return Pivot.CenterRight;
            case Anchor.BottomLeft:
                return Pivot.BottomLeft;
            case Anchor.BottomCenter:
                return Pivot.BottomCenter;
            case Anchor.BottomRight:
                return Pivot.BottomRight;
            default:
                return Pivot.TopRight;
        }
    }
}

public enum Anchor
{
    TopLeft,
    TopCenter,
    TopRight,
    CenterLeft,
    Center,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
}

public enum Pivot
{
    TopLeft,
    TopCenter,
    TopRight,
    CenterLeft,
    Center,
    CenterRight,
    BottomLeft,
    BottomCenter,
    BottomRight,
}
