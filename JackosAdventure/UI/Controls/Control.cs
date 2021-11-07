using JackosAdventure.UI.Components;
using engenious;
using engenious.Graphics;
using System;
using engenious.Input;

namespace JackosAdventure.UI.Controls
{
    internal abstract class Control : IDisposable
    {
        protected ScreenGameComponent ScreenComponent { get; }
        protected GraphicsDevice GraphicsDevice { get; }
        protected SpriteBatch SpriteBatch { get; }

        private MouseState previousMouseState;

        public Control(ScreenGameComponent screenComponent)
        {
            ScreenComponent = screenComponent;
            GraphicsDevice = screenComponent.GraphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch) { }
        public virtual void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {
                OnLeftMouseButtonClicked(new Point(mouseState.X, mouseState.Y));
            }

            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed)
            {
                OnRightMouseButtonClicked(new Point(mouseState.X, mouseState.Y));
            }

            previousMouseState = mouseState;
        }

        protected virtual void OnLeftMouseButtonClicked(Point position)
        {

        }

        protected virtual void OnRightMouseButtonClicked(Point position)
        {

        }

        public virtual void Dispose()
        {
            SpriteBatch.Dispose();
        }
    }
}