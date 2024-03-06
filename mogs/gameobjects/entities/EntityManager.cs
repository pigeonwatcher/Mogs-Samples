using System;
using System.Collections.Generic;

public class EntityManager : IService
{
    public IReadOnlyCollection<Entity> Entities => entities.AsReadOnly();

    private List<Entity> entities = new List<Entity>();
    private Dictionary<Type, List<IEntityService>> entityServices = new Dictionary<Type, List<IEntityService>>();
    private Dictionary<Type, List<IEntitySystem>> entitySystems = new Dictionary<Type, List<IEntitySystem>>();

    public T AddEntity<T>(Entity entity = null) where T : Entity
    {
        entity ??= EntityFactory.CreateEntity<T>();

        entities.Add(entity);
        NotifyServices(entity, true);
        NotifySystems(entity, true);

        return (T)entity;
    }

    public void RemoveEntity(Entity entity)
    {
        if (entities.Contains(entity))
        {
            entities.Remove(entity);
            NotifyServices(entity, false);
            NotifySystems(entity, false);
        }
    }

    public T GetEntity<T>() where T : Entity
    {
        foreach (var entity in entities)
        {
            if (entity is T)
            {
                return (T)entity;
            }
        }
        return default(T);
    }

    public void HideAllEntities()
    {
        foreach (var entity in entities)
        {
            entity.SetActive(false);
        }
    }

    public void RegisterService<T>(IEntityService service) where T : Entity
    {
        if (entityServices.TryGetValue(typeof(T), out List<IEntityService> services))
        {
            services.Add(service);
            return;
        }

        entityServices[typeof(T)] = new List<IEntityService> { service };
    }

    public void RegisterSystem<T>(IEntitySystem system) where T : Entity
    {
        if (entitySystems.TryGetValue(typeof(T), out List<IEntitySystem> systems))
        {
            systems.Add(system);
            return;
        }

        entitySystems[typeof(T)] = new List<IEntitySystem> { system };
    }

    private void NotifyServices(Entity entity, bool isAdding)
    {
        if (entityServices.TryGetValue(entity.GetType(), out var services))
        {
            foreach (var service in services)
            {
                if (isAdding) service.RegisterEntity(entity);
                else service.UnregisterEntity(entity);
            }
        }
    }

    private void NotifySystems(Entity entity, bool isAdding)
    {
        if (entitySystems.TryGetValue(entity.GetType(), out var systems))
        {
            foreach (var system in systems)
            {
                if (isAdding) system.RegisterEntity(entity);
                else system.UnregisterEntity(entity);
            }
        }
    }
}
