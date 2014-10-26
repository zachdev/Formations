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
        protected int life = 4;
        protected double Damage;
        protected double Defense;


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
        public int getLife()
        {
            return life;
        }
        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
