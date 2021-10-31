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

        public Vector2 PlayerSize { get; }


        public Camera(Vector3 position, Vector2 playerSize)
        {
            Position = position;
            PlayerSize = playerSize;
        }

        private Vector2 center;

        public void UpdateBounds(int width, int height, int tileCount)
        {
            float aspectRatio = (float)height / width;


            center = new Vector2(-tileCount / aspectRatio / 2f, -tileCount / 2f);

            Unit = new Vector2(height / tileCount, height / tileCount);
            Projection = Matrix.CreateOrthographicOffCenter(0, tileCount / aspectRatio, tileCount, 0, 10, -10);
        }

        public void Update()
        {
            var offset = new Vector3(center + PlayerSize / 2f, 0);
            View = Matrix.CreateLookAt(Position + offset, Position + offset + new Vector3(0, 0, -1), Vector3.UnitY);
            ViewProjection = View * Projection;
        }
    }
}