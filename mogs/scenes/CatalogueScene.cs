using Mogs;
using Microsoft.Xna.Framework.Graphics;

public class CatalogueScene : GameScene
{
    public CatalogueScene(Game1 game1) : base(game1) { }

    public override void Load()
    {
        base.Load();

        _UIConfig = new CatalogueUIConfig(Game1.Resources, Game1.ServiceManager);
        Game1.UIManager.SetConfig(_UIConfig);
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
