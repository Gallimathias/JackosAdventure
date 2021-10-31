using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;
using JackosAdventure.UI.Controls;

namespace JackosAdventure.UI.Components
{
    internal abstract class ScreenGameComponent : DrawableGameComponent
    {
        public FontManager Fonts { get;  }

        public ContentManager Content => Game.Content;

        private Control? activeControl;
        private readonly SpriteBatch spriteBatch;

        public ScreenGameComponent(Game game) : base(game)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Fonts = new FontManager(GraphicsDevice);            
        }

        public void NavigateTo(Control control)
        {
            activeControl?.Dispose();
            activeControl = control;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            activeControl?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            activeControl?.Draw(gameTime, spriteBatch);
        }

        protected override void LoadContent()
        {
            Skin.Pix = new Texture2D(GraphicsDevice, 1, 1);
            Skin.Pix.SetData(new[] { Color.White });

            base.LoadContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                activeControl?.Dispose();
                spriteBatch.Dispose();
                Fonts.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}
