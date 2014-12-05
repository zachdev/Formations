using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using Microsoft.Xna.Framework.GamerServices.Gamer;
using System;
using System.Collections.Generic;
using TomShane.Neoforce.Controls;

namespace Formations
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Formations : Game, IMouseListener, IKeyboardListener
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameBoardSinglePlayer gb;
        private Person person;
        private bool isGameStarted = false;
        private GameLogin login; 
        private GameLobby gameLobby;
        public static List<Texture2D> attackTextures;
        public static List<Texture2D> bloodTextures;
        public static List<Texture2D> healingTextures;
        //MouseState mouseState;
        MouseListener mouseListener;

        private SpriteFont font;
        public static SpriteFont damageFont;

        // Neoforce GUI manager
        private Manager theManager{ get; set; }

        public Formations()
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
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            mouseListener = new MouseListener(mouseState, this);
            theManager.Initialize();
            theManager.SetSkin(new Skin(theManager, "Blue"));
            gameLobby = new GameLobby(theManager);
            gameLobby.init(this);
            login = new GameLogin(theManager);
            login.init(this);
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("spriteFont");
            damageFont = Content.Load<SpriteFont>("DamageFont");

            attackTextures = new List<Texture2D>();
            attackTextures.Add(Content.Load<Texture2D>("axe"));
            attackTextures.Add(Content.Load<Texture2D>("sword2"));
            attackTextures.Add(Content.Load<Texture2D>("sword3"));

            bloodTextures = new List<Texture2D>();
            bloodTextures.Add(Content.Load<Texture2D>("axe"));
            bloodTextures.Add(Content.Load<Texture2D>("sword2"));
            bloodTextures.Add(Content.Load<Texture2D>("sword3"));

            healingTextures = new List<Texture2D>();
            healingTextures.Add(Content.Load<Texture2D>("axe"));
            healingTextures.Add(Content.Load<Texture2D>("sword2"));
            healingTextures.Add(Content.Load<Texture2D>("sword3"));



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
        internal void setPerson(Person person)
        {
            this.person = person;
            newGame();
        }
        private void challengePerson()
        {

        }
        private void newGame()
        {
            gb = new GameBoardSinglePlayer();
            gb.init(theManager, GraphicsDevice, "Formations", true); 
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (login.isLoggedIn)
            {
                gb.update();
                mouseListener.update(mouseState);
            }

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
            if (login.isLoggedIn)
            {
                gb.draw(spriteBatch);
            }
            spriteBatch.End();
            theManager.EndDraw();

            base.Draw(gameTime);

        }


    }
}
