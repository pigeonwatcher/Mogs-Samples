using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class AccessorySystem : IEntitySystem, IDrawSystem
{
    public bool Active { get; private set; }

    private WearableAccessory[] accessories = new WearableAccessory[2];
    private Cat[] cats = new Cat[2];

    private const int PLAYER_CAT_INDEX = 0;
    private const int CAT_PAL_INDEX = 1;

    public void SetActive(bool isActive)
    {
        Active = isActive;

        if (isActive)
        {
            foreach (var cat in cats)
            {
                if (cat != null)
                    cat.AccessoryChanged += UpdateAccessory;
            }
        }
        else
        {
            foreach (var cat in cats)
            {
                if (cat != null)
                    cat.BeingPet -= UpdateAccessory;
            }
        }
    }

    public void RegisterEntity(Entity entity)
    {
        if (entity is Cat cat)
        {
            int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

            cat.AccessoryChanged += UpdateAccessory;
            cats[index] = cat;
            UpdateAccessory(cat);
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entity is Cat cat)
        {
            int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

            cat.AccessoryChanged -= UpdateAccessory;
            cats[index] = null;
            UpdateAccessory(cat);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!Active) return;

        for (int i = 0; i < accessories.Length; i++)
        {
            if (cats[i] != null && accessories[i] != null && cats[i].Active)
                spriteBatch.Draw(Resources.AccessoryAtlas, accessories[i].Position, accessories[i].SpriteRect, ColorUtils.LightOffWhite, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.ACCESSORY_LAYER);
        }
    }

    private void UpdateAccessory(Cat cat)
    {
        int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

        WearableAccessory accessory = GetAccessory(cat);
        if (accessory != null)
        {
            accessories[index] = accessory;
        }
    }

    private WearableAccessory GetAccessory(Cat cat)
    {
        if (cat.CatData == null) return null;

        Accessory accessory = cat.CatData.Accessory;

        switch (accessory)
        {
            case Accessory.PaperBoat:
                return new PaperBoat(cat);
            case Accessory.Fez:
                return new Fez(cat);
            case Accessory.WitchHat:
                return new WitchHat(cat);
            case Accessory.Headband:
                return new Headband(cat);
            default:
                return null;
        }
    }
}
