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

        private AssimpScene m_scene;

        private float m_xRotation = 0.0f;
        private float m_yRotation = 0.0f;
        private int m_width = 0;
        private int m_height = 0;

        public AssimpScene Scene
        {
            get { return m_scene; }
            set { m_scene = value; }
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

        public World(String scenePath, String sceneFileName, int width, int height, OpenGL gl)
        {
            this.m_scene = new AssimpScene(scenePath, sceneFileName, gl);
            this.m_width = width;
            this.m_height = height;
        }

        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            gl.ShadeModel(OpenGL.GL_FLAT);

            gl.Enable(OpenGL.GL_DEPTH_TEST); //testiranje dubine
            gl.Enable(OpenGL.GL_CULL_FACE); //sakrivanje nevidljivih povrsina

            m_scene.LoadScene();
            m_scene.Initialize();

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

            //podloga
            gl.Begin(OpenGL.GL_QUADS);            
            gl.Color(0.36f, 0.25f, 0.20f);
            gl.Vertex(-10.0f, -10.0f, 10.0f);
            gl.Vertex(10.0f, -10.0f, 10.0f);
            gl.Vertex(10.0f, -10.0f, -10.0f);
            gl.Vertex(-10.0f, -10.0f, -10.0f);
            gl.End();

            gl.Translate(0.0f, 0.01f, 0.0f);

            //staza
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.32f, 0.32f, 0.32f);
            gl.Vertex(-5.0f, -10.0f, 10.0f);
            gl.Vertex(5.0f, -10.0f, 10.0f);
            gl.Vertex(5.0f, -10.0f, -10.0f);
            gl.Vertex(-5.0f, -10.0f, -10.0f);
            gl.End();

            gl.Translate(0.0f, 0.01f, 0.0f);

            //linija
            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(-0.15f, -10.0f, 10.0f);
            gl.Vertex(0.15f, -10.0f, 10.0f);
            gl.Vertex(0.15f, -10.0f, -10.0f);
            gl.Vertex(0.15f, -10.0f, -10.0f);
            gl.End();


            //ograde
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

            //gl.PushMatrix();
            //gl.Translate(0.0f, 0.0f, -600f);
            //m_scene.Draw();
            //gl.PopMatrix();

            //gl.PushMatrix();
            //2D tekst cyan bojom u donjem desnom uglu
            //font je arial, 14pt, underline
            gl.Viewport(m_width * 2 / 3, 0, m_width / 3, m_height / 3);
            gl.DrawText(m_width-700, 330, 0.0f, 1.0f, 1.0f, "Arial", 14, "Predmet: Racunarska grafika");
            gl.DrawText(m_width-700, 320, 0.0f, 1.0f, 1.0f, "Arial", 14, "_______________________");
            gl.DrawText(m_width-700, 260, 0.0f, 1.0f, 1.0f, "Arial", 14, "Sk.god: 2018/19.");
            gl.DrawText(m_width-700, 250, 0.0f, 1.0f, 1.0f, "Arial", 14, "______________");
            gl.DrawText(m_width-700, 190, 0.0f, 1.0f, 1.0f, "Arial", 14, "Ime: Marijana");
            gl.DrawText(m_width-700, 180, 0.0f, 1.0f, 1.0f, "Arial", 14, "___________");
            gl.DrawText(m_width-700, 120, 0.0f, 1.0f, 1.0f, "Arial", 14, "Prezime: Kolosnjaji");
            gl.DrawText(m_width-700, 110, 0.0f, 1.0f, 1.0f, "Arial", 14, "_______________");
            gl.DrawText(m_width-700, 50, 0.0f, 1.0f, 1.0f, "Arial", 14, "Sifra zad: 2.1");
            gl.DrawText(m_width-700, 40, 0.0f, 1.0f, 1.0f, "Arial", 14, "___________");
            gl.Viewport(0, 0, m_width, m_height);

            //gl.PopMatrix();
            gl.Flush();
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
