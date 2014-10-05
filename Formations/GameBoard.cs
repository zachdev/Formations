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
        private PlayerBoard playerBoard;
        private GuestBoard guestBoard;

        public GameBoard()
        {

        }

        public void init(GraphicsDevice graphicsDevice)
        {
            playerBoard = new PlayerBoard();
            guestBoard = new GuestBoard();
            playerBoard.init(graphicsDevice);
            guestBoard.init(graphicsDevice);
        }
        public void mousePressed(MouseState mouseState)
        {
            playerBoard.mousePressed(mouseState);
        }
        public void mouseReleased(MouseState mouseState)
        {
            playerBoard.mouseReleased(mouseState);
        }
        public void update(MouseState mouseState)
        {
            playerBoard.update(mouseState);
            guestBoard.update();

        }

        public void draw(SpriteBatch spriteBatch)
        { 
            playerBoard.draw(spriteBatch);
            guestBoard.draw(spriteBatch);
        }
    }
}
