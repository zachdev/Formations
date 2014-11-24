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
            unit.defend(this);
        }
        public override void defend(UnitAbstract unit)
        {
            Life -= (unit.Damage);
            if (Life <= 0)
            {
                isDead = true;
            }
        }
        public override int calculateAtt()
        {
            int temp = 0;
            return temp;
        }
        public override int calculateDamage()
        {
            int temp = 0;
            return temp;
        }
        public override int calculateMagic()
        {
            int temp = 0;
            return temp;
        }
        public override void update()
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
