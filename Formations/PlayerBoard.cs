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
            for (int i = 0; i <= 5; i++)
            {
                for(int j = 0; j <= 5; j++)
               {
                   tiles[i,j] = new TileBasic();
               }
            }
        }

        public void init(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i <= 5; i++ )
            {
                for(int j = 0; j <= 5; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(100 + (i * 50), 100 + (j * 44), graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(125 + (i * 50), 100 + (j * 44), graphicsDevice);
                    }
                }
            }
            
        }

        public void update()
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    tiles[i,j].draw(spriteBatch);
                }
            }
        }
    }
}
