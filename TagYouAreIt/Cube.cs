using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagYouAreIt
{
    public class Cube
    {
        private Model model;
        private Vector3 spherePosition;
        private List<Cube> cubes;
        private int taggedCount;
        public Vector3 Position { get; private set; }
        public bool Tagged { get; set; }

        public Cube(Model model, Vector3 position)
        {
            this.model = model;
            Position = position;
            Tagged = false;
        }

        public Cube(Model model, Vector3 position, Vector3 spherePosition, List<Cube> cubes, int taggedCount)
        {
            this.model = model;
            this.spherePosition = spherePosition;
            this.cubes = cubes;
            this.taggedCount = taggedCount;
            Position = position;
            Tagged = false;
        }

        public void Update(Vector3 spherePosition, GameTime gameTime)
        {
            // Update cube position based on distance from the sphere
            float distance = Vector3.Distance(spherePosition, Position);

            if (distance < 10)
            {
                // Move away from the sphere
                Vector3 direction = Vector3.Normalize(Position - spherePosition);
                Position += direction * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (distance > 11)
            {
                // Move closer to the sphere
                Vector3 direction = Vector3.Normalize(spherePosition - Position);
                Position += direction * 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            Matrix world = Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
            Matrix view = Matrix.CreateLookAt(Position, spherePosition, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45), 1, 1, 100);
            Matrix worldViewProjection = world * view * projection;
        }

        public void Draw(Model model, Vector3 cameraPosition, Vector3 cameraLookAt)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // Set World, View, Projection, and DiffuseColor as in the main Game class
                    // Use Matrix.CreateLookAt to ensure cubes always face the sphere
                }
                mesh.Draw();
            }
        }
    }

}
