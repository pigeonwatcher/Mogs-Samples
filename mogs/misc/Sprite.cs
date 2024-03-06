using Microsoft.Xna.Framework;
using System.Collections.Generic;

public static class Sprite
{
    /// <summary>
    /// Data class for finding the positions of sprites within an atlas / sprite-sheet.
    /// </summary>

    public const int CatWidth = 19;
    public const int CatHeight = 19;
    public readonly static Rectangle CatBounds = new Rectangle(0, 0, CatWidth, CatHeight);

    public const int ItemWidth = 12;
    public const int ItemHeight = 12;

    public static IReadOnlyDictionary<CatCoat, Rectangle> CatData = new Dictionary<CatCoat, Rectangle>
    {
        [CatCoat.Unknown] = CatSprite(0, 0),
        [CatCoat.Solid] = CatSprite(0, 1),
        [CatCoat.Striped] = CatSprite(0, 2),
        [CatCoat.Spotted] = CatSprite(0, 3),
        [CatCoat.Patched] = CatSprite(0, 4),
        [CatCoat.Pointed] = CatSprite(0, 5),
    };

    public static IReadOnlyDictionary<Emoji, Rectangle> EmojiData = new Dictionary<Emoji, Rectangle>
    {
        [Emoji.HappyEmote] = new Rectangle(0, 0, 15, 13),
        [Emoji.AngryEmote] = new Rectangle(0, 13, 15, 13),
        [Emoji.SadEmote] = new Rectangle(0, 26, 15, 13),
        [Emoji.AnnoyedEmote] = new Rectangle(0, 39, 15, 13),
        [Emoji.ThoughtBubbleMain] = new Rectangle(15, 0, 21, 19),
        [Emoji.ThoughtBubbleSmall] = new Rectangle(15, 19, 4, 4),
        [Emoji.ThoughtBubbleMedium] = new Rectangle(19, 19, 7, 6),
        [Emoji.Heart] = new Rectangle(15, 49, 3, 3),
    };

    public static IReadOnlyDictionary<ItemID, Rectangle> ItemData = new Dictionary<ItemID, Rectangle>
    {
        [ItemID.StrawberryCake] = ItemSprite(1, 0),
        [ItemID.ChocolateCake] = ItemSprite(2, 0),
        [ItemID.CheeseCake] = ItemSprite(4, 0),
        [ItemID.PrincessCake] = ItemSprite(4, 0),
        [ItemID.CinnamonBun] = ItemSprite(0, 0),
        [ItemID.CreamBun] = ItemSprite(0, 1),
        [ItemID.Eclair] = ItemSprite(1, 1),
        [ItemID.Chokladbollar] = ItemSprite(2, 1),

        [ItemID.CoffeeBagEarthy] = ItemSprite(0, 2),
        [ItemID.CoffeeBagChocolatey] = ItemSprite(1, 2),
        [ItemID.CoffeeBagNutty] = ItemSprite(2, 2),
        [ItemID.CoffeeBagSpicy] = ItemSprite(3, 2),
        [ItemID.CoffeeBagFruity] = ItemSprite(4, 2),
    };

    public static IReadOnlyDictionary<UI, Rectangle> UIData = new Dictionary<UI, Rectangle>
    {
        [UI.BasicButton] = new Rectangle(36, 14, 14, 14),
        [UI.ConfirmButton] = new Rectangle(36, 28, 14, 14),
        [UI.CancelButton] = new Rectangle(36, 42, 14, 14),
        [UI.ReturnButton] = new Rectangle(36, 56, 14, 14),
        [UI.LongButton] = new Rectangle(0, 0, 56, 14),
        [UI.FoodInvButton] = new Rectangle(50, 14, 16, 16),
        [UI.CoffeeInvButton] = new Rectangle(50, 30, 16, 16),
        [UI.MapButton] = new Rectangle(50, 46, 16, 16),
        [UI.HubButton] = new Rectangle(50, 62, 16, 16),
        [UI.PanelTypeOne] = new Rectangle(18, 14, 18, 18),
        [UI.PanelTypeTwo] = new Rectangle(18, 32, 18, 18),
        [UI.Heart] = new Rectangle(0, 32, 11, 10),
        [UI.Fish] = new Rectangle(0, 42, 10, 10),
        [UI.ItemFrame] = new Rectangle(0, 14, 16, 16),
    };

    public static IReadOnlyDictionary<RenderBackground, Rectangle> BackgroundData = new Dictionary<RenderBackground, Rectangle>
    {
        [RenderBackground.Floor] = new Rectangle(0, 0, 57, 1),
        [RenderBackground.CoffeeMachine] = new Rectangle(0, 1, 14, 22),
    };

    public static IReadOnlyDictionary<Accessory, Rectangle> AccessoryData = new Dictionary<Accessory, Rectangle>
    {
        [Accessory.PaperBoat] = new Rectangle(0, 0, 17, 9),
        [Accessory.Fez] = new Rectangle(17, 3, 6, 6),
        [Accessory.WitchHat] = new Rectangle(0, 9, 17, 10),
        [Accessory.Headband] = new Rectangle(17, 9, 13, 3),
    };

    public static IReadOnlyDictionary<MapItem, Rectangle> MapData = new Dictionary<MapItem, Rectangle>
    {
        [MapItem.CafeIcon] = new Rectangle(0, 0, 12, 12),
        [MapItem.ShopIcon] = new Rectangle(12, 0, 12, 12),
    };

    private static Rectangle CatSprite(int x, int y)
    {
        return new Rectangle(x * CatWidth, y * CatHeight, CatWidth, CatHeight);
    }

    private static Rectangle ItemSprite(int x, int y)
    {
        return new Rectangle(x * ItemWidth, y * ItemHeight, ItemWidth, ItemHeight);
    }
}

public enum MapItem
{
    ShopIcon,
    CafeIcon,
}