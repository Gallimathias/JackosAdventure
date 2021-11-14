using System.IO;
using JackosAdventure.UI.Components;
using JackosAdventure.UI.Controls;
using engenious;
using engenious.Audio;
using engenious.Graphics;
using engenious.UI;
using engenious.UI.Controls;
using System;

namespace JackosAdventure.UI.Screens
{
    internal class MenuScreen : Screen, IDisposable
    {
        private readonly TextButton playButton;
        private readonly Texture2D headTexture;
        // private readonly Song backgroundSong;

        private readonly SpriteFont font;
        private readonly SpriteFont buttonFont;
        private readonly SoundEffect sound;
        private readonly SoundEffectInstance soundInstance;
        private readonly ScreenComponent baseScreenComponent;

        public MenuScreen(ScreenComponent screenComponent) : base(screenComponent)
        {
            baseScreenComponent = screenComponent;
            var width = ScreenManager.GraphicsDevice.Viewport.Width / 3;
            var height = 60;

            var x = (ScreenManager.GraphicsDevice.Viewport.Width - width) / 2;
            var y = (ScreenManager.GraphicsDevice.Viewport.Height - height) / 2;
            
            screenComponent.Game.IsMouseVisible = true;

            headTexture = screenComponent.Assets.Load<Texture2D>(@"jacko_head_8.png") ?? throw new FileNotFoundException();
            font = screenComponent.Content.Load<SpriteFont>("fonts/Halls___") ?? throw new FileNotFoundException();
            buttonFont = screenComponent.Content.Load<SpriteFont>("fonts/golem-script") ?? throw new FileNotFoundException();
            sound = screenComponent.Content.Load<SoundEffect>("music/Twin_Musicom-Spooky_Ride") ?? throw new FileNotFoundException();

            playButton = new TextButton(screenComponent, "Play")
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Font = buttonFont,
                Background = SolidColorBrush.SaddleBrown,
                TextColor = Color.White,
                Width = 400,
                Padding = Border.All(5),
                HoveredBackground = SolidColorBrush.SandyBrown,
            };
            playButton.LeftMouseClick += PlayButtonClick;

            Controls.Add(playButton);

            soundInstance = sound.CreateInstance();
            soundInstance.IsLooped = true;
            soundInstance.Volume = 0.3f;
            soundInstance.Play();
        }

        public void PlayButtonClick(Control sender, MouseEventArgs args)
        {
            soundInstance.Stop();
            var gameScreen = new GameScreen(baseScreenComponent);
            ScreenManager.NavigateToScreen(gameScreen);
        }

        protected override void OnDraw(SpriteBatch batch, Rectangle controlArea, GameTime gameTime)
        {
            base.OnDraw(batch, controlArea, gameTime);

            const string heading = "Jackos Adventure";

            var x = controlArea.X + (controlArea.Width / 2f) - (font.MeasureString(heading).X / 2);
            var y = controlArea.Y + 20;

            batch.Begin();
            batch.Draw(headTexture, new Vector2(controlArea.X, controlArea.Y + controlArea.Height - headTexture.Height), Color.White);
            batch.DrawString(font,heading, new Vector2(x, y), Color.DarkOrange);
            batch.End();
        }

        public void Dispose()
        {
            //headTexture.Dispose();
            //font.Dispose();
            // backgroundSong.Dispose(); TODO: 
            soundInstance.Dispose();

        }
    }
}
