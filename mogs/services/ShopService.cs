using ECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class ShopService : IService
{
    private readonly InventoryService inventoryService;

    public IReadOnlyDictionary<long, ShopData> ShopDatabase => shopDatabase;
    public event Action<ShopData[]> DataChanged;

    public readonly static Dictionary<ItemID, int> ItemPrices = new Dictionary<ItemID, int>
    {
        [ItemID.StrawberryCake] = 10,
        [ItemID.ChocolateCake] = 10,
        [ItemID.CheeseCake] = 10,
        [ItemID.PrincessCake] = 10,
        [ItemID.CinnamonBun] = 8,
        [ItemID.CreamBun] = 8,
        [ItemID.Eclair] = 6,
        [ItemID.Chokladbollar] = 8,
    };

    private Dictionary<long, ShopData> shopDatabase = new Dictionary<long, ShopData>();

    public ShopService(InventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
    }

    public ShopData GetShopData(long shopID)
    {
        if (shopDatabase.TryGetValue(shopID, out ShopData shopData))
        {
            return shopData;
        }
        else
        {
            return CreateNewShop(shopID);
        }
    }

    public bool PurchaseItem(long shopID, int itemIndex)
    {
        if (shopDatabase.TryGetValue(shopID, out ShopData shopData))
        {
            if (itemIndex >= 0 && itemIndex < shopData.Inventory.Count)
            {
                ItemID itemID = shopData.Inventory[itemIndex];
                int itemPrice = ItemPrices[itemID];

                int totalCoins = inventoryService.Coins;
                if (itemPrice < totalCoins)
                {
                    inventoryService.UpdateCoins(-itemPrice);
                    inventoryService.AddItem(itemID);
                    shopData.Inventory.RemoveAt(itemIndex);
                    shopData.TotalPurchases++;

                    DataChanged?.Invoke(shopDatabase.Values.ToArray());

                    return true;
                }
                else
                {
                    Debug.WriteLine("Not Enough Coins!");
                }
            }
            else
            {
                Debug.WriteLine("Item does not exist");
            }
        }
        else
        {
            Debug.WriteLine("Shop does not exist.");
        }

        return false;
    }

    public void InjectSaveData(ShopData[] shopDatabase)
    {
        if (shopDatabase != null)
        {
            for (int i = 0; i < shopDatabase.Length; i++)
            {
                this.shopDatabase[shopDatabase[i].ID] = shopDatabase[i];
            }
        }
        else
        {
            this.shopDatabase.Clear();
        }
    }

    private ShopData CreateNewShop(long id)
    {
        ShopData shopData = new ShopData(id);
        shopData.SetAssistant(CreateAssistant());
        shopData.SetInventory(RestockInventory());

        shopDatabase[id] = shopData;

        DataChanged?.Invoke(shopDatabase.Values.ToArray());

        return shopData;
    }

    private CatAssistantData CreateAssistant()
    {
        List<CatCoat> potentialCoats = Enum.GetValues(typeof(CatCoat)).Cast<CatCoat>().ToList();
        List<CatColor> potentialColors = Enum.GetValues(typeof(CatColor)).Cast<CatColor>().ToList();

        if (potentialCoats.Contains(CatCoat.Unknown)) { potentialCoats.Remove(CatCoat.Unknown); }

        int coatIndex = Game1.Rnd.Next(potentialCoats.Count);
        int colorIndex = Game1.Rnd.Next(potentialColors.Count);

        return new CatAssistantData("", potentialCoats[coatIndex], potentialColors[colorIndex]);
    }

    private List<ItemID> RestockInventory()
    {
        int startValue = 1;
        int endValue = 8;
        List<ItemID> potentialItems = Enumerable.Range(startValue, endValue - startValue + 1)
                                                .Select(value => (ItemID)value)
                                                .ToList();


        int stockNum = 9;
        List<ItemID> inventory = new List<ItemID>();

        for (int i = 0; i < stockNum; i++)
        {
            int itemIndex = Game1.Rnd.Next(potentialItems.Count);
            inventory.Add(potentialItems[itemIndex]);
        }

        return inventory;
    }
}
