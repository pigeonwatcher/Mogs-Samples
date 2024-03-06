using ECS;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class CafeService : IUpdateService
{
    private readonly InventoryService inventoryService;

    public CafeData ActiveCafe { get; private set; }
    public IReadOnlyDictionary<long, CafeData> CafeDatabase => cafeDatabase;
    public event Action<CafeData[]> DataChanged;

    private Dictionary<long, CafeData> cafeDatabase = new Dictionary<long, CafeData>();

    public static readonly Dictionary<ItemID, int> ItemPrices = new Dictionary<ItemID, int>
    {
        [ItemID.CoffeeBagEarthy] = 10,
        [ItemID.CoffeeBagChocolatey] = 10,
        [ItemID.CoffeeBagNutty] = 10,
        [ItemID.CoffeeBagSpicy] = 10,
        [ItemID.CoffeeBagFruity] = 10,
    };

    public const float NewcomerMultiplier = 1f;
    public const float RegularMultiplier = 0.9f; // 10% Off.
    // Cafe Tiers: Newcomer, Regular, Enthusiast, Connoisseur, Patron.

    public CafeService(InventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
    }

    public void SetActiveCafe(long? cafeID)
    {
        if (cafeID == null)
        {
            ActiveCafe = null;
            return;
        }

        if (cafeDatabase.TryGetValue((long)cafeID, out CafeData cafeData))
        {
            ActiveCafe = cafeData;
        }
    }

    public CafeData GetCafeData(long cafeID)
    {
        if (cafeDatabase.TryGetValue(cafeID, out CafeData cafeData))
        {
            return cafeData;
        }
        else
        {
            return CreateNewCafe(cafeID);
        }
    }

    public bool PurchaseItem(long cafeID, int itemIndex)
    {
        if (cafeDatabase.TryGetValue(cafeID, out CafeData cafeData))
        {
            if (itemIndex >= 0 && itemIndex < cafeData.Inventory.Count)
            {
                ItemID itemID = cafeData.Inventory[itemIndex];
                int itemPrice = ItemPrices[itemID];

                int totalCoins = inventoryService.Coins;
                if (itemPrice < totalCoins)
                {
                    inventoryService.UpdateCoins(-itemPrice);
                    inventoryService.AddItem(itemID);
                    cafeData.Inventory.RemoveAt(itemIndex);

                    DataChanged?.Invoke(cafeDatabase.Values.ToArray());

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
            Debug.WriteLine("Cafe does not exist.");
        }

        return false;
    }

    public void Update(GameTime gameTime)
    {
        ActiveCafe?.UpdateTimeSpan(gameTime);
    }

    public void InjectSaveData(CafeData[] cafeDatabase)
    {
        if (cafeDatabase != null)
        {
            for (int i = 0; i < cafeDatabase.Length; i++)
            {
                this.cafeDatabase[cafeDatabase[i].ID] = cafeDatabase[i];
            }
        }
        else
        {
            this.cafeDatabase.Clear();
        }
    }

    private CafeData CreateNewCafe(long id)
    {
        CafeData cafeData = new CafeData(id);
        cafeData.SetAssistant(CreateAssistant());
        cafeData.SetInventory(RestockInventory());

        cafeDatabase[id] = cafeData;

        DataChanged?.Invoke(cafeDatabase.Values.ToArray());

        return cafeData;
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
        int startValue = 100;
        int endValue = 104;
        List<ItemID> potentialItems = Enumerable.Range(startValue, endValue - startValue + 1)
                                                .Select(value => (ItemID)value)
                                                .ToList();


        int stockNum = 3;
        List<ItemID> inventory = new List<ItemID>();

        for (int i = 0; i < stockNum; i++)
        {
            int itemIndex = Game1.Rnd.Next(potentialItems.Count);
            inventory.Add(potentialItems[itemIndex]);
        }

        return inventory;
    }
}
