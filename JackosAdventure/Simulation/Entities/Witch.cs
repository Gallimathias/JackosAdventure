using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JackosAdventure.Simulation.Entities
{
    internal class Witch : Entitie
    {
        private readonly Texture2D texture2D;
        public override Vector2 Size { get; } = new(2, 3);
        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        private int currentDirection;
        private readonly int textureSizeX;
        private readonly int textureSizeY;

        public Witch(Texture2D texture2D)
        {
            this.texture2D = texture2D;
            textureSizeX = texture2D.Width / 3;
            textureSizeY = texture2D.Height / 4;
        }

        public override void Dispose()
        {
            texture2D.Dispose();
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            const int gap = 1;

            var currentFrame = (int)(gameTime.TotalGameTime.TotalSeconds * 4 % 4);

            batch
                .Draw(
                    texture2D,
                    Position, Size,
                    new Rectangle(textureSizeX + gap, currentFrame * textureSizeY + gap, textureSizeX - gap * 2, textureSizeY - gap * 2),
                    Color.White
                );
        }

    }
}