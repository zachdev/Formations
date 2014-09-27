using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Formations
{
    abstract class TileAbstract
    {
        protected Vector2 location { get; set; }
        public abstract void update();

        public abstract void draw();
    }
}
