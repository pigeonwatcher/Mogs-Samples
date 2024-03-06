using Microsoft.Xna.Framework;

public struct CatPalette
{
    public Color PrimaryColor { get; set; }
    public Color SecondaryColor { get; set; }

    public CatPalette(Color primaryColor, Color secondaryColor)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }
}
