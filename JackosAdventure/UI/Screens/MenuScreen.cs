using System.IO;
using JackosAdventure.UI.Components;
using JackosAdventure.UI.Controls;
using engenious;
using engenious.Audio;
using engenious.Graphics;

namespace JackosAdventure.UI.Screens
{
    internal class MenuScreen : Control
    {
        private readonly Button playButton;
        private readonly Texture2D headTexture;
        // private readonly Song backgroundSong;

        private readonly SpriteFont font;


        public MenuScreen(ScreenGameComponent screenComponent) : base(screenComponent)
        {
            var width = GraphicsDevice.Viewport.Width / 3;
            var height = 60;

            var x = (GraphicsDevice.Viewport.Width - width) / 2;
            var y = (GraphicsDevice.Viewport.Height - height) / 2;

            playButton = new Button(screenComponent, new Rectangle(x, y, width, height), "Play");
            playButton.OnClick += PlayButtonClick;

            screenComponent.Game.IsMouseVisible = true;

            headTexture = screenComponent.Assets.Load<Texture2D>(@"jacko_head_8.png") ?? throw new FileNotFoundException();
            font = screenComponent.Content.Load<SpriteFont>("fonts/Halls___") ?? throw new FileNotFoundException();
            // font = screenComponent.Fonts.GetFont(Path.Combine(".", "Assets", "fonts", "Halls___.ttf"), 80);


            
            // backgroundSong = screenComponent.Content.Load<Song>(@"music\Twin Musicom - Spooky Ride.ogg");
            // MediaPlayer.Play(backgroundSong);
            // MediaPlayer.IsRepeating = true;
            // MediaPlayer.Volume = 0.3f;
            // TODO: song
        }

        public void PlayButtonClick()
        {
            var gameScreen = new GameScreen(ScreenComponent);
            ScreenComponent.NavigateTo(gameScreen);
            // MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            playButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            GraphicsDevice.Clear(Color.Black);

            const string heading = "Jackos Adventure";

            var x = (GraphicsDevice.Viewport.Width / 2f) - (font.MeasureString(heading).X / 2);
            var y = 20;

            batch.Begin();
            batch.Draw(headTexture, new Vector2(0, GraphicsDevice.Viewport.Height - headTexture.Height), Color.White);
            batch.DrawString(font,heading, new Vector2(x, y), Color.DarkOrange);
            batch.End();

            playButton.Draw(gameTime, batch);

            base.Draw(gameTime, batch);
        }

        public override void Dispose()
        {
            playButton.Dispose();
            headTexture.Dispose();
            font.Dispose();
            // backgroundSong.Dispose(); TODO: 

            base.Dispose();
        }
    }
}
