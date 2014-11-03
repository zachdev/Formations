using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class UnitManipulate: UnitAbstract
    {
        const double _Damage = 1;
        const double _Damage_Multiplyer = 1;
        const double _Defense = 1;
        const double _Defense_Multiplyer = 1;
        const double _Stamina_Move_Cost = 1;
        const double _Stamina_Att_Cost = 1;
        public override void init(bool isPlayerUnit)
        {
            this.isPlayersUnit = isPlayerUnit;
            this.Damage = _Damage;
            this.Defense = _Defense;
            this.staminaAttCost = _Stamina_Att_Cost;
            this.staminaMoveCost = _Stamina_Move_Cost;
        }
        public override string getUnitType()
        {
            return "Manipulate Unit";
        }
        public override void attack(UnitAbstract unit)
        {
            //can't attack maybe?
            //unit.defend(this);
        }
        public override void defend(UnitAbstract unit)
        {
            life -= (unit.Damage);
            if (life <= 0)
            {
                isDead = true;
            }
        }
        public override void manipulate(UnitAbstract unit)
        {
            if (unit.GetType() == typeof(UnitManipulate))
            {
                //setting functions here on unit
            }
            else if (unit.GetType() == typeof(UnitAtt))
            {
                //setting functions here on unit
            }
            else if (unit.GetType() == typeof(UnitDef))
            {
                //setting functions here on unit
            }
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
