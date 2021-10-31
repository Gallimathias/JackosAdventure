using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackosAdventure.UI.Components;
using JackosAdventure.UI.Controls;

namespace JackosAdventure.UI.Screens
{
    internal class GameScreen : Control
    {
        private readonly GameControl? gameControl;

        public GameScreen(ScreenGameComponent screenComponent) : base(screenComponent)
        {
            gameControl = new GameControl(ScreenComponent);
            screenComponent.Game.IsMouseVisible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            gameControl?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            gameControl?.Draw(gameTime, spriteBatch);
        }

        public override void Dispose()
        {
            gameControl?.Dispose();
        }
    }
}
