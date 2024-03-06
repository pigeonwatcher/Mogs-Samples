public class PaperBoat : WearableAccessory
{
    public PaperBoat(Cat cat)
    {
        SetSpriteRect(Sprite.AccessoryData[Accessory.PaperBoat]);
        SetPos(cat.Position.X, cat.Position.Y - 5);
    }
}