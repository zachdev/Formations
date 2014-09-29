using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class PlayerBoard
    {
        private TileAbstract[] tiles = new TileBasic[1];

        public PlayerBoard()
        {
            tiles[0] = new TileBasic(100, 100);
        }

        public void init()
        {
            
            tiles[0].init();
        }

        public void update()
        {

        }

        public void draw()
        {
            tiles[0].draw();
        }
    }
}
