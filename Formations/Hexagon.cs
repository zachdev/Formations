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
        Vector3[] vectors;
        VertexPositionColor[] vertices;
        BasicEffect basicEffect;
       
        public Hexagon()
        {

        }
        public void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphicsDevice.Viewport.Width,     // left, right
                graphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1);                                // near, far plane
 
            vectors= CalculateVertices(50, new Vector3(x, y,0));

            vertices = new VertexPositionColor[18];
            vertices[0] = new VertexPositionColor(new Vector3(x, y, 0), Color.Blue);
            vertices[1] = new VertexPositionColor(vectors[0], Color.Green);
            vertices[2] = new VertexPositionColor(vectors[1], Color.Green);
            vertices[3] = new VertexPositionColor(new Vector3(x, y, 0), Color.Blue);
            vertices[4] = new VertexPositionColor(vectors[1], Color.Green);
            vertices[5] = new VertexPositionColor(vectors[2], Color.Green);

            vertices[6] = new VertexPositionColor(new Vector3(x, y, 0), Color.Blue);
            vertices[7] = new VertexPositionColor(vectors[2], Color.Green);
            vertices[8] = new VertexPositionColor(vectors[3], Color.Green);
            vertices[9] = new VertexPositionColor(new Vector3(x, y, 0), Color.Blue);
            vertices[10] = new VertexPositionColor(vectors[3], Color.Green);
            vertices[11] = new VertexPositionColor(vectors[4], Color.Green);

            vertices[12] = new VertexPositionColor(new Vector3(x, y, 0), Color.Blue);
            vertices[13] = new VertexPositionColor(vectors[4], Color.Green);
            vertices[14] = new VertexPositionColor(vectors[5], Color.Green);
            vertices[15] = new VertexPositionColor(new Vector3(x, y, 0), Color.Blue);
            vertices[16] = new VertexPositionColor(vectors[5], Color.Green);
            vertices[17] = new VertexPositionColor(vectors[0], Color.Green);

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

        public void setColor(Color color)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i % 3 == 0) { vertices[i].Color = color; }
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
