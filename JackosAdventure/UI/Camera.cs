using Microsoft.Xna.Framework;

namespace JackosAdventure.UI
{
    public class Camera
    {
        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }

        public Matrix ViewProjection { get; private set; }

        public Vector3 Position { get; set; }

        public Vector2 Unit { get; private set; }


        public Camera(Vector3 position)
        {
            Position = position;
        }

        public void UpdateBounds(int width, int height, int tileCount)
        {
            float aspectRatio = (float)height / width;

            Unit = new Vector2(height / tileCount, height / tileCount);
            Projection = Matrix.CreateOrthographicOffCenter(0, tileCount / aspectRatio, tileCount, 0, 10, -10);
        }

        public void Update()
        {
            View = Matrix.CreateLookAt(Position, Position + new Vector3(0, 0, -1), Vector3.UnitY);
            ViewProjection = View * Projection;
        }
    }
}