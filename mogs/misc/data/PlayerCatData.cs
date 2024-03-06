public class PlayerCatData : CatData
{
    public int Happiness { get; set; }
    public int Hunger { get; set; }

    public const int MaxHappiness = 5;
    public const int MaxHunger = 5;

    public PlayerCatData(string name, CatCoat coat, CatColor color, Accessory accessory, int happiness, int hunger)
    {
        Name = name;
        Coat = coat;
        Color = color;
        Accessory = accessory;
        Happiness = happiness;
        Hunger = hunger;
    }
}


