using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class GameBoard
    {
        private PlayerBoard hostBoard;
        //private PlayerBoard guestBoard;

        public GameBoard()
        {

        }

        public void init(GraphicsDevice graphicsDevice)
        {
            hostBoard = new PlayerBoard();
            hostBoard.init(graphicsDevice);

        }
        public void mousePressed(MouseState mouseState)
        {
            hostBoard.mousePressed(mouseState);
        }
        public void mouseReleased(MouseState mouseState)
        {
            hostBoard.mouseReleased(mouseState);
        }
        public void update(MouseState mouseState)
        {
            hostBoard.update(mouseState);

        }

        public void draw(SpriteBatch spriteBatch)
        { 
            hostBoard.draw(spriteBatch);
        }
    }
}
