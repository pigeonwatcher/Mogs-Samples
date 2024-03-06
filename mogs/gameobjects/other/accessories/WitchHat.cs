public class WitchHat : WearableAccessory
{
    public WitchHat(Cat cat)
    {
        SetSpriteRect(Sprite.AccessoryData[Accessory.WitchHat]);
        SetPos(cat.Position.X, cat.Position.Y - 6);
    }
}