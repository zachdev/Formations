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
        private BasicEffect basicEffect;
        private VertexPositionColor[] vertices;
        private Color color = Color.White;
        public TileBasic()
        {

        }

        public override void init(float x, float y, GraphicsDevice graphicsDevice){
            Vector3[] hex = CalculateVertices(6, 29, new Vector3(x, y, 0));

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
            (0, graphicsDevice.Viewport.Width,    // left, right
            graphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);                                // near, far plane
 
            vertices = new VertexPositionColor[12];
            vertices[0].Position = hex[0];
            vertices[0].Color = color;
            vertices[1].Position = hex[1];
            vertices[1].Color = color;
            vertices[2].Position = hex[1];
            vertices[2].Color = color;
            vertices[3].Position = hex[2];
            vertices[3].Color = color;
            vertices[4].Position = hex[2];
            vertices[4].Color = color;
            vertices[5].Position = hex[3];
            vertices[5].Color = color;
            vertices[6].Position = hex[3];
            vertices[6].Color = color;
            vertices[7].Position = hex[4];
            vertices[7].Color = color;
            vertices[8].Position = hex[4];
            vertices[8].Color = color;
            vertices[9].Position = hex[5];
            vertices[9].Color = color;
            vertices[10].Position = hex[5];
            vertices[10].Color = color;
            vertices[11].Position = hex[0];
            vertices[11].Color = color;


            
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
/*
        public bool PointInPolygon(Vector2 point) {

            bool inside = false;
            Vector2[] tempArray = hex.getAllPoints();
            for (int i = 0; i < 6; i++)
            {
                if (point.Y > Math.Min(tempArray[i].Y, tempArray[i + 1].Y))
                    if (point.Y <= Math.Max(tempArray[i].Y, tempArray[i + 1].Y))
                        if (point.X <= Math.Max(tempArray[i].X, tempArray[i + 1].X)){
                            float xIntersection = tempArray[i].X + ((point.Y - tempArray[i].Y)
                            / (tempArray[i + 1].Y - tempArray[i].Y))
                            * (tempArray[i + 1].X - tempArray[i].X);
                            if (point.X <= xIntersection) { inside = !inside; }
                        }
            }
            return inside;
        }

*/
        public override void draw(SpriteBatch spriteBatch)
        {
            basicEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0,6);

        }
    }
}
