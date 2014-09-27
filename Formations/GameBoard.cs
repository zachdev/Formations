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

        public void init(PlayerBoard hostBoard, PlayerBoard guestBoard)
        {
            this.hostBoard = hostBoard;
            this.guestBoard = guestBoard;

        }

        public void update()
        {

        }

        public void draw()
        {

        }
    }
}
