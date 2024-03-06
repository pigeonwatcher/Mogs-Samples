using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CafeInfomationPanel : UIElement
{
    private readonly UIManagementService uiService;

    private Text name;
    private Text totalTime;
    private Text reputation;
    private UIEntityHandler assistantHandler;

    public CafeInfomationPanel(UIManagementService uiService)
    {
        this.uiService = uiService;
    }

    public void SetContent(CafeData cafeData, NodeFeature nodeFeature)
    {
        name?.SetText(nodeFeature.Name, Alignment.Center, Alignment.Top, 0, 3);
        totalTime?.SetText(cafeData.TimeSpan.ToString(@"hh\:mm\:ss"), Alignment.Center, Alignment.Center, 0, 0);
        reputation?.SetText("", Alignment.Center, Alignment.Top, 0, 0);

        UIEntity assistant = new UIEntity();
        assistantHandler.AddUIEntity(assistant);
        assistant.SetActive(true);
        CatData assistantData = cafeData.Assistant;
        Rectangle sprite = Sprite.CatData[assistantData.Coat];
        CatPalette palette = CatInfo.CatColorToColor[assistantData.Color];
        Vector2 catPos = Grid.Position(Anchor.Center, Pivot.Center, sprite);
        catPos.Y += -20;
        assistant.SetSpriteRect(sprite);
        assistant.SetPalette(palette.PrimaryColor, palette.SecondaryColor);
        assistant.SetPos(catPos);
        assistantHandler.SendToRender(assistant);
    }

    public override void AddChild<T>(T child)
    {
        if (child is Button cancelButton && cancelButton.Name == "cancelbutton")
        {
            cancelButton.SetClickEvent(Deactivate);
        }
        else if (child is Text nameText && nameText.Name == "nametext")
        {
            name = nameText;
        }
        else if (child is Text totalTimeText && totalTimeText.Name == "totaltimetext")
        {
            totalTime = totalTimeText;
        }
        else if (child is Text reputationText && reputationText.Name == "reputationtext")
        {
            reputation = reputationText;
        }
        else if (child is UIEntityHandler uiEntityHandler)
        {
            assistantHandler = uiEntityHandler;
        }

        base.AddChild(child);
    }

    public override void Activate()
    {
        base.Activate();

        uiService.SetModal(this, true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        assistantHandler.ClearUIEntities();
        uiService.SetModal(this, false);
    }
}