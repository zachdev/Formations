using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class UnitMag: UnitAbstract
    {
        public const int DAMAGE = 1;
        public const int LIFE = 1;
        public const int MAX_LIFE = 1;
        public const int RANGE = 1;
        public const int STAMINA_MOVE_COST = 1;
        public const int STAMINA_ATT_COST = 5;
        public const int STAMINA_PLACE_COST = 5;
        public override void init(bool isPlayerUnit)
        {
            this.isPlayersUnit = isPlayerUnit;
            this.Damage = DAMAGE;
            this.Life = LIFE;
            this.MaxLife = MAX_LIFE;
            this.Range = RANGE;
            this.StaminaAttCost = STAMINA_ATT_COST;
            this.StaminaMoveCost = STAMINA_MOVE_COST;
            this.StaminaPlaceCost = STAMINA_PLACE_COST;
        }
        public override string getUnitType()
        {
            return "Magic Unit";
        }
        public override void attack(UnitAbstract unit)
        {
            //can't attack maybe?
            //unit.defend(this);
        }
        public override void defend(UnitAbstract unit)
        {
            Life -= (unit.Damage);
            if (Life <= 0)
            {
                isDead = true;
            }
        }
        public override void manipulate(UnitAbstract unit)
        {
            if (unit.GetType() == typeof(UnitMag))
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
