using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JackosAdventure.Simulation.Entities
{
    internal class Witch : IDisposable
    {
        private readonly Texture2D texture2D;

        public Vector2 Position { get; set; }
        public Vector2 Size => new(2, 3);

        public bool IsMoving { get; internal set; }
        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        public Witch(Texture2D texture2D)
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

        internal void Draw(GameTime gameTime, SpriteBatch batch)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);
            var currentFrame = (int)(gameTime.TotalGameTime.TotalSeconds * 4 % 4);
            var gap = 1;

            batch.Draw(texture2D, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), new Rectangle(xSize + gap, currentFrame * ySize + gap, xSize - gap * 2, ySize - gap * 2), Color.White);
        }

    }
}