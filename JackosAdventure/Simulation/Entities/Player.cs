using engenious;
using engenious.Graphics;

namespace JackosAdventure.Simulation.Entities
{
    internal class Player : Entitie
    {
        private readonly Texture2D texture2D;

        public override Vector2 Size { get; } = new Vector2(2, 3);


        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        private readonly int textureSizeX;
        private readonly int textureSizeY;

        public Player(Texture2D texture2D)
        {
            this.texture2D = texture2D;

            textureSizeX = texture2D.Width / 3;
            textureSizeY = texture2D.Height / 4;
        }

        public override void Dispose()
        {
            texture2D.Dispose();
        }

        private int pingPongDirection = 1;
        private int currentFrame = 1;
        private int lastValue = 0;


        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {

            var halfTexelX = 0.5f / texture2D.Width;
            var halfTexelY = 0.5f / texture2D.Height;

            var sourceRect = new RectangleF((float)currentFrame * textureSizeX / texture2D.Width + halfTexelX, (float)((int)CurrentDirection * textureSizeY) / texture2D.Height + halfTexelY, (float)textureSizeX / texture2D.Width - 2 * halfTexelX, (float)textureSizeY / texture2D.Height - halfTexelY);

            batch.Draw(
                texture2D, Position, sourceRect,
                Color.White, 0, Vector2.Zero, Size / sourceRect.Size, SpriteBatch.SpriteEffects.None, -(Position.Y + Size.Y) / 1000f);
            //batch.Draw(
            //    texture2D,
            //    Position, Size,
            //   new Rectangle(currentFrame * textureSizeX, (int)CurrentDirection * textureSizeY, textureSizeX, textureSizeY),
            //        Color.White, 0, Vector2.Zero, SpriteBatch.SpriteEffects.None, -(Position.Y + Size.Y) / 1000f
            //);

            if (IsMoving)
            {
                var value = (int)(gameTime.TotalGameTime.TotalSeconds * 4 % 3);

                if (lastValue != value)
                {
                    currentFrame += pingPongDirection;
                    lastValue = value;
                }

                if (currentFrame == 2)
                {
                    pingPongDirection = -1;
                }
                else if (currentFrame == 0)
                {
                    pingPongDirection = 1;
                }
            }
            else
            {
                currentFrame = 1;
            }
        }

    }
}