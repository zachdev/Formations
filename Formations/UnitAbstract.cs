using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    abstract class UnitAbstract : IUpdateDraw
    {
        private bool _isDead = false;
        private bool _isPlayersUnit;
        private int _life;
        protected int MaxLife;
        private int _range;
        private int _damage;
        private int _staminaAttCost;
        private int _staminaMoveCost;
        private int _staminaPlaceCost; 
        public bool isDead
        { 
            get { return _isDead; } 
            protected set { _isDead = value; } 
        }
        public int Life
        {
            get { return _life; }
            protected set { _life = value; }
        }
        public int Damage
        {
            get { return _damage; }
            protected set { _damage = value; }
        }
        public bool isPlayersUnit
        {
            get { return _isPlayersUnit; }
            protected set { _isPlayersUnit = value; }
        }
        public int Range 
        {
            get { return _range;}
            protected set { _range = value; } 
        }
        public int StaminaMoveCost
        {
            get { return _staminaMoveCost;}
            protected set { _staminaMoveCost = value; }
        }
        public int StaminaAttCost
        {
            get { return _staminaAttCost;}
            protected set { _staminaAttCost = value; }
        }
        public int StaminaPlaceCost
        {
            get { return _staminaPlaceCost;}
            protected set { _staminaPlaceCost = value; }
        }
        public abstract void init(bool isOwnedByPlayer);
        public abstract string  getUnitType();
        public abstract void attack(UnitAbstract unit);
        public abstract void defend(UnitAbstract unit);
        public abstract void manipulate(UnitAbstract unit);
        public abstract void calculateAtt(UnitAbstract unit);
        public abstract void calculateDef(UnitAbstract unit);
        public abstract void calculateMul(UnitAbstract unit);
        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
