using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

public class PlayerCatService : IEntityService
{
    /// <summary>
    /// Service Class intended to handle the business logic for interacting with the player cat.
    /// </summary>

    private PlayerCat playerCat;

    public event Action<PlayerCatData> HungerChanged;
    public event Action<PlayerCatData> HappinessChanged;
    public event Action<PlayerCatData> DataChanged;

    private PlayerCatData saveData;

    // Subscribed to the EntityManager. EntityManager informs the PlayerCatService when a PlayerCat entity has been created.
    public void RegisterEntity(Entity entity)
    {
        if (entity is PlayerCat playerCat)
        {
            this.playerCat = playerCat;
            this.playerCat.SetData(saveData);

            this.playerCat.HungerUpdated = () => { HungerChanged?.Invoke(this.playerCat.Data); };
            this.playerCat.HappinessUpdated = () => { HappinessChanged?.Invoke(this.playerCat.Data); };
            this.playerCat.SaveData = () => { DataChanged?.Invoke(this.playerCat.Data); };

            HungerChanged?.Invoke(this.playerCat.Data);
            HappinessChanged?.Invoke(this.playerCat.Data);

            Debug.WriteLine("Player Cat Registered!");
        }
    }

    // Subscribed to the EntityManager. EntityManager informs the PlayerCatService when a PlayerCat entity has been removed.
    public void UnregisterEntity(Entity entity)
    {
        if (entity is PlayerCat)
        {
            playerCat = null;
            Debug.WriteLine("Player Cat Unregistered!");
        }
    }

    public int GetHappiness()
    {
        if (playerCat == null) { return 0; }

        return playerCat.Data.Happiness;
    }

    public int GetHunger()
    {
        if (playerCat == null) { return 0; }

        return playerCat.Data.Hunger;
    }

    public Rectangle GetBounds()
    {
        if (playerCat == null) { return Rectangle.Empty; }

        return playerCat.Bounds;
    }

    public bool Feed(ItemID item)
    {
        if (playerCat == null) { return false; }

        if (playerCat.Feed(item))
        {
            return true;
        }

        return false;
    }

    public Accessory GetAccessory()
    {
        return playerCat.Data.Accessory;
    }

    public void SetAccessory(Accessory accessory)
    {
        playerCat.SetAccessory(accessory);
    }

    public void InjectSaveData(PlayerCatData saveData)
    {
        if (saveData != null)
        {
            this.saveData = saveData;
            Debug.WriteLine("Player Cat Data Loaded!");
        }
        else
        {
            this.saveData = new PlayerCatData("Oliver", CatCoat.Patched, CatColor.WhiteGinger, Accessory.None, 2, 2);
            Debug.WriteLine("Player Cat Data Could Not Be Found! Loaded Default.");
        }
    }
}
