using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagYouAreIt
{
    internal class Sphere
    {
            private Model model;
            public Vector3 Position { get; private set; }

            public Sphere(Model model, Vector3 position)
            {
                this.model = model;
                Position = position;
            }

            public void Update(GameTime gameTime)
            {
                // If you have specific update logic for the sphere, put it here.
            }

            public void Draw(float aspectRatio, Vector3 cameraPosition, Vector3 cameraLookAt)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = Matrix.CreateTranslation(Position);
                        effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
                        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 1, 100);
                        effect.DiffuseColor = Color.White.ToVector3();
                    }
                    mesh.Draw();
                }
            }
    }
}
