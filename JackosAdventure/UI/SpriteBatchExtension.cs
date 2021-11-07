using engenious;
using engenious.Graphics;

namespace JackosAdventure
{
    internal static class SpriteBatchExtension
    {
        public static void Draw(this SpriteBatch batch, Texture2D text, Vector2 pos, Vector2 size, Rectangle sourceRectangle, Color color, float rotation, Vector2 offset, SpriteBatch.SpriteEffects effects, float layerDepth)
        {
            batch.Draw(text, pos, sourceRectangle, color, rotation, offset, size, effects, layerDepth);
        }
        public static void Draw(this SpriteBatch batch, Texture2D text, Vector2 pos, Vector2 size, Rectangle sourceRectangle, Color color)
        {
            batch.Draw(text, pos, size, sourceRectangle, color, 0, Vector2.Zero, SpriteBatch.SpriteEffects.None, 0f);
        }
    }
}
