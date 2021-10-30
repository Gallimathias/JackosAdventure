using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JackosAdventure.UI.Controls
{
    class Reaper
    {
        private readonly Texture2D texture2D;

        public Vector2 Position { get; set; }
        public Vector2 Size => new Vector2(3, 4);
        public Rectangle Area { get; set; }

        public bool IsMoving { get; private set; }
        public int CurrentDirection { get; private set; }

        public Reaper(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            Area = new Rectangle(1, 1, 20, 20);
        }

        private int pingPongDirection = 1;
        private int currentFrame = 1;
        private int lastValue = 0;

        internal void Draw(GameTime gameTime, SpriteBatch batch, int centerX, int centerY)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);

            batch.Draw(texture2D, new Rectangle((int)(Position.X), (int)(Position.Y), (int)Size.X, (int)Size.Y), new Rectangle(currentFrame * xSize, (int)CurrentDirection * ySize, xSize, ySize), Color.White);

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

        internal void Update(GameTime gameTime)
        {
            const float speed = 4f;

            var dir = 1;
            
            var isinarea = Area.Contains((int)Position.X, (int)Position.Y);
            if (isinarea) { 
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            
            Position += new Vector2(dir, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
        }

    }
}
