using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    
    class TileBasic : TileAbstract
    {
        private Hexagon hex;
        public TileBasic()
        {

        }

        public override void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            hex = new Hexagon();
            hex.init(x, y, graphicsDevice);

            
        }
        public override void update()
        {

        }
        public override void draw(SpriteBatch spriteBatch)
        {
            hex.draw(spriteBatch);
        }
    }
}
