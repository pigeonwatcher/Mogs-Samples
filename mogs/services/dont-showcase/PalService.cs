using ECS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class PalService : IService
{
    private readonly EntityManager entityManager;
    private readonly CoffeeMachineService coffeeMachineService;
    private readonly InventoryService inventoryService;

    public Pal? ActivePal { get; private set; }
    public DateTime? PalVisitTime { get; private set; }

    public event Action<List<Pal>> DataChanged;
    public Action<Pal> PalVisit;
    public Action<Pal, int> PalLeft;

    private CatPal palEntity;

    private const float WAIT_TIME = 0.25f;
    private const float MAX_VISIT_TIME = 0.5f;

    public PalService(EntityManager entityManager, CoffeeMachineService coffeeMachineService, InventoryService inventoryService)
    {
        this.entityManager = entityManager;
        this.coffeeMachineService = coffeeMachineService;
        this.inventoryService = inventoryService;
    }

    public void SetPal(Pal? pal)
    {
        ActivePal = pal;
        PalVisitTime = pal != null ? DateTime.Now : null;
    }

    public void RegisterPalVisit(Pal pal)
    {
        if(!Progress.Pals.VisitedPals.Contains(pal))
        {
            Progress.Pals.VisitedPals.Add(pal);
            DataChanged?.Invoke(Progress.Pals.VisitedPals);
        }
    }

    public void CheckPalVisit()
    {
        if (coffeeMachineService.BrewedCoffee != null && ActivePal == null)
        {
            TimeSpan timeSpan = DateTime.Now - (DateTime)coffeeMachineService.BrewedTime;

            if (timeSpan.TotalMinutes >= WAIT_TIME)
            {
                SpawnPal();
            }
        }
        else if (ActivePal != null) 
        {
            TimeSpan timeSpan = DateTime.Now - (DateTime)PalVisitTime;

            if (timeSpan.TotalMinutes >= MAX_VISIT_TIME)
            {
                UnspawnPal();
            }
            else
            {
                palEntity.SetActive(true);
                palEntity.SetEnabled(true);
            }
        }
    }

    public void InjectSaveData(List<Pal> pals)
    {
        if(pals != null)
        {
            Progress.Pals.VisitedPals = pals;
        }
    }

    private void SpawnPal()
    {
        List<Pal> possibleCatPals = new List<Pal>();

        foreach (var catPal in CatInfo.Pals.Data)
        {
            Pal pal = catPal.Key;
            CatPalData catData = catPal.Value;

            if (catData.FavouriteCoffee == coffeeMachineService.BrewedCoffee)
            {
                possibleCatPals.Add(pal);
            }
        }

        if (possibleCatPals.Count > 0)
        {
            Pal pal = possibleCatPals[Game1.Rnd.Next(possibleCatPals.Count)];
            CatPalData palData = CatInfo.Pals.Data[pal];

            entityManager.AddEntity<CatPal>();
            palEntity = entityManager.GetEntity<CatPal>();
            palEntity.LoadData(palData);

            SetPal(pal);
            RegisterPalVisit(pal);
            PalVisit?.Invoke(pal);
        }
    }

    private void UnspawnPal()
    {
        entityManager.RemoveEntity(palEntity);
        coffeeMachineService.CoffeeConsumed();
        int gift = GetVisitationGift();
        inventoryService.UpdateCoins(gift);
        PalLeft?.Invoke((Pal)ActivePal, gift);
        Debug.WriteLine("Oops");
        SetPal(null);
    }

    private int GetVisitationGift()
    {
        int coins = 10;
        return coins; // 10 Coins.
    }
}
