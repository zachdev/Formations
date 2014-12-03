using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Formations
{
    class UnitAtt : UnitAbstract
    {
        public const int DAMAGE = 2;
        public const int LIFE = 2;
        public const int MAX_LIFE = 2;
        public const int RANGE = 1;
        public const int STAMINA_MOVE_COST = 1;
        public const int STAMINA_ATT_COST = 2;
        public const int STAMINA_PLACE_COST = 4;

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
            // Particle engine stuff
            bloodParticles = new ParticleEngine(Game1.bloodTextures, new Vector2(400, 240));
            attackParticles = new ParticleEngine(Game1.attackTextures, new Vector2(400, 240));
            healingParticles = new ParticleEngine(Game1.healingTextures, new Vector2(400, 240));
            floatingText = new FloatingText(this);
        }
        public override string getUnitType()
        {
            return "Attack Unit";
        }
        public override void attack(UnitAbstract unit)
        {
            // Start particle effect
            attackParticles.particlesOn = true;
            attackParticles.EmitterLocation = new Vector2(ContainingTile.getX(), ContainingTile.getY());
            unit.defend(this);
            incrementAttack();
        }
        public override void defend(UnitAbstract unit)
        {
            bloodParticles.particlesOn = true;
            bloodParticles.EmitterLocation = new Vector2(ContainingTile.getX(), ContainingTile.getY());
            int damage = calculateDamage(unit.calculateAtt());
            floatingText.displayDamageTaken(damage, true);
            Life -= (damage);
            if (Life <= 0)
            {
                isDead = true;
            }
        }
        public override int calculateAtt()
        {
            int result = Damage;
            TileBasic[] surroundingTiles = ContainingTile.getSurroundingTiles();
            for (int i = 1; i < surroundingTiles.Length; i++ )
            {
                if (surroundingTiles[i] == null) { continue; }
                UnitAbstract unit = surroundingTiles[i].getUnit();
                if (unit == null) { continue; }
                if (unit.Player.Equals(Player) && unit.GetType() == typeof(UnitAtt))
                {
                    result++;
                }
            }
            return result;
        }
        public override void getHealed()
        {
            healingParticles.particlesOn = true;
            healingParticles.EmitterLocation = new Vector2(ContainingTile.getX(), ContainingTile.getY());
        }
        public override int calculateRange()
        {
            return Range;
        }
        public override void update()
        {
            attackParticles.Update();
            bloodParticles.Update();
            healingParticles.Update();
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            attackParticles.Draw(spriteBatch);
            bloodParticles.Draw(spriteBatch);
            healingParticles.Draw(spriteBatch);
            // Damage text

            floatingText.draw(spriteBatch);
        }
    }
}
