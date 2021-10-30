using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JackosAdventure.UI.Controls
{
    internal class Player : IDisposable
    {
        private readonly Texture2D texture2D;

        public Vector2 Position { get; set; }
        public Vector2 Size => new(78, 108);

        public bool IsMoving { get; internal set; }
        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        public Player(Texture2D texture2D)
        {
            this.texture2D = texture2D;
        }

        internal void Update(GameTime gameTime)
        {
            
        }

        public void Dispose()
        {
            texture2D.Dispose();
        }

        private int pingPongDirection = 1;
        private int currentFrame = 1;
        private int lastValue = 0;

        internal void Draw(GameTime gameTime, SpriteBatch batch, int centerX, int centerY)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);

            batch.Draw(texture2D, new Rectangle((int)(centerX - Size.X / 2), (int)(centerY - Size.Y / 2), (int)Size.X, (int)Size.Y), new Rectangle(currentFrame * xSize, (int)CurrentDirection * ySize, xSize, ySize), Color.White);

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

        public enum Direction
        {
            Down = 0,
            Left = 1,
            Right = 2,
            Up = 3,
        }

    }
}