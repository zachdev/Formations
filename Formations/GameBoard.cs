using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class GameBoard
    {
        private PlayerBoard hostBoard;
        private PlayerBoard guestBoard;

        public GameBoard()
        {

        }

        public void init(GraphicsDevice graphicsDevice)
        {
            hostBoard = new PlayerBoard();
            hostBoard.init(graphicsDevice);

        }

        public void update(Vector2 point)
        {
            hostBoard.update(point);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            hostBoard.draw(spriteBatch);
        }
    }
}
