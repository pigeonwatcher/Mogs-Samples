using ECS;
using Microsoft.Xna.Framework.Graphics;

public class MainMenuScene : GameScene
{
    public MainMenuScene(Game1 game1) : base(game1) { }

    private UIManagementService uiManagementService;

    public override void Initialise()
    {
        uiManagementService = Game1.ServiceManager.GetService<UIManagementService>();
    }

    public override void Load()
    {
        base.Load();

        _UIConfig = new MainMenuUIConfig(Game1.Resources, Game1.ServiceManager);
        uiManagementService.SetConfig(_UIConfig);
        uiManagementService.ActivateAll();
    }

    public override void Unload()
    {
        base.Unload();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {

    }
}
