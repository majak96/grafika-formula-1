using SharpGL;
using SharpGL.SceneGraph.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF1S2._1
{
    class World
    {
        #region Atributi i Properties

        private AssimpScene m_scene1;
        private AssimpScene m_scene2;

        private float m_xRotation = 0.0f;
        private float m_yRotation = 0.0f;
        private int m_width = 0;
        private int m_height = 0;

        public AssimpScene Scene1
        {
            get { return m_scene1; }
            set { m_scene1 = value; }
        }

        public AssimpScene Scene2
        {
            get { return m_scene2; }
            set { m_scene2 = value; }
        }

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public float RotationY
        {
            get { return m_yRotation; }
            set { m_yRotation = value; }
        }

        public float RotationX
        {
            get { return m_xRotation; }
            set { m_xRotation = value; }
        }
        #endregion

        public World(String scenePath, String sceneFileName1, String sceneFileName2, int width, int height, OpenGL gl)
        {
            this.m_scene1 = new AssimpScene(scenePath, sceneFileName1, gl);
            this.m_scene2 = new AssimpScene(scenePath, sceneFileName2, gl);
            this.m_width = width;
            this.m_height = height;
        }

        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            gl.ShadeModel(OpenGL.GL_FLAT);

            gl.Enable(OpenGL.GL_DEPTH_TEST); //testiranje dubine
            gl.Enable(OpenGL.GL_CULL_FACE); //sakrivanje nevidljivih povrsina

            m_scene1.LoadScene();
            m_scene1.Initialize();

            m_scene2.LoadScene();
            m_scene2.Initialize();

        }

        public void Resize(OpenGL gl, int width, int height)
        {
            m_width = width;
            m_height = height;

            gl.Viewport(0, 0, m_width, m_height); //viewport preko celog prozora

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();

            //projekcija u perspektivi sa fov=50, near=1, far=20000
            gl.Perspective(50f, (float)m_width / m_height, 1f, 20000f);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            gl.LoadIdentity();
        }

        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
            //gl.PushMatrix();

            gl.Translate(0.0f, 5.0f, -20.0f);
            gl.Rotate(-1.0f, 0.0f, 0.0f);

            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            drawPodloga(gl);

            gl.Translate(0.0f, 0.01f, 0.0f);

            drawStaza(gl);

            gl.Translate(0.0f, 0.01f, 0.0f);

            drawLinija(gl);

            drawOgrade(gl);

            gl.PushMatrix();
            drawFormula1(gl);

            drawFormula2(gl);

            drawText(gl);

            gl.Flush();
        }

        public void drawPodloga(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.36f, 0.25f, 0.20f);
            gl.Vertex(-10.0f, -10.0f, 10.0f);
            gl.Vertex(10.0f, -10.0f, 10.0f);
            gl.Vertex(10.0f, -10.0f, -10.0f);
            gl.Vertex(-10.0f, -10.0f, -10.0f);
            gl.End();
        }

        public void drawStaza(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.32f, 0.32f, 0.32f);
            gl.Vertex(-5.0f, -10.0f, 10.0f);
            gl.Vertex(5.0f, -10.0f, 10.0f);
            gl.Vertex(5.0f, -10.0f, -10.0f);
            gl.Vertex(-5.0f, -10.0f, -10.0f);
            gl.End();
        }

        public void drawLinija(OpenGL gl)
        {
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(-0.15f, -10.0f, 10.0f);
            gl.Vertex(0.15f, -10.0f, 10.0f);
            gl.Vertex(0.15f, -10.0f, -10.0f);
            gl.Vertex(0.15f, -10.0f, -10.0f);
            gl.End();
        }

        public void drawOgrade(OpenGL gl)
        {
            gl.PushMatrix();
            gl.Color(0.55f, 0.13f, 0.13f);
            gl.Translate(-6f, -9.5f, 0f);
            gl.Scale(0.20f, 0.5f, 10.0f);
            Cube cube1 = new Cube();
            cube1.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();

            gl.PushMatrix();
            gl.Color(0.55f, 0.13f, 0.13f);
            gl.Translate(6f, -9.5f, 0f);
            gl.Scale(0.20f, 0.5f, 10.0f);
            Cube cube2 = new Cube();
            cube2.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);
            gl.PopMatrix();
        }

        public void drawFormula1(OpenGL gl)
        {
            gl.Translate(1.2f, -10.1f, 5.0f);
            gl.Scale(0.04f, 0.04f, 0.04f);
            gl.Rotate(0.0f, 180.0f, 0.0f);
            m_scene1.Draw();
            gl.PopMatrix();
        }

        public void drawFormula2(OpenGL gl)
        {
            gl.Translate(-2.5f, -9.9f, 6.2f);
            gl.Rotate(90.0f, 180.0f, 0.0f);
            gl.Scale(1.6f, 1.53f, 1.6f);
            m_scene2.Draw();
        }

        public void drawText(OpenGL gl)
        {
            //2D tekst cyan bojom u donjem desnom uglu
            //font je arial, 14pt, underline
            gl.Viewport(m_width * 2 / 3, 0, m_width / 3, m_height / 3);
            gl.DrawText(m_width - 700, 330, 0.0f, 1.0f, 1.0f, "Arial", 14, "Predmet: Racunarska grafika");
            gl.DrawText(m_width - 700, 320, 0.0f, 1.0f, 1.0f, "Arial", 14, "_______________________");
            gl.DrawText(m_width - 700, 260, 0.0f, 1.0f, 1.0f, "Arial", 14, "Sk.god: 2018/19.");
            gl.DrawText(m_width - 700, 250, 0.0f, 1.0f, 1.0f, "Arial", 14, "______________");
            gl.DrawText(m_width - 700, 190, 0.0f, 1.0f, 1.0f, "Arial", 14, "Ime: Marijana");
            gl.DrawText(m_width - 700, 180, 0.0f, 1.0f, 1.0f, "Arial", 14, "___________");
            gl.DrawText(m_width - 700, 120, 0.0f, 1.0f, 1.0f, "Arial", 14, "Prezime: Kolosnjaji");
            gl.DrawText(m_width - 700, 110, 0.0f, 1.0f, 1.0f, "Arial", 14, "_______________");
            gl.DrawText(m_width - 700, 50, 0.0f, 1.0f, 1.0f, "Arial", 14, "Sifra zad: 2.1");
            gl.DrawText(m_width - 700, 40, 0.0f, 1.0f, 1.0f, "Arial", 14, "___________");
            gl.Viewport(0, 0, m_width, m_height);
        }

        #region Destruktori

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~World()
        {
            this.Dispose(false);
        }

        #endregion

        #region IDisposable Metode

        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    //Oslobodi managed resurse
            //}
        }

        #endregion
    }

}
