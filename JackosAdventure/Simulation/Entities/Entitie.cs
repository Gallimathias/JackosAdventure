using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackosAdventure.Simulation.Entities
{
    internal abstract class Entitie : IDisposable
    {
        public Vector2 Position { get; set; }
        public abstract Vector2 Size { get; }
        public bool IsMoving { get; internal set; }

        public virtual void Dispose()
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
