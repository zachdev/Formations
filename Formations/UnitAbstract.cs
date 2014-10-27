using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    abstract class UnitAbstract : IUpdateDraw
    {
        public bool isDead = false;
        protected bool isPlayersUnit;
        public double life = 4;
        public double Damage;
        public double Defense;


        public abstract void init(bool isOwnedByPlayer);
        public bool isOwnedByPlayer()
        {
            return isPlayersUnit;
        }
        public abstract string  getUnitType();
        public abstract void attack(UnitAbstract unit);
        public abstract void defend(UnitAbstract unit);
        public abstract void manipulate(UnitAbstract unit);
        public abstract void calculateAtt(UnitAbstract unit);
        public abstract void calculateDef(UnitAbstract unit);
        public abstract void calculateMul(UnitAbstract unit);
        public double getLife()
        {
            return life;
        }
        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
