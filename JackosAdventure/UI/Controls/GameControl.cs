using JackosAdventure.Simulation.Entities;
using JackosAdventure.Simulation.World;
using JackosAdventure.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Velentr.Font;

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

        private Viewport currentViewport;
        private Matrix inverseMatrix;

        private readonly Font font;
        private readonly Text text;
        private bool witchInteracting;

        public GameControl(ScreenGameComponent screenComponent) : base(screenComponent)
        {
            using var fileStream = File.OpenRead(Path.Combine(".", "Assets", "graveyard.map"));
            using var reader = new BinaryReader(fileStream);
            var map = Map.Deserialize(reader);

            renderer = new ChunkRenderer(screenComponent, map);

            playerTexture = screenComponent.Content.Load<Texture2D>("jacko_a_3.png");
            witchTexture = screenComponent.Content.Load<Texture2D>("witch2_cauldron_3.png");
            reaperTexture = screenComponent.Content.Load<Texture2D>("reaper_3.png");
            player = new Player(playerTexture);
            witch = new Witch(witchTexture);
            reaper = new Reaper(reaperTexture);
            witch.Position = new Vector2(30, 10);
            reaper.Position = new Vector2(2, 7);
            reaper.Area = new Rectangle(1, 1, 10, 10);

            camera = new Camera(Vector3.UnitZ, player.Size);

            font = screenComponent.Fonts.GetFont(Path.Combine(".", "Assets", "fonts", "golem-script.ttf"), 48);
            text = font.MakeText("*Die Hexe lacht*");

        }

        private double talkingTime;

        public override void Update(GameTime gameTime)
        {
            var deviceViewPort = GraphicsDevice.Viewport;
            if (currentViewport.X != deviceViewPort.X
                || currentViewport.Y != deviceViewPort.Y
                || currentViewport.Width != deviceViewPort.Width
                || currentViewport.Height != deviceViewPort.Height)
            {
                ViewPortChanged(currentViewport, deviceViewPort);
                currentViewport = deviceViewPort;
            }

            var keyBoardState = Keyboard.GetState();
            var isInteracting = keyBoardState.IsKeyDown(Keys.E);

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


            camera.Position += new Vector3(dir, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            camera.Update();

            player.Position = new Vector2(camera.Position.X, camera.Position.Y);
            player.Update(gameTime);

            witch.Update(gameTime);

            if (isInteracting && witch.InteractionArea.Contains((int)player.Position.X, (int)player.Position.Y))
            {
                witchInteracting = true;
                talkingTime = gameTime.TotalGameTime.TotalSeconds + 5;
            }

            if (!witch.InteractionArea.Contains((int)player.Position.X, (int)player.Position.Y)
                || gameTime.TotalGameTime.TotalSeconds > talkingTime)
            {
                witchInteracting = false;
            }

            reaper.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            renderer.Draw(camera);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, transformationMatrix: camera.ViewProjection * inverseMatrix);

            witch.Draw(gameTime, spriteBatch);
            reaper.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            if (witchInteracting)
            {
                var x = GraphicsDevice.Viewport.Width / 2 - text.Width / 2;
                var y = GraphicsDevice.Viewport.Height - text.Height - 10;
                spriteBatch.DrawString(text, new Vector2(x, y), Color.White);
            }
            spriteBatch.End();

        }

        private void ViewPortChanged(Viewport currentViewPort, Viewport newViewPort)
        {
            var inverseMatrix = new Matrix
                (
                2.0f / GraphicsDevice.Viewport.Width, 0, 0, 0,
                0, -2.0f / GraphicsDevice.Viewport.Height, 0, 0,
                0, 0, 1, 0,
                 -1, 1, 0, 1
                );

            this.inverseMatrix = Matrix.Invert(inverseMatrix);

            camera.UpdateBounds(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 20);
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
