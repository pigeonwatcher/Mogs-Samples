using Microsoft.Xna.Framework;
using System;

public static class EntityFactory
{
    /// <summary>
    /// Factory for creating the PlayerCat and CatPal entities.
    /// </summary>

    public static T CreateEntity<T>() where T : Entity
    {
        Type type = typeof(T);

        if(type == typeof(PlayerCat))
        {
            return (T)(Entity)CreatePlayerCat();
        }
        else if(type == typeof(CatPal)) 
        {
            return (T)(Entity)CreateCatPal();
        }

        return default(T);
    }

    private static PlayerCat CreatePlayerCat() 
    {
        PlayerCat playerCat = new PlayerCat();
        Rectangle spriteRect = Sprite.CatData[CatCoat.Solid];
        Vector2 position = Grid.Position(Anchor.Center, Pivot.Center, spriteRect);
        position.X += 2;
        playerCat.SetAtlas(Resources.EntityAtlas);
        playerCat.SetPos(position);
        playerCat.SetActive(true);
        playerCat.SetEnabled(true);

        return playerCat;
    }

    private static CatPal CreateCatPal()
    {
        CatPal pal = new CatPal();
        pal.LoadData(CatInfo.Pals.Data[Pal.Oliver]);
        Vector2 position = Grid.Position(Anchor.Center, Pivot.Center, pal.SpriteRect);
        position.X -= 20;
        pal.SetAtlas(Resources.EntityAtlas);
        pal.SetPos(position);
        pal.SetActive(true);
        pal.SetEnabled(true);

        return pal;
    }
}
