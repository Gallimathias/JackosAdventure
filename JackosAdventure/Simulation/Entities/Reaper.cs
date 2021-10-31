using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JackosAdventure.Simulation.Entities
{
    internal class Reaper : Entitie
    {
        private readonly Texture2D texture2D;

        public override Vector2 Size => new Vector2(3, 4);
        public Rectangle Area { get; set; }

        public Direction CurrentDirection { get; private set; }

        private readonly Random random;

        public Reaper(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            Area = new Rectangle(1, 1, 10, 10);
            random = new Random();
        }

        private int pingPongDirection = 1;
        private int currentFrame = 1;
        private int lastValue = 0;

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);

            batch.Draw(texture2D, Position, Size, new Rectangle(currentFrame * xSize, (int)CurrentDirection * ySize, xSize, ySize), Color.White);

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
            var dir = 1;

            var isInArea = Area.Contains((int)newPosition.X, (int)newPosition.Y);
            if (isInArea)
            {
                IsMoving = true;
                Position = newPosition;
            }
            else if(!isWaiting)
            {
                IsMoving = false;
                isWaiting = true;
                waitingSeconds = gameTime.TotalGameTime.TotalSeconds + 5;
            }

            if(gameTime.TotalGameTime.TotalSeconds > waitingSeconds && isWaiting)
            {
                CurrentDirection = (Direction)random.Next(0, 4);
                isWaiting = false;
            }
            
        }

    }
}
