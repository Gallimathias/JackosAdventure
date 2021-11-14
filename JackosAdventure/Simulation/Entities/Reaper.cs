using JackosAdventure.UI;
using engenious;
using engenious.Graphics;
using System;

namespace JackosAdventure.Simulation.Entities
{
    internal class Reaper : Entitie
    {
        private readonly Texture2D texture2D;

        public override Vector2 Size { get; } = new Vector2(2, 3);

        public Rectangle Area { get; set; }

        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        private readonly Random random;

        private readonly int textureSizeX;
        private readonly int textureSizeY;

        public Reaper(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            Area = new Rectangle(1, 1, 10, 10);
            random = new Random();

            textureSizeX = texture2D.Width / 3;
            textureSizeY = texture2D.Height / 4;
        }

        private double waitingSeconds;
        private Vector2 targetPosition;

        public override void Update(GameTime gameTime)
        {
            const float speed = 4f;
            Vector2 direction = (targetPosition - Position);
            if (direction.LengthSquared > 0)
                direction.Normalize();

            if (Math.Abs(direction.X) > 0 && Math.Abs(direction.Y) > 0)
            {
                if (Math.Abs(direction.X) >= Math.Abs(direction.Y))
                {
                    CurrentDirection = direction.X < 0 ? Direction.Left : Direction.Right;
                }
                else
                {
                    CurrentDirection = direction.Y < 0 ? Direction.Up : Direction.Down;
                }
            }

            var newPosition = Position + direction * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            if (IsMoving)
            {
                var newDirection = (targetPosition - newPosition).Normalized();
                if (newDirection.Dot(direction) < 0)
                {
                    Position = targetPosition;
                    IsMoving = false;
                    waitingSeconds = gameTime.TotalGameTime.TotalSeconds + random.Next(1, 11);
                }
                else
                {
                    Position = newPosition;
                }
            }

            if (gameTime.TotalGameTime.TotalSeconds > waitingSeconds && !IsMoving)
            {
                var x = (float)(Area.X + (random.NextDouble() * Area.Width));
                var y = (float)(Area.Y + (random.NextDouble() * Area.Height));
                targetPosition = new Vector2(x, y);

                IsMoving = true;
            }

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
            const int gap = 1;

            batch.Draw(
                texture2D,
                Position, Size,
                new Rectangle(currentFrame * textureSizeX + gap, (int)CurrentDirection * textureSizeY + gap, textureSizeX - gap * 2, textureSizeY - gap * 2),
                Color.White,
                0,
                Vector2.Zero,
                SpriteBatch.SpriteEffects.None,
                -(Position.Y + Size.Y) / 1000f
            );

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
