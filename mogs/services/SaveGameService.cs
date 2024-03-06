using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class SaveGameService : IService
{
    private PlayerCatService playerCatService;
    private InventoryService inventoryService;
    private PalService palService;
    private ShopService shopService;
    private CafeService cafeService;

    public bool SaveDataLoaded { get; private set; }
    public static PlayerCatData PlayerCatDataSav {  get; private set; }
    public static int CoinsSav { get; private set; }
    public static List<ItemID> FoodInventorySav { get; private set; }
    public static List<ItemID> CoffeeInventorySav { get; private set; }
    public static List<Pal> VisitorsSav { get; private set; }
    public static ShopData[] ShopDatabaseSav { get; private set; }
    public static CafeData[] CafeDatabaseSav { get; private set; }

    private string savLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private string savPath => Path.Combine(savLocation, "sav.json");


    // Use events. SaveGameService subscribes to these events.
    public void Initialise(ServiceManager serviceManager)
    {
        playerCatService = serviceManager.GetService<PlayerCatService>();
        inventoryService = serviceManager.GetService<InventoryService>();
        palService = serviceManager.GetService<PalService>();
        shopService = serviceManager.GetService<ShopService>();
        cafeService = serviceManager.GetService<CafeService>();

        playerCatService.DataChanged += UpdatePlayerCatData;
        inventoryService.DataChanged += UpdateInventoryData;
        palService.DataChanged += UpdatePalVisitors;
        shopService.DataChanged += UpdateShopDatabase;
        cafeService.DataChanged += UpdateCafeDatabase;

        LoadGame();

        playerCatService.InjectSaveData(PlayerCatDataSav);
        inventoryService.InjectSaveData(CoinsSav, FoodInventorySav, CoffeeInventorySav);
        palService.InjectSaveData(VisitorsSav);
        shopService.InjectSaveData(ShopDatabaseSav);
        cafeService.InjectSaveData(CafeDatabaseSav);
    }

    private void LoadGame()
    {
        if (File.Exists(savPath))
        {
            string json = File.ReadAllText(savPath);
            var options = new JsonSerializerOptions
            {
                Converters = { new TimeSpanConverter() }
            };
            SaveData saveData = JsonSerializer.Deserialize<SaveData>(json, options);
            PlayerCatDataSav = saveData.PlayerCatData;
            CoinsSav = saveData.Coins;
            FoodInventorySav = saveData.FoodInventory;
            CoffeeInventorySav = saveData.CoffeeInventory;
            VisitorsSav = saveData.Visitors;
            ShopDatabaseSav = saveData.ShopDatabase;
            CafeDatabaseSav = saveData.CafeDatabase;

            SaveDataLoaded = true;
        }
    }

    public void NewGame()
    {
        playerCatService.InjectSaveData(null);
        inventoryService.InjectSaveData(0, null, null);
        palService.InjectSaveData(null);
        shopService.InjectSaveData(null);
        cafeService.InjectSaveData(null);
    }

    private void SaveGame()
    {
        // Get all current data and turn it into json file.
        SaveData saveData = new SaveData
        {
            PlayerCatData = PlayerCatDataSav,
            Coins = CoinsSav,
            FoodInventory = FoodInventorySav,
            CoffeeInventory = CoffeeInventorySav,
            Visitors = VisitorsSav,
            ShopDatabase = ShopDatabaseSav,
            CafeDatabase = CafeDatabaseSav,
        };

        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Converters = { new TimeSpanConverter() }
        };
        string json = JsonSerializer.Serialize(saveData, options);
        File.WriteAllText(savPath, json);
    }

    private void UpdatePlayerCatData(PlayerCatData playerCatData)
    {
        if (playerCatData != null)
        {
            PlayerCatDataSav = playerCatData;
            SaveGame();
        }
    }

    private void UpdateInventoryData(int coins, List<ItemID> foodInventory, List<ItemID> coffeeInventory)
    {
        if (coins < -1)
        {
            CoinsSav = coins;
        }
        if (foodInventory != null)
        {
            FoodInventorySav = foodInventory;
        }
        if (coffeeInventory != null)
        {
            CoffeeInventorySav = coffeeInventory;
        }

        SaveGame();
    }

    private void UpdatePalVisitors(List<Pal> visitedPals)
    {
        if (visitedPals != null)
        {
            VisitorsSav = visitedPals;
            SaveGame();
        }
    }

    private void UpdateShopDatabase(ShopData[] shopDatabase)
    {
        if(shopDatabase != null) 
        { 
            ShopDatabaseSav = shopDatabase;
            SaveGame();
        }
    }

    private void UpdateCafeDatabase(CafeData[] cafeDatabase)
    {
        if (cafeDatabase != null)
        {
            CafeDatabaseSav = cafeDatabase;
            SaveGame();
        }
    }
}

public class SaveData
{
    public PlayerCatData PlayerCatData {  get; set; }
    public int Coins { get; set; }
    public List<ItemID> FoodInventory { get; set; }
    public List<ItemID> CoffeeInventory { get; set; }
    public List<Pal> Visitors { get; set; }
    public ShopData[] ShopDatabase { get; set; }
    public CafeData[] CafeDatabase { get; set; }
}

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timeSpanString = reader.GetString();
        return TimeSpan.ParseExact(timeSpanString, "dd\\:hh\\:mm\\:ss", null);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd\\:hh\\:mm\\:ss"));
    }
}
