using ECS;
using Microsoft.Xna.Framework.Graphics;

public class ShopScene : GameScene
{
    public ShopScene(Game1 game1) : base(game1) { }

    public long ShopID { get; set; }

    private UIManagementService uiManagementService;

    public override void Initialise()
    {
        uiManagementService = Game1.ServiceManager.GetService<UIManagementService>();
        RegisterSystem<EntityRenderSystem>();
    }

    public override void Load()
    {
        base.Load();

        _UIConfig = new ShopUIConfig(Game1.Resources, Game1.ServiceManager);
        uiManagementService.SetConfig(_UIConfig);
        uiManagementService.GetUIElement<ShopFront>().SetShop(ShopID);
        uiManagementService.ActivateAll();

        _Background ??= new CatBackground();
    }

    public override void Unload()
    {
        base.Unload();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _Background.Draw(spriteBatch);
    }
}
