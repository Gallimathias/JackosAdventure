using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackosAdventure
{
    internal static class SpriteBatchExtension
    {
        public static void Draw(this SpriteBatch batch, Texture2D text, Vector2 pos, Vector2 size, Rectangle sourceRectangle, Color color, float rotation, Vector2 offset, SpriteEffects effects, float layerDepth)
        {
            var scale = new Vector2(size.X / sourceRectangle.Width, size.Y / sourceRectangle.Height);
            batch.Draw(text, pos, sourceRectangle, color, rotation, offset, scale, effects, layerDepth);
        }
        public static void Draw(this SpriteBatch batch, Texture2D text, Vector2 pos, Vector2 size, Rectangle sourceRectangle, Color color)
        {
            batch.Draw(text, pos, size, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
