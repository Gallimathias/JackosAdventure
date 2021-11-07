using JackosAdventure.UI.Components;
using engenious;
using engenious.Graphics;
using System;
using System.IO;

namespace JackosAdventure.UI.Controls
{
    internal class Button : Control
    {
        public event Action? OnClick;

        private readonly Rectangle controlArea;
        private readonly SpriteFont font;
        private readonly string text;
        private readonly Vector2 textSize;

        public Button(ScreenGameComponent screenComponent, Rectangle position, string title) : base(screenComponent)
        {
            controlArea = position;
            font = screenComponent.Content.Load<SpriteFont>("fonts/golem-script") ?? throw new FileNotFoundException();//screenComponent.Fonts.GetFont(Path.Combine(".", "Assets", "fonts", "golem-script.ttf"), 48);
            text = title;
            textSize = font.MeasureString(text);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            var x = (controlArea.Width / 2f) - (textSize.X / 2) + controlArea.X;
            var y = (controlArea.Height / 2f) - (textSize.Y / 2) + controlArea.Y + 3;

            spriteBatch.Begin();
            spriteBatch.Draw(Skin.Pix!, controlArea, Color.SaddleBrown);
            spriteBatch.DrawString(font, text, new Vector2(x, y), Color.White);
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
    }
}
