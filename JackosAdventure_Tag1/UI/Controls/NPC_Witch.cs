using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Velentr.Font;

namespace JackosAdventure.UI.Controls
{
    internal class NPC_Witch : IDisposable
    {
        private readonly Texture2D texture2D;

        public Vector2 Position { get; set; }
        public Vector2 Size => new Vector2(2, 3);

        public bool IsMoving { get; internal set; }

        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;

        //private Text firstText;

        public NPC_Witch(Texture2D texture2D)
        {
            this.texture2D = texture2D;
        }

        internal void Update(GameTime gameTime)
        {
            //firstText = font.MakeText("Test");
        }

        public void Dispose()
        {
            texture2D.Dispose();
        }

        private int currentFrame = 1;

        internal void Draw(GameTime gameTime, SpriteBatch batch, int centerX, int centerY)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);

            batch.Draw(texture2D, new Rectangle((int)(centerX - Size.X / 2), (int)(centerY - Size.Y / 2), (int)Size.X, (int)Size.Y), new Rectangle(currentFrame * xSize, (int)CurrentDirection * ySize, xSize, ySize), Color.White);

            currentDirection = (int)(gameTime.TotalGameTime.TotalSeconds * 4 % 4);
        }
        /// <summary>
        /// Gibt eine Text-Nachricht auf dem Bildschirm aus.
        /// </summary>
        public void Speech()
        {
            Console.WriteLine("Test");
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