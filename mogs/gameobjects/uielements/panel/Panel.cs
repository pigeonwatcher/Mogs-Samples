using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Panel : UIElement
{
    public int Width => nineSlice.Width;
    public int Height => nineSlice.Height;
    public int UnitSize => nineSlice.UnitSize;

    private NineSliceRenderer nineSlice;

    public Panel(Texture2D atlas, Rectangle sprite)
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

    public override void SetPos(Vector2 pos)
    {
        base.SetPos(pos);
        nineSlice.SetPosition(pos);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
        {
            nineSlice.Draw(spriteBatch, LayerDepth);
            base.Draw(spriteBatch);
        }
    }

    public override void ResetPos()
    {
        nineSlice.ResetPosition();

        base.ResetPos();
    }

    public override void AdjustPos(Vector2 adjustPosition)
    {
        nineSlice.AdjustPosition(adjustPosition);

        base.AdjustPos(adjustPosition);
    }
}
