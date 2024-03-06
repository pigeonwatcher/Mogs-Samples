using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class UIFactory
{
    /// <summary>
    /// Factory for creating UIElements.
    /// </summary>

    public static Text CreateTextType1()
    {
        StaticSpriteFont font = Resources.Font1;

        Text text = new Text(font);
        return text;
    }

    public static Text CreateTextType2()
    {
        StaticSpriteFont font = Resources.Font2;

        Text text = new Text(font);
        return text;
    }

    public static Button CreateBasicButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.BasicButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        return button;
    }

    public static Button CreateConfirmButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.ConfirmButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        button.SetName("confirmbutton");
        return button;
    }

    public static Button CreateCancelButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.CancelButton];

        Button button = new Button(atlas, sprite, ColorUtils.CancelPink, Color.LightGray);
        button.SetName("cancelbutton");
        return button;
    }

    public static Button CreateReturnButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.ReturnButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        button.SetName("returnbutton");
        return button;
    }

    public static Button CreateLongButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.LongButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        return button;
    }

    public static Button CreateFoodInvButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.FoodInvButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        button.SetName("foodinvbutton");
        return button;
    }

    public static Button CreateCoffeeInvButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.CoffeeInvButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        button.SetName("coffeeinvbutton");
        return button;
    }

    public static Button CreateMapButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.MapButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        button.SetName("mapbutton");
        return button;
    }

    public static Button CreateHubButton()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.HubButton];

        Button button = new Button(atlas, sprite, ColorUtils.LightOffWhite, Color.LightGray);
        button.SetName("hubbutton");
        return button;
    }

    public static Panel CreatePanelTypeOne()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.PanelTypeOne];

        Panel panel = new Panel(atlas, sprite);
        return panel;
    }

    public static Panel CreatePanelTypeTwo()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.PanelTypeTwo];

        Panel panel = new Panel(atlas, sprite);
        return panel;
    }

    public static Panel CreateBackgroundPanel(float xUnits, float yUnits)
    {
        Panel backgroundPanel = CreatePanelTypeOne();
        backgroundPanel.Setup(xUnits, yUnits);
        Vector2 panelPos = Grid.Position(Anchor.Center, Pivot.Center, backgroundPanel.Width, backgroundPanel.Height);
        backgroundPanel.SetPos(panelPos);
        backgroundPanel.SetName("backgroundpanel");

        return backgroundPanel;
    }

    public static InteractiveElement CreateInteractiveElement()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.PanelTypeTwo];

        InteractiveElement panel = new InteractiveElement(atlas, sprite);
        return panel;
    }

    public static ItemSlot CreateItemSlot()
    {
        Texture2D atlas = Resources.UIAtlas;
        Rectangle sprite = Sprite.UIData[UI.ItemFrame];

        ItemSlot itemSlot = new ItemSlot(atlas, sprite);
        return itemSlot;
    }

    public static DisplayItem CreateDisplayItem()
    {
        Texture2D uiAtlas = Resources.UIAtlas;
        Texture2D itemAtlas = Resources.ItemAtlas;

        DisplayItem displayItem = new DisplayItem(uiAtlas, itemAtlas);
        return displayItem;
    }

    public static InteractiveInventory CreateInteractiveInventory()
    {
        Texture2D uiAtlas = Resources.UIAtlas;
        Texture2D itemAtlas = Resources.ItemAtlas;

        InteractiveInventory inventory = new InteractiveInventory(uiAtlas, itemAtlas);
        return inventory;
    }

    public static InteractiveElement CreateInfomationCard(float x, float y)
    {
        InteractiveElement panel = CreateInteractiveElement();
        panel.Setup(x, y);

        return panel;
    }

    public static SlideBox CreateSlideBox()
    {
        return new SlideBox();
    }

    public static UIEntity CreateUIEntity()
    {
        return new UIEntity();
    }

    public static UIEntityHandler CreateUIEntityHandler(EntityManager entityManager)
    {
        return new UIEntityHandler(entityManager);
    }
}
