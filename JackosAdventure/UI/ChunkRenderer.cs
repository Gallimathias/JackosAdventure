using JackosAdventure.Simulation.World;
using JackosAdventure.UI.Components;
using engenious;
using engenious.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using engenious.UI;

namespace JackosAdventure.UI
{
    internal class ChunkRenderer : IDisposable
    {
        private readonly VertexBuffer vertexBuffer;
        private readonly IndexBuffer indexBuffer;
        private readonly GraphicsDevice graphicsDevice;
        private readonly BasicEffect basicEffect;
        private readonly TextureAtlas atlas;

        private readonly Map map;
        private ScreenComponent ScreenComponent { get; }

        public ChunkRenderer(ScreenComponent screenComponent, Map map)
        {
            ScreenComponent = screenComponent;
            graphicsDevice = screenComponent.GraphicsDevice;
            this.map = map;

            basicEffect = new BasicEffect(graphicsDevice)
            {
                TextureEnabled = true
            };

            var mapTextures = new List<string>();

            foreach (var type in map.TileTypes)
            {
                mapTextures.Add(type + ".png");
            }

            atlas = CreateTextureAtlas(graphicsDevice, mapTextures, screenComponent, 64, 64);

            using var atlasBmp = Texture2D.ToBitmap(atlas.Atlas);
            atlasBmp.Save("atlas.png", ImageFormat.Png);
            
            int width = map.Width;
            int height = map.Height;
            int vertexCount = width * height * 4;
            vertexBuffer
                = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, vertexCount);

            indexBuffer
                = new IndexBuffer(graphicsDevice, DrawElementsType.UnsignedShort, width * height * 6);

            var vertices = new VertexPositionTexture[vertexCount];
            var indices = new ushort[indexBuffer.IndexCount];

            int vIndex = 0, iIndex = 0;

            var grass = atlas.Textures["grass_blue.png"];

            for (float y = 0; y < height; y++)
            {
                for (float x = 0; x < width; x++)
                {
                    var tile = map.Tiles[(int)x, (int)y];
                    var texture = atlas.Textures[tile.Name + ".png"];

                    indices[iIndex++] = (ushort)(vIndex + 1);
                    indices[iIndex++] = (ushort)(vIndex + 2);
                    indices[iIndex++] = (ushort)(vIndex + 3);

                    indices[iIndex++] = (ushort)(vIndex + 1);
                    indices[iIndex++] = (ushort)(vIndex + 2);
                    indices[iIndex++] = (ushort)(vIndex + 0);


                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 0, y + 0, 0), texture);
                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 1, y + 0, 0), new Vector2(texture.X + atlas.TextureSize.X, texture.Y));
                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 0, y + 1, 0), new Vector2(texture.X, texture.Y + atlas.TextureSize.Y));
                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 1, y + 1, 0), new Vector2(texture.X + atlas.TextureSize.X, texture.Y + atlas.TextureSize.Y));
                }
            }
            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);
        }

        private TextureAtlas CreateTextureAtlas(GraphicsDevice graphicsDevice, List<string> textures, ScreenComponent screenComponent, int textureWidth, int textureHeight)
        {
            const int gap = 3;

            var dictionary = new Dictionary<string, Vector2>();

            var quadSize = (int)MathF.Ceiling(MathF.Sqrt(textures.Count));

            var width = quadSize * textureWidth + (quadSize - 1) * gap;
            var height = quadSize * textureHeight + (quadSize - 1) * gap;

            var textureAtlas
                = new RenderTarget2D(
                    graphicsDevice,
                    width,
                    height,
                    PixelInternalFormat.Rgb8
                );

            graphicsDevice.SetRenderTarget(textureAtlas);

            var disposableList = new List<Texture2D>();

            var batch = new SpriteBatch(graphicsDevice);
            var t = Matrix.CreateScaling(1,-1,1) * Matrix.CreateTranslation(0, -height, 0);
            batch.Begin(sortMode: SpriteBatch.SpriteSortMode.Immediate, rasterizerState: RasterizerState.CullCounterClockwise, transformMatrix: t);

            int x = 0, y = 0;
            var halfTexelX = 0.5f / width;
            var halfTexelY = 0.5f / height;

            foreach (var textureName in textures)
            {
                var texture = screenComponent.Assets.Load<Texture2D>(textureName) ?? throw new FileNotFoundException();
                disposableList.Add(texture);

                if (textureWidth != texture.Width || textureHeight != texture.Height)
                    throw new ArgumentException($"Textures should be of size {textureWidth}x{textureHeight}");

                batch.Draw(texture, new Rectangle(x, y, textureWidth, textureHeight), Color.White);
                var textureX = x / (float)width;
                var textureY = y / (float)height;
                dictionary.Add(textureName, new Vector2(textureX + halfTexelX, textureY + halfTexelY));

                x += textureWidth + gap;
                if (x + textureWidth > width)
                {
                    x = 0;
                    y += textureHeight + gap;
                }
            }

            batch.End();
            graphicsDevice.SetRenderTarget(null!);

            foreach (var texture in disposableList)
            {
                texture.Dispose();
            }

            return new TextureAtlas(dictionary, textureAtlas, new Vector2(textureWidth / (float)width - halfTexelX * 2, textureHeight / (float)height - halfTexelY * 2));
        }

        public void Dispose()
        {
            vertexBuffer.Dispose();
            indexBuffer.Dispose();
            basicEffect.Dispose();
        }

        public void Draw(Camera camera)
        {
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.VertexBuffer = vertexBuffer;
            graphicsDevice.IndexBuffer = indexBuffer;

            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.Texture = atlas.Atlas;
            basicEffect.World = Matrix.Identity;

            foreach (var p in basicEffect.CurrentTechnique!.Passes)
            {
                p.Apply();

                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.Triangles, 0, 0, (int)vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

            }
        }

        public class TextureAtlas
        {
            public Dictionary<string, Vector2> Textures { get; }
            public Texture2D Atlas { get; }
            public Vector2 TextureSize { get; }

            public TextureAtlas(Dictionary<string, Vector2> textures, Texture2D atlas, Vector2 size)
            {
                Textures = textures;
                Atlas = atlas;
                TextureSize = size;
            }

        }
    }
}
