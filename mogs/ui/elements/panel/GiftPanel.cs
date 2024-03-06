using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class GiftPanel : UIElement
{
    private readonly UIManagementService uiService;
    private readonly PalService palService;

    public delegate string PopupConfig(Action<string> setContent, Action activate);

    private string textString;
    private Text text;
    private DisplayItem item;

    public GiftPanel(UIManagementService uiService, PopupConfig panelText, PalService palService)
    {
        this.uiService = uiService;
        this.palService = palService;

        textString = panelText();

        palService.PalVisit = (pal) =>
        {
            SetContent($"{pal} has come to visit!");
            Activate();
        };
        palService.PalLeft = (pal, gift) => 
        {
            SetContent($"{pal} has left behind a thank you gift of {gift} coins!");
            Activate();
        };

        SetLayer(Layer.UI_POPUP);
    }

    public override void AddChild<T>(T child)
    {
        if (child is Button closeButton && closeButton.Name == "closebutton")
        {
            closeButton.SetClickEvent(Deactivate);
        }
        else if (child is Text _text)
        {
            text = _text;
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

        uiService.SetModal(this, false);
    }

    public void SetContent(string _string)
    {
        text.SetText(_string, Alignment.Center, Alignment.Top, 4, 2);
    }
}
