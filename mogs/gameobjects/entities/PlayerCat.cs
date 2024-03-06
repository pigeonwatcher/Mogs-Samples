using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

public class PlayerCat : Cat
{
    public PlayerCatData Data => (PlayerCatData)CatData;
    public Action HungerUpdated { get; set; }
    public Action HappinessUpdated { get; set; }
    public Action SaveData { get; set; }

    public PlayerCat()
    {
        SetLayer(Layer.PLAY_LAYER);
    }

    public void SetData(PlayerCatData data)
    {
        CatData = data;

        SetAccessory(data.Accessory);
        SetSpriteRect(Sprite.CatData[data.Coat]);
        CatPalette palette = CatInfo.CatColorToColor[data.Color];
        Palette[0] = palette.PrimaryColor.ToVector4();
        Palette[1] = palette.SecondaryColor.ToVector4();
    }

    public bool Feed(ItemID item)
    {
        if (Data.Hunger > 0)
        {
            Data.Hunger--;
            HungerUpdated?.Invoke();
            SaveData?.Invoke();
            return true;
        }

        return false;
    }

    public void Hungry()
    {
        Debug.WriteLine("Hunger Increased");
        if (Data.Hunger < PlayerCatData.MaxHunger)
        {
            Data.Hunger++;
            HungerUpdated?.Invoke();
            SaveData?.Invoke();
        }
    }

    public void SetAccessory(Accessory accessory)
    {
        Data.SetAccessory(accessory);

        AccessoryChanged?.Invoke(this);
        SaveData?.Invoke();
    }

    public override void Pet()
    {
        if (Data.Happiness < PlayerCatData.MaxHappiness)
        {
            Data.Happiness++;
            HappinessUpdated?.Invoke();
            SaveData?.Invoke();
        }

        base.Pet();
    }

    public override void SetSpriteRect(Rectangle spriteRect)
    {
        base.SetSpriteRect(spriteRect);
        Bounds = BoundsUtilities.Calculate(Position, SpriteRect);
    }

    public override void SetPos(Vector2 position)
    {
        base.SetPos(position);
        Bounds = BoundsUtilities.Calculate(Position, SpriteRect);
    }
}
