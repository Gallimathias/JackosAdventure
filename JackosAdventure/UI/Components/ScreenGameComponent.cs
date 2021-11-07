using System.IO;
using engenious;
using engenious.Graphics;
using engenious.Content;
using JackosAdventure.UI.Controls;

namespace JackosAdventure.UI.Components
{
    internal abstract class ScreenGameComponent : DrawableGameComponent
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
        
        public ContentManagerBase Content => Game.Content;

        private Control? activeControl;
        private readonly SpriteBatch spriteBatch;

        public ScreenGameComponent(Game game) : base(game)
        {
            Assets = new AssetManager(game.GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
        
        // TODO: dispose
        // protected override void Dispose(bool disposing)
        // {
        //     if (disposing)
        //     {
        //         activeControl?.Dispose();
        //         spriteBatch.Dispose();
        //     }
        //
        //     base.Dispose(disposing);
        // }

    }
}
