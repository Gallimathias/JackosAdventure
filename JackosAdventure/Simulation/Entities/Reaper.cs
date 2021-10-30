using JackosAdventure.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JackosAdventure.Simulation.Entities
{
    internal class Reaper : Entitie
    {
        private readonly Texture2D texture2D;

        public override Vector2 Size { get; } = new(2, 3);

        public Rectangle Area { get; set; }

        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        private Vector2 currentTarget;

        private readonly Random random;

        private readonly int textureSizeX;
        private readonly int textureSizeY;

        public Reaper(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            random = new Random();

            textureSizeX = texture2D.Width / 3;
            textureSizeY = texture2D.Height / 4;
        }

        private double waitingSeconds;
        private bool isWaiting;

        public override void Update(GameTime gameTime)
        {
            const float speed = 4f;
            Vector2 direction = default;

            switch (CurrentDirection)
            {
                case Direction.Down:
                    direction = Vector2.UnitY;
                    break;
                case Direction.Left:
                    direction = -Vector2.UnitX;
                    break;
                case Direction.Right:
                    direction = Vector2.UnitX;
                    break;
                case Direction.Up:
                    direction = -Vector2.UnitY;
                    break;
            }

            var newPosition = Position + direction * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            if (Area.Contains((int)newPosition.X, (int)newPosition.Y))
            {
                IsMoving = true;
                Position = newPosition;
            }
            else if (!isWaiting)
            {
                IsMoving = false;
                waitingSeconds = gameTime.TotalGameTime.TotalSeconds + 10;
                isWaiting = true;
            }

            if (gameTime.TotalGameTime.TotalSeconds > waitingSeconds && isWaiting)
            {
                CurrentDirection = (Direction)random.Next(0, 4);
                isWaiting = false;
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
                Color.White
            );

            /*batch
                .Draw(
                    texture2D,
                    new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                    new Rectangle(currentFrame * textureSizeX + gap, (int)CurrentDirection * textureSizeY + gap, textureSizeX - gap * 2, textureSizeY - gap * 2),
                    Color.White, 0, Vector2.Zero, SpriteEffects.None, (Position.Y + Size.Y) / 1000f
                );*/

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