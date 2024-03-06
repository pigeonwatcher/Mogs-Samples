using ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public abstract class GameScene
{
    protected readonly Game1 Game1;

    public bool IsEnabled { get; private set; }

    private List<ISystem> systems = new List<ISystem>();

    protected UIConfig _UIConfig;
    protected Background _Background;

    public GameScene(Game1 game1)
    {
        Game1 = game1;
    }

    public void SetEnabled(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    public virtual void Initialise() { }

    protected ISystem RegisterSystem<T>() where T : ISystem
    {
        ISystem system = Game1.SystemManager.GetSystem<T>();

        if (system != null)
        {
            systems.Add(system);
        }

        return system;
    }

    public virtual void Load()
    {
        foreach (ISystem system in systems)
        {
            system.SetActive(true);
        }
    }

    public virtual void Unload()
    {
        foreach (ISystem system in systems)
        {
            system.SetActive(false);
        }
    }

    public virtual void Update(GameTime gameTime) { }

    public abstract void Draw(SpriteBatch spriteBatch);
}
