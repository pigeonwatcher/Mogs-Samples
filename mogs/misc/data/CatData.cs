public class CatData
{
    public string Name { get; protected set; }
    public CatCoat Coat { get; protected set; }
    public CatColor Color { get; protected set; }
    public Accessory Accessory { get; protected set; } 

    public void SetAccessory(Accessory accessory)
    {
        Accessory = accessory;
    }
}
