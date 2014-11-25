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
        public override void init(bool isPlayerUnit, Player player)
        {
            this.isPlayersUnit = isPlayerUnit;
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
            return "Magic Unit";
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
            foreach (TileBasic tile in surroundingTiles)
            {
                UnitAbstract unit = tile.getUnit();
                UnitDef defUnit;
                if (unit == null) { continue; }
                if (unit.Player.Equals(Player) && unit.GetType() == typeof(UnitDef))
                {
                    defUnit = (UnitDef)unit;
                    result = defUnit.absorbDamage(result);
                }
            }
            return result;
        }
        public override int calculateRange()
        {
            int calculatedRange = Range;
            TileBasic[] surroundingTiles = ContainingTile.getSurroundingTiles();
            foreach (TileBasic tile in surroundingTiles)
            {
                UnitAbstract unit = tile.getUnit();
                if (unit == null) { continue; }
                if (unit.Player.Equals(Player) && unit.GetType() == typeof(UnitMag))
                {
                    calculatedRange++;
                }
            }
            return calculatedRange;
        }
        public override TileBasic[] getAttackableTiles()
        {
            int calculatedRange = calculateRange();
            List<TileBasic> attackableTiles = ContainingTile.getSurroundingTiles().ToList<TileBasic>();
            for (int i = 0; i < calculatedRange - 1; i++)
            {
                foreach (TileBasic tile in attackableTiles)
                {
                    foreach (TileBasic newTile in tile.getSurroundingTiles())
                    {
                        if (!attackableTiles.Contains(newTile))
                        {
                            attackableTiles.Add(newTile);
                        }
                    }
                }                
            }
            return attackableTiles.ToArray<TileBasic>();
        }
        public override void update()
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
