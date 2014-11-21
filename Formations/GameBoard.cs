using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using TomShane.Neoforce.Controls;

namespace Formations
{
    class GameBoard : IMouseListener, IKeyboardListener
    {
        private Player player;
        private Guest guest;
        private Manager uiManager;
        private string gameName;
        private Vector2 gameNameLocation = new Vector2(500, 10);
        private int movesLeftInPhase = 2;
        private bool isPlayerTurn = true;
        private bool isFirstPhase = true;
        private bool attackInProgress = false;
        private bool moveInProgress = false;
        private bool manipulateInProgress = false;
        private bool isSmallBoard = false;
        private bool endTurnIsVisible = false;
        private MouseState currentMouseState;
        private Label hexInfo;
        private Label gameInfo;
        private Label gameNameLabel;
        private Hexagon turnButton;
        private Hexagon attUnit;
        private Hexagon defUnit;
        private Hexagon mulUnit;
        private Hexagon attAction;
        private Hexagon manipulateAction;
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
        public GameBoard()
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
        public void init(Manager uiManager, GraphicsDevice graphicsDevice, string gameName)
        {
            this.gameName = gameName;
            player = new Player();
            guest = new Guest();
            this.uiManager = uiManager;

            player.init("<PlayerNameHere>", createUnitArray(10, 5, 1), graphicsDevice, uiManager);
            guest.init("<GuestNameHere>", createUnitArray(10, 3, 2), graphicsDevice, uiManager);

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,       // left, right
                graphicsDevice.Viewport.Height, 0,      // bottom, top
                0, 1);                                  // near, far plane
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
            //setting the surrounding tiles for each tile
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].initTileArray(tiles, boardWidth, boardHeight, i, j);
                }
            }
            changeInX = (float)Math.Sqrt((float)(largeTileSideLength * largeTileSideLength) - (float)(largeTileSideLength / 2) * (float)(largeTileSideLength / 2));
            changeInY = (float)(largeTileSideLength / 2);
            turnButton = new Hexagon(20);
            attUnit = new Hexagon(unitSideLength);
            defUnit = new Hexagon(unitSideLength);
            mulUnit = new Hexagon(unitSideLength);
            
            attAction = new Hexagon(unitSideLength);
            manipulateAction = new Hexagon(unitSideLength);
            moveAction = new Hexagon(unitSideLength);
            attUnit.init(0, 0, graphicsDevice, GameColors.attButton, GameColors.attButton);
            mulUnit.init(0,0,graphicsDevice,GameColors.attButton,GameColors.attButton);
            defUnit.init(0, 0, graphicsDevice, GameColors.attButton, GameColors.attButton);
            attAction.init(0,0,graphicsDevice,GameColors.attButton,GameColors.attButton);
            moveAction.init(0, 0, graphicsDevice, GameColors.moveButton, GameColors.moveButton);
            manipulateAction.init(0, 0, graphicsDevice, GameColors.ManipulateButton, GameColors.ManipulateButton);
            turnButton.init(500,50, graphicsDevice, GameColors.turnButtonInsideColor, GameColors.turnButtonOutsideColor);
            


            //Resize Button
            resizeButton = new Button(uiManager);
            resizeButton.SetPosition(10, 150);
            resizeButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.resizeBoard);
            resizeButton.Text = "Small Map";

            //End Turn Button/Window
            endTurn = new Button(uiManager);
            endTurn.SetPosition(10, 100);
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
            //Game Name Label
            gameNameLabel = new Label(uiManager);
            gameNameLabel.SetPosition(550,10);
            gameNameLabel.Text = gameName;
            gameNameLabel.SetSize(200,10);

            // Chat stuff
            chatManager = new Chat();
            chatManager.init(uiManager);
            chatButton = new Button(uiManager);
            chatButton.SetPosition(1125, 10);
            chatButton.Click += new TomShane.Neoforce.Controls.EventHandler(chatManager.toggle);
            chatButton.Text = "Chat";
            
            Label chatLabel = new Label(uiManager);
            uiManager.Add(chatButton);
            uiManager.Add(endTurn);
            uiManager.Add(resizeButton);
            uiManager.Add(gameNameLabel);
        }
        /// <summary>
        /// Creates the UnitAbstract Array with the correct number of attack units defense units and Manipulation units
        /// </summary>
        /// <param name="numberAtt">The correct number of Attack units to add to the UnitAbstract array</param> 
        /// <param name="numberDef">The correct number of Defensive units to add to the UnitAbstract array</param>
        /// <param name="numberManip">The correct number of Manipulation units to add to the UnitAbstract array</param>
        /// <returns></returns>
        private UnitAbstract[,] createUnitArray(int numberAtt, int numberDef, int numberManip)
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
            for (int i = 0; i < numberManip; i++)
            {
                tempArray[2, i] = new UnitManipulate();
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
                            if (manipulateInProgress)
                            {
                                manipulateUnit(mouseState);
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
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].isSelected())
                    {
                        tiles[i, j].mouseReleased(mouseState);
                        if (tiles[i, j].hasUnit() && ((isPlayerTurn && tiles[i, j].getUnit().isOwnedByPlayer())
                            || (!isPlayerTurn && !tiles[i, j].getUnit().isOwnedByPlayer()))
                            && attAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        { 
                            System.Console.WriteLine("Attack");
                            player.selectedTile = tiles[i, j];
                            attackInProgress = true;
                            return;
                        }
                        if (tiles[i, j].hasUnit() && ((isPlayerTurn && tiles[i, j].getUnit().isOwnedByPlayer())
                            || (!isPlayerTurn && !tiles[i, j].getUnit().isOwnedByPlayer()))
                            && moveAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            //Console.WriteLine("Move");
                            player.selectedTile = tiles[i, j];
                            moveInProgress = true;
                            return;
                        }
                        if (tiles[i, j].hasUnit() && ((isPlayerTurn && tiles[i, j].getUnit().isOwnedByPlayer())
                            || (!isPlayerTurn && !tiles[i, j].getUnit().isOwnedByPlayer()))
                            && manipulateAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            //Console.WriteLine("Manipulate");
                            player.selectedTile = tiles[i, j];
                            manipulateInProgress = true;
                            return;
                        }
                        if (attUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            if (isPlayerTurn && playerCanSetUnit(i, j, mouseState) && player.Stamina >= UnitAtt.STAMINA_PLACE_COST)
                            { 
                                tiles[i, j].setUnit(player.getAttUnit());
                                player.useStamina(UnitAtt.STAMINA_PLACE_COST);
                                move();
                                
                            }
                            if (!isPlayerTurn && guestCanSetUnit(i, j, mouseState) && guest.Stamina >= UnitAtt.STAMINA_PLACE_COST)
                            {
                                
                                tiles[i, j].setUnit(guest.getAttUnit());
                                guest.useStamina(UnitAtt.STAMINA_PLACE_COST);
                                move();
                            }

                        }
                        else if (defUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            if (isPlayerTurn && playerCanSetUnit(i, j, mouseState) && player.Stamina >= UnitDef.STAMINA_PLACE_COST) 
                            {
                                tiles[i, j].setUnit(player.getDefUnit());
                                player.useStamina(UnitDef.STAMINA_PLACE_COST);
                                move();
                            }
                            if (!isPlayerTurn && guestCanSetUnit(i, j, mouseState) && guest.Stamina >= UnitDef.STAMINA_PLACE_COST)
                            {
                                tiles[i, j].setUnit(guest.getDefUnit());
                                guest.useStamina(UnitDef.STAMINA_PLACE_COST);
                                move();
                            }
                            
                        }
                        else if (mulUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {

                            if (isPlayerTurn && playerCanSetUnit(i, j, mouseState) && player.Stamina >= UnitManipulate.STAMINA_PLACE_COST)
                            { 
                                tiles[i, j].setUnit(player.getMulUnit());
                                player.useStamina(UnitManipulate.STAMINA_PLACE_COST);
                                move();
                            }
                            if (!isPlayerTurn && guestCanSetUnit(i, j, mouseState) && guest.Stamina >= UnitManipulate.STAMINA_PLACE_COST)
                            {
                                tiles[i, j].setUnit(guest.getMulUnit());
                                guest.useStamina(UnitManipulate.STAMINA_PLACE_COST);
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
            else if (manipulateAction.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                hexInfo.Text = "Manipulate";
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
                hexInfo.Text = "Set Manipulate Unit";
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
        private void manipulateUnit(MouseState mouseState)
        {
            TileBasic[] currentSurroundingTiles = player.selectedTile.getSurroundingTiles();
            for (int i = 1; i < currentSurroundingTiles.Length; i++)
            {//starts on 1 because 0 is the attacker
                if (currentSurroundingTiles[i] != null && currentSurroundingTiles[i].isPointInTile(mouseState))
                {
                    //Console.WriteLine("ManipulateUnit");
                    if (!currentSurroundingTiles[i].hasUnit())
                    {
                        currentSurroundingTiles[i].getUnit().manipulate(currentSurroundingTiles[0].getUnit());
                        //Console.WriteLine("manipulateUnit Move");
                    }
                }
            }
            player.selectedTile = null;
            moveInProgress = false;
        }
        private void moveUnit(MouseState mouseState)
        {
            TileBasic[] currentSurroundingTiles = player.selectedTile.getSurroundingTiles();
            for (int i = 1; i < currentSurroundingTiles.Length; i++)
            {//starts on 1 because 0 is the attacker
                if (currentSurroundingTiles[i] != null && currentSurroundingTiles[i].isPointInTile(mouseState))
                {
                    //Console.WriteLine("moveUnit");
                    if (!currentSurroundingTiles[i].hasUnit())
                    {
                        if (player.Stamina >= currentSurroundingTiles[0].getUnit().staminaMoveCost)
                        {
                            currentSurroundingTiles[i].setUnit(currentSurroundingTiles[0].getUnit());
                            player.useStamina((int)currentSurroundingTiles[0].getUnit().staminaMoveCost);
                            currentSurroundingTiles[0].setUnit(null);
                        }
                        //Console.WriteLine("moveUnit Move");
                    }
                }
            }
            player.selectedTile = null;
            moveInProgress = false;
        }
        private void unitAttackUnit(MouseState mouseState)
        {
            TileBasic[] currentSurroundingTiles = player.selectedTile.getSurroundingTiles();
            for (int i = 1; i < currentSurroundingTiles.Length; i++)
            {//starts on 1 because 0 is the attacker
                if (currentSurroundingTiles[i] != null && currentSurroundingTiles[i].isPointInTile(mouseState) && currentSurroundingTiles[i].hasUnit() && !currentSurroundingTiles[i].getUnit().isOwnedByPlayer())
                {
                    if(player.Stamina >= currentSurroundingTiles[i].getUnit().staminaAttCost)
                    {
                        currentSurroundingTiles[0].getUnit().attack(currentSurroundingTiles[i].getUnit());
                        player.useStamina((int)currentSurroundingTiles[i].getUnit().staminaAttCost);

                        
                        attackParticleEngine.particlesOn = true;

                        attackParticleEngine.EmitterLocation = new Vector2(currentSurroundingTiles[i].getX(), currentSurroundingTiles[i].getY());

                        //Timer timer = new System.Timers.Timer(100);
                        //timer.Elapsed += attackAnimation;
                        //timer.Start();
                       // timer.Stop();
                    }
                        
                    if (currentSurroundingTiles[i].getUnit().isDead)
                    {
                        currentSurroundingTiles[i].setUnit(null);
                        
                    }
                }
            }
            player.selectedTile = null;
            attackInProgress = false;
        }


        private void move()
        {
            if (isFirstPhase)
            { 
                movesLeftInPhase--;
                //new TomShane.Neoforce.Controls.EventHandler(this.newTurn);
            }
        }
        private bool playerCanSetUnit(int tileX, int tileY, MouseState mouseState)
        {
            bool result = false;

            if (player.getTotalAtt() > 0 && !tiles[tileX, tileY].hasUnit())
            {
                if (isFirstPhase && !tiles[tileX, tileY].isGuestControled()) { result = true; }
                else if (tiles[tileX, tileY].isPlayerControled()  && !tiles[tileX, tileY].isGuestControled() ) { result = true; }
                
            }
            else if (player.getTotalDef() > 0 && !tiles[tileX, tileY].hasUnit())
            {
                if (isFirstPhase && !tiles[tileX, tileY].isGuestControled()) { result = true; }
                else if (tiles[tileX, tileY].isPlayerControled() && !tiles[tileX, tileY].isGuestControled()) { result = true; }

            }
            else if (player.getTotalMul() > 0 && !tiles[tileX, tileY].hasUnit())
            {
                if (isFirstPhase && !tiles[tileX, tileY].isGuestControled()) { result = true; }
                else if (tiles[tileX, tileY].isPlayerControled() && !tiles[tileX, tileY].isGuestControled()) { result = true; }

            }

            return result;
        }
        private bool guestCanSetUnit(int tileX, int tileY, MouseState mouseState)
        {
            bool result = false;
            if (guest.getTotalAtt() > 0 && !tiles[tileX, tileY].hasUnit())
            {
                if (isFirstPhase && !tiles[tileX, tileY].isPlayerControled()) { result = true; }
                else if (!tiles[tileX, tileY].isPlayerControled() && tiles[tileX, tileY].isGuestControled()) { result = true; }

            }
            else if (guest.getTotalDef() > 0 && !tiles[tileX, tileY].hasUnit())
            {
                if (isFirstPhase && !tiles[tileX, tileY].isPlayerControled()) { result = true; }
                else if (!tiles[tileX, tileY].isPlayerControled() && tiles[tileX, tileY].isGuestControled()) { result = true; }

            }
            else if (guest.getTotalMul() > 0 && !tiles[tileX, tileY].hasUnit())
            {
                if (isFirstPhase && !tiles[tileX, tileY].isPlayerControled()) { result = true; }
                else if (!tiles[tileX, tileY].isPlayerControled() && tiles[tileX, tileY].isGuestControled()) { result = true; }

            }
            return result;
        }
        private void newTurn(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            if (isPlayerTurn)
            {
                turnButton.setInsideColor(GameColors.turnButtonInsideColorGuest);
                player.newTurn();
                isPlayerTurn = false;
            }
            else
            {
                turnButton.setInsideColor(GameColors.turnButtonInsideColor);
                guest.newTurn();
                isPlayerTurn = true;
            }
            if (movesLeftInPhase == 0)
            {
                isFirstPhase = false;
            }
        }
        private void recalculateControlArea()
        {
            //resets the board to have no control
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].updateGuestControl(false);
                    tiles[i,j].updatePlayerControl(false);
                }
            }
            //finds units and set the area of their control
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].getUnit() == null) { continue; }
                    if (tiles[i, j].getUnit().isOwnedByPlayer()) { tiles[i, j].updateSurroundingTilesControl(true, true); }
                    if (!tiles[i, j].getUnit().isOwnedByPlayer()) { tiles[i, j].updateSurroundingTilesControl(true, false); }
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
            guest.draw(spriteBatch);
            player.draw(spriteBatch);
            turnButton.draw(spriteBatch);

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
            else if(manipulateInProgress)
            {

            }
            else
            {
                if (!isFirstPhase && currentTile.hasUnit() && currentTile.isSelected() && !isSmallBoard)
                {

                    if ((currentUnit.isOwnedByPlayer() && isPlayerTurn) || (!currentUnit.isOwnedByPlayer() && !isPlayerTurn))
                    {
                        attAction.moveHex(x + changeInX, y - changeInY, GameColors.attButton, GameColors.attButton);
                        moveAction.moveHex(x - changeInX, y - changeInY, GameColors.moveButton, GameColors.moveButton);
                        attAction.draw(spriteBatch);
                        moveAction.draw(spriteBatch);
                        if (currentUnit.GetType() == typeof(UnitManipulate))
                        {
                            manipulateAction.moveHex(x, y - largeTileSideLength, GameColors.ManipulateButton, GameColors.ManipulateButton);
                            manipulateAction.draw(spriteBatch);
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
            manipulateAction.moveHex(-100, -100, GameColors.ManipulateButton, GameColors.ManipulateButton);
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
                    gameInfo.Text += tempUnit.getUnitType() + "\n";
                    gameInfo.Text += "Total Life: " + tempUnit.life + "\n";
                    gameInfo.Text += "Total Damage: " + tempUnit.Damage + "\n";
                    gameInfo.Text += "Total Defense: " + tempUnit.Defense + "\n";
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
