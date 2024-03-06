using System;
using System.Collections.Generic;

public class Toolbar : UIElement 
{
    private List<Button> buttons = new List<Button>();

    public Toolbar()
    {
        SetLayer(Layer.UI_TOOLBAR);
    }

    public void Setup(Action button1, Action button2, Action button3, Action button4)
    {
        buttons[0].SetClickEvent(button1);
        buttons[1].SetClickEvent(button2);
        buttons[2].SetClickEvent(button3);
        buttons[3].SetClickEvent(button4);
    }

    public override void AddChild<T>(T child) 
    {
        if(child is Button button)
        {
            buttons.Add(button);
        }

        base.AddChild(child);
    }
}
