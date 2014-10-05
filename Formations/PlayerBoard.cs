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
        private const int sizeOfBoard = 8;
        private int boardOffsetX = 610;
        private int boardOffsetY = 110;
        private int tileSideLength = 40;
        private int xTileOffset= 36;//used to give the tiles an offset for odd rows
        private int xAdjustment = 71;
        private int yAdjustment = 61;

        private TileBasic[,] tiles = new TileBasic[sizeOfBoard,sizeOfBoard];
        private VertexPositionColor[] vertices = new VertexPositionColor[6];
        private VertexPositionColor[] borderLines = new VertexPositionColor[8];
        private BasicEffect basicEffect;
        public PlayerBoard()
        {
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
               {
                   tiles[i,j] = new TileBasic(tileSideLength);
               }
            }
        }

        public void init(GraphicsDevice graphicsDevice)
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,       // left, right
                graphicsDevice.Viewport.Height, 0,      // bottom, top
                0, 1);                                  // near, far plane
            createBorder();
            for (int i = 0; i < sizeOfBoard; i++)
            {
                for (int j = 0; j < sizeOfBoard; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(boardOffsetX + (i * xAdjustment), boardOffsetY + (j * yAdjustment), graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(boardOffsetX + xTileOffset + (i * xAdjustment), boardOffsetY + (j * yAdjustment), graphicsDevice);
                    }

                }
            }
            
        }
        private void createBorder()
        {
            Console.WriteLine(sizeOfBoard*1.5 + " is the number" + tileSideLength);
            float border = 10;
            float halfHexWidth = (float)Math.Sqrt(tileSideLength * tileSideLength - (tileSideLength/2) * (tileSideLength/2));
            float widthOfBoard = halfHexWidth * (sizeOfBoard * 2) + sizeOfBoard*2 + border;
            float heightOfBoard = tileSideLength * (sizeOfBoard * 1.5f) + (sizeOfBoard * 1.5f) - (tileSideLength/2f) + border;
            vertices[0] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.MistyRose);
            vertices[1] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY - tileSideLength - border, 0), Color.MistyRose);
            vertices[2] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.MistyRose);
            vertices[3] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.MistyRose);
            vertices[4] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY + heightOfBoard, 0), Color.MistyRose);
            vertices[5] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.MistyRose);

            borderLines[0] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.Blue);
            borderLines[1] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY - tileSideLength - border, 0), Color.Blue);
            borderLines[2] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY - tileSideLength - border, 0), Color.Blue);
            borderLines[3] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[4] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[5] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[6] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[7] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.Blue);
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
            basicEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, borderLines, 0, 4);
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
