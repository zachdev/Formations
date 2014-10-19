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
            tileHex.init(x, y, graphicsDevice, GameColors.startingInsideColor, GameColors.startingOutsideColor);
        }
        public float getX()
        {
            return x;
        }
        public float getY()
        {
            return y;
        }
        public void setUnit(UnitAbstract newUnit)
        {
            if (newUnit == null)//if the passed in unit is null then it is ok to switch
            {
                tileHex.setOutsideColor(GameColors.startingOutsideColor);
                tileHex.setInsideColor(GameColors.startingInsideColor);
                unit = newUnit;
            }
            else//if the passed in unit is not null then you need to check if there is already a unit and what type the passed in unit is
            {
                if (newUnit.GetType() == typeof(UnitAtt) && unit == null) 
                {
                    tileHex.setOutsideColor(GameColors.attUnitOutsideColor);
                    tileHex.setInsideColor(GameColors.attUnitInsideColor);
                    unit = newUnit;
                }
                if (newUnit.GetType() == typeof(UnitDef) && unit == null) 
                {
                    tileHex.setOutsideColor(GameColors.defUnitOutsideColor);
                    tileHex.setInsideColor(GameColors.defUnitInsideColor);
                    unit = newUnit;
                }
                if (newUnit.GetType() == typeof(UnitMul) && unit == null) 
                { 
                    tileHex.setOutsideColor(GameColors.mulUnitOutsideColor);
                    tileHex.setInsideColor(GameColors.mulUnitInsideColor);
                    unit = newUnit;
                }
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
                setTileInsideColor(GameColors.selectedInsideColor);
            }
            else
            {
                if (unit == null)
                {
                    tileHex.setInsideColor(GameColors.startingInsideColor);
                }
                else
                {
                    if (unit.GetType() == typeof(UnitAtt))
                    {
                        tileHex.setInsideColor(GameColors.attUnitInsideColor);
                    }
                    if (unit.GetType() == typeof(UnitDef))
                    {
                        tileHex.setInsideColor(GameColors.defUnitInsideColor);
                    }
                    if (unit.GetType() == typeof(UnitMul))
                    {
                        tileHex.setInsideColor(GameColors.mulUnitInsideColor);
                    }
                }
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

                    tileHex.setInsideColor(GameColors.hoverInsideColor);
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
                        if (unit.GetType() == typeof(UnitAtt))
                        {
                            tileHex.setInsideColor(GameColors.attUnitInsideColor);
                        }
                        if (unit.GetType() == typeof(UnitDef))
                        {
                            tileHex.setInsideColor(GameColors.defUnitInsideColor);
                        }
                        if (unit.GetType() == typeof(UnitMul))
                        {
                            tileHex.setInsideColor(GameColors.mulUnitInsideColor);
                        }
                    }
                    else
                    {
                        tileHex.setInsideColor(GameColors.startingInsideColor);
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
