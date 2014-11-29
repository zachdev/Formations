using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class GameColors
    {


        //Inside Colors
        public static Color noUnitInsideColor = Color.Green;
        public static Color attUnitInsideColor = Color.Red;
        public static Color defUnitInsideColor = Color.Gray;
        public static Color mulUnitInsideColor = Color.Purple;

        //Outside Colors
        public static Color noControlOutsideColor = Color.DarkGreen;
        public static Color playerControlOutsideColor = Color.MidnightBlue;
        public static Color guestControlOutsideColor = Color.Orange;
        public static Color bothControlOutsideColor = Color.Black;
        public static Color attUnitOutsideColor = attUnitInsideColor;
        public static Color defUnitOutsideColor = defUnitInsideColor;
        public static Color mulUnitOutsideColor = mulUnitInsideColor;

        //Border Colors
        public static Color hoverBorderColor = Color.Red;
        public static Color selectedBorderColor = Color.Purple;
        public static Color normalBorderColor = Color.Black;

        //Button Colors
        public static Color turnButtonInsideColor = Color.Blue;
        public static Color turnButtonOutsideColor = Color.DarkOrange;
        public static Color turnButtonInsideColorGuest = Color.Yellow;
        public static Color attButton = Color.Red;
        public static Color ManipulateButton = Color.LightPink;
        public static Color moveButton = Color.Blue;

        //Background Colors
        public static Color boardAreaBackground = Color.DarkBlue;
    }
}
