using System;
using System.Linq;

public class CoffeeMachineService : IService
{
    private readonly InventoryService inventoryService;

    public ItemID? BrewedCoffee { get; private set; }
    public DateTime? BrewedTime { get; private set; }

    public event Action CoffeeBrewed;

    public CoffeeMachineService(InventoryService _inventoryService)
    {
        inventoryService = _inventoryService;
    }

    public void GrindCoffee(int itemIndex)
    {
        if (BrewedCoffee == null)
        {
            ItemID item = inventoryService.CoffeeInventory.ElementAt(itemIndex);
            BrewCoffee(item);
            inventoryService.RemoveItem(InventoryType.Coffee, itemIndex);
        }
    }

    public void CoffeeConsumed()
    {
        BrewedCoffee = null;
        BrewedTime = null;
    }

    private void BrewCoffee(ItemID coffeeItem)
    {
        if (BrewedCoffee == null)
        {
            BrewedCoffee = coffeeItem;
            BrewedTime = DateTime.Now;
        }
    }
}
