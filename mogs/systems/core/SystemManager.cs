using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SystemManager
{    
    private List<ISystem> systems = new List<ISystem>();
    private List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();
    private List<IDrawSystem> drawSystems = new List<IDrawSystem>();
    private List<ITouchSystem> touchSystems = new List<ITouchSystem>();

    public void AddSystem(ISystem system)
    {
        if(!systems.Contains(system))
        {
            systems.Add(system);

            if(system is IUpdateSystem updateSystem)
            {
                updateSystems.Add(updateSystem);
            }
            if(system is IDrawSystem drawSystem) 
            { 
                drawSystems.Add(drawSystem);
            }
            if(system is ITouchSystem touchSystem)
            {
                touchSystems.Add(touchSystem);
            }
        }
    }

    public void RemoveSystem(ISystem system) 
    {
        if (systems.Contains(system))
        {
            systems.Remove(system);

            if (system is IUpdateSystem updateSystem)
            {
                if(updateSystems.Contains(updateSystem))
                {
                    updateSystems.Remove(updateSystem);
                }
            }
            if (system is IDrawSystem drawSystem)
            {
                if (drawSystems.Contains(drawSystem))
                {
                    drawSystems.Remove(drawSystem);
                }
            }
        }
    }

    public T GetSystem<T>() where T : ISystem
    {
        foreach (var system in systems)
        {
            if (system is T)
            {
                return (T)system;
            }
        }
        return default(T);
    }

    public void ToggleSystem<T>(bool isActive) where T : ISystem
    {
        GetSystem<T>()?.SetActive(isActive);
    }

    public void Update(GameTime gameTime)
    {
        for(int i = 0; i < updateSystems.Count; i++)
        {
            updateSystems[i].Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < drawSystems.Count; i++)
        {
            drawSystems[i].Draw(spriteBatch);
        }
    }

    public void OnTouch(IInputService input)
    {
        for (int i = 0; i < touchSystems.Count; i++)
        {
            touchSystems[i].OnTouch(input);
        }
    }

    public void OnRelease(IInputService input)
    {
        for (int i = 0; i < touchSystems.Count; i++)
        {
            touchSystems[i].OnRelease(input);
        }
    }
}
