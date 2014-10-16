using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    abstract class UnitAbstract : IUpdateDraw
    {
        public abstract void init();
        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
