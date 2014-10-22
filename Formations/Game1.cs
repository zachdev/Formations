using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.DrawableGameComponent;
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
       // MouseState mouseState;
        MouseListener mouseListener;

        private SpriteFont font;

        // Neoforce GUI manager
        TomShane.Neoforce.Controls.Manager theManager;
        Window window;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            this.IsMouseVisible = true;
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            mouseListener = new MouseListener(mouseState, this);
            gb = new GameBoard();
            gb.init(GraphicsDevice, font, "<GameNameHere>");

            theManager = new Manager(this, graphics, "Default");
            theManager.Initialize();

            window = new Window(theManager);
            window.Init();
            window.Text = "My First Neoforce Window";
            window.Top = 150; // this is in pixels, top-left is the origin
            window.Left = 250;
            window.Width = 350;
            window.Height = 350;
            theManager.Add(window);
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

            spriteBatch.Begin();
            gb.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);

            theManager.Draw(gameTime);

        }
    }
}
