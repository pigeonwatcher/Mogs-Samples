using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class CatalogueUIConfig : UIConfig
{
    public CatalogueUIConfig(Resources resources, ServiceManager serviceManager) : base(resources, serviceManager) 
    {
        var sceneService = ServiceManager.GetService<SceneManagementService>();

        UIElement titleTxt = CreateTitleText();
        UIElement palProgress = CreatePalProgressScreen();

        Button returnButton = (Button)CreateReturnButton();
        returnButton.SetClickEvent(() => { sceneService.ChangeScene<HubMenuScene>(); });

        UIElements.Add(titleTxt);
        UIElements.Add(palProgress);
        UIElements.Add(returnButton);
    }

    private UIElement CreateTitleText()
    {
        Text titleTxt = UIFactory.CreateTextType2();
        titleTxt.SetName("titletext");
        titleTxt.SetText("Catalogue", Alignment.Center, Alignment.Top, 0, 1);

        return titleTxt;
    }

    private PalProgress CreatePalProgressScreen()
    {
        PalProgress palProgress = new PalProgress();

        float cardX = 5.7f;
        float cardY = 8f;
        Rectangle spriteRect = Sprite.UIData[UI.PanelTypeOne];
        int cardWidth = PanelUtilities.GetLengthForUnit(cardX, spriteRect);
        int cardHeight = PanelUtilities.GetLengthForUnit(cardY, spriteRect);
        int width = Grid.ScreenWidth;
        int height = cardHeight * 2;
        Vector2 pos = Grid.Position(Anchor.Center, width, height);
        Rectangle box = new Rectangle((int)pos.X, (int)pos.Y, width, height);

        SlideBox slideBox = UIFactory.CreateSlideBox();
        int itemAmount = CatInfo.Pals.Data.Count - 1;
        int itemsPerRow = 2;
        int itemsPerSlide = 4;
        palProgress.AddChild(slideBox);

        var entityService = ServiceManager.GetService<EntityManager>();
        UIEntityHandler uiEntityHandler = UIFactory.CreateUIEntityHandler(entityService);

        Text slideText = UIFactory.CreateTextType2();
        slideBox.AddChild(slideText);

        SlideBox.UpdateItem updateItem = UpdateItem(uiEntityHandler);
        SlideBox.DisposeItems disposeItems = DisposeItems(uiEntityHandler);
        slideBox.Setup(box, itemAmount, itemsPerRow, itemsPerSlide, updateItem, disposeItems);

        slideBox.AddChild(uiEntityHandler);

        for (int i = 0; i < itemsPerSlide * 3; i++)
        {
            InteractiveElement card = UIFactory.CreateInteractiveElement();
            card.Setup(cardX, cardY);
            card.SetPos(Vector2.Zero);
            slideBox.AddChild(card);
            card.SetLayer(card.LayerDepth + Layer.UI_CHILDOFFSET);

            Text catNameTxt = UIFactory.CreateTextType1();
            catNameTxt.SetName("catname");
            catNameTxt.SetParent(card);
            card.AddChild(catNameTxt);

            Text numberTxt = UIFactory.CreateTextType1();
            numberTxt.SetName("number");
            numberTxt.SetParent(card);
            card.AddChild(numberTxt);

            UIEntity uiEntity = UIFactory.CreateUIEntity();
            card.AddChild(uiEntity);
            uiEntityHandler.AddUIEntity(uiEntity);
        }

        return palProgress;
    }

    private UIElement CreateReturnButton()
    {
        Button returnButton = UIFactory.CreateReturnButton();
        returnButton.SetName("returnbutton");
        Vector2 buttonPos = Grid.Position(Anchor.BottomLeft, Pivot.BottomLeft, returnButton.SpriteRect);
        buttonPos.X += 1;
        buttonPos.Y += -1;
        returnButton.SetPos(buttonPos);

        return returnButton;
    }

    private SlideBox.UpdateItem UpdateItem(UIEntityHandler uiEntityHandler)
    {
        SlideBox.UpdateItem updateItem = (items, index, actualIndex) =>
        {
            int yMargin = 3;
            int yMarginBottom = -2;

            Text catNameTxt = items[index].GetChild<Text>("catname");
            Text numberTxt = items[index].GetChild<Text>("number");
            UIEntity uiEntity = uiEntityHandler.UIEntities[index];

            var cat = CatInfo.Pals.Data.ElementAtOrDefault(actualIndex);
            Pal catPal = cat.Key;
            CatPalData catPalData = cat.Value;

            bool visited = Progress.Pals.VisitedPals.Contains(catPal);
            string catName = cat.Value == null || visited == false ? "???" : cat.Value.Name;
            catPalData = visited == false ? new CatPalData("", CatCoat.Unknown, CatColor.White, ItemID.CoffeeBagEarthy) : catPalData;

            if (catPalData != null)
            {
                Rectangle sprite = Sprite.CatData[catPalData.Coat];
                CatPalette palette = CatInfo.CatColorToColor[catPalData.Color];
                Vector2 catPos = Grid.Position(Anchor.Center, Pivot.Center, sprite, items[index].RectTransform);
                uiEntity.SetSpriteRect(sprite);
                uiEntity.SetPalette(palette.PrimaryColor, palette.SecondaryColor);
                uiEntity.SetPos(catPos);

                uiEntityHandler.SendToRender(uiEntity);
            }

            catNameTxt.SetText($"{catName}", Alignment.Center, Alignment.Top, 0, yMargin);
            numberTxt.SetText($"{actualIndex + 1}", Alignment.Center, Alignment.Bottom, 0, yMarginBottom);
        };

        return updateItem;
    }

    private SlideBox.DisposeItems DisposeItems(UIEntityHandler uiEntityHandler)
    {
        SlideBox.DisposeItems disposeItems = () =>
        {
            uiEntityHandler.ClearUIEntities();
        };

        return disposeItems;
    }
}
