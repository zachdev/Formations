using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{   
    class Player
    {
        private string playerName;
        private Vector2 playerInfoLocation = new Vector2(800,10);
        private UnitAbstract[] units;
        private SpriteFont font;
        public Player()
        {

        }

        public void init(string nameOfPlayer, int numberOfUnits, SpriteFont font)
        {
            this.playerName = nameOfPlayer;
            units = new UnitAbstract[numberOfUnits];
            this.font = font;

        }


        public void update(MouseState mouseState)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, playerName, playerInfoLocation,Color.Wheat);
        }
    }
}
