using engenious;
using engenious.Graphics;

namespace JackosAdventure.Simulation.Entities
{
    internal class Witch : Entitie
    {
        private readonly Texture2D texture2D;
        public override Vector2 Size { get; } = new Vector2(2, 3);
        public Direction CurrentDirection { get => (Direction)currentDirection; set => currentDirection = (int)value; }

        public Rectangle InteractionArea { get; set; }

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

        public override void Update(GameTime gameTime)
        {
            //firstText = font.MakeText("Test");
            const int gap = 1;

            InteractionArea = new Rectangle((int)Position.X - gap, (int)Position.Y - gap, (int)Size.X + gap * 2, (int)Size.Y + gap * 2);
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
                    Color.White, 
                    0, 
                    Vector2.Zero, 
                    SpriteBatch.SpriteEffects.None, 
                    -(Position.Y + Size.Y) / 1000f
                );
        }

    }
}