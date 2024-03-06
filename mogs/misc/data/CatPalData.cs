using System.Collections.Generic;

public class CatPalData : CatData 
{
    public ItemID FavouriteCoffee { get; private set; }
    public ItemID[] FavouriteFoods { get; private set; }
    public List<ItemID> Gifts { get; private set; }

    public CatPalData(string name, CatCoat coat, CatColor color, ItemID favouriteCoffee)
    {
        Name = name;
        Coat = coat;
        Color = color;
        FavouriteCoffee = favouriteCoffee;
    }
}
