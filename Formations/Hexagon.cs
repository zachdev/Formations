using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    class Hexagon
    {
        private Vector3[] vectors;
        private VertexPositionColor[] vertices;
        private BasicEffect basicEffect;
        private int tileSideLength;
       
        public Hexagon(int tileSideLength)
        {
            this.tileSideLength = tileSideLength;
        }
        public void init(float x, float y, GraphicsDevice graphicsDevice, Color insideColor, Color outsideColor)
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,     // left, right
                graphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1);                                // near, far plane
 
            vectors= CalculateVertices(tileSideLength, new Vector3(x, y,0));

            vertices = new VertexPositionColor[18];
            vertices[0] = new VertexPositionColor(new Vector3(x, y, 0), insideColor);
            vertices[1] = new VertexPositionColor(vectors[0], outsideColor);
            vertices[2] = new VertexPositionColor(vectors[1], outsideColor);
            vertices[3] = new VertexPositionColor(new Vector3(x, y, 0), insideColor);
            vertices[4] = new VertexPositionColor(vectors[1], outsideColor);
            vertices[5] = new VertexPositionColor(vectors[2], outsideColor);

            vertices[6] = new VertexPositionColor(new Vector3(x, y, 0), insideColor);
            vertices[7] = new VertexPositionColor(vectors[2], outsideColor);
            vertices[8] = new VertexPositionColor(vectors[3], outsideColor);
            vertices[9] = new VertexPositionColor(new Vector3(x, y, 0), insideColor);
            vertices[10] = new VertexPositionColor(vectors[3], outsideColor);
            vertices[11] = new VertexPositionColor(vectors[4], outsideColor);

            vertices[12] = new VertexPositionColor(new Vector3(x, y, 0), insideColor);
            vertices[13] = new VertexPositionColor(vectors[4], outsideColor);
            vertices[14] = new VertexPositionColor(vectors[5], outsideColor);
            vertices[15] = new VertexPositionColor(new Vector3(x, y, 0), insideColor);
            vertices[16] = new VertexPositionColor(vectors[5], outsideColor);
            vertices[17] = new VertexPositionColor(vectors[0], outsideColor);

        }
        public static Vector3[] CalculateVertices(float sideLenght, Vector3 center)
        {
            Vector3[] tempVectors = new Vector3[6];
            double angle;
            Vector3 temp = new Vector3();
            for (int i = 0; i < tempVectors.Length; i++)
            {
                angle = 2 * Math.PI / 6 * (i + 0.05) -.57;
                temp.X = (float)(center.X + sideLenght * Math.Cos(angle));
                temp.Y = (float)(center.Y + sideLenght * Math.Sin(angle));
                temp.Z = 0f;
                tempVectors[i] = temp;
            }
            return tempVectors;

        }

        public void setInsideColor(Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i % 3 == 0) { vertices[i].Color = color; }
            }
        }
        public void setOutsideColor(Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i % 3 != 0) { vertices[i].Color = color; }
            }
        }
        public bool IsPointInPolygon(int pointX, int pointY)
        {
          
            bool isInside = false;
            Vector3 temp1;
            Vector3 temp2;
            for (int i = 0, j = vectors.Length - 1; i < vectors.Length; j = i++)
            {
                temp1 = vectors[i];
                temp2 = vectors[j];
                if (((temp1.Y > pointY) != (temp2.Y > pointY)) &&
                    (pointX < (temp2.X - temp1.X) * (pointY - temp1.Y) / (temp2.Y - temp1.Y) + temp1.X))
                 {
                    isInside = !isInside;
                 }
            }
            return isInside;
        }

        public void draw(SpriteBatch spriteBatch)
        {

                basicEffect.CurrentTechnique.Passes[0].Apply();
                spriteBatch.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, 6);
            
        }
    }
}
