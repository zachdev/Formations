﻿using Microsoft.Xna.Framework.Graphics;
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
        private TileBasic _containingTile;
        private Player _player;
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
        public TileBasic ContainingTile
        {
            get { return _containingTile; }
            set { _containingTile = value; }
        }
        public Player Player
        {
            get { return _player; }
            protected set { _player = value; }
        }
        public abstract void init(bool isOwnedByPlayer, Player player);
        public abstract string  getUnitType();
        public abstract void attack(UnitAbstract unit);
        public abstract void defend(UnitAbstract unit);
        public abstract int calculateAtt();
        public abstract int calculateDamage(int attackDamage);
        public abstract int calculateRange();
        public abstract TileBasic[] getAttackableTiles();
        public abstract void update();
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
