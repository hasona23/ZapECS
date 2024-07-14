using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap_ecs.Core
{
    public class World
    {
        public List<Entity> Entities { get; private set; }
        private List<GameSystem> updateSystems;
        private List<GameSystem> drawSystems;
        private List<Entity> remove;

        // Method to add an entity to the world
        public World() 
        {
            Entities = new List<Entity>();   
            updateSystems = new List<GameSystem>();
            drawSystems = new List<GameSystem>();
            remove = new List<Entity>();
        }
        public void AddEntity(Entity entity)
        {

            if (entity != null) 
            {
                foreach (var sys in updateSystems)
                {
                    if (entity.HasComponents(sys.GetSystemTypes()))
                    {
                        sys.entities.Add(entity);
                    }
                }
                foreach (var system in drawSystems)
                {
                    if (entity.HasComponents(system.GetSystemTypes())) { system.entities.Add(entity); }
                }
                Entities.Add(entity);
            }
        }

       

        // Method to remove an entity from the world
        public void DeleteEntity(Entity entity)
        {
            foreach (var sys in updateSystems)
            {
                sys.entities.Remove(entity);
            }
            foreach (var system in drawSystems)
            {
                system.entities.Remove(entity);
            }
            this.Entities.Remove(entity);
            
        }

        public void RemoveEntity(Entity entity) 
        {
            remove.Add(entity);
        }
        // Method to add a system to the world
        public void AddUpdateSystem(GameSystem system)
        {
            if (!updateSystems.Contains(system))
            {
                updateSystems.Add(system);
                system.world = this;
                foreach (var entity in Entities)
                {
                    if (entity.HasComponents(system.GetSystemTypes())) { system.entities.Add(entity); }
                }
            }
        }
        public void AddDrawSystem(GameSystem system)
        {
            if (!drawSystems.Contains(system))
            {
                drawSystems.Add(system);
                system.world = this;
                foreach (var entity in Entities)
                {
                    if (entity.HasComponents(system.GetSystemTypes())) { system.entities.Add(entity); }
                } 
            }
        }

        // Method to simulate an update step in the world
        public void Update(GameTime gameTime)
        {
            foreach (var system in updateSystems)
            {
                system.Update(gameTime);
                foreach (var entity in remove)
                {
                    this.DeleteEntity(entity);
                }
                remove.Clear();
            }
        }

        // Method to simulate a draw step in the world
        public void Draw(GameTime gt, SpriteBatch sb)
        {
            foreach (var system in drawSystems)
            {
                system.Update(gt,sb);
            }
        }
    }

}
