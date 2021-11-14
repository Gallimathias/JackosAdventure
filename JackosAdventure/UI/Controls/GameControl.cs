using JackosAdventure.Simulation.Entities;
using JackosAdventure.Simulation.World;
using JackosAdventure.UI.Components;
using engenious;
using engenious.Graphics;
using System.IO;
using engenious.Input;
using Color = engenious.Color;
using Rectangle = engenious.Rectangle;
using engenious.UI;
using System;
using engenious.UI.Controls;
using JackosAdventure.Simulation;

namespace JackosAdventure.UI.Controls
{
    internal class GameControl : Control, IDisposable
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

        private readonly SpriteFont font;
        private const string text = "*Die Hexe lacht*";
        private readonly Vector2 textSize;
        private bool witchInteracting;
        private DialogControl dialogControl;
        private Dialog witchDialog;

        public GameControl(ScreenComponent screenComponent) : base(screenComponent)
        {
            using var fileStream = File.OpenRead(Path.Combine(".", "Assets", "graveyard.map"));
            using var reader = new BinaryReader(fileStream);
            var map = Map.Deserialize(reader);

            renderer = new ChunkRenderer(screenComponent, map);

            playerTexture = screenComponent.Assets.Load<Texture2D>("jacko_a_3.png") ?? throw new FileNotFoundException();
            witchTexture = screenComponent.Assets.Load<Texture2D>("witch2_cauldron_3.png") ?? throw new FileNotFoundException();
            reaperTexture = screenComponent.Assets.Load<Texture2D>("reaper_3.png") ?? throw new FileNotFoundException();
            player = new Player(playerTexture);
            witch = new Witch(witchTexture);
            reaper = new Reaper(reaperTexture);
            witch.Position = new Vector2(30, 10);
            reaper.Position = new Vector2(2, 7);
            reaper.Area = new Rectangle(1, 1, 10, 10);

            camera = new Camera(Vector3.UnitZ, player.Size);

            font = screenComponent.Content.Load<SpriteFont>("fonts/golem-script") ?? throw new FileNotFoundException();
            Skin.Current.TextFont = screenComponent.Content.Load<SpriteFont>("fonts/Text") ?? throw new FileNotFoundException();
            textSize = font.MeasureString(text);
            // font = screenComponent.Fonts.GetFont(Path.Combine(".", "Assets", "fonts", "golem-script.ttf"), 48);
            // text = font.MakeText("*Die Hexe lacht*");

            var byebye = new Dialog.DialogOption("ByeBye");
            witchDialog = new Dialog("*hehehehehehe*", "Hexe", new Dialog.DialogOption("Hallo", new Dialog("Schuppe Schuppe", options: byebye)), byebye);

            dialogControl = new DialogControl(screenComponent)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(new Color(Color.Black, 0.7f)),
            };

            var grid = new Grid(screenComponent)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            grid.Rows.Add(new RowDefinition() { ResizeMode = ResizeMode.Parts, Height = 2 });
            grid.Rows.Add(new RowDefinition() { ResizeMode = ResizeMode.Parts, Height = 1 });
            grid.Columns.Add(new ColumnDefinition() { ResizeMode = ResizeMode.Parts, Width = 1 });

            grid.AddControl(dialogControl, 0, 1);
            Children.Add(grid);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            var deviceViewPort = ScreenManager.GraphicsDevice.Viewport;
            if (currentViewport.X != deviceViewPort.X
                || currentViewport.Y != deviceViewPort.Y
                || currentViewport.Width != deviceViewPort.Width
                || currentViewport.Height != deviceViewPort.Height)
            {
                ViewPortChanged(currentViewport, deviceViewPort);
                currentViewport = deviceViewPort;
            }
            var dir = new Vector2(0, 0);

            if (!dialogControl.Visible)
            {
                var keyBoardState = Keyboard.GetState();
                var isInteracting = keyBoardState.IsKeyDown(Keys.E);

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

                if (dir.LengthSquared > 0)
                    dir.Normalize();

                if (isInteracting && witch.InteractionArea.Contains((int)player.Position.X, (int)player.Position.Y))
                {
                    dialogControl.Show(witchDialog);
                }
            }
            const float speed = 4f;


            camera.Position += new Vector3(dir, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            camera.Update();

            player.Position = new Vector2(camera.Position.X, camera.Position.Y);
            player.Update(gameTime);

            witch.Update(gameTime);

            reaper.Update(gameTime);
        }

        protected override void OnDraw(SpriteBatch spriteBatch, Rectangle controlArea, GameTime gameTime)
        {
            base.OnDraw(spriteBatch, controlArea, gameTime);

            renderer.Draw(camera);

            spriteBatch.Begin(SpriteBatch.SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, camera.ViewProjection, false);

            witch.Draw(gameTime, spriteBatch);
            reaper.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void ViewPortChanged(Viewport currentViewPort, Viewport newViewPort)
        {
            camera.UpdateBounds(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height, 20);
        }


        public void Dispose()
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
            var viewport = ScreenManager.GraphicsDevice.Viewport;

            pos = viewport.Unproject(pos, camera.Projection, camera.View, Matrix.Identity);

            return new Vector2(pos.X, pos.Y);
        }
    }
}
