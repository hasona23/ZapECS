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
        private Dictionary<int, Entity> entities;
        private int ID = 0;
        private Dictionary<Type,GameSystem> updateSystems;
        private Dictionary<Type,GameSystem> drawSystems;
        //private Dictionary<Type, GameSystem> systems;
        private List<int> remove;

        // Method to add an entity to the world
        public World() 
        {
            entities = new Dictionary<int, Entity>();
            updateSystems = new Dictionary<Type, GameSystem>();
            drawSystems = new Dictionary<Type, GameSystem>();
            remove = new List<int>();
        }
        public void AddEntity(Entity entity)
        {
            entities[ID] = entity;
            UpdateEntityRegistration(entity);
        }
        public Entity AddAndGetEntity() 
        {
            Entity entity = new Entity(ID++);
            entities[entity.Id] = entity;
            return entity;
        }
        public void DeleteEntity(int id)
        {
            remove.Add(id);
        }

        public Entity GetEntityById(int id)
        {
            return entities[id];
        }
        public void AddUpdateSystem(GameSystem system)
        {
            updateSystems[system.GetType()] = system;
            system.BindToWorld(this);
            foreach(Entity entity in entities.Values) 
            {
                system.UpdateEntityRegistration(entity);
            }
        }

        public T GetUpdateSystem<T>() where T : GameSystem
        {
            return (T)updateSystems[typeof(T)];
        }
        public void AddDrawSystem(GameSystem system)
        {
            drawSystems[system.GetType()] = system;
            system.BindToWorld(this);
            foreach(Entity entity in entities.Values) 
            {
                system.UpdateEntityRegistration(entity);
            }
            
        }

        public T GetDrawSystem<T>() where T : GameSystem
        {
            return (T)drawSystems[typeof(T)];
        }
        private void UpdateEntityRegistration(Entity entity)
        {
            foreach (GameSystem system in drawSystems.Values)
            {
                system.UpdateEntityRegistration(entity);
            }
            foreach (GameSystem system in updateSystems.Values)
            {
                system.UpdateEntityRegistration(entity);
            }

        }
        public void AddComponentToEntity(Entity entity, Component component)
        {
            entity.AddComponent(component);
            UpdateEntityRegistration(entity);
        }

        public void RemoveComponentFromEntity<T>(Entity entity) where T : Component
        {
            entity.RemoveComponent<T>();
            UpdateEntityRegistration(entity);
        }

        // Method to simulate an update step in the world
        public void Update(GameTime gameTime)
        {
            foreach (var system in updateSystems.Values)
            {
                system.Update(gameTime);
            }
            DisposeEntities();
        }
        public bool EntityExistId(int id) 
        {
            return entities.ContainsKey(id);
        }
        public void DisposeEntities() 
        {
            foreach (int id in remove)
            {
                if (!EntityExistId(id)) //safeguard against deleting twice
                    continue;

                foreach (GameSystem system in updateSystems.Values)
                {
                    system.DeleteEntity(id);
                }
                foreach (GameSystem system in drawSystems.Values)
                {
                    system.DeleteEntity(id);
                }

                entities.Remove(id);
            }
            remove.Clear();
        }

        // Method to simulate a draw step in the world
        public void Draw(GameTime gt, SpriteBatch sb)
        {
            foreach (var system in drawSystems.Values)
            {
                system.Update(gt,sb);
            }
        }
    }

}
