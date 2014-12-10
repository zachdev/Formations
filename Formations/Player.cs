using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;
using System.Data.Entity;
using System.Runtime.Serialization;


namespace Formations
{

    [DataContract]
    public class Player
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
        [IgnoreDataMember]
        private StaminaComponent bar;
        [IgnoreDataMember]
        private UnitsComponent unitsBar;
        [IgnoreDataMember]
        private VertexPositionColor[] vertices = new VertexPositionColor[6];
        [IgnoreDataMember]
        private VertexPositionColor[] borderLines = new VertexPositionColor[8];
        [IgnoreDataMember]
        private Color borderColor;
        [IgnoreDataMember]
        private Manager uiManager;

        public string playerName { get; set; }

        public int playerId { get; set; }


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

        public Manager UiManager { get { return uiManager; } private set { uiManager = value; } }

        public UnitAtt[] AttUnitArray { get { return _attUnitArray;} }

        public UnitDef[] DefUnitArray { get { return _defUnitArray;} }

        public UnitMag[] MagUnitArray { get { return _magUnitArray;} }



        [IgnoreDataMember]
        private Label playersNameLabel;
        [IgnoreDataMember]
        private Label totalAttUnitLabel;
        [IgnoreDataMember]
        private Label totalDefUnitLabel;
        [IgnoreDataMember]
        private Label totalMagUnitLabel;
        [IgnoreDataMember]
        private Label staminaPointsLeft;
        [IgnoreDataMember]
        private Vector2 playerInfoLocation;
        [IgnoreDataMember]
        private Vector2 playerAttNumberLocation;
        [IgnoreDataMember]
        private Vector2 playerDefNumberLocation;
        [IgnoreDataMember]
        private Vector2 playerMagNumberLocation;
        [IgnoreDataMember]
        private Vector2 guestInfoLocation = new Vector2(190, 10);
        [IgnoreDataMember]
        private Vector2 guestAttNumberLocation = new Vector2(280, 41);
        [IgnoreDataMember]
        private Vector2 guestDefNumberLocation = new Vector2(280, 51);
        [IgnoreDataMember]
        private Vector2 guestMagNumberLocation = new Vector2(280, 62);
        [IgnoreDataMember]
        private Vector2 hostInfoLocation = new Vector2(790, 10);
        [IgnoreDataMember]
        private Vector2 hostAttNumberLocation = new Vector2(880, 41);
        [IgnoreDataMember]
        private Vector2 hostDefNumberLocation = new Vector2(880, 51);
        [IgnoreDataMember]
        private Vector2 hostMagNumberLocation = new Vector2(880, 62);

        [IgnoreDataMember]
        private Hexagon attHex;
        [IgnoreDataMember]
        private Hexagon defHex;
        [IgnoreDataMember]
        private Hexagon magHex;
        public Player(bool isHost)
        {
            this.IsHost = isHost;
        }

        public Player()
        {
            // TODO: Complete member initialization
        }

        public void init(string nameOfPlayer, UnitAbstract[,] units, GraphicsDevice graphicsDevice, Manager uiManager)
        {
            playerName = nameOfPlayer;
            this.uiManager = uiManager;
            uiManager.SetSkin(new Skin(uiManager, "Default"));
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
                borderColor = GameColors.HostControlOutsideColor;
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
                borderColor = GameColors.guestControlOutsideColor;
            }
            //Label

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
            Stamina += 30;
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
                setBorderColor(borderColor);
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
        public void update()
        {
            foreach (UnitAbstract unit in _attUnitArray)
            {
                if (unit != null) { unit.update(); }
            }
            foreach (UnitAbstract unit in _defUnitArray)
            {
                if (unit != null) { unit.update(); }
            }
            foreach (UnitAbstract unit in _magUnitArray)
            {
                if (unit != null) { unit.update(); }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (UnitAbstract unit in _attUnitArray)
            {
                if (unit != null) { unit.draw(spriteBatch); }
            }
            foreach (UnitAbstract unit in _defUnitArray)
            {
                if (unit != null) { unit.draw(spriteBatch);}
            }
            foreach (UnitAbstract unit in _magUnitArray)
            {
                if (unit != null) { unit.draw(spriteBatch); }
            }
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
                                                                                                                                                                                         