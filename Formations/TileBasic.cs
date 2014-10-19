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

        private bool selected = false;
        private bool hovered = false;
        private int tileSideLength;
        private float x;
        private float y;
        public TileBasic(int tileSideLength)
        {
            this.tileSideLength = tileSideLength;

        }
        public override void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            this.x = x;
            this.y = y;
            tileHex = new Hexagon(tileSideLength);
            tileHex.init(x, y, graphicsDevice);
        }
        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
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
                setTileInsideColor(Color.Purple);
            }
            else
            {
                setTileInsideColor(Color.Red);
            }
        }
        public bool isHovered()
        {
            return hovered;
        }
        public void setTileInsideColor(Color color)
        {
            tileHex.setInsideColor(color);
        }
        public void setTileOutsideColor(Color color)
        {
            tileHex.setOutsideColor(color);
        }
        public bool isPointInTile(MouseState mouseState)
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
                    tileHex.setInsideColor(Color.Red);
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
                    tileHex.setInsideColor(Color.Blue);
                }
            }
        }
        public override void update()
        {

        }
        public void drawButtons(SpriteBatch spriteBatch)
        {

        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if(unit != null)
            {
                Console.WriteLine("Unit Type: " + unit.GetType().ToString());
                if (unit.GetType() == typeof(UnitAtt)) { tileHex.setOutsideColor(Color.Black); }
                else if (unit.GetType() == typeof(UnitDef)) { tileHex.setOutsideColor(Color.IndianRed); }
                else if (unit.GetType() == typeof(UnitMul)) { tileHex.setOutsideColor(Color.AliceBlue); }
            }
            tileHex.draw(spriteBatch);

        }
    }
}
