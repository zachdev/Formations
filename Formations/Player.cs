using Microsoft.Xna.Framework;
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
        public TileBasic selectedTile{get; set;}
        private string playerName;
        private Vector2 playerInfoLocation = new Vector2(800,10);
        private Vector2 attNumberLocation = new Vector2(60, 192);
        private Vector2 defNumberLocation = new Vector2(60, 236);
        private Vector2 manipNumberLocation = new Vector2(60, 280);
        private UnitAtt[] attUnitArray = new UnitAtt[20];
        private UnitDef[] defUnitArray = new UnitDef[20];
        private UnitManipulate[] mulUnitArray = new UnitManipulate[20];
        private Manager uiManager;
        private Label playersNameLabel;
        private Label totalAttUnitLabel;
        private Label totalDefUnitLabel;
        private Label totalManipUnitLabel;
        private Label staminaPointsLeft;
        private int totalAtt = 0;
        private int totalDef = 0;
        private int totalMul = 0;
        public int _stamina;
        public int Stamina{ get{ return _stamina;}}
        private Hexagon attHex;
        private Hexagon defHex;
        private Hexagon mulHex;
        public Player()
        {
            _stamina = 5;
        }

        public void init(string nameOfPlayer, UnitAbstract[,] units, GraphicsDevice graphicsDevice, Manager uiManager)
        {
            playerName = nameOfPlayer;
            for (int i = 0; i < 20; i++)
            {
                if (units[0, i] != null)
                {
                    attUnitArray[i] = (UnitAtt)units[0, i];
                    attUnitArray[i].init(true);
                    totalAtt++;
                }

            }
            for (int i = 0; i < 20; i++)
            {
                if (units[1, i] != null)
                {
                    defUnitArray[i] = (UnitDef)units[1, i];
                    defUnitArray[i].init(true);
                    totalDef++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[2, i] != null)
                {
                    mulUnitArray[i] = (UnitManipulate)units[2, i];
                    mulUnitArray[i].init(true);
                    totalMul++;
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
            totalAttUnitLabel.SetPosition((int)attNumberLocation.X, (int)attNumberLocation.Y);
            totalAttUnitLabel.Text = totalAtt + "";
            totalDefUnitLabel = new Label(uiManager);
            totalDefUnitLabel.SetPosition((int)defNumberLocation.X, (int)defNumberLocation.Y);
            totalDefUnitLabel.Text = totalDef + "";
            totalManipUnitLabel = new Label(uiManager);
            totalManipUnitLabel.SetPosition((int)manipNumberLocation.X, (int)manipNumberLocation.Y);
            totalManipUnitLabel.Text = totalMul + "";
            staminaPointsLeft = new Label(uiManager);
            staminaPointsLeft.SetPosition((int)playerInfoLocation.X + 160, (int)playerInfoLocation.Y);
            staminaPointsLeft.Text = _stamina + "";
            uiManager.Add(playersNameLabel);
            uiManager.Add(totalAttUnitLabel);
            uiManager.Add(totalDefUnitLabel);
            uiManager.Add(totalManipUnitLabel);
            uiManager.Add(staminaPointsLeft);

            attHex = new Hexagon(20);
            defHex = new Hexagon(20);
            mulHex = new Hexagon(20);
            attHex.init(40, 200, graphicsDevice, GameColors.attUnitInsideColor, GameColors.attUnitOutsideColor);
            defHex.init(40, 245, graphicsDevice, GameColors.defUnitInsideColor, GameColors.defUnitOutsideColor);
            mulHex.init(40, 290, graphicsDevice, GameColors.mulUnitInsideColor, GameColors.mulUnitOutsideColor);

        }
        public int getTotalAtt()
        {
            return totalAtt;
        }
        public UnitAtt getAttUnit()
        {
            if (totalAtt > 0) 
            {
                totalAtt--;
                totalAttUnitLabel.Text = totalAtt + "";
                return attUnitArray[totalAtt]; 
            }
            return null;
        }
        public int getTotalDef()
        {
            return totalDef;
        }
        public UnitDef getDefUnit()
        {
            
            if (totalDef > 0) 
            { 
                totalDef--;
                totalDefUnitLabel.Text = totalDef + "";
                return defUnitArray[totalDef];
            }
            return null;
        }
        public int getTotalMul()
        {
            return totalMul;
        }
        public UnitManipulate getMulUnit()
        { 
            if (totalMul > 0) 
            {
                totalMul--;
                totalManipUnitLabel.Text = totalMul + "";
                return mulUnitArray[totalMul]; 
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
            mulHex.draw(spriteBatch);

        }    
    }
}
