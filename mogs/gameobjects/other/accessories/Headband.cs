public class Headband : WearableAccessory
{
    public Headband(Cat cat)
    {
        SetSpriteRect(Sprite.AccessoryData[Accessory.Headband]);
        SetPos(cat.Position.X + 3, cat.Position.Y + 1);
    }
}