using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class PlayerBoard
    {
        private TileAbstract[,] tiles = new TileBasic[6,6];

        public PlayerBoard()
        {
            for (int i = 0; i <= 4; i++)
            {
                for(int j = 0; j <= 4; j++)
               {
                   tiles[i,j] = new TileBasic();
               }
            }
        }

        public void init(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i <= 4; i++ )
            {
                for(int j = 0; j <= 4; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(i * 1.75f, -j * 1.52f, graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(.875f + i * 1.75f, -j * 1.52f, graphicsDevice);
                    }
                }
            }
            
        }

        public void update()
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    tiles[i,j].draw(spriteBatch);
                }
            }
        }
    }
}
