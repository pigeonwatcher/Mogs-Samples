using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HomeUIConfig : UIConfig
{
    public HomeUIConfig(Resources resources, ServiceManager serviceManager) : base(resources, serviceManager)
    {
        UIElement coffeeMachine = CreateCoffeeMachineUI();
        UIElement foodInventory = CreateFoodInventoryUI();
        UIElement statusMeter = CreateStatusMeter();
        Toolbar toolbar = (Toolbar)CreateToolbar();
        UIElement giftPanel = CreateGiftPanel();

        SceneManagementService sceneService = ServiceManager.GetService<SceneManagementService>();
        toolbar.Setup(foodInventory.Activate, 
                      coffeeMachine.Activate,
                      () => sceneService.ChangeScene<ExploreScene>(), 
                      () => sceneService.ChangeScene<HubMenuScene>());

        UIElements.Add(foodInventory);
        UIElements.Add(coffeeMachine);
        UIElements.Add(statusMeter);
        UIElements.Add(toolbar);
        UIElements.Add(giftPanel);
    }

    private UIElement CreateFoodInventoryUI()
    {
        PlayerCatService playerCatService = ServiceManager.GetService<PlayerCatService>();
        InventoryService inventoryService = ServiceManager.GetService<InventoryService>();
        UIManagementService uiService = ServiceManager.GetService<UIManagementService>();
        FoodInventory foodInventory = new FoodInventory(playerCatService, inventoryService, uiService);

        float backgroundPanelX = 12f;
        float backgroundPanelY = 10f;
        Panel backgroundPanel = UIFactory.CreatePanelTypeOne();
        backgroundPanel.Setup(backgroundPanelX, backgroundPanelY + 1f);
        Vector2 backgroundPanelPos = Grid.Position(Anchor.BottomCenter, Pivot.BottomCenter, backgroundPanel.Width, backgroundPanel.Height - backgroundPanel.UnitSize);
        backgroundPanel.SetPos(backgroundPanelPos);
        foodInventory.AddChild(backgroundPanel);

        Button closeButton = UIFactory.CreateCancelButton();
        Vector2 buttonPos = Grid.Position(Anchor.BottomCenter, Pivot.BottomCenter, closeButton.SpriteRect, backgroundPanel.RectTransform);
        buttonPos = new Vector2(buttonPos.X, buttonPos.Y - backgroundPanel.UnitSize - 2);
        closeButton.SetName("CloseButton");
        closeButton.SetPos(buttonPos);
        foodInventory.AddChild(closeButton);
        closeButton.AdjustLayer(Layer.UI_CHILDOFFSET);

        InteractiveInventory inventory = UIFactory.CreateInteractiveInventory();
        inventory.SetParent(backgroundPanel.RectTransform);
        foodInventory.AddChild(inventory);

        Vector2 startPos = new Vector2(foodInventory.Position.X, backgroundPanel.RectTransform.Height);
        Slide slide = new Slide(foodInventory, 100, startPos);
        foodInventory.SetTransition(slide);

        return foodInventory;
    }

    private UIElement CreateCoffeeMachineUI()
    {
        InventoryService inventoryService = ServiceManager.GetService<InventoryService>();
        CoffeeMachineService coffeeMachineService = ServiceManager.GetService<CoffeeMachineService>();
        UIManagementService uiService = ServiceManager.GetService<UIManagementService>();
        CoffeeInventory coffeeInventory = new CoffeeInventory(inventoryService, coffeeMachineService, uiService);

        float backgroundPanelX = 12f;
        float backgroundPanelY = 10f;
        Panel backgroundPanel = UIFactory.CreatePanelTypeOne();
        backgroundPanel.Setup(backgroundPanelX, backgroundPanelY + 1f);
        Vector2 backgroundPanelPos = Grid.Position(Anchor.BottomCenter, Pivot.BottomCenter, backgroundPanel.Width, backgroundPanel.Height - backgroundPanel.UnitSize);
        backgroundPanel.SetPos(backgroundPanelPos);
        coffeeInventory.AddChild(backgroundPanel);

        float namePanelX = 10f;
        float namePanelY = 2f;
        Panel namePanel = UIFactory.CreatePanelTypeTwo();
        namePanel.Setup(namePanelX, namePanelY);
        namePanel.SetName("NamePanel");
        Vector2 namePanelPos = Grid.Position(Anchor.TopCenter, Pivot.TopCenter, namePanel.Width, namePanel.Height);
        namePanelPos.Y += 1;
        namePanel.SetPos(namePanelPos);
        Text namePanelText = UIFactory.CreateTextType1();
        namePanelText.SetParent(namePanel);
        namePanel.AddChild(namePanelText);
        coffeeInventory.AddChild(namePanel);

        Button closeButton = UIFactory.CreateCancelButton();
        Vector2 closeButtonPos = Grid.Position(Anchor.BottomCenter, Pivot.BottomCenter, closeButton.SpriteRect, backgroundPanel.RectTransform);
        closeButtonPos = new Vector2(closeButtonPos.X, closeButtonPos.Y - backgroundPanel.UnitSize - 2);
        closeButton.SetName("CloseButton");
        closeButton.SetPos(closeButtonPos);
        coffeeInventory.AddChild(closeButton);
        closeButton.AdjustLayer(Layer.UI_CHILDOFFSET);

        Button grindButton = UIFactory.CreateLongButton();
        Vector2 grindButtonPos = Grid.Position(Anchor.Center, Pivot.Center, grindButton.SpriteRect);
        grindButtonPos.Y -= 4;
        grindButton.SetName("GrindButton");
        grindButton.SetPos(grindButtonPos);
        coffeeInventory.AddChild(grindButton);

        Text grindText = UIFactory.CreateTextType1();
        grindText.SetParent(grindButton);
        grindText.SetText("Grind", Alignment.Center, Alignment.Center);
        grindButton.AddChild(grindText);

        InteractiveInventory inventory = UIFactory.CreateInteractiveInventory();
        inventory.SetParent(backgroundPanel.RectTransform);
        coffeeInventory.AddChild(inventory);

        ItemSlot itemSlot = UIFactory.CreateItemSlot();
        Vector2 itemSlotPos = Grid.Position(Anchor.Center, Pivot.Center, itemSlot.Sprite);
        itemSlotPos.Y -= 30;
        itemSlot.SetPos(itemSlotPos);
        coffeeInventory.AddChild(itemSlot);

        Vector2 startPos = new Vector2(coffeeInventory.Position.X, Grid.ScreenHeight);
        Slide slide = new Slide(coffeeInventory, 200, startPos);
        coffeeInventory.SetTransition(slide);

        return coffeeInventory;
    }

    private UIElement CreateStatusMeter()
    {
        PlayerCatService playerCatService = ServiceManager.GetService<PlayerCatService>();
        Texture2D uiAtlas = Resources.UIAtlas;

        StatusMeter statusMeter = new StatusMeter(playerCatService, uiAtlas);
        return statusMeter;
    }

    private UIElement CreateToolbar()
    {
        Toolbar toolbar = new Toolbar();

        Rectangle sprite = Sprite.UIData[UI.FoodInvButton];
        Vector2 bottomLeft = Grid.Position(Anchor.BottomLeft, Pivot.BottomLeft, sprite.Width, sprite.Height);
        int margin = 2;
        int numOfButtons = 4;
        int evenWidthSpacing = Grid.EvenWidthSpacing(sprite.Width, numOfButtons, 0, margin);

        Button[] buttons = new Button[numOfButtons];
        buttons[0] = UIFactory.CreateFoodInvButton();
        buttons[1] = UIFactory.CreateCoffeeInvButton();
        buttons[2] = UIFactory.CreateMapButton();
        buttons[3] = UIFactory.CreateHubButton();

        for (int i = 0; i < numOfButtons; i++)
        {
            Button button = buttons[i];
            button.SetPos(new Vector2(bottomLeft.X + margin + (evenWidthSpacing * i), bottomLeft.Y - margin));

            toolbar.AddChild(button);
        }

        return toolbar;
    }

    private UIElement CreateGiftPanel()
    {
        var uiService = ServiceManager.GetService<UIManagementService>();
        var palService = ServiceManager.GetService<PalService>();

        PopupPanel.PopupConfig popupConfig = (popupPanel) => 
        {
            palService.PalVisit = (pal) =>
            {
                popupPanel.SetContent($"{pal} has come to visit!", Alignment.Center, Alignment.Top, 4, 2);
                popupPanel.Activate();
            };
            palService.PalLeft = (pal, gift) =>
            {
                popupPanel.SetContent($"{pal} has left behind a thank you gift of {gift} coins!");
                popupPanel.Activate();
            };
        }; 

        PopupPanel popupPanel = new PopupPanel(uiService);
        popupPanel.SetConfig(popupConfig);

        Panel backgroundPanel = UIFactory.CreatePanelTypeOne();
        backgroundPanel.SetName("backgroundpanel");
        backgroundPanel.Setup(10f, 8f);
        Vector2 panelPos = Grid.Position(Anchor.Center, Pivot.Center, backgroundPanel.Width, backgroundPanel.Height);
        backgroundPanel.SetPos(panelPos);
        popupPanel.AddChild(backgroundPanel);

        Button closeButton = UIFactory.CreateCancelButton();
        Vector2 closeButtonPos = Grid.Position(Anchor.BottomCenter, Pivot.BottomCenter, closeButton.SpriteRect, backgroundPanel.RectTransform);
        closeButtonPos = new Vector2(closeButtonPos.X, closeButtonPos.Y - 2);
        closeButton.SetName("closebutton");
        closeButton.SetPos(closeButtonPos);
        popupPanel.AddChild(closeButton);
        closeButton.AdjustLayer(Layer.UI_CHILDOFFSET);

        Text mainTxt = UIFactory.CreateTextType1();
        mainTxt.SetParent(backgroundPanel);
        popupPanel.AddChild(mainTxt);
        mainTxt.AdjustLayer(Layer.UI_CHILDOFFSET);

        return popupPanel;
    }
}
