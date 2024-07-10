using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap_ecs.Core
{
    public class Entity
    {
        //public int Id { get; private set; }
        //private static int IdCount = 0;
        public string name { get; set; }

        // Dictionary to store components by type
        private Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();
        private List<GameSystem> systems = new List<GameSystem>();
        
        // Constructor to initialize the entity with an ID
        public Entity(string name = "entity")
        {
            this.name = name;
         //   Id = IdCount++;
        }

        // Method to add a component to the entity
        public void AddComponent<T>(T component) where T : class, IComponent
        {
            components[typeof(T)] = component;

        }

        // Method to get a component of a specific type
        public T GetComponent<T>() where T : class, IComponent
        {
            if (components.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }
            return null;
        }

        // Method to check if the entity has a component of a specific type
        public bool HasComponent<T>() where T : class, IComponent
        {
            return components.ContainsKey(typeof(T));
        }
        public bool HasComponents(List<Type> types) 
        {
            foreach (var type in types)
            {
                if (!components.ContainsKey(type)) 
                {
                    return false;
                }   
            }
            return true;
        }

        // Method to remove a component from the entity
        public void RemoveComponent<T>() where T : class, IComponent
        {

            components.Remove(typeof(T));
            foreach(var system in systems)
            {
                if (!this.HasComponents(system.GetSystemTypes()))
                {
                    system.entities.Remove(this);
                    systems.Remove(system);
                }
            }
        }
    }
}
