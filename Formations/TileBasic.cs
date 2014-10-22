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
        private bool guestControled;
        private bool playerControled;
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
            tileHex.init(x, y, graphicsDevice, GameColors.noUnitInsideColor, GameColors.noControlOutsideColor);
        }
        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
        }
        public UnitAbstract getUnit()
        {
            return unit;
        }
        public void setUnit(UnitAbstract newUnit)
        {
            if (newUnit == null)//if the passed in unit is null then it is ok to switch
            {
                tileHex.setOutsideColor(GameColors.noControlOutsideColor);
                tileHex.setInsideColor(GameColors.noUnitInsideColor);
                unit = newUnit;
            }
            else//if the passed in unit is not null then you need to check if there is already a unit and what type the passed in unit is
            {
                if (newUnit.GetType() == typeof(UnitAtt) && unit == null) 
                {
                    tileHex.setInsideColor(GameColors.attUnitInsideColor);
                    tileHex.setOutsideColor(GameColors.attUnitOutsideColor);
                    unit = newUnit;
                }
                if (newUnit.GetType() == typeof(UnitDef) && unit == null) 
                {
                    tileHex.setInsideColor(GameColors.defUnitInsideColor);
                    tileHex.setOutsideColor(GameColors.defUnitOutsideColor);

                    unit = newUnit;
                }
                if (newUnit.GetType() == typeof(UnitMul) && unit == null) 
                {
                    tileHex.setInsideColor(GameColors.mulUnitInsideColor);
                    tileHex.setOutsideColor(GameColors.mulUnitOutsideColor);

                    unit = newUnit;
                }
            }
        }
        public bool isGuestControled()
        {
            return guestControled;
        }
        public void updateGuestControl(bool control){
            guestControled = control;
            updateControlColor();
        }
        public void updatePlayerControl(bool control){
            playerControled = control;
            updateControlColor();
        }
        private void updateControlColor()
        {
            if (guestControled && !playerControled && unit == null) { setTileOutsideColor(GameColors.guestControlOutsideColor); }
            if (!guestControled && playerControled && unit == null) { setTileOutsideColor(GameColors.playerControlOutsideColor); }
            if (guestControled && playerControled && unit == null) { setTileOutsideColor(GameColors.bothControlOutsideColor); }
            if (!guestControled && !playerControled && unit == null) { setTileOutsideColor(GameColors.noControlOutsideColor); }
        }
        
        public bool isPlayerControled()
        {
            return playerControled;
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
                tileHex.setBorderColor(GameColors.selectedBorderColor);
            }
            else
            {
                tileHex.setBorderColor(GameColors.hoverBorderColor);
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
                    tileHex.setBorderColor(GameColors.hoverBorderColor);
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
                    if (unit != null)
                    {
                        if (unit.isOwnedByPlayer())
                        {
                            tileHex.setBorderColor(GameColors.playerControlOutsideColor);
                        }
                        else
                        {
                            tileHex.setBorderColor(GameColors.guestControlOutsideColor);
                        }
                    }
                    else
                    {
                        tileHex.setBorderColor(GameColors.normalBorderColor);
                    }
                    
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
            tileHex.draw(spriteBatch);
        }
    }
}
