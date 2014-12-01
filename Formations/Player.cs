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
        private TileBasic _selectedTile;
        private int _stamina = 5;
        private bool _isHost;
        public bool isPlayersTurn = false;
        private int _totalAttNotPlaced = 0;
        private int _totalDefNotPlaced = 0;
        private int _totalMagNotPlaced = 0;
        private int count = 0;
        private int borderPosition = 0;
        private UnitAtt[] _attUnitArray = new UnitAtt[20];
        private UnitDef[] _defUnitArray = new UnitDef[20];
        private UnitMag[] _magUnitArray = new UnitMag[20];
        private StaminaComponent bar;
        private UnitsComponent unitsBar;
        private VertexPositionColor[] vertices = new VertexPositionColor[6];
        private VertexPositionColor[] borderLines = new VertexPositionColor[8];
        private string playerName;
        public TileBasic SelectedTile
        {
            get { return _selectedTile; }
            set { _selectedTile = value; }
        }
        public int Stamina
        {
            get { return _stamina; }
            private set
            {
                _stamina = value;
                if (_stamina > 30) { _stamina = 30; }
            }
        }
        public bool IsHost
        {
            get { return _isHost; }
            private set { _isHost = value; }
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

        private Vector2 playerInfoLocation;
        private Vector2 playerAttNumberLocation;
        private Vector2 playerDefNumberLocation;
        private Vector2 playerMagNumberLocation;

        private Vector2 guestInfoLocation = new Vector2(190, 10);
        private Vector2 guestAttNumberLocation = new Vector2(280, 41);
        private Vector2 guestDefNumberLocation = new Vector2(280, 51);
        private Vector2 guestMagNumberLocation = new Vector2(280, 62);

        private Vector2 hostInfoLocation = new Vector2(790, 10);
        private Vector2 hostAttNumberLocation = new Vector2(880, 41);
        private Vector2 hostDefNumberLocation = new Vector2(880, 51);
        private Vector2 hostMagNumberLocation = new Vector2(880, 62);


        private Hexagon attHex;
        private Hexagon defHex;
        private Hexagon magHex;
        public Player(bool isHost)
        {
            this.IsHost = isHost;
        }

        public void init(string nameOfPlayer, UnitAbstract[,] units, GraphicsDevice graphicsDevice, Manager uiManager)
        {
            playerName = nameOfPlayer;
            for (int i = 0; i < 20; i++)
            {
                if (units[0, i] != null)
                {
                    _attUnitArray[i] = (UnitAtt)units[0, i];
                    _attUnitArray[i].init(IsHost, this);
                    AttUnitsNotPlaced++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[1, i] != null)
                {
                    _defUnitArray[i] = (UnitDef)units[1, i];
                    _defUnitArray[i].init(IsHost, this);
                    DefUnitsNotPlaced++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[2, i] != null)
                {
                    _magUnitArray[i] = (UnitMag)units[2, i];
                    _magUnitArray[i].init(IsHost, this);
                    MagUnitsNotPlaced++;
                }
            }
            if (IsHost)
            {
                playerInfoLocation = guestInfoLocation;
                playerAttNumberLocation = guestAttNumberLocation;
                playerDefNumberLocation = guestDefNumberLocation;
                playerMagNumberLocation = guestMagNumberLocation;
                bar = new StaminaComponent(300, 35);
                bar.init(graphicsDevice);
                unitsBar = new UnitsComponent(300, 50, _totalAttNotPlaced, _totalDefNotPlaced, _totalMagNotPlaced);
                unitsBar.init(graphicsDevice);
                createBoardArea(200, 30);
            }
            else
            {
                playerInfoLocation = hostInfoLocation;
                playerAttNumberLocation = hostAttNumberLocation;
                playerDefNumberLocation = hostDefNumberLocation;
                playerMagNumberLocation = hostMagNumberLocation;
                bar = new StaminaComponent(900, 35);
                bar.init(graphicsDevice);
                unitsBar = new UnitsComponent(900, 50, _totalAttNotPlaced, _totalDefNotPlaced, _totalMagNotPlaced);
                unitsBar.init(graphicsDevice);
                createBoardArea(800, 30);
            }
            //Label
            this.uiManager = uiManager;
            uiManager.SetSkin(new Skin(uiManager, "Default"));
            playersNameLabel = new Label(uiManager);
            playersNameLabel.SetPosition((int)playerInfoLocation.X, (int)playerInfoLocation.Y);
            playersNameLabel.Text = playerName;
            playersNameLabel.SetSize(190,20);
            playersNameLabel.Color = Color.White;
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
            staminaPointsLeft.SetPosition((int)playerInfoLocation.X - 15 , (int)playerInfoLocation.Y - 30);
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
            magHex.init(40, 290, graphicsDevice, GameColors.magUnitInsideColor, GameColors.magUnitOutsideColor);

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
            Stamina += 5;
            staminaPointsLeft.Text = Stamina + "";
        }
        public void useStamina(int staminaToUse)
        {
            Stamina -= staminaToUse;
            staminaPointsLeft.Text = Stamina + "";
        }
        public void setBorderColor(Color color)
        {
            for (int i = 0; i < borderLines.Length; i++)
            {
                borderLines[i].Color = color;
            }
        }
        public void updateBoader()
        {
            if (count % 3 == 0)
            {
                borderPosition = (borderPosition + 1) % 8;
                if (_isHost)
                {
                    setBorderColor(Color.Blue);
                }
                else
                {
                    setBorderColor(Color.Yellow);
                }
                int positionOne = borderPosition;
                int positionTwo = (borderPosition + 7) % 8;
                borderLines[positionOne].Color = Color.Black;
                borderLines[positionTwo].Color = Color.Black;
            }
            count++;
        }
        public void resetUnits()
        {
            foreach (UnitAtt unit in AttUnitArray)
            {
                if (unit == null) { continue; }
                unit.resetAttacks();
            }
            foreach (UnitDef unit in DefUnitArray)
            {
                if (unit == null) { continue; }
                unit.resetAttacks();
            }
            foreach (UnitMag unit in MagUnitArray)
            {
                if (unit == null) { continue; }
                unit.resetAttacks();
            }
        }
        private void createBoardArea(int x, int y)
        {
            int boardX = x - 5;
            int boardY = y - 18;
            float border = 10;
                
            float widthOfBoard = 250;
            float heightOfBoard = 72;
            vertices[0] = new VertexPositionColor(new Vector3(boardX - border, boardY - border, 0), GameColors.playerAreaBackground);
            vertices[1] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY - border, 0), GameColors.playerAreaBackground);
            vertices[2] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY + heightOfBoard, 0), GameColors.playerAreaBackground);
            vertices[3] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY + heightOfBoard, 0), GameColors.playerAreaBackground);
            vertices[4] = new VertexPositionColor(new Vector3(boardX - border, boardY + heightOfBoard, 0), GameColors.playerAreaBackground);
            vertices[5] = new VertexPositionColor(new Vector3(boardX - border, boardY - border, 0), GameColors.playerAreaBackground);

            borderLines[0] = new VertexPositionColor(new Vector3(boardX - border, boardY - border, 0), Color.White);
            borderLines[1] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY - border, 0), Color.White);
            borderLines[2] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY - border, 0), Color.White);
            borderLines[3] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY + heightOfBoard, 0), Color.White);
            borderLines[4] = new VertexPositionColor(new Vector3(boardX + widthOfBoard, boardY + heightOfBoard, 0), Color.White);
            borderLines[5] = new VertexPositionColor(new Vector3(boardX - border, boardY + heightOfBoard, 0), Color.White);
            borderLines[6] = new VertexPositionColor(new Vector3(boardX - border, boardY + heightOfBoard, 0), Color.White);
            borderLines[7] = new VertexPositionColor(new Vector3(boardX - border, boardY - border, 0), Color.White);
        }
        public void update(MouseState mouseState)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            if(isPlayersTurn)
            {
                updateBoader();
            }
            else
            {
                setBorderColor(Color.White);
            }
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, borderLines, 0, 4);
            attHex.draw(spriteBatch);
            defHex.draw(spriteBatch);
            magHex.draw(spriteBatch);
            bar.updateBar(Stamina);
            unitsBar.updateUnitHex(AttUnitsNotPlaced, DefUnitsNotPlaced, MagUnitsNotPlaced);
            bar.draw(spriteBatch);
            unitsBar.draw(spriteBatch);

        }    
    }
}
                                                                                                                                                                                         