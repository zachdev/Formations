using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    /// <summary>
    /// GuestBoard
    /// </summary>
    class Guest
    {
        private string guestName;
        private Vector2 guestInfoLocation = new Vector2(100, 10);
        private UnitAbstract[] units;
        private SpriteFont font;
        public Guest()
        {


        }

        public void init(string nameOfGuest, UnitAbstract[] units, SpriteFont font)
        {
            guestName = nameOfGuest;
            this.units = units;
            this.font = font;

        }
        public void update()
        {
            //insert server code here
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, guestName, guestInfoLocation, Color.Wheat);
        }
    }
}
