using Agario.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using static OpenTK.Graphics.OpenGL.GL;

namespace Agario
{
    internal class Microball
    {
        public List<Vector3> gameVerts;
        public List<Vector2> gameGraphic = new List<Vector2>();
        public List<uint> gameInd;

        VAO gameVAO;
        VBO gameVertexVBO;
        VBO gameGraphicVBO;
        IBO gameIBO;

        Texture texture;

        public Microball()
        {
            GeneratePlayerMesh(0.5f, out gameVerts, out gameInd);
            for (int i = 0; i < gameVerts.Count; i++)
            {
                Vector3 n = gameVerts[i];
                n.Normalize();
                float u = (float)(MathHelper.Atan2(n.X, n.Z) / (2 * MathHelper.Pi) + 0.5f);
                float v = n.Y * 0.5f + 0.5f;
                gameGraphic.Add(new Vector2(u, v));
            }
            BuildObject("ball.PNG");
        }
        /*GenerateCoinMesh, который генерирует вершины и индексы для монеты на основе заданного масштаба (scale), 
        количества сегментов по окружности (slices) и количества сегментов по высоте (stacks).
        Метод генерирует вершины, равномерно распределенные по сферической поверхности, 
        используя сферические координаты. Затем он генерирует индексы для соединения вершин в треугольники.*/
        private void GeneratePlayerMesh(float scale, out List<Vector3> vertices, out List<uint> indices)
        {
            vertices = new List<Vector3>();
            indices = new List<uint>();
            // Define the number of segments for the surface of the player mesh around the circumference
            int slices = 22;
            // Define the number of segments for the height of the player mesh
            int stacks = 10;
            // Generate vertices for the player mesh
            for (int i = 0; i <= stacks; i++)
            {
                // Calculate the vertical coordinate (v) from 0 to 1
                float v = (float)i / stacks;
                // Calculate the polar angle (phi) from 0 to 2pi
                float phi = v * (float)Math.PI;

                for (int j = 0; j <= slices; j++)
                {
                    // Calculate the horizontal coordinate (u) from 0 to 1
                    float u = (float)j / slices;
                    // Calculate the azimuthal angle (theta) from 0 to 2pi
                    float theta = u * 2f * (float)Math.PI;
                    // Calculate the x, y, z coordinates of the vertex
                    // using spherical coordinates
                    float x = (float)(Math.Cos(theta) * Math.Sin(phi));
                    float y = (float)Math.Cos(phi);
                    float z = (float)(Math.Sin(theta) * Math.Sin(phi));

                    vertices.Add(new Vector3(x, y, z) * scale);
                }
            }

            for (int i = 0; i < stacks; i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    int first = (i * (slices + 1)) + j;
                    int second = first + slices + 1;

                    indices.Add((uint)first);
                    indices.Add((uint)second);
                    indices.Add((uint)(first + 1));

                    indices.Add((uint)second);
                    indices.Add((uint)(second + 1));
                    indices.Add((uint)(first + 1));
                }
            }
        }

        public void BuildObject(String path)
        {
            gameVAO = new VAO();
            gameVAO.Bind();

            gameVertexVBO = new VBO(gameVerts);
            gameVertexVBO.Bind();
            gameVAO.LinkToVAO(0, 3, gameVertexVBO);

            gameGraphicVBO = new VBO(gameGraphic);
            gameGraphicVBO.Bind();
            gameVAO.LinkToVAO(1, 2, gameGraphicVBO);

            gameIBO = new IBO(gameInd);

            texture = new Texture(path);
        }

        public void Render(ShaderProgram program)
        {
            program.Bind();
            gameVAO.Bind();
            gameIBO.Bind();
            texture.Bind();
            GL.DrawElements(PrimitiveType.Triangles, gameInd.Count, DrawElementsType.UnsignedInt, 0);
        }

        public void Delete()
        {
            gameVAO.Delete();
            gameVertexVBO.Delete();
            gameGraphicVBO.Delete();
            gameIBO.Delete();
            texture.Delete();
        }
    }
}
