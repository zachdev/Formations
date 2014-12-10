using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Formations
{
    [DataContract]
    public class SerialClass
    {
        //public Player[] players;
        public TileBasic[,] tiles;

        public SerialClass(Player[] players, TileBasic[,] tiles)
        {
            //this.players = players;
            this.tiles = tiles;
        }

    }
}
