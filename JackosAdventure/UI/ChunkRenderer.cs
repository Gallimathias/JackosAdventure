using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using JackosAdventure.UI.Components;

namespace JackosAdventure.UI
{
    internal class ChunkRenderer : IDisposable
    {
        private readonly VertexBuffer vertexBuffer;
        private readonly IndexBuffer indexBuffer;
        private readonly GraphicsDevice graphicsDevice;
        private readonly Texture2D grass;
        private readonly BasicEffect basicEffect;

        public ChunkRenderer(ScreenGameComponent screenComponent)
        {
            graphicsDevice = screenComponent.GraphicsDevice;

            grass = screenComponent.Content.Load<Texture2D>("grass.png");
            basicEffect = new BasicEffect(graphicsDevice)
            {
                TextureEnabled = true
            };

            const int width = 100;
            const int height = 100;
            int vertexCount = width * height * 4;
            vertexBuffer
                = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, vertexCount, BufferUsage.None);

            indexBuffer
                = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, width * height * 6, BufferUsage.None);

            var vertices = new VertexPositionTexture[vertexCount];
            var indices = new ushort[indexBuffer.IndexCount];

            int vIndex = 0, iIndex = 0;

            for (float y = 0; y < height; y++)
            {
                for (float x = 0; x < width; x++)
                {
                    indices[iIndex++] = (ushort)(vIndex + 2);
                    indices[iIndex++] = (ushort)(vIndex + 1);
                    indices[iIndex++] = (ushort)(vIndex + 0);

                    indices[iIndex++] = (ushort)(vIndex + 1);
                    indices[iIndex++] = (ushort)(vIndex + 2);
                    indices[iIndex++] = (ushort)(vIndex + 3);


                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 0, y + 0, 0), new Vector2(0, 0));
                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 1, y + 0, 0), new Vector2(1, 0));
                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 0, y + 1, 0), new Vector2(0, 1));
                    vertices[vIndex++] = new VertexPositionTexture(new Vector3(x + 1, y + 1, 0), new Vector2(1, 1));
                }
            }
            vertexBuffer.SetData(vertices);
            indexBuffer.SetData(indices);
        }

        public void Dispose()
        {
            vertexBuffer.Dispose();
            indexBuffer.Dispose();
            grass.Dispose();
            basicEffect.Dispose();
        }

        public void Draw(Camera camera)
        {
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Projection;
            basicEffect.Texture = grass;
            basicEffect.World = Matrix.Identity;

            foreach (var p in basicEffect.CurrentTechnique.Passes)
            {
                p.Apply();


                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

            }
        }
    }
}
