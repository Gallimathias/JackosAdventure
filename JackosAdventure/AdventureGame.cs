using JackosAdventure.UI.Components;
using engenious;
using System;
using engenious.Graphics;

namespace JackosAdventure
{
    internal class AdventureGame : Game
    {

        public AdventureGame()
        {
            GraphicsDevice.Viewport = new Viewport(0,0, 1280, 720);
            Window.ClientSize = new Size(1280, 720);

            Window.Title = "Jacko's Adventure";
        }

        protected override void Initialize()
        {
            var screenComponent = new ScreenComponent(this);

            Components.Add(screenComponent);
            base.Initialize();
        }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

    }
}
