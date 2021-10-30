using JackosAdventure.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JackosAdventure.Simulation.Entities
{
    internal class Reaper : IDisposable
    {
        private readonly Texture2D texture2D;

        public Vector2 Position { get; set; }
        public Vector2 Size => new(2, 3);

        public Rectangle Area { get; set; }

        public bool IsMoving { get; internal set; }
        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        private Vector2 currentTarget;

        private readonly Random random;

        public Reaper(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            random = new Random();
        }

        private double waitingSeconds;
        private bool isWaiting;

        internal void Update(GameTime gameTime)
        {
            const float speed = 4f;
            Vector2 direction = default;
            //Position += new Vector2(1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            switch (CurrentDirection)
            {
                case Direction.Down:
                    direction = new Vector2(0, 1);
                    break;
                case Direction.Left:
                    direction = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    direction = new Vector2(1, 0);
                    break;
                case Direction.Up:
                    direction = new Vector2(0, -1);
                    break;
            }

            var newPosition = Position + direction * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            if (Area.Contains((int)newPosition.X, (int)newPosition.Y))
            {
                IsMoving = true;
                Position = newPosition;
            }
            else if(!isWaiting)
            {
                IsMoving = false;
                waitingSeconds = gameTime.TotalGameTime.TotalSeconds + 10;
                isWaiting = true;
            }

            if(gameTime.TotalGameTime.TotalSeconds > waitingSeconds && isWaiting)
            {
                CurrentDirection = (Direction)random.Next(0, 4);
                isWaiting = false;
            }

        }

        public void Dispose()
        {
            texture2D.Dispose();
        }

        private int pingPongDirection = 1;
        private int currentFrame = 1;
        private int lastValue = 0;

        internal void Draw(GameTime gameTime, SpriteBatch batch)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);
            var gap = 1;

            batch.Draw(texture2D, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), new Rectangle(currentFrame * xSize + gap, (int)CurrentDirection * ySize + gap, xSize - gap * 2, ySize - gap * 2), Color.White);

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