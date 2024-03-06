using System;
using System.Collections.Generic;

public class InventoryService : IService
{
    public int Coins { get; private set; }
    public IReadOnlyCollection<ItemID> FoodInventory => foodInventory.AsReadOnly();
    public IReadOnlyCollection<ItemID> CoffeeInventory => coffeeInventory.AsReadOnly();

    public event Action<int, List<ItemID>, List<ItemID>> DataChanged;
    public event Action CoinsUpdated;
    public event Action InventoryUpdated;

    private List<ItemID> foodInventory = new List<ItemID>();
    private List<ItemID> coffeeInventory = new List<ItemID>();

    private readonly List<ItemID> defaultFoodInventory = new List<ItemID>
    {
        ItemID.StrawberryCake,
        ItemID.ChocolateCake,
        ItemID.CheeseCake,
    };

    private readonly List<ItemID> defaultCoffeeInventory = new List<ItemID>
    {
        ItemID.CoffeeBagEarthy,
        ItemID.CoffeeBagChocolatey,
        ItemID.CoffeeBagNutty,
        ItemID.CoffeeBagSpicy,
        ItemID.CoffeeBagFruity,
    };

    public void UpdateCoins(int amount)
    {
        Coins += amount;
        CoinsUpdated?.Invoke();
        DataChanged?.Invoke(Coins, foodInventory, coffeeInventory);
    }

    public void AddItem(ItemID itemID)
    {
        List<ItemID> inventory = GetInventory(itemID);
        if (inventory != null)
        {
            inventory.Add(itemID);
            InventoryUpdated?.Invoke();
            DataChanged?.Invoke(Coins, foodInventory, coffeeInventory);
        }
    }

    public void RemoveItem(InventoryType inventoryType, int itemIndex)
    {
        List<ItemID> inventory = inventoryType == InventoryType.Food ? foodInventory : coffeeInventory;
        if (itemIndex >= 0 && itemIndex < inventory.Count)
        {
            inventory.RemoveAt(itemIndex);
            InventoryUpdated?.Invoke();
            DataChanged?.Invoke(Coins, foodInventory, coffeeInventory);
        }
    }

    public void InjectSaveData(int coins, List<ItemID> foodInventory, List<ItemID> coffeeInventory)
    {
        Coins = coins == 0 ? 100 : coins;
        this.foodInventory = foodInventory ?? defaultFoodInventory;
        this.coffeeInventory = coffeeInventory ?? defaultCoffeeInventory;
    }

    private List<ItemID> GetInventory(ItemID itemID)
    {
        int itemValue = (int)itemID;
        if (itemValue >= 0 && itemValue <= 99)
        {
            return foodInventory;
        }
        else if (itemValue >= 100 && itemValue <= 199)
        {
            return coffeeInventory;
        }
        return null;
    }
}

public enum InventoryType
{
    Coffee,
    Food
}