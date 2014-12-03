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
    public class Game1 : Game, IMouseListener, IKeyboardListener
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameBoardSinglePlayer gb;
        //MouseState mouseState;
        MouseListener mouseListener;

        private SpriteFont font;
        private SpriteFont damageFont;

        // Neoforce GUI manager
        private Manager theManager{ get; set; }

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
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            mouseListener = new MouseListener(mouseState, this);
            theManager.Initialize();
            theManager.SetSkin(new Skin(theManager, "Blue"));
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
            gb = new GameBoardSinglePlayer();
            gb.init(theManager, GraphicsDevice, "<GameNameHere>", true);

            // Particle engine stuff
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("axe"));
            textures.Add(Content.Load<Texture2D>("sword2"));
            textures.Add(Content.Load<Texture2D>("sword3"));
            ParticleEngine attackParticleEngine = new ParticleEngine(textures, new Vector2(400, 240));
            gb.setAttackParticleEngine(attackParticleEngine);
            // TODO: use this.Content to load your game content here

            // Add damage font to gameboard
            damageFont = Content.Load<SpriteFont>("DamageFont");
            gb.setDamageFont(damageFont);
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
