using System.Collections.Generic;

public static class ItemData
{
    public static Dictionary<ItemID, CoffeeItem> CoffeeItems = new Dictionary<ItemID, CoffeeItem>
    {
        [ItemID.CoffeeBagEarthy] = new CoffeeItem(ItemID.CoffeeBagEarthy, "Earthy Coffee", "Earthy"),
        [ItemID.CoffeeBagChocolatey] = new CoffeeItem(ItemID.CoffeeBagChocolatey, "Chocolatey Coffee", "Chocolatey"),
        [ItemID.CoffeeBagNutty] = new CoffeeItem(ItemID.CoffeeBagNutty, "Nutty Coffee", "Nutty"),
        [ItemID.CoffeeBagSpicy] = new CoffeeItem(ItemID.CoffeeBagSpicy, "Spicy Coffee", "Spicy"),
        [ItemID.CoffeeBagFruity] = new CoffeeItem(ItemID.CoffeeBagFruity, "Fruity Coffee", "Fruity"),
    };
}

public enum ItemID
{
    StrawberryCake = 1,
    ChocolateCake = 2,
    CheeseCake = 3,
    PrincessCake = 4,
    CinnamonBun = 5,
    CreamBun = 6,
    Eclair = 7,
    Chokladbollar = 8,

    CoffeeBagEarthy = 100,
    CoffeeBagChocolatey = 101,
    CoffeeBagNutty = 102,
    CoffeeBagSpicy = 103,
    CoffeeBagFruity = 104,
}