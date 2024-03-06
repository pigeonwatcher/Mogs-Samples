public class Fez : WearableAccessory
{
    public Fez(Cat cat)
    {
        SetSpriteRect(Sprite.AccessoryData[Accessory.Fez]);
        SetPos(cat.Position.X + 6, cat.Position.Y - 3);
    }
}