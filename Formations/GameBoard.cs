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
    class GameBoard : IMouseListener, IKeyboardListener
    {
        private Player player;
        private Guest guest;
        private Manager uiManager;
        private string gameName;
        private Vector2 gameNameLocation = new Vector2(500, 10);
        private SpriteFont font;
        private bool isPlayersTurn = true;
        private int movesLeftInPhase = 10;
        private bool isFirstPhase = true;
       //private bool isGamePhase = false;
        private bool attackInProgress = false;
        private bool moveInProgress = false;
        private bool manipulateInProgress = false;
        private MouseState currentMouseState;
        private Label hexInfo;
        private Label gameInfo;
        private Hexagon firstPhase;
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
        private int tileSideLength = 30;
        private float boardOffsetX = 150;
        private float boardOffsetY = 130;
        private float xTileOffset = 27.5F;
        private float xAdjustment = 55;
        private float yAdjustment = 47;
        private float changeInX;
        private float changeInY;
        private TileBasic[,] tiles = new TileBasic[boardWidth, boardHeight];
        private VertexPositionColor[] vertices = new VertexPositionColor[6];
        private VertexPositionColor[] borderLines = new VertexPositionColor[8];
        private VertexPositionColor[] buttonsBackground = new VertexPositionColor[6];
        private VertexPositionColor[] buttonsBorderLines = new VertexPositionColor[8];
        private BasicEffect basicEffect;

        // Chat class
        private Chat chatManager;
        private Button chatButton;


        public GameBoard()
        {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
               {
                   tiles[i,j] = new TileBasic(tileSideLength);
               }
            }
            unitSideLength = tileSideLength / 2;

        }
        private UnitAbstract[,] createUnitArray(int numberAtt, int numberDef, int numberMul)
        {
            UnitAbstract[,] tempArray = new UnitAbstract[3,20];

            for (int i = 0; i < numberAtt; i++)
            {
                tempArray[0, i] = new UnitAtt();
            }
            for (int i = 0; i < numberDef; i++)
            {
                tempArray[1, i] = new UnitDef();
            }
            for (int i = 0; i < numberMul; i++)
            {
                tempArray[2, i] = new UnitManipulate();
            }
            return tempArray;
        }
        public void init(Manager uiManager, GraphicsDevice graphicsDevice, SpriteFont font, string gameName)
        {
            this.font = font;
            this.gameName = gameName;
            player = new Player();
            guest = new Guest();
            this.uiManager = uiManager;

            player.init("<PlayerNameHere>", createUnitArray(10, 5, 1), font, graphicsDevice);
            guest.init("<GuestNameHere>", createUnitArray(10, 3, 2), font, graphicsDevice);

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,       // left, right
                graphicsDevice.Viewport.Height, 0,      // bottom, top
                0, 1);                                  // near, far plane
            createBoardArea();
            //createButtonArea();
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (j % 2 == 0)
                    {
                        tiles[i, j].init(boardOffsetX + (i * xAdjustment), boardOffsetY + (j * yAdjustment), graphicsDevice);
                    }
                    else
                    {
                        tiles[i, j].init(boardOffsetX + xTileOffset + (i * xAdjustment), boardOffsetY + (j * yAdjustment), graphicsDevice);
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
            changeInX = (float)Math.Sqrt((float)(tileSideLength * tileSideLength) - (float)(tileSideLength / 2) * (float)(tileSideLength / 2));
            changeInY = (float)(tileSideLength / 2);
            firstPhase = new Hexagon(20);
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
            firstPhase.init(400, 50, graphicsDevice, GameColors.turnButtonOutsideColor, GameColors.turnButtonInsideColor);

            // Chat stuff
            chatManager = new Chat();
            chatManager.init(uiManager);
            uiManager.SetSkin(new Skin(uiManager, "Blue"));
            chatButton = new Button(uiManager);
            chatButton.SetPosition(1150, 10);
            chatButton.Click += new TomShane.Neoforce.Controls.EventHandler(chatManager.toggle);
            chatButton.Text = "<";
            

            Label chatLabel = new Label(uiManager);
            uiManager.Add(chatButton);

            
        }


        public void mousePressed(MouseState mouseState)
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
                        if(moveInProgress)
                        {
                            moveUnit(mouseState);
                        }
                        if(manipulateInProgress)
                        {
                            manipulateUnit(mouseState);
                        }
                    }
                }
            }
        }
        public void mouseReleased(MouseState mouseState)
        {
            
            if(turnButton.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                newTurn();
                return;
            }

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    if (tiles[i, j].isSelected())
                    {
                        tiles[i, j].mouseReleased(mouseState);
                        if (tiles[i, j].hasUnit() && ((isPlayersTurn && tiles[i, j].getUnit().isOwnedByPlayer())
                            || (!isPlayersTurn && !tiles[i, j].getUnit().isOwnedByPlayer()))
                            && attAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        { 
                            System.Console.WriteLine("Attack");
                            player.selectedTile = tiles[i, j];
                            attackInProgress = true;
                            return;
                        }
                        if (tiles[i, j].hasUnit() && ((isPlayersTurn && tiles[i, j].getUnit().isOwnedByPlayer())
                            || (!isPlayersTurn && !tiles[i, j].getUnit().isOwnedByPlayer()))
                            && moveAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            //Console.WriteLine("Move");
                            player.selectedTile = tiles[i, j];
                            moveInProgress = true;
                            return;
                        }
                        if (tiles[i, j].hasUnit() && ((isPlayersTurn && tiles[i, j].getUnit().isOwnedByPlayer())
                            || (!isPlayersTurn && !tiles[i, j].getUnit().isOwnedByPlayer()))
                            && manipulateAction.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            //Console.WriteLine("Manipulate");
                            player.selectedTile = tiles[i, j];
                            manipulateInProgress = true;
                            return;
                        }
                        if (attUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            
                            if (isPlayersTurn && playerCanSetUnit(i,j,mouseState))
                            { 
                                tiles[i, j].setUnit(player.getAttUnit());
                                move();
                                
                            }
                            if (!isPlayersTurn && guestCanSetUnit(i, j, mouseState))
                            {
                                
                                tiles[i, j].setUnit(guest.getAttUnit());
                                move();
                            }

                        }
                        else if (defUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {
                            if (isPlayersTurn && playerCanSetUnit(i, j, mouseState)) 
                            {
                                tiles[i, j].setUnit(player.getDefUnit());
                                move();
                            }
                            if (!isPlayersTurn && guestCanSetUnit(i, j, mouseState))
                            {
                                tiles[i, j].setUnit(guest.getDefUnit());
                                move();
                            }
                            
                        }
                        else if (mulUnit.IsPointInPolygon(mouseState.X, mouseState.Y))
                        {

                            if (isPlayersTurn && playerCanSetUnit(i, j, mouseState))
                            { 
                                tiles[i, j].setUnit(player.getMulUnit());
                                move();
                            }
                            if (!isPlayersTurn && guestCanSetUnit(i, j, mouseState))
                            {
                                tiles[i, j].setUnit(guest.getMulUnit());
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
            if (hexInfo == null)
            {
                hexInfo = new Label(uiManager);
                uiManager.Add(hexInfo);
                hexInfo.SetSize(150, 25);
                hexInfo.TextColor = Color.Black;
                
            }

            hexInfo.SetPosition(mouseState.X,mouseState.Y - 15);
            setHoverLabel(mouseState);

            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    tiles[i, j].mouseMoved(mouseState);
                }
            }
        }
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
                if (currentSurroundingTiles[i].isPointInTile(mouseState))
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
                        currentSurroundingTiles[i].setUnit(currentSurroundingTiles[0].getUnit());
                        currentSurroundingTiles[0].setUnit(null);
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
                if (currentSurroundingTiles[i].isPointInTile(mouseState) && currentSurroundingTiles[i].hasUnit() && !currentSurroundingTiles[i].getUnit().isOwnedByPlayer())
                {
                    currentSurroundingTiles[0].getUnit().attack(currentSurroundingTiles[i].getUnit());
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
                newTurn();
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
        private void newTurn()
        {
            if (isPlayersTurn)
            {
                turnButton.setInsideColor(GameColors.turnButtonInsideColorGuest);
                isPlayersTurn = false;
            }
            else
            {
                turnButton.setInsideColor(GameColors.turnButtonInsideColor);
                isPlayersTurn = true;
            }
            if (movesLeftInPhase == 0)
            {
                firstPhase.setInsideColor(Color.Gray);
                firstPhase.setOutsideColor(Color.Gray);
                isFirstPhase = false;
                //isGamePhase = true;
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

        }

        public void draw(SpriteBatch spriteBatch)
        {
            resetButtons();
            basicEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, borderLines, 0, 4);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, buttonsBackground, 0, 2);
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, buttonsBorderLines, 0, 4);
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
            

            spriteBatch.DrawString(font, gameName, gameNameLocation, Color.White);
            guest.draw(spriteBatch);
            player.draw(spriteBatch);
            turnButton.draw(spriteBatch);
            firstPhase.draw(spriteBatch);
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
                if (currentTile.hasUnit() && currentTile.isSelected())
                {

                    if ((currentUnit.isOwnedByPlayer() && isPlayersTurn) || (!currentUnit.isOwnedByPlayer() && !isPlayersTurn))
                    {
                        attAction.moveHex(x + changeInX, y - changeInY, GameColors.attButton, GameColors.attButton);
                        moveAction.moveHex(x - changeInX, y - changeInY, GameColors.moveButton, GameColors.moveButton);
                        attAction.draw(spriteBatch);
                        moveAction.draw(spriteBatch);
                        if (currentUnit.GetType() == typeof(UnitManipulate))
                        {
                            manipulateAction.moveHex(x, y - tileSideLength, GameColors.ManipulateButton, GameColors.ManipulateButton);
                            manipulateAction.draw(spriteBatch);
                        }
                    }
                }
                else if(!currentTile.hasUnit() && currentTile.isSelected())
                {
                    attUnit.moveHex(x, y - tileSideLength, GameColors.attUnitInsideColor, GameColors.attUnitOutsideColor);
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

            float width = boardOffsetX - 50;
            float height = 600 - boardOffsetY - 10;
            if (gameInfo == null)
            {
                gameInfo = new Label(uiManager);
                gameInfo.Init();
                gameInfo.Height = (int)height;
                gameInfo.Width = (int)width;
                //gameInfo.DrawBorders = true;
                gameInfo.MaximumWidth = (int)width;
                gameInfo.SetPosition(10, (int)boardOffsetY);
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
            
            /*
            buttonsBackground[0] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.MistyRose);
            buttonsBackground[1] = new VertexPositionColor(new Vector3(width, boardOffsetY - 40, 0), Color.MistyRose);
            buttonsBackground[2] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.MistyRose);
            buttonsBackground[3] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.MistyRose);
            buttonsBackground[4] = new VertexPositionColor(new Vector3(10, boardOffsetY + height, 0), Color.MistyRose);
            buttonsBackground[5] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.MistyRose);

            buttonsBorderLines[0] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.Blue);
            buttonsBorderLines[1] = new VertexPositionColor(new Vector3(width, boardOffsetY - 40, 0), Color.Blue);
            buttonsBorderLines[2] = new VertexPositionColor(new Vector3(width, boardOffsetY - 40, 0), Color.Blue);
            buttonsBorderLines[3] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[4] = new VertexPositionColor(new Vector3(width, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[5] = new VertexPositionColor(new Vector3(10, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[6] = new VertexPositionColor(new Vector3(10, boardOffsetY + height, 0), Color.Blue);
            buttonsBorderLines[7] = new VertexPositionColor(new Vector3(10, boardOffsetY - 40, 0), Color.Blue);
            */
        }
        private void createBoardArea()
        {
            float border = 10;
            float halfHexWidth = (float)Math.Sqrt(tileSideLength * tileSideLength - (tileSideLength / 2) * (tileSideLength / 2));
            float widthOfBoard = halfHexWidth * (boardWidth * 2) + boardWidth * 2 + border;
            float heightOfBoard = tileSideLength * (boardHeight * 1.5f) + (boardHeight * 1.5f) - (tileSideLength / 2f) + border;
            vertices[0] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.MistyRose);
            vertices[1] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY - tileSideLength - border, 0), Color.MistyRose);
            vertices[2] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.MistyRose);
            vertices[3] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.MistyRose);
            vertices[4] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY + heightOfBoard, 0), Color.MistyRose);
            vertices[5] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.MistyRose);

            borderLines[0] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.Blue);
            borderLines[1] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY - tileSideLength - border, 0), Color.Blue);
            borderLines[2] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY - tileSideLength - border, 0), Color.Blue);
            borderLines[3] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[4] = new VertexPositionColor(new Vector3(boardOffsetX + widthOfBoard, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[5] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[6] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY + heightOfBoard, 0), Color.Blue);
            borderLines[7] = new VertexPositionColor(new Vector3(boardOffsetX - halfHexWidth - border, boardOffsetY - tileSideLength - border, 0), Color.Blue);
        }
    }
}
