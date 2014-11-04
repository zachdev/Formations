using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TomShane.Neoforce.Controls;

namespace Formations
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game, IMouseListener, IKeyboardListener
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameBoard gb;
        //MouseState mouseState;
        MouseListener mouseListener;

        private SpriteFont font;

        // Neoforce GUI manager
        private Manager theManager{ get; set; }
        
        //TextBox txtWindow;
        Label testLable;
        Button testButton;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            theManager = new Manager(this, graphics, "Default");
            theManager.AutoCreateRenderTarget = true;
            theManager.TargetFrames = 60;
            theManager.LogUnhandledExceptions = false;
            theManager.ShowSoftwareCursor = true;
            

            graphics.PreferredBackBufferWidth = 1200;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
            //this.IsMouseVisible = true;
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            mouseListener = new MouseListener(mouseState, this);
            theManager.Initialize();
            theManager.SetSkin(new Skin(theManager, "Blue"));
            testButton = new Button(theManager);

            //txtWindow = new TextBox(theManager);
            /*
             * !!!Dan this is the Button Code here!!!
             * Button code  uncomment for large button in the upper left of the screen
             * 
            testButton.Init();
            this.testButton.Click += new TomShane.Neoforce.Controls.EventHandler(this.button_Click);//addes this method to what gets called when the Click happens
            testButton.Text = "Sign In";
            testButton.Width = 200;
            testButton.Height = 200;

            testButton.Anchor = Anchors.Bottom;
            testButton.Visible = true;
            theManager.Add(testButton); 
            */
            //adding a lable
            testLable = new Label(theManager);
            theManager.Add(testLable);
            testLable.Text = "HI";

            //theManager.Add(txtWindow);
            //theManager.Add(testButton);
            
            /*
             * for making a window that you can move around the screen
            window = new Window(theManager);
            window.Init();
            window.Text = "My First Neoforce Window";
            window.Top = 150; // this is in pixels, top-left is the origin
            window.Left = 250;
            window.Width = 350;
            window.Height = 350;
            theManager.Add(window);
            */
        }
        void button_Click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            
            System.Console.Out.WriteLine("Button Clicked");
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("spriteFont");
            gb = new GameBoard();
            gb.init(theManager, GraphicsDevice, "<GameNameHere>");
            // TODO: use this.Content to load your game content here
            



        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            gb.update();
            mouseListener.update(mouseState);
            // TODO: Add your update logic here

            base.Update(gameTime);

            theManager.Update(gameTime);
        }
        public void mousePressed(MouseState mouseState)
        {
            gb.mousePressed(mouseState);
        }
        public void mouseReleased(MouseState mouseState)
        {
            gb.mouseReleased(mouseState);
        }
        public void mouseDragged(MouseState mouseState)
        {
            gb.mouseDragged(mouseState);
        }
        public void mouseMoved(MouseState mouseState)
        {
            gb.mouseMoved(mouseState);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            
            // TODO: Add your drawing code here
            
            theManager.BeginDraw(gameTime);

                
           
            spriteBatch.Begin();
            
            

            gb.draw(spriteBatch);

            spriteBatch.End();
            theManager.EndDraw();

            base.Draw(gameTime);

        }
    }
}
