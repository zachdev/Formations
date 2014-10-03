using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{   
    class PlayerBoard
    {
        private int sizeOfBoard = 1;
        private TileAbstract[,] tiles = new TileBasic[6,6];

        public PlayerBoard()
        {
            for (int i = 0; i <= sizeOfBoard; i++)
            {
                for (int j = 0; j <= sizeOfBoard; j++)
               {
                   tiles[i,j] = new TileBasic();
               }
            }
        }

        public void init(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i <= sizeOfBoard; i++)
            {
                for (int j = 0; j <= sizeOfBoard; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(100 + (i * 25), 100 + (j * 25), graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(125 + (i * 25), 125 + (j * 25), graphicsDevice);
                    }

                }
            }
            
        }

        public void update(Vector2 point)
        {
            for (int i = 0; i <= sizeOfBoard; i++)
            {
                for (int j = 0; j <= sizeOfBoard; j++)
                {
                    tiles[i, j].update(point);
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i <= sizeOfBoard; i++)
            {
                for (int j = 0; j <= sizeOfBoard; j++)
                {
                    tiles[i,j].draw(spriteBatch);
                }
            }
        }
    }
}
