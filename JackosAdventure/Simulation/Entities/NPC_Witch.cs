using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Velentr.Font;

namespace JackosAdventure.Simulation.Entities
{
    internal class NPC_Witch : Entitie
    {
        private readonly Texture2D texture2D;

        public override Vector2 Size => new Vector2(2, 3);

        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        public Rectangle InteractionArea { get; set; }

        private int currentDirection;

        //private Text firstText;

        public NPC_Witch(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            
        }

        public override void Update(GameTime gameTime)
        {
            //firstText = font.MakeText("Test");
            const int gap = 5;

            InteractionArea = new Rectangle((int)Position.X - gap, (int)Position.Y - gap, (int)Size.X + gap * 2, (int)Size.Y + gap * 2);
        }

        public override void Dispose()
        {
            texture2D.Dispose();
        }

        private int currentFrame = 1;

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            int xSize = texture2D.Width / 3;
            int ySize = (texture2D.Height / 4);

            batch.Draw(texture2D, Position, Size, new Rectangle(currentFrame * xSize, (int)CurrentDirection * ySize, xSize, ySize), Color.White);

            currentDirection = (int)(gameTime.TotalGameTime.TotalSeconds * 4 % 4);
        }
        /// <summary>
        /// Gibt eine Text-Nachricht auf dem Bildschirm aus.
        /// </summary>
        public void Speech()
        {
            Console.WriteLine("Test");
        }       
    }
}