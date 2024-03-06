using Microsoft.Xna.Framework;

public class CatPal : Cat
{
    public CatPalData Data => (CatPalData)CatData;

    public void LoadData(CatPalData data)
    {
        SetLayer(Layer.PLAY_LAYER);
        CatData = data;
        SetSpriteRect(Sprite.CatData[Data.Coat]);
        CatPalette palette = CatInfo.CatColorToColor[Data.Color];
        Palette[0] = palette.PrimaryColor.ToVector4();
        Palette[1] = palette.SecondaryColor.ToVector4();
    }

    public override void SetPos(Vector2 position)
    {
        base.SetPos(position);
        Bounds = BoundsUtilities.Calculate(Position, SpriteRect);
    }
}
