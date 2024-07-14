using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap_ecs.Core
{
    public class Entity
    {
        public int Id { get; private set; }
        public string name { get; set; }

        // Dictionary to store components by type
        private Dictionary<Type, Component> components = new Dictionary<Type, Component>();
        private List<GameSystem> systems = new List<GameSystem>();
        
        // Constructor to initialize the entity with an ID
        public Entity(int id,string name)
        {
            this.Id = id;
            this.name = name;
        }
        public Entity(int id)
        {
            this.Id = id;
            this.name = "entity";
        }
       

        // Method to add a component to the entity
        public void AddComponent<T>(T component) where T : Component
        {
            components[component.GetType()] = component;
        }

        // Method to get a component of a specific type
        public T GetComponent<T>() where T : Component
        {
            if (components.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            return null;
        }

        // Method to check if the entity has a component of a specific type
        public bool HasComponent<T>() where T : Component
        {
            return components.ContainsKey(typeof(T));
        }

        public bool HasComponent(Type componentType)
        {
            return components.ContainsKey(typeof(Type));
        }

        public bool HasComponents(params Type[] types) 
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

       public void RemoveComponent<T>() where T : Component
       {
            components.Remove(typeof(T)); 
       }
    }
}
