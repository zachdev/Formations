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
        private UnitAbstract[] attUnitArray = new UnitAbstract[20];
        private UnitAbstract[] defUnitArray = new UnitAbstract[20];
        private UnitAbstract[] mulUnitArray = new UnitAbstract[20];
        private int totalAtt = 0;
        private int totalDef = 0;
        private int totalMul = 0;
        private Hexagon attHex;
        private Hexagon defHex;
        private Hexagon mulHex;
        private SpriteFont font;
        public Player()
        {

        }

        public void init(string nameOfPlayer, UnitAbstract[,] units, SpriteFont font, GraphicsDevice graphicsDevice)
        {
            playerName = nameOfPlayer;
            for (int i = 0; i < 20; i++)
            {
                if (units[0, i] != null)
                {
                    attUnitArray[i] = units[0 , i];
                    totalAtt++;
                }
                    
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[1, i] != null)
                {
                    defUnitArray[i] = units[1, i];
                    totalDef++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[2, i] != null)
                {
                    mulUnitArray[i] = units[2, i];
                    totalMul++;
                }
            }
            this.font = font;
            attHex = new Hexagon(7);
            defHex = new Hexagon(7);
            mulHex = new Hexagon(7);
            attHex.init(800, 50, graphicsDevice);
            defHex.init(800, 65, graphicsDevice);
            mulHex.init(800, 80, graphicsDevice);
            attHex.setColor(Color.Brown);
            defHex.setColor(Color.Black);
            mulHex.setColor(Color.AliceBlue);

        }


        public void update(MouseState mouseState)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, playerName, playerInfoLocation,Color.Wheat);
            spriteBatch.DrawString(font, totalAtt + "", new Vector2(815, 40), Color.Wheat);
            spriteBatch.DrawString(font, totalDef + "", new Vector2(815, 55), Color.Wheat);
            spriteBatch.DrawString(font, totalMul + "", new Vector2(815, 70), Color.Wheat);
            attHex.draw(spriteBatch);
            defHex.draw(spriteBatch);
            mulHex.draw(spriteBatch);

        }    
    }
}
