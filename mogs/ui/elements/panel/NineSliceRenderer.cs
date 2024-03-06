using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NineSliceRenderer
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int UnitSize { get; private set; }

    // 9-Slice Panel.
    private NineSlice[] slices = new NineSlice[9];

    private float unitsXNoCorners;
    private float unitsYNoCorners;
    private Vector2 position;
    private Color color = Color.White;

    public NineSliceRenderer(Rectangle sprite, float xUnits, float yUnits)
    {
        unitsXNoCorners = xUnits - 2;
        unitsYNoCorners = yUnits - 2;

        Rectangle spriteData = sprite;
        int spriteSize = spriteData.Width;
        UnitSize = spriteSize / 3;

        Width = (int)(UnitSize * xUnits);
        Height = (int)(UnitSize * yUnits);

        int slice1x = spriteData.X;
        int slice1y = spriteData.Y;
        int slice2x = spriteData.X + UnitSize;
        int slice2y = spriteData.Y + UnitSize;
        int slice3x = spriteData.X + (UnitSize * 2);
        int slice3y = spriteData.Y + (UnitSize * 2);

        // Setup Slice Sprites.
        Rectangle topLeftSprite = new Rectangle(slice1x, slice1y, UnitSize, UnitSize);
        Rectangle topCenterSprite = new Rectangle(slice2x, slice1y, UnitSize, UnitSize);
        Rectangle topRightSprite = new Rectangle(slice3x, slice1y, UnitSize, UnitSize);
        Rectangle centerLeftSprite = new Rectangle(slice1x, slice2y, UnitSize, UnitSize);
        Rectangle centerSprite = new Rectangle(slice2x, slice2y, UnitSize, UnitSize);
        Rectangle centerRightSprite = new Rectangle(slice3x, slice2y, UnitSize, UnitSize);
        Rectangle bottomLeftSprite = new Rectangle(slice1x, slice3y, UnitSize, UnitSize);
        Rectangle bottomCenterSprite = new Rectangle(slice2x, slice3y, UnitSize, UnitSize);
        Rectangle bottomRightSprite = new Rectangle(slice3x, slice3y, UnitSize, UnitSize);

        // Setup Panel Slices.
        NineSlice topLeftPosition = new NineSlice(topLeftSprite, new RectangleF((int)this.position.X, (int)this.position.Y, UnitSize, UnitSize));
        NineSlice topCenterPosition = new NineSlice(topCenterSprite, new RectangleF((int)this.position.X + UnitSize, (int)this.position.Y, (int)(UnitSize * unitsXNoCorners), UnitSize));
        NineSlice topRightPosition = new NineSlice(topRightSprite, new RectangleF((int)this.position.X + (int)(UnitSize * (unitsXNoCorners + 1)), (int)this.position.Y, UnitSize, UnitSize));
        NineSlice centerLeftPosition = new NineSlice(centerLeftSprite, new RectangleF((int)this.position.X, (int)this.position.Y + UnitSize, UnitSize, (int)(UnitSize * unitsYNoCorners)));
        NineSlice centerPosition = new NineSlice(centerSprite, new RectangleF((int)this.position.X + UnitSize, (int)this.position.Y + UnitSize, (int)(UnitSize * unitsXNoCorners), (int)(UnitSize * unitsYNoCorners)));
        NineSlice centerRightPosition = new NineSlice(centerRightSprite, new RectangleF((int)this.position.X + (int)(UnitSize * (unitsXNoCorners + 1)), (int)this.position.Y + UnitSize, UnitSize, (int)(UnitSize * unitsYNoCorners)));
        NineSlice bottomLeftPosition = new NineSlice(bottomLeftSprite, new RectangleF((int)this.position.X, (int)(this.position.Y + (UnitSize * 2) + (UnitSize * (unitsYNoCorners - 1))), UnitSize, UnitSize));
        NineSlice bottomCenterPosition = new NineSlice(bottomCenterSprite, new RectangleF((int)this.position.X + UnitSize, (int)(this.position.Y + (UnitSize * 2) + (UnitSize * (unitsYNoCorners - 1))), (int)(UnitSize * unitsXNoCorners), UnitSize));
        NineSlice bottomRightPosition = new NineSlice(bottomRightSprite, new RectangleF((int)(this.position.X + (UnitSize * (unitsXNoCorners + 1))), (int)(this.position.Y + (UnitSize * 2) + (UnitSize * (unitsYNoCorners - 1))), UnitSize, UnitSize));

        slices[0] = topLeftPosition;
        slices[1] = topCenterPosition;
        slices[2] = topRightPosition;
        slices[3] = centerLeftPosition;
        slices[4] = centerPosition;
        slices[5] = centerRightPosition;
        slices[6] = bottomLeftPosition;
        slices[7] = bottomCenterPosition;
        slices[8] = bottomRightPosition;
    }

    public void SetColor(Color _color)
    {
        color = _color;
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;

        slices[0].SetPosition(new Vector2((int)this.position.X, (int)this.position.Y));
        slices[1].SetPosition(new Vector2((int)this.position.X + UnitSize, (int)this.position.Y));
        slices[2].SetPosition(new Vector2((int)this.position.X + (int)(UnitSize * (unitsXNoCorners + 1)), (int)this.position.Y));
        slices[3].SetPosition(new Vector2((int)this.position.X, (int)this.position.Y + UnitSize));
        slices[4].SetPosition(new Vector2((int)this.position.X + UnitSize, (int)this.position.Y + UnitSize));
        slices[5].SetPosition(new Vector2((int)this.position.X + (int)(UnitSize * (unitsXNoCorners + 1)), (int)this.position.Y + UnitSize));
        slices[6].SetPosition(new Vector2((int)this.position.X, (int)(this.position.Y + (UnitSize * 2) + (UnitSize * (unitsYNoCorners - 1)))));
        slices[7].SetPosition(new Vector2((int)this.position.X + UnitSize, (int)(this.position.Y + (UnitSize * 2) + (UnitSize * (unitsYNoCorners - 1)))));
        slices[8].SetPosition(new Vector2((int)(this.position.X + (int)(UnitSize * (unitsXNoCorners + 1))), (int)(this.position.Y + (UnitSize * 2) + (UnitSize * (unitsYNoCorners - 1)))));
    }

    public void MovePosition(Vector2 movePos)
    {
        for (int i = 0; i < slices.Length; i++)
        {
            slices[i].MovePosition(movePos);
        }
    }

    public void ResetPosition()
    {
        for (int i = 0; i < slices.Length; i++)
        {
            slices[i].ResetPosition();
        }
    }

    public void AdjustPosition(Vector2 adjustment)
    {
        for (int i = 0; i < slices.Length; i++)
        {
            slices[i].AdjustPosition(adjustment);
        }
    }

    public void Draw(SpriteBatch spriteBatch, float layerDepth)
    {
        for (int i = 0; i < slices.Length; i++)
        {
            spriteBatch.Draw(Resources.UIAtlas, slices[i].Position, slices[i].SpriteRect, color, 0f, Vector2.Zero, slices[i].Scale, SpriteEffects.None, layerDepth);
        }
    }
}
