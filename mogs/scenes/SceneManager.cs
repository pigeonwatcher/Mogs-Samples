using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

public class SceneManager : IService
{
    public delegate void ModifyScene(GameScene scene);

    public GameScene ActiveScene { get; private set; }

    private GameScene nextScene;
    private List<GameScene> scenes = new List<GameScene>();

    private SceneFade fade;

    public void SetFadeScreen(GraphicsDevice graphics)
    {
        fade = new SceneFade(graphics);
    }

    public void AddScene<T>(T scene) where T : GameScene
    {
        scenes.Add(scene);
    }

    public void ChangeScene<T>(ModifyScene modifyScene = null) where T : GameScene
    {
        foreach (GameScene scene in scenes)
        {
            if (scene is T)
            {
                modifyScene?.Invoke(scene);

                nextScene = scene;
                StartSceneTransition();
                return;
            }
        }

        Debug.WriteLine("Scene Not Found!");
    }

    public void Initialise()
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes[i].Initialise();
        }
    }

    public void Update(GameTime gameTime)
    {
        ActiveScene?.Update(gameTime);
        fade.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        ActiveScene?.Draw(spriteBatch);
        fade.Draw(spriteBatch);
    }

    private void StartSceneTransition()
    {
        if (ActiveScene != null)
        {
            fade.TransitionCompleted = LoadNextScene;
            fade.StartTransitionOut();
            ActiveScene.SetEnabled(false);
        }
        else
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (ActiveScene != null)
        {
            Debug.WriteLine("UNLOADING SCENE");
            ActiveScene.Unload();
        }

        ActiveScene = nextScene;
        nextScene = null;
        ActiveScene.Load();
        fade.TransitionCompleted = OnTransitionCompleted;
        fade.StartTransitionIn();
    }

    private void OnTransitionCompleted()
    {
        ActiveScene.SetEnabled(true);
        fade.TransitionCompleted = null;
    }
}