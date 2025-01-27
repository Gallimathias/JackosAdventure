﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

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


        public void Draw(GameTime gameTime, SpriteBatch batch)
        {
            batch.Draw(
                texture2D,
                Position, Size,
               new Rectangle(currentFrame * textureSizeX, (int)CurrentDirection * textureSizeY, textureSizeX, textureSizeY),
                    Color.White, 0, Vector2.Zero, SpriteEffects.None, (Position.Y + Size.Y) / 1000f
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