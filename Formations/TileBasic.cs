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
        private Hexagon hex;
        private bool selected = false;
        private bool hovered = false;
        private int tileSideLength;
   
        public TileBasic(int tileSideLength)
        {
            this.tileSideLength = tileSideLength;
        }

        public override void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            hex = new Hexagon(tileSideLength);
            hex.init(x, y, graphicsDevice);

            
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
            hex.setColor(color);
        }
        public bool isPoininTile(MouseState mouseState)
        {
            return hex.IsPointInPolygon(mouseState.X, mouseState.Y);
        }
        public override void mousePressed(MouseState mouseState)
        {
            
        }
        public override void mouseReleased(MouseState mouseState)
        {
            
        }
        public override void mouseDragged(MouseState mouseState)
        {

        }
        public override void mouseMoved(MouseState mouseState)
        {
            if (hex.IsPointInPolygon(mouseState.X, mouseState.Y))
            {
                if (!hovered)
                {
                    hovered = true;   
                }
                if (!selected)
                {
                    hex.setColor(Color.Red);
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
                    hex.setColor(Color.Blue);
                }
            }
        }
        public override void update()
        {

        }
        public override void draw(SpriteBatch spriteBatch)
        {
            hex.draw(spriteBatch);
        }
    }
}
