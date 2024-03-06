using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class PopupPanel : UIElement
{
    private readonly UIManagementService uiService;

    public delegate void PopupConfig(PopupPanel popupPanel);

    private Text text;
    private PopupConfig popupConfig;

    public PopupPanel(UIManagementService uiService)
    {
        this.uiService = uiService;

        SetLayer(Layer.UI_POPUP);
    }

    public void SetConfig(PopupConfig popupConfig)
    {
        popupConfig(this);
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

    public void SetContent(string _string, Alignment horizontalAlignment = Alignment.Center, Alignment verticalAlignment = Alignment.Top, int xMargin = 0, int yMargin = 0)
    {
        text.SetText(_string, horizontalAlignment, verticalAlignment, xMargin, yMargin);
    }
}
