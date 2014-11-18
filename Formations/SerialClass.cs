using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class SerialClass
    {
        Player person;
        Player guest;
        TileBasic[,] tiles;
        Chat chat;

        public SerialClass(Player person, Player guest, TileBasic[,] tiles, Chat chat)
        {
            this.person = person;
            this.guest = guest;
            this.tiles = tiles;
            this.chat = chat;
        }

    }
}
