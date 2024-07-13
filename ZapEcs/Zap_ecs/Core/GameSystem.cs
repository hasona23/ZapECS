using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Zap_ecs.Core;



public abstract class GameSystem 
{
    public List<Entity> entities;
    public List<Type> ComponentTypes { get; private set; }
    public World world;
    
    public GameSystem() 
    {
        ComponentTypes = new List<Type>();
        entities = new List<Entity>();
    }
    public void SetRequirments(params Type[] types) 
    {
        ComponentTypes = types.ToList();
    }
    public Type[] GetSystemTypes() 
    {
        return ComponentTypes.ToArray();
    }

    public abstract void Update(GameTime gt, SpriteBatch sb = null);
}