using Mogs;
using Microsoft.Xna.Framework.Graphics;

public class MainMenuScene : GameScene
{
    public MainMenuScene(Game1 game1) : base(game1) { }

    public override void Load()
    {
        base.Load();

        _UIConfig = new MainMenuUIConfig(Game1.Resources, Game1.ServiceManager);
        Game1.UIManager.SetConfig(_UIConfig);
        Game1.UIManager.ActivateAll();
    }

    public override void Unload()
    {
        base.Unload();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {

    }
}
