using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Zap_ecs.Core;



public abstract class GameSystem 
{
    private HashSet<int> registeredEntityIds;
    private List<Type> requiredComponents;
    protected World world;
    protected List<Entity> Entities
    {
        get
        {
            var result = new List<Entity>();
            foreach(int entityId in registeredEntityIds)
            {
                if (world.EntityExistId(entityId))
                    result.Add(world.GetEntityById(entityId));
            }

            return result;
        }
    }
    protected GameSystem() 
    {
      
       registeredEntityIds = new HashSet<int>();
       requiredComponents = new List<Type>();
        
    }
    public void BindToWorld(World world) 
    {
        this.world = world;
    }
    public void UpdateEntityRegistration(Entity entity)
    {
        bool matches = Matches(entity);
        if (registeredEntityIds.Contains(entity.Id))
        {
            if (!matches)
            {
                registeredEntityIds.Remove(entity.Id);
            }
        }
        else
        {
            if (matches)
            {
                registeredEntityIds.Add(entity.Id);
            }
        }
    }
    public virtual void DeleteEntity(int id)
    {
        if (registeredEntityIds.Contains(id))
        {
            registeredEntityIds.Remove(id);
        }
    }
    protected void AddRequiredComponent<T>() where T : Component
    {
        requiredComponents.Add(typeof(T));
    }
    protected void AddRequiredComponents(params Type[] types) 
    {
        foreach(var type in types) 
        {
            requiredComponents.Add(type);
        }
    }
    private bool Matches(Entity entity)
    {
        return entity.HasComponents(requiredComponents.ToArray());
    }

    public abstract void Update(GameTime gt, SpriteBatch sb = null);
}