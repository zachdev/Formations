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
    class GameBoardSinglePlayer : IMouseListener, IKeyboardListener
    {
        private Player[] players = new Player[2];
        private Manager uiManager;
        private string gameName;
        private Vector2 gameNameLocation = new Vector2(500, 10);
        private int movesLeftInPhase = 2;
        private bool isHost;
        private bool isHostsTurn = true;
        private bool isFirstPhase = true;
        private bool attackInProgress = false;
        private bool moveInProgress = false;
        private bool magicInProgress = false;
        private bool isSmallBoard = false;
        private bool endTurnIsVisible = false;
        private MouseState currentMouseState;
        private Label hexInfo;
        private Label gameInfo;
        private Label gameNameLabel;
        private Label phaseLabel;
        private Hexagon turnSignal;
        private Hexagon attUnit;
        private Hexagon defUnit;
        private Hexagon mulUnit;
        private Hexagon attAction;
        private Hexagon magicAction;
        private Hexagon moveAction;
        private TileBasic currentTile;
        private int unitSideLength;
        private const int boardHeight = 10;
        private const int boardWidth = 19;
        private int largeTileSideLength = 30;
        private int smallTileSideLength = 15;
        private float largeBoardOffsetX = 130;
        private float largeBoardOffsetY = 130;
        private float smallBoardOffsetX = 660;
        private float smallBoardOffsetY = 357;
        private float xTileOffset = 27.5F;
        private float xAdjustment = 55;
        private float yAdjustment = 47;
        private float changeInX;
        private float changeInY;
        private TileBasic[,] tiles = new TileBasic[boardWidth, boardHeight];
        private VertexPositionColor[] vertices = new VertexPositionColor[6];
        private VertexPositionColor[] borderLines = new VertexPositionColor[8];
        private BasicEffect basicEffect;

        // Chat class
        private Chat chatManager;
        private Button chatButton;

        // Particles
        private ParticleEngine attackParticleEngine;


        // Various buttons
        private Button resizeButton;
        private Button endTurn;
        private Window endTurnWindow;
        private Button endYesButton;
        private Button endNoButton;

        

        /// <summary>
        /// Default Constructor
        /// creates TilesArray for the Board
        /// </summary>
        public GameBoardSinglePlayer()
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
               {
                   tiles[i, j] = new TileBasic(largeTileSideLength);
               }
            }
            unitSideLength = largeTileSideLength / 2;
        }
        /// <summary>
        /// Initalizer for this object
        /// </summary>
        /// <param name="uiManager"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="font"></param>
        /// <param name="gameName"></param>
        public void init(Manager uiManager, GraphicsDevice graphicsDevice, string gameName, bool isHost)
        {
            this.gameName = gameName;
            this.isHost = isHost;
            this.uiManager = uiManager;

            players[0] = new Player(true);
            players[1] = new Player(false);
            /*
             * need to pass in the player from the connection here to create it maybe
             */
            if (isHost)
            {
                players[0].init("<PlayerNameHere>", createUnitArray(10, 5, 5), graphicsDevice, uiManager);
                players[1].init("<GuestNameHere>", createUnitArray(10, 5, 5), graphicsDevice, uiManager);
            }
            else
            {
                players[0].init("<GuestNameHere>", createUnitArray(10, 5, 5), graphicsDevice, uiManager);
                players[1].init("<PlayerNameHere>", createUnitArray(10, 5, 5), graphicsDevice, uiManager);
            }
            /*
             * setting up the graphics here
             */ 
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,       // left, right
                graphicsDevice.Viewport.Height, 0,      // bottom, top
                0, 1);                                  // near, far plane
            /*
             *setting up the board area
             */ 
            createBoardArea();
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(largeBoardOffsetX + (i * xAdjustment), largeBoardOffsetY + (j * yAdjustment), graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(largeBoardOffsetX + xTileOffset + (i * xAdjustment), largeBoardOffsetY + (j * yAdjustment), graphicsDevice);
                    }
                }
            }
            /*
             * runs through the board setting the surrounding tiles for each tile
             */ 
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].initTileArray(tiles, boardWidth, boardHeight, i, j);
                }
            }
            /*
             * finding the hrizontal distance change of a tile
             */ 
            changeInX = (float)Math.Sqrt((float)(largeTileSideLength * largeTileSideLength) - (float)(largeTileSideLength / 2) * (float)(largeTileSideLength / 2));
            changeInY = (float)(largeTileSideLength / 2);
            /*
             * setting up buttons and shapes
             */ 
            turnSignal = new Hexagon(20);
            attUnit = new Hexagon(unitSideLength);
            defUnit = new Hexagon(unitSideLength);
            mulUnit = new Hexagon(unitSideLength);
            attAction = new Hexagon(unitSideLength);
            moveAction = new Hexagon(unitSideLength);
            magicAction = new Hexagon(unitSideLength);
            /*
             * initiallizing the Hexagons
             */ 
            turnSignal.init(600,50, graphicsDevice, GameColors.turnButtonInsideColor, GameColors.turnButtonOutsideColor);
            attUnit.init(0, 0, graphicsDevice, GameColors.attButton, GameColors.attButton);
            mulUnit.init(0,0,graphicsDevice,GameColors.attButton,GameColors.attButton);
            defUnit.init(0, 0, graphicsDevice, GameColors.attButton, GameColors.attButton);
            attAction.init(0,0,graphicsDevice,GameColors.attButton,GameColors.attButton);
            moveAction.init(0, 0, graphicsDevice, GameColors.moveButton, GameColors.moveButton);
            magicAction.init(0, 0, graphicsDevice, GameColors.ManipulateButton, GameColors.ManipulateButton);
            /*
             * Resize Button
             */ 
            resizeButton = new Button(uiManager);
            resizeButton.SetPosition(10, 120);
            resizeButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.resizeBoard);
            resizeButton.Text = "Small Map";
            /*
             * End Turn Button/Window
             */ 
            endTurn = new Button(uiManager);
            endTurn.SetPosition(10, 150);
            endTurn.Click += new TomShane.Neoforce.Controls.EventHandler(this.toggleEndTurn);
            endTurn.Text = "EndTurn";
            endYesButton = new Button(uiManager);
            endYesButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.newTurn);
            endYesButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.toggleEndTurn);
            endYesButton.Text = "Yes";
            endYesButton.SetPosition(0, 0);
            endYesButton.SetSize(100, 100);
            endNoButton = new Button(uiManager);
            endNoButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.toggleEndTurn);
            endNoButton.Text = "No";
            endNoButton.SetPosition(0, 100);
            endNoButton.SetSize(100, 100);
            endTurnWindow = new Window(uiManager);
            endTurnWindow.SetSize(114,235);
            endTurnWindow.SetPosition(500, 250);
            endTurnWindow.Text = "  End Turn?";
            endTurnWindow.Shadow = true;
            endTurnWindow.CloseButtonVisible = false;
            endTurnWindow.Add(endYesButton);
            endTurnWindow.Add(endNoButton);
            /*
             * Game Name Label
             */ 
            gameNameLabel = new Label(uiManager);
            gameNameLabel.SetPosition(550,10);
            gameNameLabel.Text = gameName;
            gameNameLabel.SetSize(200,10);
            /*
             * phase label
             */
            phaseLabel = new Label(uiManager);
            phaseLabel.SetPosition(550, 70);
            phaseLabel.SetSize(200,20);
            phaseLabel.Text = "Phase 1 - Land Grab";
            /*
             * Chat stuff
             */ 
            chatManager = new Chat();
            chatManager.init(uiManager);
            chatButton = new Button(uiManager);
            chatButton.SetPosition(10, 90);
            chatButton.Click += new TomShane.Neoforce.Controls.EventHandler(chatManager.toggle);
            //chatButton.Click += new TomShane.Neoforce.Controls.EventHandler(resizeBoard);
            chatButton.Text = "Chat";
            
            Label chatLabel = new Label(uiManager);
            uiManager.Add(chatButton);
            uiManager.Add(endTurn);
            uiManager.Add(resizeButton);
            uiManager.Add(gameNameLabel);
            uiManager.Add(phaseLabel);
        }
        /// <summary>
        /// Creates the UnitAbstract Array with the correct number of attack units defense units and Manipulation units
        /// </summary>
        /// <param name="numberAtt">The correct number of Attack units to add to the UnitAbstract array</param> 
        /// <param name="numberDef">The correct number of Defensive units to add to the UnitAbstract array</param>
        /// <param name="numberMag">The correct number of Manipulation units to add to the UnitAbstract array</param>
        /// <returns></returns>
        private UnitAbstract[,] createUnitArray(int numberAtt, int numberDef, int numberMag)
        {
            UnitAbstract[,] tempArray = new UnitAbstract[3, 20];

            for (int i = 0; i < numberAtt; i++)
            {
                tempArray[0, i] = new UnitAtt();
            }
            for (int i = 0; i < numberDef; i++)
            {
                tempArray[1, i] = new UnitDef();
            }
            for (int i = 0; i < numberMag; i++)
            {
                tempArray[2, i] = new UnitMag();
            }
            return tempArray;
        }
        public void mousePressed(MouseState mouseState)
        {
            if (!chatManager.chatIsVisible())// Check if the chat window is visible
            {
                for (int i = 0; i < boardWidth; i++)
                {
                    for (int j = 0; j < boardHeight; j++)
                    {
                        //calculate here maybe
                        if (tiles[i, j].isHovered())
                        {
                            tiles[i, j].mousePressed(mouseState);
                            if (attackInProgress)
                            {
                                unitAttackUnit(mouseState);
                            }
                            if (moveInProgress)
                            {
                                moveUnit(mouseState);
                            }
                        }
                    }
                }
            }
        }
        public void showEndTurn()
        {
            uiManager.Add(endTurnWindow);
            endTurnIsVisible = true;
        }

        public void hideEndTurn()
        {
            uiManager.Remove(endTurnWindow);
            endTurnIsVisible = false;
        }
        public void toggleEndTurn(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (endTurnIsVisible)
            {
                hideEndTurn();
            }
            else
            {
                showEndTurn();
            }
        }
        public void mouseReleased(MouseState mouseState)
        {
            //selecting the correct player  
            Player self;
            if (!(isHost && isHostsTurn))
            {
                self = players[0];
            }
            else
            {
                self = players[1];
            }
            //checking board for where the mouse was released
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].isSelected())
                    {
                        tiles[i, j].mouseReleased(mouseState);
                        if (tiles[i, j].hasUnit() && ((isHostsTurn && tiles[i, j].getUnit().IsHostsUnit)
                            || (!isHostsTurn && !tiles[i, j].getUnit().IsHostsUnit))
                            && attAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        { 
                            //System.Console.WriteLine("Attack");
                            self.SelectedTile = tiles[i, j];
                            attackInProgress = true;
                            TileBasic[] attackableTiles = tiles[i, j].getUnit().getAttackableTiles();
                            foreach (TileBasic tile in attackableTiles)
                            {
                                tile.setAsAttackArea();
                            }
                            return;
                        }
                        if (tiles[i, j].hasUnit() && ((isHostsTurn && tiles[i, j].getUnit().IsHostsUnit)
                            || (!isHostsTurn && !tiles[i, j].getUnit().IsHostsUnit))
                            && moveAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            //Console.WriteLine("Move");
                            self.SelectedTile = tiles[i, j];
                            moveInProgress = true;
                            return;
                        }
                        if (tiles[i, j].hasUnit() && ((isHostsTurn && tiles[i, j].getUnit().IsHostsUnit)
                            || (!isHostsTurn && !tiles[i, j].getUnit().IsHostsUnit))
                            && magicAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            //Console.WriteLine("Magic");
                            self.SelectedTile = tiles[i, j];
                            magicInProgress = true;
                            return;
                        }
                        if (self.AttUnitsNotPlaced > 0  && attUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            if (playerCanSetUnit(i, j, mouseState) && self.Stamina >= UnitAtt.STAMINA_PLACE_COST)
                            {
                                tiles[i, j].setUnit(self.getAttUnit());
                                self.useStamina(UnitAtt.STAMINA_PLACE_COST);
                                move();

                            }
                        }
                        else if (self.DefUnitsNotPlaced > 0 && defUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            if (playerCanSetUnit(i, j, mouseState) && self.Stamina >= UnitDef.STAMINA_PLACE_COST) 
                            {
                                tiles[i, j].setUnit(self.getDefUnit());
                                self.useStamina(UnitDef.STAMINA_PLACE_COST);
                                move();
                            }
                        }
                        else if (self.MagUnitsNotPlaced > 0 && mulUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {

                            if (playerCanSetUnit(i, j, mouseState) && self.Stamina >= UnitMag.STAMINA_PLACE_COST)
                            { 
                                tiles[i, j].setUnit(self.getMagUnit());
                                self.useStamina(UnitMag.STAMINA_PLACE_COST);
                                move();
                            }
                        }
                    }
                }
            }
            
            recalculateControlArea();
        }
        public void mouseDragged(MouseState mouseState)
        {
            currentMouseState = mouseState;
            if (hexInfo == null)
            {
                hexInfo = new Label(uiManager);
                uiManager.Add(hexInfo);
                hexInfo.SetSize(100, 25);
                hexInfo.TextColor = Color.Black;
            }

            hexInfo.SetPosition(mouseState.X, mouseState.Y - 15);
            setHoverLabel(mouseState);
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].mouseDragged(mouseState);
                }
            }
        }
        public void mouseMoved(MouseState mouseState)
        {
            currentMouseState = mouseState;

            if (!chatManager.chatIsVisible() && !endTurnIsVisible)
            {
                if (hexInfo == null)
                {
                    hexInfo = new Label(uiManager);
                    uiManager.Add(hexInfo);
                    hexInfo.SetSize(150, 25);
                    hexInfo.TextColor = Color.Black;

                }

                hexInfo.SetPosition(mouseState.X, mouseState.Y - 15);
                setHoverLabel(mouseState);

                for (int i = 0; i < boardWidth; i++)
                {
                    for (int j = 0; j < boardHeight; j++)
                    {
                        tiles[i, j].mouseMoved(mouseState);
                    }
                }
            }  
        }

        //TODO: change or remove
        private void setHoverLabel(MouseState mouseState)
        {
            if (attAction.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Attack";
            }
            else if (moveAction.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Move";
            }
            else if (magicAction.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Magic";
            }
            else if (attUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Set Attack Unit";
            }
            else if (defUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Set Defense Unit";
            }
            else if (mulUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Set Magic Unit";
            }
            else
            {
                hexInfo.Text = "";
                foreach (var tile in tiles)
                {
                    if (tile.isHovered() && tile.hasUnit())
                    {
                        hexInfo.Text = tile.getUnit().getUnitType();
                    }   
                }
            }
        }
        private void moveUnit(MouseState mouseState)
        {
            //selecting the correct player  
            Player self;
            if (!(isHost && isHostsTurn))
            {
                self = players[0];
            }
            else
            {
                self = players[1];
            }
            //
            TileBasic[] currentSurroundingTiles = self.SelectedTile.getSurroundingTiles();
            for (int i = 1; i < currentSurroundingTiles.Length; i++)
            {//starts on 1 because 0 is the attacker
                if (currentSurroundingTiles[i] != null && currentSurroundingTiles[i].isPointInTile(mouseState))
                {
                    //Console.WriteLine("moveUnit");
                    if (!currentSurroundingTiles[i].hasUnit())
                    {
                        if (self.Stamina >= currentSurroundingTiles[0].getUnit().StaminaMoveCost)
                        {
                            currentSurroundingTiles[i].setUnit(currentSurroundingTiles[0].getUnit());
                            self.useStamina(currentSurroundingTiles[0].getUnit().StaminaMoveCost);
                            currentSurroundingTiles[0].setUnit(null);
                        }
                        //Console.WriteLine("moveUnit Move");
                    }
                }
            }
            self.SelectedTile = null;
            moveInProgress = false;
        }
        private void unitAttackUnit(MouseState mouseState)
        {
            //selecting the correct player  
            Player self;
            if (!(isHost && isHostsTurn))
            {
                self = players[0];
            }
            else
            {
                self = players[1];
            }

            TileBasic[] currentAttackableTiles = self.SelectedTile.getUnit().getAttackableTiles();

            for (int i = 1; i < currentAttackableTiles.Length; i++)
            {//starts on 1 because 0 is the attacker
                if (currentAttackableTiles[i] != null && currentAttackableTiles[i].isPointInTile(mouseState) && currentAttackableTiles[i].hasUnit() && !(currentAttackableTiles[i].getUnit().Player.Equals(self)))
                {
                    if(self.Stamina >= self.SelectedTile.getUnit().calculateAttackCost())
                    {
                        self.useStamina(self.SelectedTile.getUnit().calculateAttackCost());
                        currentAttackableTiles[0].getUnit().attack(currentAttackableTiles[i].getUnit());

                        // Start particle effect
                        attackParticleEngine.particlesOn = true;
                        attackParticleEngine.EmitterLocation = new Vector2(currentAttackableTiles[i].getX(), currentAttackableTiles[i].getY());
                    }   
                    if (currentAttackableTiles[i].getUnit().isDead)
                    {
                        currentAttackableTiles[i].setUnit(null);
                        //maybe some death effects here i.e blood and gore
                    }
                }
            }
            self.SelectedTile = null;
            attackInProgress = false;
        }
        private void move()
        {
            if (isFirstPhase)
            { 
                movesLeftInPhase--;
                showEndTurn();
                //update phase info here
            }
        }
        private bool playerCanSetUnit(int tileX, int tileY, MouseState mouseState)
        {
            bool result = false;
            //selecting the correct player  
            Player self;
            if (!(isHost && isHostsTurn))
            {
                self = players[0];
            }
            else
            {
                self = players[1];
            }

            if (isFirstPhase && (((tiles[tileX, tileY].isHostControlled() == true) && (tiles[tileX, tileY].isGuestControlled() == false) && self.IsHost)
                || ((tiles[tileX, tileY].isHostControlled() == false) && (tiles[tileX, tileY].isGuestControlled() == true) && !self.IsHost)
                || ((tiles[tileX, tileY].isHostControlled() == false) && (tiles[tileX, tileY].isGuestControlled() == false))))
            {
                result = true; 
            }
            else if (((tiles[tileX, tileY].isHostControlled() == true) && (tiles[tileX, tileY].isGuestControlled() == false) && self.IsHost)
                || ((tiles[tileX, tileY].isHostControlled() == false) && (tiles[tileX, tileY].isGuestControlled() == true) && !self.IsHost))
            {
                result = true; 
            }
            return result;
        }
        private void newTurn(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            //selecting the correct player  
            Player self;
            if (!(isHost && isHostsTurn))
            {
                self = players[0];
            }
            else
            {
                self = players[1];
            }
            //need to reset units to 0 attacks

            self.resetUnits();
            //switching turns here
            if (isHostsTurn)
            {
                turnSignal.setInsideColor(GameColors.turnButtonInsideColorGuest);
                isHostsTurn = false;
            }
            else
            {
                turnSignal.setInsideColor(GameColors.turnButtonInsideColor);
                isHostsTurn = true;
            }
            self.newTurn();
            //checking if phase 1 has ended
            if (movesLeftInPhase == 0)
            {
                isFirstPhase = false;
                phaseLabel.Text = "Phase 2 - Game Phase";

            }
        }
        private void recalculateControlArea()
        {
            //resets the board to have no control
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].updateHostControl(false);
                    tiles[i, j].updateGuestControl(false);
                }
            }
            //finds units and set the area of their control
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].getUnit() == null) { continue; }
                    if (tiles[i, j].getUnit().IsHostsUnit) { tiles[i, j].updateSurroundingTilesControl(true, true); }
                    if (!tiles[i, j].getUnit().IsHostsUnit) { tiles[i, j].updateSurroundingTilesControl(true, false); }
                }
            }
        }
        public void update()
        {
            //attackParticleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            
            attackParticleEngine.Update();

        }

        public void draw(SpriteBatch spriteBatch)
        {
            resetButtons();
            basicEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, borderLines, 0, 4);
            bool found = false;
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i,j].draw(spriteBatch);
                    if (tiles[i, j].isHovered())
                    {
                       // Console.WriteLine("selected: " + i + ", " + j);
                        currentTile = tiles[i, j];
                        found = true;
                    }
                    else if(!found){
                        currentTile = null;
                    }
                }
            }
            drawUnitButtons(currentTile, spriteBatch);
            // drawUnitInfo(spriteBatch);
            players[0].draw(spriteBatch);
            players[1].draw(spriteBatch);
            turnSignal.draw(spriteBatch);
            // Particles
            attackParticleEngine.Draw(spriteBatch);
        }
        private void drawUnitButtons(TileBasic currentTile, SpriteBatch spriteBatch)
        {
            if (currentTile == null)
            {
                return;
            }
            drawUnitInfo(spriteBatch);
            float x = currentTile.getX();
            float y = currentTile.getY();
            UnitAbstract currentUnit = currentTile.getUnit();
            if (moveInProgress)
            {
                moveAction.moveHex(currentMouseState.X, currentMouseState.Y, GameColors.moveButton, GameColors.moveButton);
                moveAction.draw(spriteBatch);
            }
            else if(attackInProgress)
            {
                attAction.moveHex(currentMouseState.X, currentMouseState.Y, GameColors.attButton, GameColors.attButton);
                attAction.draw(spriteBatch);
            }
            else if(magicInProgress)
            {

            }
            else
            {
                if (!isFirstPhase && currentTile.hasUnit() && currentTile.isSelected() && !isSmallBoard)
                {

                    if ((currentUnit.IsHostsUnit && isHostsTurn) || (!currentUnit.IsHostsUnit && !isHostsTurn))
                    {
                        attAction.moveHex(x + changeInX, y - changeInY, GameColors.attButton, GameColors.attButton);
                        moveAction.moveHex(x - changeInX, y - changeInY, GameColors.moveButton, GameColors.moveButton);
                        attAction.draw(spriteBatch);
                        moveAction.draw(spriteBatch);
                        if (currentUnit.GetType() == typeof(UnitMag))
                        {
                            magicAction.moveHex(x, y - largeTileSideLength, GameColors.ManipulateButton, GameColors.ManipulateButton);
                            magicAction.draw(spriteBatch);
                        }
                    }
                }
                else if (!currentTile.hasUnit() && currentTile.isSelected() && !isSmallBoard)
                {
                    attUnit.moveHex(x, y - largeTileSideLength, GameColors.attUnitInsideColor, GameColors.attUnitOutsideColor);
                    defUnit.moveHex(x + changeInX, y - changeInY, GameColors.defUnitInsideColor, GameColors.defUnitOutsideColor);
                    mulUnit.moveHex(x - changeInX, y - changeInY, GameColors.mulUnitInsideColor, GameColors.mulUnitOutsideColor);
                    attUnit.draw(spriteBatch);
                    defUnit.draw(spriteBatch);
                    mulUnit.draw(spriteBatch);
                }
            }
        }
        private void resetButtons()
        {
            createButtonArea();
            attAction.moveHex(-100, -100, GameColors.attButton, GameColors.attButton);
            moveAction.moveHex(-100, -100, GameColors.moveButton, GameColors.moveButton);
            magicAction.moveHex(-100, -100, GameColors.ManipulateButton, GameColors.ManipulateButton);
            attUnit.moveHex(-100, -100, GameColors.attUnitInsideColor, GameColors.attUnitOutsideColor);
            defUnit.moveHex(-100, -100, GameColors.defUnitInsideColor, GameColors.defUnitOutsideColor);
            mulUnit.moveHex(-100, -100, GameColors.mulUnitInsideColor, GameColors.mulUnitOutsideColor);
        }
        private void drawUnitInfo(SpriteBatch spriteBatch)
        {
            //if (currentTile.getUnit() != null) spriteBatch.DrawString(font, currentTile.getUnit().getUnitType(), new Vector2(buttonsBackground[0].Position.X, buttonsBackground[0].Position.Y), Color.Black);
            
        }
        private void createButtonArea()
        {

            float width = largeBoardOffsetX - 50;
            float height = 600 - largeBoardOffsetY - 10;
            if (gameInfo == null)
            {
                gameInfo = new Label(uiManager);
                gameInfo.Init();
                gameInfo.Height = (int)height;
                gameInfo.Width = (int)width;
                //gameInfo.DrawBorders = true;
                gameInfo.MaximumWidth = (int)width;
                gameInfo.SetPosition(10, (int)largeBoardOffsetY);
                gameInfo.TextColor = Color.White;
                //gameInfo.WordWrap = true;
                uiManager.Add(gameInfo);
            }
            gameInfo.Text = "";
            UnitAbstract tempUnit;
            foreach (var tile in tiles)
            {   tempUnit = tile.getUnit();
                if (tile.isHovered() && tile.hasUnit())
                {
                    gameInfo.Text += "\n\n\n";
                    gameInfo.Text += tempUnit.getUnitType() + "\n";
                    gameInfo.Text += "Life: " + tempUnit.Life + "\n";
                    gameInfo.Text += "Damage: " + tempUnit.calculateAtt() + "\n";
                    gameInfo.Text += "Range: " + tempUnit.calculateRange() + "\n";
                    gameInfo.Text += "__________\n";
                    gameInfo.Text += "Stamina Cost\n";
                    gameInfo.Text += "Attack $" + tempUnit.calculateAttackCost() + "\n";
                    gameInfo.Text += "Move $" + tempUnit.StaminaMoveCost + "\n";
                    gameInfo.Text += "Place $" + tempUnit.StaminaPlaceCost + "\n";

                }
            }
        }
        private void resizeTiles(int multiplyer, float xOffset, float yOffset, int tileLength)
        {
            float currentBoardOffsetX = xOffset;
            float currentBoardOffsetY = yOffset;
            int currentTileSideLength = tileLength;

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].resizeHex(currentBoardOffsetX + (i * xAdjustment / multiplyer), currentBoardOffsetY + (j * yAdjustment / multiplyer), currentTileSideLength);
                    }
                    else
                    {
                        tiles[i, j].resizeHex(currentBoardOffsetX + xTileOffset / multiplyer + (i * xAdjustment / multiplyer), currentBoardOffsetY + (j * yAdjustment / multiplyer), currentTileSideLength);
                    }
                }
            }
        }
        private void resizeBoard(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
            float currentBoardOffsetX;
            float currentBoardOffsetY;
            int currenttileSideLength;
            int multiplyer;
            if (!isSmallBoard)
            {
                currentBoardOffsetX = smallBoardOffsetX;
                currentBoardOffsetY = smallBoardOffsetY;
                currenttileSideLength = smallTileSideLength;
                multiplyer = 2;
                resizeButton.Text = "Large Map";
                isSmallBoard = true;
            }
            else
            {
                currentBoardOffsetX = largeBoardOffsetX;
                currentBoardOffsetY = largeBoardOffsetY;
                currenttileSideLength = largeTileSideLength;
                multiplyer = 1;
                resizeButton.Text = "Small Map";
                isSmallBoard = false;
            }
            resizeTiles(multiplyer, currentBoardOffsetX, currentBoardOffsetY, currenttileSideLength);
            float border = 10/multiplyer;
            float halfHexWidth = (float)Math.Sqrt(currenttileSideLength * currenttileSideLength - (currenttileSideLength / 2) * (currenttileSideLength / 2));
            float widthOfBoard = (xAdjustment * boardWidth + border)/multiplyer;
            float heightOfBoard = (yAdjustment * boardHeight - currenttileSideLength + 2 * border) / multiplyer;
            vertices[0] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY - currenttileSideLength - border, 0), GameColors.boardAreaBackground);
            vertices[1] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY - currenttileSideLength - border, 0), GameColors.boardAreaBackground);
            vertices[2] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY + heightOfBoard, 0), GameColors.boardAreaBackground);
            vertices[3] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY + heightOfBoard, 0), GameColors.boardAreaBackground);
            vertices[4] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY + heightOfBoard, 0), GameColors.boardAreaBackground);
            vertices[5] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY - currenttileSideLength - border, 0), GameColors.boardAreaBackground);

            borderLines[0] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY - currenttileSideLength - border, 0), Color.Blue);
            borderLines[1] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY - currenttileSideLength - border, 0), Color.Blue);
            borderLines[2] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY - currenttileSideLength - border, 0), Color.Blue);
            borderLines[3] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[4] = new VertexPositionColor(new Vector3(currentBoardOffsetX + widthOfBoard, currentBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[5] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[6] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[7] = new VertexPositionColor(new Vector3(currentBoardOffsetX - halfHexWidth - border, currentBoardOffsetY - currenttileSideLength - border, 0), Color.Blue);
        }
        private void createBoardArea()
        {
            float border = 10;
            float halfHexWidth = (float)Math.Sqrt(largeTileSideLength * largeTileSideLength - (largeTileSideLength / 2) * (largeTileSideLength / 2));
            float widthOfBoard = (xAdjustment * boardWidth + border);
            float heightOfBoard = (yAdjustment * boardHeight - largeTileSideLength + 2 * border);
            vertices[0] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY - largeTileSideLength - border, 0), GameColors.boardAreaBackground);
            vertices[1] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY - largeTileSideLength - border, 0), GameColors.boardAreaBackground);
            vertices[2] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY + heightOfBoard, 0), GameColors.boardAreaBackground);
            vertices[3] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY + heightOfBoard, 0), GameColors.boardAreaBackground);
            vertices[4] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY + heightOfBoard, 0), GameColors.boardAreaBackground);
            vertices[5] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY - largeTileSideLength - border, 0), GameColors.boardAreaBackground);

            borderLines[0] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY - largeTileSideLength - border, 0), Color.Blue);
            borderLines[1] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY - largeTileSideLength - border, 0), Color.Blue);
            borderLines[2] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY - largeTileSideLength - border, 0), Color.Blue);
            borderLines[3] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[4] = new VertexPositionColor(new Vector3(largeBoardOffsetX + widthOfBoard, largeBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[5] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[6] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[7] = new VertexPositionColor(new Vector3(largeBoardOffsetX - halfHexWidth - border, largeBoardOffsetY - largeTileSideLength - border, 0), Color.Blue);
        }

        internal void setAttackParticleEngine(ParticleEngine attackParticleEngine)
        {
            this.attackParticleEngine = attackParticleEngine;
        }
    }
}
