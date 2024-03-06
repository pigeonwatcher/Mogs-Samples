using Microsoft.Xna.Framework;

public static class PanelUtilities
{
    public static int GetLengthForUnit(float unit, Rectangle sprite)
    {
        int sectionLength = sprite.Width / 3;
        int length = (int)(sectionLength * unit);
        return length;
    }
}
