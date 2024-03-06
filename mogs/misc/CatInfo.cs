using Microsoft.Xna.Framework;
using System.Collections.Generic;

public static class CatInfo
{

    public static readonly Dictionary<CatColor, CatPalette> CatColorToColor = new Dictionary<CatColor, CatPalette>
    {
        { CatColor.White, new CatPalette(ColorUtils.LightOffWhite, ColorUtils.LightOffWhite) },
        { CatColor.Grey, new CatPalette(ColorUtils.LightGrey, ColorUtils.DarkGrey) },
        { CatColor.Ginger, new CatPalette(ColorUtils.LightGinger, ColorUtils.DarkGinger) },
        { CatColor.Brown, new CatPalette(ColorUtils.LightBrown, ColorUtils.DarkBrown) },
        { CatColor.Black, new CatPalette(ColorUtils.DarkGrey, ColorUtils.DarkGrey) },
        { CatColor.WhiteGinger, new CatPalette(ColorUtils.LightOffWhite, ColorUtils.DarkGinger) },
        { CatColor.WhiteGrey, new CatPalette(ColorUtils.LightOffWhite, ColorUtils.DarkGrey) },
        { CatColor.WhiteBrown, new CatPalette(ColorUtils.LightOffWhite, ColorUtils.DarkBrown) },
    };

    public static class Pals
    {
        public static IReadOnlyDictionary<Pal, CatPalData> Data = new Dictionary<Pal, CatPalData>
        {
            [Pal.Oliver] = new CatPalData("Oliver", CatCoat.Striped, CatColor.Brown, ItemID.CoffeeBagEarthy),
            [Pal.Alice] = new CatPalData("Alice", CatCoat.Patched, CatColor.WhiteGrey, ItemID.CoffeeBagChocolatey),
            [Pal.Molly] = new CatPalData("Molly", CatCoat.Solid, CatColor.White, ItemID.CoffeeBagEarthy),
            [Pal.Shadow] = new CatPalData("Shadow", CatCoat.Solid, CatColor.Black, ItemID.CoffeeBagSpicy),
            [Pal.Cookie] = new CatPalData("Cookie", CatCoat.Spotted, CatColor.Brown, ItemID.CoffeeBagNutty),
            [Pal.Jasper] = new CatPalData("Jasper", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
            [Pal.Tom] = new CatPalData("Tom", CatCoat.Solid, CatColor.Grey, ItemID.CoffeeBagChocolatey),
            [Pal.Mittens] = new CatPalData("Mittens", CatCoat.Pointed, CatColor.WhiteBrown, ItemID.CoffeeBagFruity),
            [Pal.Colonel] = new CatPalData("Colonel", CatCoat.Pointed, CatColor.Grey, ItemID.CoffeeBagSpicy),
            [Pal.Felix] = new CatPalData("Felix", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
            [Pal.Lux] = new CatPalData("Lux", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
            [Pal.Luna] = new CatPalData("Luna", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
            [Pal.Simba] = new CatPalData("Simba", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
            [Pal.Bagpuss] = new CatPalData("Bagpuss", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
            [Pal.Puss] = new CatPalData("Puss", CatCoat.Striped, CatColor.Ginger, ItemID.CoffeeBagEarthy),
        };

    }
}

