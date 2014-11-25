﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace Formations
{   
    class Player
    {
        private TileBasic _selectedTile;
        private int _stamina = 5;
        private bool _isOpponent;
        private int _totalAttNotPlaced = 0;
        private int _totalDefNotPlaced = 0;
        private int _totalMagNotPlaced = 0;
        private UnitAtt[] _attUnitArray = new UnitAtt[20];
        private UnitDef[] _defUnitArray = new UnitDef[20];
        private UnitMag[] _magUnitArray = new UnitMag[20];
        private string playerName;
        public TileBasic SelectedTile
        {
            get { return _selectedTile; }
            set { _selectedTile = value; }
        }
        public int Stamina
        {
            get { return _stamina; }
            private set { _stamina = value; }
        }
        public bool IsOpponent
        {
            get { return _isOpponent; }
            private set { _isOpponent = value; }
        }
        public int AttUnitsNotPlaced
        {
            get { return _totalAttNotPlaced; }
            private set { _totalAttNotPlaced = value; }
        }
        public int DefUnitsNotPlaced
        {
            get { return _totalDefNotPlaced; }
            private set { _totalDefNotPlaced = value; }
        }
        public int MagUnitsNotPlaced
        {
            get { return _totalMagNotPlaced; }
            private set { _totalMagNotPlaced = value; }
        }
        public UnitAtt[] AttUnitArray { get { return _attUnitArray;} }
        public UnitDef[] DefUnitArray { get { return _defUnitArray;} }
        public UnitMag[] MagUnitArray { get { return _magUnitArray;} }


        private Manager uiManager;

        private Label playersNameLabel;
        private Label totalAttUnitLabel;
        private Label totalDefUnitLabel;
        private Label totalMagUnitLabel;
        private Label staminaPointsLeft;

        private Vector2 playerInfoLocation = new Vector2(800,10);
        private Vector2 playerAttNumberLocation = new Vector2(60, 192);
        private Vector2 playerDefNumberLocation = new Vector2(60, 236);
        private Vector2 playerMagNumberLocation = new Vector2(60, 280);

        private Vector2 opponentInfoLocation = new Vector2(200, 10);
        private Vector2 opponentAttNumberLocation = new Vector2(265, 38);
        private Vector2 opponentDefNumberLocation = new Vector2(265, 52);
        private Vector2 opponentMagNumberLocation = new Vector2(265, 68);


        private Hexagon attHex;
        private Hexagon defHex;
        private Hexagon magHex;
        public Player(bool isOpponent)
        {
            this.IsOpponent = isOpponent;
        }

        public void init(string nameOfPlayer, UnitAbstract[,] units, GraphicsDevice graphicsDevice, Manager uiManager)
        {
            playerName = nameOfPlayer;
            for (int i = 0; i < 20; i++)
            {
                if (units[0, i] != null)
                {
                    _attUnitArray[i] = (UnitAtt)units[0, i];
                    _attUnitArray[i].init(!IsOpponent, this);
                    AttUnitsNotPlaced++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[1, i] != null)
                {
                    _defUnitArray[i] = (UnitDef)units[1, i];
                    _defUnitArray[i].init(!IsOpponent, this);
                    DefUnitsNotPlaced++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[2, i] != null)
                {
                    _magUnitArray[i] = (UnitMag)units[2, i];
                    _magUnitArray[i].init(!IsOpponent, this);
                    MagUnitsNotPlaced++;
                }
            }

            //Label
            this.uiManager = uiManager;
            uiManager.SetSkin(new Skin(uiManager, "Default"));
            playersNameLabel = new Label(uiManager);
            playersNameLabel.SetPosition((int)playerInfoLocation.X, (int)playerInfoLocation.Y);
            playersNameLabel.Text = playerName;
            playersNameLabel.SetSize(150,20);
            totalAttUnitLabel = new Label(uiManager);
            totalAttUnitLabel.SetPosition((int)playerAttNumberLocation.X, (int)playerAttNumberLocation.Y);
            totalAttUnitLabel.Text = AttUnitsNotPlaced + "";
            totalDefUnitLabel = new Label(uiManager);
            totalDefUnitLabel.SetPosition((int)playerDefNumberLocation.X, (int)playerDefNumberLocation.Y);
            totalDefUnitLabel.Text = DefUnitsNotPlaced + "";
            totalMagUnitLabel = new Label(uiManager);
            totalMagUnitLabel.SetPosition((int)playerMagNumberLocation.X, (int)playerMagNumberLocation.Y);
            totalMagUnitLabel.Text = MagUnitsNotPlaced + "";
            staminaPointsLeft = new Label(uiManager);
            staminaPointsLeft.SetPosition((int)playerInfoLocation.X + 160, (int)playerInfoLocation.Y);
            staminaPointsLeft.Text = Stamina + "";
            uiManager.Add(playersNameLabel);
            uiManager.Add(totalAttUnitLabel);
            uiManager.Add(totalDefUnitLabel);
            uiManager.Add(totalMagUnitLabel);
            uiManager.Add(staminaPointsLeft);

            attHex = new Hexagon(20);
            defHex = new Hexagon(20);
            magHex = new Hexagon(20);
            attHex.init(40, 200, graphicsDevice, GameColors.attUnitInsideColor, GameColors.attUnitOutsideColor);
            defHex.init(40, 245, graphicsDevice, GameColors.defUnitInsideColor, GameColors.defUnitOutsideColor);
            magHex.init(40, 290, graphicsDevice, GameColors.mulUnitInsideColor, GameColors.mulUnitOutsideColor);

        }
        /// <summary>
        /// gets the next unplaced Attack Unit
        /// </summary>
        /// <returns></returns>
        public UnitAtt getAttUnit()
        {
            if (AttUnitsNotPlaced > 0) 
            {
                AttUnitsNotPlaced--;
                totalAttUnitLabel.Text = AttUnitsNotPlaced + "";
                return _attUnitArray[AttUnitsNotPlaced]; 
            }
            return null;
        }
        /// <summary>
        /// gets the next unplaced Defense Unit
        /// </summary>
        /// <returns></returns>
        public UnitDef getDefUnit()
        {
            
            if (DefUnitsNotPlaced > 0) 
            { 
                DefUnitsNotPlaced--;
                totalDefUnitLabel.Text = DefUnitsNotPlaced + "";
                return _defUnitArray[DefUnitsNotPlaced];
            }
            return null;
        }
        /// <summary>
        /// gets the next unplaced Magic Unit
        /// </summary>
        /// <returns></returns>
        public UnitMag getMagUnit()
        { 
            if (MagUnitsNotPlaced > 0) 
            {
                MagUnitsNotPlaced--;
                totalMagUnitLabel.Text = MagUnitsNotPlaced + "";
                return _magUnitArray[MagUnitsNotPlaced]; 
            }
            return null;
        }
        public void newTurn()
        {
            _stamina += 5;
            staminaPointsLeft.Text = _stamina + "";
        }
        public void useStamina(int staminaToUse)
        {
            _stamina -= staminaToUse;
            staminaPointsLeft.Text = _stamina + "";
        }
        public void update(MouseState mouseState)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            attHex.draw(spriteBatch);
            defHex.draw(spriteBatch);
            magHex.draw(spriteBatch);

        }    
    }
}
