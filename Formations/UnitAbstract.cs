using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    abstract class UnitAbstract : IUpdateDraw
    {
        protected bool isPlayersUnit;
        public abstract void init(bool isOwnedByPlayer);
        public bool isOwnedByPlayer()
        {
            return isPlayersUnit;
        }
        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
