using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CoffeeInventory : UIElement
{
    private readonly InventoryService inventoryService;
    private readonly CoffeeMachineService coffeeMachineService;
    private readonly UIManagementService uiService;

    private Button grindButton;
    private ItemSlot itemSlot;
    private InteractiveInventory inventory;
    private Text namePanelText;

    public CoffeeInventory(InventoryService _inventoryService, CoffeeMachineService _coffeeMachineService, UIManagementService _uiService)
    {
        SetLayer(Layer.UI_COFFEEINV);

        inventoryService = _inventoryService;
        coffeeMachineService = _coffeeMachineService;
        uiService = _uiService;
    }

    public override void AddChild<T>(T child)
    {
        if (child is Button closeButton && closeButton.Name == "CloseButton")
        {
            closeButton.SetClickEvent(Deactivate);
        }
        else if (child is Button grindButton && grindButton.Name == "GrindButton")
        {
            this.grindButton = grindButton;
            grindButton.SetClickEvent(Grind);
        }
        else if (child is Panel namePanel && namePanel.Name == "NamePanel")
        {
            namePanelText = namePanel.GetChild<Text>();
        }
        else if (child is InteractiveInventory inventory)
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
        else if(child is ItemSlot itemSlot)
        {
            this.itemSlot = itemSlot;
        }

        base.AddChild(child);
    }

    public override void Activate()
    {
        Refresh();

        base.Activate();

        uiService.DeactivateUIElement<Toolbar>();

        uiService.SetModal(this, true);
        grindButton.SetEnabled(false);

        inventoryService.InventoryUpdated += Refresh;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        uiService.ActivateUIElement<Toolbar>();

        uiService.SetModal(this, false);
        itemSlot.SetItem(null);
        namePanelText.SetText(null, Alignment.Center, Alignment.Center);

        inventoryService.InventoryUpdated -= Refresh;
    }

    public override void OnRelease(IInputService inputSystem)
    {
        if(inventory.ActiveItem != null && itemSlot.Bounds.Contains(inputSystem.TouchPosition)) 
        {
            itemSlot.SetItem(inventory.ActiveItem);
            namePanelText.SetText(ItemData.CoffeeItems[inventory.ActiveItem.Item].Description, Alignment.Center, Alignment.Center);
            grindButton.SetInactive(false);
            inventory.SetActiveItem(null);
        }
        if (inventory.ActiveItem != null && itemSlot.Item != null && !itemSlot.Bounds.Contains(inputSystem.TouchPosition))
        {
            itemSlot.SetItem(null);
            namePanelText.SetText(null, Alignment.Center, Alignment.Center);
            grindButton.SetInactive(true);
        }

        base.OnRelease(inputSystem);
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        grindButton.SetInactive(true);
    }

    private void Refresh()
    {
        IEnumerable<ItemID> items = inventoryService.CoffeeInventory;
        inventory.Refresh(items);
    }

    private void Grind()
    {
        if(itemSlot.Item != null)
        {
            coffeeMachineService.GrindCoffee(itemSlot.Item.Index);
            Debug.WriteLine("Coffee Grinded!");
        }
    }
}
