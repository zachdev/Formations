using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class UnitDef : UnitAbstract
    {
        public override void init(bool isPlayerUnit)
        {
            this.isPlayersUnit = isPlayerUnit;

        }
        public override string getUnitType()
        {
            return "Defense Unit";
        }
        public override void update()
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
