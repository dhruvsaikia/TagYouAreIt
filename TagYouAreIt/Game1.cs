using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

            cubes = new List<Cube>();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                float x = (float)random.NextDouble() * 10 - 5;
                float z = (float)random.NextDouble() * 10 - 5;
                cubes.Add(new Cube(cubeModel, new Vector3(x, 0, z), spherePosition, cubes, taggedCount));
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sphereModel = Content.Load<Model>("Ball");
            cubeModel = Content.Load<Model>("Cube");
            spriteFont = Content.Load<SpriteFont>("Arial12");
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
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (ModelMesh mesh in sphereModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateTranslation(spherePosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), graphics.GraphicsDevice.Viewport.AspectRatio, 1, 100);
                    effect.DiffuseColor = Color.White.ToVector3();
                }
                mesh.Draw();
            }

            foreach (Cube cube in cubes)
            {
                cube.Draw(cubeModel, cameraPosition, cameraLookAt);
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