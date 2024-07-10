using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Zap_ecs.Core;
using Zap_ecs;
using System.Diagnostics;




namespace banchmark
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        drawSystem draw = new drawSystem();
        MoveSystem move = new MoveSystem();
        World world = new World();
        Texture2D t;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Globals.WindowSize = new Point(1280, 720);
            _graphics.PreferredBackBufferWidth = Globals.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Globals.WindowSize.Y;
            _graphics.ApplyChanges();
        }
        public Entity model()
        {
            Entity e = new Entity();
            Sprite s = new Sprite(t);
            Velocity v = new Velocity(new Vector2(new Random().Next(1,100), new Random().Next(1,100)), 10f);
            Transform tr = new Transform(new Vector2(new Random().Next(1, Globals.WindowSize.X), new Random().Next(1, Globals.WindowSize.Y)), 2);
            e.AddComponent(s);
            e.AddComponent(tr);
            e.AddComponent(v);
            return e;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            t = Content.Load<Texture2D>("tile_0040");
            for (int i = 0; i < 56000; i++)
            {
                Entity e = model();
                if (e == null) { Debug.WriteLine("NULL"); }
                world.AddEntity(e);
            }
            world.AddUpdateSystem(move);
            world.AddDrawSystem(draw);
            font = Content.Load<SpriteFont>("font");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                world.RemoveEntity(world.Entities[new Random().Next(world.Entities.Count)]);
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                world.AddEntity(model());
            
           

            world.Update(gameTime);
            base.Update(gameTime);
        }
        FpsCounter fps = new FpsCounter();
        SpriteFont font;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            fps.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            world.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(font, fps.AverageFramesPerSecond.ToString(), new Vector2(50, 50), Color.White);
            _spriteBatch.DrawString(font, world.Entities.Count.ToString(), new Vector2(50, 100), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
