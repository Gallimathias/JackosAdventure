using engenious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackosAdventure.UI.Controls;
using JackosAdventure.UI.Screens;
using engenious.UI;
using engenious.Graphics;
using System.IO;

namespace JackosAdventure.UI.Components
{
    internal class ScreenComponent : BaseScreenComponent
    {
        public class AssetManager
        {
            public GraphicsDevice GraphicsDevice { get; }
            public AssetManager(GraphicsDevice graphicsDevice)
            {
                GraphicsDevice = graphicsDevice;
            }
            public T? Load<T>(string path)
            {
                if (typeof(T) == typeof(Texture2D))
                    return (T)(object)Texture2D.FromFile(GraphicsDevice, Path.Combine("Assets", path));
                return default;
            }
        }

        public AssetManager Assets { get; }
        private MenuScreen? menu;

        public ScreenComponent(Game game) : base(game)
        {
            Assets = new AssetManager(game.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            menu = new MenuScreen(this);
            NavigateToScreen(menu);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
    }
}
