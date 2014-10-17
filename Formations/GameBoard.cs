using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class GameBoard : IMouseListener, IKeyboardListener
    {
        private Player player;
        private Guest guest;
        private string gameName;
        private Vector2 gameNameLocation = new Vector2(500, 10);
        private SpriteFont font;
        private const int boardHeight = 10;
        private const int boardWidth = 19;
        private int tileSideLength = 30;
        private int boardOffsetX = 175;
        private int boardOffsetY = 130;
        private int xTileOffset = 27;
        private int xAdjustment = 53;
        private int yAdjustment = 46;

        private TileBasic[,] tiles = new TileBasic[boardWidth, boardHeight];
        private VertexPositionColor[] vertices = new VertexPositionColor[6];
        private VertexPositionColor[] borderLines = new VertexPositionColor[8];
        private VertexPositionColor[] buttonsBackground = new VertexPositionColor[6];
        private VertexPositionColor[] buttonsBorderLines = new VertexPositionColor[8];
        private BasicEffect basicEffect;
        public GameBoard()
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
               {
                   tiles[i,j] = new TileBasic(tileSideLength);
               }
            }
        }
        private UnitAbstract[,] createUnitArray(int numberAtt, int numberDef, int numberMul)
        {
            UnitAbstract[,] tempArray = new UnitAbstract[3,20];

            for (int i = 0; i < numberAtt; i++)
            {
                tempArray[0, i] = new UnitAtt();
            }
            for (int i = 0; i < numberAtt; i++)
            {
                tempArray[1, i] = new UnitDef();
            }
            for (int i = 0; i < numberAtt; i++)
            {
                tempArray[2, i] = new UnitMul();
            }
            return tempArray;
        }
        public void init(GraphicsDevice graphicsDevice, SpriteFont font, string gameName)
        {
            this.font = font;
            this.gameName = gameName;
            player = new Player();
            guest = new Guest();

            player.init("<PlayerNameHere>", createUnitArray(5, 5, 5), font, graphicsDevice);
            guest.init("<GuestNameHere>", createUnitArray(10, 3, 2), font);

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,       // left, right
                graphicsDevice.Viewport.Height, 0,      // bottom, top
                0, 1);                                  // near, far plane
            createBoardArea();
            createButtonArea();
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
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


        public void mousePressed(MouseState mouseState)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].isHovered())
                    {
                        tiles[i, j].mousePressed(mouseState);
                    }
                }
            }
        }
        public void mouseReleased(MouseState mouseState)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].isSelected())
                    {
                        tiles[i, j].mouseReleased(mouseState);
                    }
                }
            }
        }
        public void mouseDragged(MouseState mouseState)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].mouseDragged(mouseState);
                }
            }
        }
        public void mouseMoved(MouseState mouseState)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].mouseMoved(mouseState);
                }
            }
        }
        public void update()
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            
            basicEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, borderLines, 0, 4);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, buttonsBackground, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, buttonsBorderLines, 0, 4);
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i,j].draw(spriteBatch);
                }
            }
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].isSelected())
                    {
                        tiles[i, j].drawButtons(spriteBatch);
                    } 
                }
            }
            spriteBatch.DrawString(font, gameName, gameNameLocation, Color.White);
            guest.draw(spriteBatch);
            player.draw(spriteBatch);
        }
        private void createButtonArea()
        {

            float width = boardOffsetX - 50;
            float height = 600 - boardOffsetY - 10;
            buttonsBackground[0] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.MistyRose);
            buttonsBackground[1] = new VertexPositionColor(new Vector3(width, boardOffsetY - 40, 0), Color.MistyRose);
            buttonsBackground[2] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.MistyRose);
            buttonsBackground[3] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.MistyRose);
            buttonsBackground[4] = new VertexPositionColor(new Vector3(10, boardOffsetY + height, 0), Color.MistyRose);
            buttonsBackground[5] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.MistyRose);

            buttonsBorderLines[0] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.Blue);
            buttonsBorderLines[1] = new VertexPositionColor(new Vector3(width, boardOffsetY - 40, 0), Color.Blue);
            buttonsBorderLines[2] = new VertexPositionColor(new Vector3(width, boardOffsetY - 40, 0), Color.Blue);
            buttonsBorderLines[3] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[4] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[5] = new VertexPositionColor(new Vector3(10, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[6] = new VertexPositionColor(new Vector3(10, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[7] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.Blue);

        }
        private void createBoardArea()
        {
            float border = 10;
            float halfHexWidth = (float)Math.Sqrt(tileSideLength * tileSideLength - (tileSideLength / 2) * (tileSideLength / 2));
            float widthOfBoard = halfHexWidth * (boardWidth * 2) + boardWidth * 2 - 10;
            float heightOfBoard = tileSideLength * (boardHeight * 1.5f) + (boardHeight * 1.5f) - (tileSideLength / 2f) + border;
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
    }
}
