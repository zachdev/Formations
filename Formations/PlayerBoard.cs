using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{   
    class PlayerBoard
    {
        private int sizeOfBoard = 4;
        private TileBasic[,] tiles = new TileBasic[6,6];

        public PlayerBoard()
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
               {
                   tiles[i,j] = new TileBasic();
               }
            }
        }

        public void init(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(100 + (i * 88), 100 + (j * 76), graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(144 + (i * 88), 100 + (j * 76), graphicsDevice);
                    }

                }
            }
            
        }
        public void mousePressed(MouseState mouseState)
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    if (tiles[i, j].isHovered())
                    {
                        if (tiles[i, j].isSelected())
                        {
                            tiles[i, j].setSelected(false);
                        }
                        else
                        {
                            tiles[i, j].setSelected(true);
                        }
                        
                    }
                }
            }
        }
        public void mouseReleased(MouseState mouseState)
        {

        }
        public void update(MouseState mouseState)
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    tiles[i, j].update(mouseState);
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    tiles[i,j].draw(spriteBatch);
                }
            }
        }
    }
}
