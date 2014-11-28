using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class UnitDef : UnitAbstract
    {
        public const int DAMAGE = 1;
        public const int LIFE = 5;
        public const int MAX_LIFE = 5;
        public const int RANGE = 1;
        public const int STAMINA_MOVE_COST = 2;
        public const int STAMINA_ATT_COST = 2;
        public const int STAMINA_PLACE_COST = 5;
        private int absorbAmount = 1;
        public override void init(bool isHostsUnit, Player player)
        {
            this.IsHostsUnit = isHostsUnit;
            this.Damage = DAMAGE;
            this.Life = LIFE;
            this.MaxLife = MAX_LIFE;
            this.Range = RANGE;
            this.StaminaAttCost = STAMINA_ATT_COST;
            this.StaminaMoveCost = STAMINA_MOVE_COST;
            this.StaminaPlaceCost = STAMINA_PLACE_COST;
            this.Player = player;
        }
        public override string getUnitType()
        {
            return "Defense Unit";
        }
        public override void attack(UnitAbstract unit)
        {
            unit.defend(this);
        }
        public override void defend(UnitAbstract unit)
        {
            Life -= (calculateDamage(unit.calculateAtt()));
            if (Life <= 0)
            {
                isDead = true;
            }
        }
        public override int calculateAtt()
        {
            return Damage;
        }
        public override int calculateDamage(int attackDamage)
        {
            int result = attackDamage;
            TileBasic[] surroundingTiles = ContainingTile.getSurroundingTiles();
            for (int i = 1; i < surroundingTiles.Length; i++)//starts on 1 because 0 is its self
            {
                
                UnitAbstract unit = surroundingTiles[i].getUnit();
                UnitDef defUnit;
                if (unit == null) { continue; }
                if (unit.Player.Equals(Player) && unit.GetType() == typeof(UnitDef))
                {
                    if (result == 0) { return result; }
                    defUnit = (UnitDef)unit;
                    result = defUnit.absorbDamage(result);
                }
            }
            return result;
        }
        public override int calculateRange()
        {
            return Range;
        }

        public override void update()
        {

        }
        public int absorbDamage(int damage)
        {
            Life -= absorbAmount;
            damage -= absorbAmount;
            if (Life <= 0)
            {
                isDead = true;
            }
            return damage;
        }
        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
