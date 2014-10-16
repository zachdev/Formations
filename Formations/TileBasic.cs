using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    
    class TileBasic : TileAbstract
    {
        private UnitAbstract unit = null;
        private Hexagon tileHex;
        private Hexagon attUnit;
        private Hexagon defUnit;
        private Hexagon mulUnit;
        private bool selected = false;
        private bool hovered = false;
        private int tileSideLength;
        private int unitSideLength;
   
        public TileBasic(int tileSideLength)
        {
            this.tileSideLength = tileSideLength;
            unitSideLength = tileSideLength / 2;
        }
        public override void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            tileHex = new Hexagon(tileSideLength);
            tileHex.init(x, y, graphicsDevice);
            float changeInX = (float)Math.Sqrt((float)(tileSideLength*tileSideLength) - (float)(tileSideLength/2)*(float)(tileSideLength/2));
            float changeInY = (float)(tileSideLength/2);
            attUnit = new Hexagon(unitSideLength);
            defUnit = new Hexagon(unitSideLength);
            mulUnit = new Hexagon(unitSideLength);
            attUnit.init(x, y - tileSideLength, graphicsDevice);
            defUnit.init(x + changeInX, y + changeInY, graphicsDevice);
            mulUnit.init(x - changeInX, y + changeInY, graphicsDevice);

        }
        public void setUnit(UnitAbstract unit)
        {
            if (this.unit == null)
            {
                this.unit = unit;
            }

        }
        public bool hasUnit()
        {
            if (unit == null) { return false; }
            return true;
        }
        public bool isSelected()
        {
            return selected;
        }
        public void setSelected(bool isSelected)
        {
            selected = isSelected;
            if (selected)
            {
                setTileColor(Color.Purple);
            }
            else
            {
                setTileColor(Color.Red);
            }
        }
        public bool isHovered()
        {
            return hovered;
        }
        public void setTileColor(Color color)
        {
            tileHex.setColor(color);
        }
        public bool isPoininTile(MouseState mouseState)
        {
            return tileHex.IsPointInPolygon(mouseState.X, mouseState.Y);
        }
        public override void mousePressed(MouseState mouseState)
        {
            if (isSelected())
            {
                setSelected(false);
            }
            else
            {
                setSelected(true);
            }
        }
        public override void mouseReleased(MouseState mouseState)
        {
            if (isSelected())
            {
                setSelected(false);
            }
            else
            {
                setSelected(true);
            }
        }
        public override void mouseDragged(MouseState mouseState)
        {

        }
        public override void mouseMoved(MouseState mouseState)
        {
            if (tileHex.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                if (!hovered)
                {
                    hovered = true;   
                }
                if (!selected)
                {
                    tileHex.setColor(Color.Red);
                }
            }
            else
            {
                if (hovered)
                {
                    hovered = false;
                }
                if (!selected)
                {
                    tileHex.setColor(Color.Blue);
                }
            }
        }
        public override void update()
        {

        }
        public void drawButtons(SpriteBatch spriteBatch)
        {
            if (isSelected())
            {
                attUnit.draw(spriteBatch);
                defUnit.draw(spriteBatch);
                mulUnit.draw(spriteBatch);
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            tileHex.draw(spriteBatch);

        }
    }
}
