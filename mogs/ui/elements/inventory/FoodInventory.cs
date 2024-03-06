using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FoodInventory : UIElement
{
    private readonly PlayerCatService playerCatService;
    private readonly InventoryService inventoryService;
    private readonly UIManagementService uiService;

    private InteractiveInventory inventory;

    public FoodInventory(PlayerCatService _playerCatService, InventoryService _inventoryService, UIManagementService _uiService)
    {
        SetLayer(Layer.UI_FOODINV);

        playerCatService = _playerCatService;
        inventoryService = _inventoryService;
        uiService = _uiService;
    }

    public override void AddChild<T>(T child)
    {
        if (child is Button closeButton && closeButton.Name == "CloseButton")
        {
            closeButton.SetClickEvent(Deactivate);
        }
        else if(child is InteractiveInventory inventory) 
        { 
            this.inventory = inventory;
            this.inventory.Config((item, input) =>
            {
                item.Move(input.TouchPosition.ToVector2());
            },
            null,
            3,
            6);
        }

        base.AddChild(child);
    }

    public override void Activate()
    {
        Refresh();

        base.Activate();

        uiService.DeactivateUIElement<Toolbar>();

        inventoryService.InventoryUpdated += Refresh;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        uiService.ActivateUIElement<Toolbar>();

        inventoryService.InventoryUpdated -= Refresh;
    }

    public override void OnRelease(IInputService inputSystem)
    {
        if(inventory.ActiveItem != null)
        {
            if (playerCatService.GetBounds().Contains(inputSystem.TouchPosition))
            {
                if(playerCatService.Feed(inventory.ActiveItem.Item))
                {
                    inventoryService.RemoveItem(InventoryType.Food, inventory.ActiveItem.Index);
                }
            }
        }

        base.OnRelease(inputSystem);
    }

    private void Refresh()
    {
        IEnumerable<ItemID> items = inventoryService.FoodInventory;
        inventory.Refresh(items);
    }
}
