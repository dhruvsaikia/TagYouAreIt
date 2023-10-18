using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static TagYouAreIt.Sphere;

namespace TagYouAreIt
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Model sphereModel;
        private Model cubeModel;
        private SpriteFont spriteFont;
        private Vector3 cameraPosition;
        private Vector3 cameraLookAt;
        private List<Cube> cubes;
        private int taggedCount;
        private Vector3 spherePosition = Vector3.Zero;
        private Sphere sphere;

        private Matrix world = Matrix.Identity;
        private Matrix projection = Matrix.Identity;
        private Matrix view = Matrix.Identity;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 700;
            graphics.PreferredBackBufferHeight = 700;
            graphics.ApplyChanges();

            cameraPosition = new Vector3(0, 40, 10);
            cameraLookAt = new Vector3(0, 0, 0);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sphereModel = Content.Load<Model>("Ball");
            cubeModel = Content.Load<Model>("Cube");
            spriteFont = Content.Load<SpriteFont>("Arial12");
            sphere = new Sphere(sphereModel, spherePosition);




            cubes = new List<Cube>();
            Random random = new Random();

            const float radius = 4.0f;

            for (int i = 0; i < 10; i++)
            {
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;

                float x = radius * (float)Math.Cos(angle);
                float z = radius * (float)Math.Sin(angle);

                cubes.Add(new Cube(cubeModel, new Vector3(x, 0, z), spherePosition, cubes, taggedCount));
            }


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            Vector3 movement = Vector3.Zero;

            if (keyboardState.IsKeyDown(Keys.W))
                movement.Z = -10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.S))
                movement.Z = 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.A))
                movement.X = -10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.D))
                movement.X = 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            spherePosition += movement;

            foreach (Cube cube in cubes)
            {
                cube.Update(spherePosition, gameTime);

                if (Vector3.Distance(spherePosition, cube.Position) < 1 && !cube.Tagged)
                {
                    cube.Tagged = true;
                    taggedCount++;
                }
            }

            base.Update(gameTime);
            sphere.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            sphere.Draw(graphics.GraphicsDevice.Viewport.AspectRatio, cameraPosition, cameraLookAt);

            foreach (Cube cube in cubes)
            {
                cube.Draw(graphics.GraphicsDevice.Viewport.AspectRatio, cameraPosition, cameraLookAt);;
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, "Tagged: " + taggedCount, new Vector2(10, 10), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        static void Main()
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
}