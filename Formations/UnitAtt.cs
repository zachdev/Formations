using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class UnitAtt : UnitAbstract
    {
        const double _Damage = 2;
        const double _Damage_Multiplyer = 2;
        const double _Defense = 1;
        const double _Defense_Multiplyer = 1;
        public override void init(bool isPlayerUnit)
        {
            this.isPlayersUnit = isPlayerUnit;
            this.Damage = _Damage;
            this.Defense = _Defense;
        }
        public override string getUnitType()
        {
            return "Attack Unit";
        }
        public override void attack(UnitAbstract unit)
        {

        }
        public override void defend(UnitAbstract unit)
        {

        }
        public override void manipulate(UnitAbstract unit)
        {

        }
        public override void calculateAtt(UnitAbstract unit)
        {

        }
        public override void calculateDef(UnitAbstract unit)
        {

        }
        public override void calculateMul(UnitAbstract unit)
        {

        }
        public override void update()
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
