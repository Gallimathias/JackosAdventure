using engenious;
using engenious.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JackosAdventure.Simulation.Entities
{
    internal abstract class Entitie : IDisposable
    {
        public Vector2 Position { get; set; }
        public abstract Vector2 Size { get; }
        public bool IsMoving { get; set; }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
        public virtual void Dispose() { }
    }
}
