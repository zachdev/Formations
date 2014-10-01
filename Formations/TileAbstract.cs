using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Formations
{
    abstract class TileAbstract
    {
        protected Vector2 location { get; set; }
        public abstract void init(float x, float y, GraphicsDevice graphicsDevice);
        public abstract void update();

        public abstract void draw(SpriteBatch spriteBatch);
    }
}
