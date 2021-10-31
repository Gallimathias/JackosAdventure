﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using JackosAdventure.UI.Components;

namespace JackosAdventure.UI.Controls
{
    internal class GameControl : Control
    {
        private readonly ChunkRenderer renderer;
        private readonly Camera camera;
        private readonly Player player;
        private readonly NPC_Witch witch;
        public Reaper Reaper { get; }
        private readonly Texture2D playerTexture;
        private readonly Texture2D witchTexture;
        private readonly Texture2D reaperTexture;

        private Viewport currentViewport;
        private Matrix inverseMatrix;

        public GameControl(ScreenGameComponent screenComponent) : base(screenComponent)
        {
            renderer = new ChunkRenderer(screenComponent);

            playerTexture = screenComponent.Content.Load<Texture2D>("jacko_a_3.png");
            witchTexture = screenComponent.Content.Load<Texture2D>("witch2_cauldron_3.png");
            reaperTexture = screenComponent.Content.Load<Texture2D>("reaper_3.png");
            player = new Player(playerTexture);
            witch = new NPC_Witch(witchTexture);
            Reaper = new Reaper(reaperTexture);
            witch.Position = new Vector2(30, 10);
           
            camera = new Camera(Vector3.UnitZ, player.Size);

        }

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
            Reaper.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            renderer.Draw(camera);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, transformationMatrix: camera.ViewProjection * inverseMatrix);

            witch.Draw(gameTime, spriteBatch, 0,0 );
            Reaper.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);

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
