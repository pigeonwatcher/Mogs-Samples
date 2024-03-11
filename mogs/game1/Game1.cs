using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mogs
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Resources Resources { get; private set; }
        public SystemManager SystemManager { get; private set; } 
        public ServiceManager ServiceManager { get; private set; }
        public EntityManager EntityManager { get; private set; }
        public SceneManager SceneManager { get; private set; }
        public UIManager UIManager { get; private set; }

        private EntityRenderSystem entityRenderSystem;
        private HungerSystem hungerSystem;
        private CatInteractionSystem catInteractionSystem;
        private PettingAnimationSystem pettingAnimationSystem;
        private EmoteSystem emoteSystem;
        private AccessorySystem accessoryRenderSystem;

        private SaveGameService saveGameService;
        private IInputService inputService;
        private InventoryService inventoryService;
        private ShopService shopService;
        private CafeService cafeService;
        private CoffeeMachineService coffeeMachineService;
        private PlayerCatService playerCatService;
        private PalService palService;

        private MainMenuScene mainMenuScene;
        private HomeScene homeScene;
        private ExploreScene exploreScene;
        private ShopScene shopScene;
        private CafeScene cafeScene;
        private HubMenuScene hubMenuScene;
        private CafeProgressScene cafeProgressScene;
        private ShopProgressScene shopProgressScene;
        private AccessoriesScene accessoriesScene;
        private CatalogueScene catalogueScene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Resources = new Resources(graphics, Content);
            SystemManager = new SystemManager();
            ServiceManager = new ServiceManager();
            SceneManager = new SceneManager();
            EntityManager = new EntityManager();
            UIManager = new UIManager();

            saveGameService = new SaveGameService();
            inputService = new InputService();
            inventoryService = new InventoryService();
            shopService = new ShopService(inventoryService);
            cafeService = new CafeService(inventoryService);
            coffeeMachineService = new CoffeeMachineService(inventoryService);
            playerCatService = new PlayerCatService();
            palService = new PalService(EntityManager, coffeeMachineService, inventoryService);

            entityRenderSystem = new EntityRenderSystem();
            hungerSystem = new HungerSystem();
            catInteractionSystem = new CatInteractionSystem();
            pettingAnimationSystem = new PettingAnimationSystem();
            emoteSystem = new EmoteSystem(catInteractionSystem);
            accessoryRenderSystem = new AccessorySystem();

            mainMenuScene = new MainMenuScene(this);
            homeScene = new HomeScene(this);
            exploreScene = new ExploreScene(this);
            shopScene = new ShopScene(this);
            cafeScene = new CafeScene(this);
            hubMenuScene = new HubMenuScene(this);
            cafeProgressScene = new CafeProgressScene(this); 
            shopProgressScene = new ShopProgressScene(this);
            accessoriesScene = new AccessoriesScene(this);
            catalogueScene = new CatalogueScene(this);

            SystemManager.AddSystem(entityRenderSystem);
            SystemManager.AddSystem(hungerSystem);
            SystemManager.AddSystem(catInteractionSystem);
            SystemManager.AddSystem(pettingAnimationSystem);
            SystemManager.AddSystem(emoteSystem);
            SystemManager.AddSystem(accessoryRenderSystem);

            ServiceManager.AddService(saveGameService);
            ServiceManager.AddService(EntityManager);
            ServiceManager.AddService(SceneManager);
            ServiceManager.AddService(UIManager);
            ServiceManager.AddService(inventoryService);
            ServiceManager.AddService(shopService);
            ServiceManager.AddService(cafeService);
            ServiceManager.AddService(coffeeMachineService);
            ServiceManager.AddService(playerCatService);
            ServiceManager.AddService(palService);

            SceneManager.AddScene(mainMenuScene);
            SceneManager.AddScene(homeScene);
            SceneManager.AddScene(exploreScene);
            SceneManager.AddScene(shopScene);
            SceneManager.AddScene(cafeScene);
            SceneManager.AddScene(hubMenuScene);
            SceneManager.AddScene(cafeProgressScene);
            SceneManager.AddScene(shopProgressScene);
            SceneManager.AddScene(accessoriesScene);
            SceneManager.AddScene(catalogueScene);

            EntityManager.RegisterService<PlayerCat>(playerCatService);
            EntityManager.RegisterSystem<PlayerCat>(emoteSystem);
            EntityManager.RegisterSystem<CatPal>(emoteSystem);
            EntityManager.RegisterSystem<PlayerCat>(accessoryRenderSystem);
            EntityManager.RegisterSystem<PlayerCat>(entityRenderSystem);
            EntityManager.RegisterSystem<Entity>(entityRenderSystem);
            EntityManager.RegisterSystem<PlayerCat>(hungerSystem);
            EntityManager.RegisterSystem<PlayerCat>(catInteractionSystem);
            EntityManager.RegisterSystem<PlayerCat>(pettingAnimationSystem);
            EntityManager.RegisterSystem<CatPal>(entityRenderSystem);
            EntityManager.RegisterSystem<CatPal>(catInteractionSystem);
            EntityManager.RegisterSystem<CatPal>(pettingAnimationSystem);
        }

        protected override void Initialize()
        {
            SetPlatformSettings();

            Resources.Initialise();
            saveGameService.Initialise(ServiceManager);
            inputService.OnTouch += ServiceManager.OnTouch;
            inputService.OnRelease += ServiceManager.OnRelease;
            inputService.OnTouch += SystemManager.OnTouch;
            inputService.OnRelease += SystemManager.OnRelease;
            SceneManager.Initialise();

            base.Initialize();

            entityRenderSystem.Initialise();
            SceneManager.SetFadeScreen(GraphicsDevice);
            SceneManager.ChangeScene<MainMenuScene>();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Resources.LoadGeneralContent();            
        }

        protected override void Update(GameTime gameTime)
        {
            inputService.Update(); 
            ServiceManager.Update(gameTime);
            SystemManager.Update(gameTime);
            SceneManager.Update(gameTime);
            UIManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
           GraphicsDevice.Clear(ColorUtils.RiceWhite);

            entityRenderSystem.Render();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Grid.ScaleMatrix);
            ServiceManager.Draw(spriteBatch);
            SystemManager.Draw(spriteBatch);
            SceneManager.Draw(spriteBatch);
            UIManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
