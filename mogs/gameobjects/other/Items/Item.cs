public abstract class Item
{
    public ItemID ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Item(ItemID iD, string name, string description)
    {
        ID = iD;
        Name = name;
        Description = description;
    }
}
