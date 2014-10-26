using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private string playerName;
        private Vector2 playerInfoLocation = new Vector2(50,10);
        private UnitAtt[] attUnitArray = new UnitAtt[20];
        private UnitDef[] defUnitArray = new UnitDef[20];
        private UnitManipulate[] mulUnitArray = new UnitManipulate[20];

        private int totalAtt = 0;
        private int totalDef = 0;
        private int totalMul = 0;
        private Hexagon attHex;
        private Hexagon defHex;
        private Hexagon mulHex;
        private SpriteFont font;
        public Guest()
        {

        }

        public void init(string nameOfPlayer, UnitAbstract[,] units, SpriteFont font, GraphicsDevice graphicsDevice)
        {
            playerName = nameOfPlayer;
            for (int i = 0; i < 20; i++)
            {
                if (units[0, i] != null)
                {
                    attUnitArray[i] = (UnitAtt)units[0, i];
                    attUnitArray[i].init(false);
                    totalAtt++;
                }
                    
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[1, i] != null)
                {
                    defUnitArray[i] = (UnitDef)units[1, i];
                    defUnitArray[i].init(false);
                    totalDef++;
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (units[2, i] != null)
                {
                    mulUnitArray[i] = (UnitManipulate)units[2, i];
                    mulUnitArray[i].init(false);
                    totalMul++;
                }
            }
            this.font = font;
            attHex = new Hexagon(7);
            defHex = new Hexagon(7);
            mulHex = new Hexagon(7);
            attHex.init(50, 50, graphicsDevice, GameColors.attUnitInsideColor, GameColors.attUnitOutsideColor);
            defHex.init(50, 65, graphicsDevice, GameColors.defUnitInsideColor, GameColors.defUnitOutsideColor);
            mulHex.init(50, 80, graphicsDevice, GameColors.mulUnitInsideColor, GameColors.mulUnitOutsideColor);

        }
        public int getTotalAtt()
        {
            return totalAtt;
        }
        public UnitAtt getAttUnit()
        {
            if (totalAtt > 0) 
            {
                totalAtt--;
                return attUnitArray[totalAtt]; 
            }
            return null;
        }
        public int getTotalDef()
        {
            return totalDef;
        }
        public UnitDef getDefUnit()
        {
            
            if (totalDef > 0) 
            { 
                totalDef--;
                return defUnitArray[totalDef];
            }
            return null;
        }
        public int getTotalMul()
        {
            return totalMul;
        }
        public UnitManipulate getMulUnit()
        { 
            if (totalMul > 0) 
            {
                totalMul--;
                return mulUnitArray[totalMul]; 
            }
            return null;
        }
        public void update(MouseState mouseState)
        {

        }

        public void draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.DrawString(font, playerName, playerInfoLocation,Color.Wheat);
            spriteBatch.DrawString(font, totalAtt + "", new Vector2(65, 40), Color.Wheat, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font, totalDef + "", new Vector2(65, 55), Color.Wheat, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 1);
            spriteBatch.DrawString(font, totalMul + "", new Vector2(65, 70), Color.Wheat, 0, new Vector2(0, 0), .5f, SpriteEffects.None, 1);
            attHex.draw(spriteBatch);
            defHex.draw(spriteBatch);
            mulHex.draw(spriteBatch);

        }    
    }
}
