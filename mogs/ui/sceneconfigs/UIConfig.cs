using System.Collections.Generic;

public class UIConfig
{
    protected readonly Resources Resources;
    protected readonly ServiceManager ServiceManager;

    public List<UIElement> UIElements { get; private set; } = new List<UIElement>();

    public UIConfig(Resources resources, ServiceManager serviceManager) 
    { 
        Resources = resources;
        ServiceManager = serviceManager;
    }
}