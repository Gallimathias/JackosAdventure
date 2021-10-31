using JackosAdventure.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Velentr.Font;

namespace JackosAdventure.UI.Controls
{
    internal class Button : Control
    {
        public event Action? OnClick;

        private readonly Rectangle controlArea;
        private readonly Font font;
        private readonly Text text;

        public Button(ScreenGameComponent screenComponent, Rectangle position, string title) : base(screenComponent)
        {
            controlArea = position;
            font = screenComponent.Fonts.GetFont(Path.Combine(".", "Assets", "fonts", "golem-script.ttf"), 48);
            text = font.MakeText(title);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            var x = (controlArea.Width / 2) - (text.Size.X / 2) + controlArea.X;
            var y = (controlArea.Height / 2) - (text.Size.Y / 2) + controlArea.Y + 15;

            spriteBatch.Begin();
            spriteBatch.Draw(Skin.Pix!, controlArea, Color.SaddleBrown);
            spriteBatch.DrawString(text, new Vector2(x, y), Color.White);
            spriteBatch.End();
            base.Draw(gameTime, spriteBatch);
        }

        protected override void OnLeftMouseButtonClicked(Point position)
        {
            if (controlArea.Contains(position))
            {
                OnClick?.Invoke();
            }
        }

        public override void Dispose()
        {
            font.Dispose();
            base.Dispose();
        }
    }
}
