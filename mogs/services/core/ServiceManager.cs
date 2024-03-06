using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServiceManager
{
    private List<IService> services = new List<IService>();
    private List<IUpdateService> updateServices = new List<IUpdateService>();
    private List<IDrawService> drawServices = new List<IDrawService>();
    private List<ITouchService> touchServices = new List<ITouchService>();

    public void AddService<T>(T service) where T : IService
    {
        if(!services.Contains(service))
        {
            services.Add(service);

            if(service is IUpdateService updateService)
            {
                updateServices.Add(updateService);
            }
            if (service is IDrawService drawService)
            {
                drawServices.Add(drawService);
            }
            if(service is ITouchService touchService)
            {
                touchServices.Add(touchService);
            }
        }
    }

    public T GetService<T>() where T : IService
    {
        foreach (var service in services)
        {
            if (service is T)
            {
                return (T)service;
            }
        }
        return default(T);
    }

    public void Update(GameTime gameTime)
    {
        for(int i = 0; i < updateServices.Count; i++) 
        {
            updateServices[i].Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch) 
    {
        for (int i = 0; i < drawServices.Count; i++)
        {
            drawServices[i].Draw(spriteBatch);
        }
    }

    public void OnTouch(IInputService input)
    {
        for (int i = 0; i < touchServices.Count; i++)
        {
            touchServices[i].OnTouch(input);
        }
    }

    public void OnRelease(IInputService input)
    {
        for (int i = 0; i < touchServices.Count; i++)
        {
            touchServices[i].OnRelease(input);
        }
    }
}
