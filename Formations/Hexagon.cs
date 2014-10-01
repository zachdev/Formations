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
        VertexBuffer vertexBuffer;

        BasicEffect basicEffect;
        Matrix world;
        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), 1200f / 600f , 0.01f, 100f);

        public Hexagon()
        {

        }
        public void init(float x, float y, GraphicsDevice graphicsDevice)
        {
            world = Matrix.CreateTranslation(x, y, 0);
            basicEffect = new BasicEffect(graphicsDevice);
            Vector3[] vectors= CalculateVertices(6, 1, new Vector3(0,1,0));

            VertexPositionColor[] vertices = new VertexPositionColor[18];
            vertices[0] = new VertexPositionColor(new Vector3(-0.855f, 1.5f, 0), Color.Blue);
            vertices[1] = new VertexPositionColor(vectors[0], Color.Green);
            vertices[2] = new VertexPositionColor(vectors[1], Color.Green);
            vertices[3] = new VertexPositionColor(new Vector3(-0.855f, 1.5f, 0), Color.Blue);
            vertices[4] = new VertexPositionColor(vectors[1], Color.Green);
            vertices[5] = new VertexPositionColor(vectors[2], Color.Green);

            vertices[6] = new VertexPositionColor(new Vector3(-0.855f,1.5f,0), Color.Blue);
            vertices[7] = new VertexPositionColor(vectors[2], Color.Green);
            vertices[8] = new VertexPositionColor(vectors[3], Color.Green);
            vertices[9] = new VertexPositionColor(new Vector3(-0.855f, 1.5f, 0), Color.Blue);
            vertices[10] = new VertexPositionColor(vectors[3], Color.Green);
            vertices[11] = new VertexPositionColor(vectors[4], Color.Green);

            vertices[12] = new VertexPositionColor(new Vector3(-0.855f, 1.5f, 0), Color.Blue);
            vertices[13] = new VertexPositionColor(vectors[4], Color.Green);
            vertices[14] = new VertexPositionColor(vectors[5], Color.Green);
            vertices[15] = new VertexPositionColor(new Vector3(-0.855f, 1.5f, 0), Color.Blue);
            vertices[16] = new VertexPositionColor(vectors[5], Color.Green);
            vertices[17] = new VertexPositionColor(vectors[0], Color.Green);

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 100, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
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
        public void draw(SpriteBatch spriteBatch)
        {
            
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            spriteBatch.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                spriteBatch.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 6);
            }
        }
    }
}
