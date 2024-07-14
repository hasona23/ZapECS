using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zap_ecs.Core;
using Zap_ecs;
namespace banchmark;
    public class Sprite : IComponent
    {
        public Texture2D texture;
        public Sprite(Texture2D t) { texture = t; }
    }
    public class  Transform:IComponent
    {
        public Vector2 pos;
        public float scale;

        public Transform(Vector2 pos, float scale = 2f)
        {
            this.pos = pos;
            this.scale = scale;
        }
    }
    public class  Velocity:IComponent
    {
        public Vector2 vel;
        public float speed;

        public Velocity(Vector2 vel, float speed)
        {
            this.vel = vel;
            this.speed = speed;
        }
    }


    public class drawSystem : GameSystem
    {
        public drawSystem():base()
        {
        AddRequiredComponent<Sprite>();
        }
        public override void Update(GameTime gt, SpriteBatch sb = null)
        {
            if (sb != null)
            {
                foreach (var id in this.registeredEntityIds)
                {
                Entity entity = world.GetEntityById(id);
                    var sprite = entity.GetComponent<Sprite>();
                    var transform = entity.GetComponent<Transform>();
                    sb.Draw(sprite.texture, new Rectangle(new Point((int)transform.pos.X, (int)transform.pos.Y), new Point((int)(transform.scale * sprite.texture.Width), (int)(transform.scale * sprite.texture.Width))), Color.White);
                }
            }
        }
    }
    public class MoveSystem : GameSystem
    {
    public MoveSystem() : base()
    {
        AddRequiredComponent<Velocity>();
    }

        public override void Update(GameTime gt, SpriteBatch sb = null)
        {
            foreach (var id in this.registeredEntityIds)
            {
                Entity entity = world.GetEntityById(id);
                var velocity = entity.GetComponent<Velocity>();
                var transform = entity.GetComponent<Transform>();
                if (velocity.vel.Length()>0) {
                    velocity.vel.Normalize();
                }
                var changeX = velocity.vel.X * velocity.speed;
                var changeY = velocity.vel.Y * velocity.speed;
                if (transform.pos.X + changeX >= Globals.WindowSize.X || transform.pos.X+changeX <= 0)  
                {
                    velocity.vel.X *= -1;
                }
            transform.pos.X += changeX;
            if (transform.pos.Y + changeY >= Globals.WindowSize.Y || transform.pos.Y + changeY <= 0)
            {
                velocity.vel.Y *= -1;
            }
            transform.pos.Y += changeY;

            }

        }

        
        
    }

