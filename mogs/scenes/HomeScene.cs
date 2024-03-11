using Mogs;
using Microsoft.Xna.Framework.Graphics;

public class HomeScene : GameScene
{
    public HomeScene(Game1 game1) : base(game1) { }

    private PalService palService;

    private PlayerCat playerCat;

    public override void Initialise()
    {
        // Get Services.
        palService = Game1.ServiceManager.GetService<PalService>();

        // Register Systems.
        RegisterSystem<HungerSystem>();
        RegisterSystem<CatInteractionSystem>();
        RegisterSystem<PettingAnimationSystem>();
        RegisterSystem<EntityRenderSystem>();
        RegisterSystem<EmoteSystem>();
        RegisterSystem<AccessorySystem>();
    }

    public override void Load()
    {
        base.Load();

        _UIConfig = new HomeUIConfig(Game1.Resources, Game1.ServiceManager);
        Game1.UIManager.SetConfig(_UIConfig);
        Game1.UIManager.ActivateUIElement<Toolbar>();
        Game1.UIManager.ActivateUIElement<StatusMeter>();

        palService.CheckPalVisit();

        playerCat ??= Game1.EntityManager.AddEntity<PlayerCat>();
        playerCat.SetActive(true);
        playerCat.SetEnabled(true);

        _Background ??= new HomeBackground();
    }

    public override void Unload()
    {
        base.Unload();

        Game1.EntityManager.HideAllEntities();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _Background.Draw(spriteBatch);
    }
}
