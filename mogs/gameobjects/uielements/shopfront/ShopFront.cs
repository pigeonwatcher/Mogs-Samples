using Microsoft.Xna.Framework;

public class ShopFront : UIElement
{
    private readonly ShopService shopService;
    private readonly InventoryService inventoryService;

    private ShopData shopData;
    private Text shopName;
    private Text coinCounter;
    private UIEntity uiEntity;
    private UIEntityHandler uiEntityHandler;
    private InteractiveInventory inventory;

    public ShopFront(ShopService shopService, InventoryService inventoryService)
    {
        this.shopService = shopService;
        this.inventoryService = inventoryService;
    }

    public void SetShop(long shopID)
    {
        shopData = shopService.GetShopData(shopID);
    }

    public override void AddChild<T>(T child)
    {
        if (child is InteractiveInventory _inventory)
        {
            inventory = _inventory;
            inventory.Config(
            (item, input) => { },
            (item, input) =>
            {
                if (shopService.PurchaseItem(shopData.ID, item.Index))
                {
                    inventory.Refresh(shopData.Inventory);
                }
            },
            3,
            9);
        }
        else if (child is Text _shopName && child.Name == "shopname")
        {
            shopName = _shopName;
        }
        else if (child is Text _coinCounter && child.Name == "coincounter")
        {
            coinCounter = _coinCounter;
        }
        else if (child is UIEntityHandler _uiElementHandler)
        {
            uiEntityHandler = _uiElementHandler;
        }
        else if (child is UIEntity _uiEntity)
        {
            uiEntity = _uiEntity;
        }

        base.AddChild(child);
    }

    public override void Activate()
    {
        base.Activate();
        inventoryService.CoinsUpdated += UpdateCoinsDisplay;

        shopName.SetText($"Shop {shopData.ID}", Alignment.Center, Alignment.Top, 0, 1);
        UpdateCoinsDisplay();

        uiEntity.SetAtlas(Resources.EntityAtlas);
        uiEntity.SetSpriteRect(Sprite.CatData[shopData.Assistant.Coat]);
        Vector2 position = Grid.Position(Anchor.Center, Pivot.Center, uiEntity.Entity.SpriteRect);
        position.Y -= 26;
        uiEntity.SetPos(position);
        CatPalette palette = CatInfo.CatColorToColor[shopData.Assistant.Color];
        uiEntity.SetPalette(palette.PrimaryColor, palette.SecondaryColor);
        uiEntity.SetActive(true);
        uiEntityHandler.AddUIEntity(uiEntity);
        uiEntityHandler.SendToRender(uiEntity);

        inventory.Refresh(shopData.Inventory);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        inventoryService.CoinsUpdated -= UpdateCoinsDisplay;

        uiEntityHandler.ClearUIEntities();
    }

    private void UpdateCoinsDisplay()
    {
        int coins = inventoryService.Coins;
        coinCounter.SetText($"Coins: {coins}", Alignment.Right, Alignment.Bottom, -2, -2);
    }
}
