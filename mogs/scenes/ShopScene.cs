using Mogs;
using Microsoft.Xna.Framework.Graphics;

public class ShopScene : GameScene
{
    public ShopScene(Game1 game1) : base(game1) { }

    public long ShopID { get; set; }

    public override void Initialise()
    {
        RegisterSystem<EntityRenderSystem>();
    }

    public override void Load()
    {
        base.Load();

        _UIConfig = new ShopUIConfig(Game1.Resources, Game1.ServiceManager);
        Game1.UIManager.SetConfig(_UIConfig);
        Game1.UIManager.GetUIElement<ShopFront>().SetShop(ShopID);
        Game1.UIManager.ActivateAll();

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
