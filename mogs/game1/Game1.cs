﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ECS
{
    public partial class Game1 : Game
    {
        public static bool IsItemDragging = false;
        public static Random Rnd = new Random();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Resources Resources { get; private set; }
        public SystemManager SystemManager { get; private set; } 
        public ServiceManager ServiceManager { get; private set; }
        public EntityManager EntityManager { get; private set; }


        private NodeFeatures nodeFeatures;
        private WayFeatures wayFeatures;

        private MapRenderSystem mapRenderSystem;
        private MapElementSystem mapElementSystem;
        private EntityRenderSystem entityRenderSystem;
        private HungerSystem hungerSystem;
        private CatInteractionSystem catInteractionSystem;
        private PettingAnimationSystem pettingAnimationSystem;
        private EmoteSystem emoteSystem;
        private AccessorySystem accessoryRenderSystem;

        private SaveGameService saveGameService;
        private IInputService inputService;
        private UIManagementService uiManagementService; 
        private SceneManagementService sceneManagementService; 
        private InventoryService inventoryService;
        private ShopService shopService;
        private CafeService cafeService;
        private CoffeeMachineService coffeeMachineService;
        private PlayerCatService playerCatService;
        private PalService palService;
        private MapDataParser mapDataParser;

        private MainMenuScene mainMenuScene;
        private HomeScene homeScene;
        private ExploreScene exploreScene;
        private MapScene mapScene;
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

            nodeFeatures = new NodeFeatures();
            wayFeatures = new WayFeatures();

            saveGameService = new SaveGameService();
            inputService = new InputService();
            sceneManagementService = new SceneManagementService();
            EntityManager = new EntityManager();
            uiManagementService = new UIManagementService();
            inventoryService = new InventoryService();
            shopService = new ShopService(inventoryService);
            cafeService = new CafeService(inventoryService);
            coffeeMachineService = new CoffeeMachineService(inventoryService);
            playerCatService = new PlayerCatService();
            palService = new PalService(EntityManager, coffeeMachineService, inventoryService);
            mapDataParser = new MapDataParser(nodeFeatures, wayFeatures);

            entityRenderSystem = new EntityRenderSystem();
            hungerSystem = new HungerSystem();
            catInteractionSystem = new CatInteractionSystem();
            pettingAnimationSystem = new PettingAnimationSystem();
            emoteSystem = new EmoteSystem(catInteractionSystem);
            accessoryRenderSystem = new AccessorySystem();
            mapElementSystem = new MapElementSystem(uiManagementService, sceneManagementService);
            mapRenderSystem = new MapRenderSystem(nodeFeatures, wayFeatures, mapElementSystem);

            mainMenuScene = new MainMenuScene(this);
            homeScene = new HomeScene(this);
            exploreScene = new ExploreScene(this);
            mapScene = new MapScene(this);
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
            SystemManager.AddSystem(mapRenderSystem);
            SystemManager.AddSystem(mapElementSystem);

            ServiceManager.AddService(saveGameService);
            ServiceManager.AddService(nodeFeatures);
            ServiceManager.AddService(wayFeatures);
            ServiceManager.AddService(EntityManager);
            ServiceManager.AddService(sceneManagementService);
            ServiceManager.AddService(uiManagementService);
            ServiceManager.AddService(inventoryService);
            ServiceManager.AddService(shopService);
            ServiceManager.AddService(cafeService);
            ServiceManager.AddService(coffeeMachineService);
            ServiceManager.AddService(playerCatService);
            ServiceManager.AddService(palService);
            ServiceManager.AddService(mapDataParser);

            sceneManagementService.AddScene(mainMenuScene);
            sceneManagementService.AddScene(homeScene);
            sceneManagementService.AddScene(exploreScene);
            sceneManagementService.AddScene(mapScene);
            sceneManagementService.AddScene(shopScene);
            sceneManagementService.AddScene(cafeScene);
            sceneManagementService.AddScene(hubMenuScene);
            sceneManagementService.AddScene(cafeProgressScene);
            sceneManagementService.AddScene(shopProgressScene);
            sceneManagementService.AddScene(accessoriesScene);
            sceneManagementService.AddScene(catalogueScene);

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
            sceneManagementService.Initialise();

            base.Initialize();

            entityRenderSystem.Initialise();
            sceneManagementService.SetFadeScreen(GraphicsDevice);
            sceneManagementService.ChangeScene<MainMenuScene>();
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
           GraphicsDevice.Clear(ColorUtils.RiceWhite);

            entityRenderSystem.Render();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Grid.ScaleMatrix);
            ServiceManager.Draw(spriteBatch);
            SystemManager.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
