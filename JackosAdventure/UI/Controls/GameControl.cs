using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using JackosAdventure.UI.Components;
using JackosAdventure.Simulation.Entities;

namespace JackosAdventure.UI.Controls
{
    internal class GameControl : Control
    {
        private readonly ChunkRenderer renderer;
        private readonly Camera camera;
        private readonly Player player;
        private readonly Witch witch;
        private readonly Reaper reaper;
        private readonly Texture2D playerTexture;
        private readonly Texture2D witchTexture;
        private readonly Texture2D reaperTexture;

        public GameControl(ScreenGameComponent screenComponent) : base(screenComponent)
        {
            renderer = new ChunkRenderer(screenComponent);
            camera = new Camera(Vector3.UnitZ);

            playerTexture = screenComponent.Content.Load<Texture2D>("jacko_a_3.png");
            witchTexture = screenComponent.Content.Load<Texture2D>("witch_cauldron.png");
            reaperTexture = screenComponent.Content.Load<Texture2D>("reaper_blade_3.png");
            player = new Player(playerTexture);
            witch = new Witch(witchTexture);
            reaper = new Reaper(reaperTexture);
            witch.Position = new Vector2(30, 10);
            reaper.Position = new Vector2(2, 7);
            reaper.Area = new Rectangle(1, 1, 10, 10);
        }

        public override void Update(GameTime gameTime)
        {
            var keyBoardState = Keyboard.GetState();

            var dir = new Vector2(0, 0);
            player.IsMoving = false;

            if (keyBoardState.IsKeyDown(Keys.W))
            {
                dir += new Vector2(0, -1);
                player.IsMoving = true;
                player.CurrentDirection = Direction.Up;
            }

            if (keyBoardState.IsKeyDown(Keys.S))
            {
                dir += new Vector2(0, 1);
                player.IsMoving = true;
                player.CurrentDirection = Direction.Down;
            }

            if (keyBoardState.IsKeyDown(Keys.A))
            {
                dir += new Vector2(-1, 0);
                player.IsMoving = true;
                player.CurrentDirection = Direction.Left;
            }

            if (keyBoardState.IsKeyDown(Keys.D))
            {
                dir += new Vector2(1, 0);
                player.IsMoving = true;
                player.CurrentDirection = Direction.Right;
            }

            if (dir.LengthSquared() > 0)
                dir.Normalize();

            const float speed = 4f;

            camera.UpdateBounds(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 20);
            camera.Position += new Vector3(dir, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            camera.Update();
            
            player.Position = new Vector2(camera.Position.X, camera.Position.Y);
            player.Update(gameTime);

            witch.Update(gameTime);
            reaper.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            renderer.Draw(camera);

            var inverseMatrix = new Matrix
                (
                2.0f / GraphicsDevice.Viewport.Width, 0, 0, 0,
                0, -2.0f / GraphicsDevice.Viewport.Height, 0, 0,
                0, 0, 1, 0,
                 -1, 1, 0, 1
                );

            inverseMatrix = Matrix.Invert(inverseMatrix);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, transformationMatrix: camera.ViewProjection * inverseMatrix);

            witch.Draw(gameTime, spriteBatch);
            reaper.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            player.Draw(gameTime, spriteBatch, GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            

            spriteBatch.End();

        }

        public override void Dispose()
        {
            player.Dispose();
            playerTexture.Dispose();
            witch.Dispose();
            witchTexture.Dispose();
            reaperTexture.Dispose();
            reaper.Dispose();
            renderer.Dispose();
        }

        private Vector2 ScreenToWorld(Vector2 screen)
        {
            var pos = new Vector3(screen.X, screen.Y, 0);
            var viewport = GraphicsDevice.Viewport;

            pos = viewport.Unproject(pos, camera.Projection, camera.View, Matrix.Identity);

            return new Vector2(pos.X, pos.Y);
        }
    }
}
