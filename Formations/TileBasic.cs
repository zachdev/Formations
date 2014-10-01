using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    
    class TileBasic : TileAbstract
    {
        private Hexagon hex;
        public TileBasic()
        {

        }

        public override void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            hex = new Hexagon();
            hex.init(x, y, graphicsDevice);

            
        }
        public override void update()
        {

        }
        
        public static Vector3[] CalculateVertices(int nSides, double nSideLength, Vector3 ptFirstVertex)
        {
            if (nSides < 3)
                throw new ArgumentException("Polygons can't have less than 3 sides...");

            Vector3[] aptsVertices = new Vector3[nSides];
            double deg = (45.0 * (nSides - 2)) / nSides;
            double step = 360.0 / nSides;
            double rad = deg * (Math.PI / 180);

            double nSinDeg = Math.Sin(rad);
            double nCosDeg = Math.Cos(rad);

            aptsVertices[0] = ptFirstVertex;

            for (int i = 1; i < aptsVertices.Length; i++)
            {
                double x = aptsVertices[i - 1].X - nCosDeg * nSideLength;
                double y = aptsVertices[i - 1].Y - nSinDeg * nSideLength;
                aptsVertices[i] = new Vector3((float)x, (float)y, 0);


                //recalculate the degree for the next vertex
                deg -= step;
                rad = deg * (Math.PI / 180);

                nSinDeg = Math.Sin(rad);
                nCosDeg = Math.Cos(rad);

            }
            return aptsVertices;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            hex.draw(spriteBatch);
        }
    }
}
