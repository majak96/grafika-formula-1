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
        private float m_centerZoom = 0.0f;

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

        public float CenterZoom
        {
            get { return m_centerZoom; }
            set { m_centerZoom = value; }
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

            SetupLightning(gl); //podesavanje svetla

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


        public void SetupLightning(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_LIGHTING);

            SetupTackastoSvetlo(gl);
            SetupReflektorskoSvetlo(gl);

            gl.Enable(OpenGL.GL_COLOR_MATERIAL); //color tracking mehanizam
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE); //ambijentalna i difuzna komponenta

            gl.Enable(OpenGL.GL_NORMALIZE); //automatska normalizacija

        }
        public void SetupTackastoSvetlo(OpenGL gl)
        {
            float[] ambientColor = { 0.1f, 0.1f, 0.0f, 1.0f };
            float[] diffuseColor = { 0.97f, 0.98f, 0.61f, 1.0f }; //tackasti izvor svetlo-zute boje

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambientColor);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, diffuseColor);

            //specijalni slucaj reflektorskog izvora
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f); //tackasti izvor -> cutoff = 180 stepeni

            gl.Enable(OpenGL.GL_LIGHT0);

            //gore levo u odnosu na centar scene
            float[] lightPosition = { -10.0f, 6.0f, -26.0f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPosition);

        }

        public void SetupReflektorskoSvetlo(OpenGL gl)
        {
            float[] ambientColor = { 0.1f, 0.1f, 0.1f, 1.0f };
            float[] diffuseColor = { 1.0f, 1.0f, 1.0f, 1.0f }; //reflektorski izvor bele boje

            float[] direction = { 0.0f, -1.0f, 0.0f }; //smer ka dole 

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambientColor);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, diffuseColor);

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction); //smer svetla
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 45.0f); //reflektorsko svetlo, cutoff = 45 stepeni

            gl.Enable(OpenGL.GL_LIGHT1);

            //iznad automobila
            float[] lightPosition = { 0.0f, 5.0f, -15.0f, 1.0f };



            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightPosition);

        }

        public void Draw(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();

            //pozicioniranje kamere - da gleda na scenu spreda i odgore
            gl.LookAt(0.0f, 7.0f, 0.0f, 0.0f, 0.0f, -26.0f, 0.0f, 1.0f, 0.0f);
            //gl.PushMatrix();

            gl.Translate(0.0f, 0.0f, m_centerZoom - 11.0f);
            //gl.Rotate(-1.0f, 0.0f, 0.0f);

            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            DrawPodloga(gl);

            DrawStaza(gl);

            DrawLinija(gl);

            DrawOgrade(gl);
         
            DrawFormula1(gl);

            drawFormula2(gl);

            DrawText(gl);

            gl.Flush();
        }

        public void DrawPodloga(OpenGL gl)
        {
            //gl.Scale(1.0f, 1.0f, 2.0f);

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.36f, 0.25f, 0.20f); //braon podloga

            gl.Normal(0.0f, 0.1f, 0.0f); //normala

            gl.Vertex(-10.0f, 0.0f, 0.0f);
            gl.Vertex(10.0f, 0.0f, 0.0f);
            gl.Vertex(10.0f, 0.0f, -30.0f);
            gl.Vertex(-10.0f, 0.0f, -30.0f);

            gl.End();
        }

        public void DrawStaza(OpenGL gl)
        {
            gl.Translate(0.0f, 0.01f, 0.0f); //staza je malo iznad podloge

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.32f, 0.32f, 0.32f); //siva staza

            gl.Normal(0.0f, 0.1f, 0.0f); //normala

            gl.Vertex(-5.0f, 0.0f, 0.0f);
            gl.Vertex(5.0f, 0.0f, 0.0f);
            gl.Vertex(5.0f, 0.0f, -30.0f);
            gl.Vertex(-5.0f, 0.0f, -30.0f);

            gl.End();
        }

        public void DrawLinija(OpenGL gl)
        {
            gl.Translate(0.0f, 0.01f, 0.0f); //linija je malo iznad staze

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f); //bela linija

            gl.Vertex(-0.10f, 0.0f, 0.0f);
            gl.Vertex(0.10f, 0.0f, 0.0f);
            gl.Vertex(0.10f, 0.0f, -30.0f);
            gl.Vertex(-0.10f, 0.0f, -30.0f);

            gl.End();
        }

        public void DrawOgrade(OpenGL gl)
        {
            //leva ograda
            gl.PushMatrix();
            gl.Color(0.55f, 0.13f, 0.13f); //crvena kocka
            gl.Translate(-6f, 0.5f, -15.0f);
            gl.Scale(0.20f, 0.5f, 15.0f);

            Cube cube1 = new Cube();
            cube1.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();

            //desna ograda
            gl.PushMatrix();
            gl.Color(0.55f, 0.13f, 0.13f); //crvena kocka
            gl.Translate(6f, 0.5f, -15.0f);
            gl.Scale(0.20f, 0.5f, 15.0f);

            Cube cube2 = new Cube();
            cube2.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();
        }

        public void DrawFormula1(OpenGL gl)
        {
            //desna formula
            gl.PushMatrix();
            gl.Translate(1.2f, -0.1f, -5.0f);
            gl.Scale(0.04f, 0.04f, 0.04f);
            gl.Rotate(0.0f, 180.0f, 0.0f);

            m_scene1.Draw();

            gl.PopMatrix();
        }

        public void drawFormula2(OpenGL gl)
        {
            //leva formula
            gl.PushMatrix();
            gl.Translate(-2.5f, 0.06f, -3.8f);
            gl.Rotate(90.0f, 180.0f, 0.0f);
            gl.Scale(1.6f, 1.53f, 1.6f);

            m_scene2.Draw();

            gl.PopMatrix();
        }

        public void DrawText(OpenGL gl)
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

        public float[] FindFaceNormal(float x1, float y1, float z1,
                                              float x2, float y2, float z2,
                                              float x3, float y3, float z3)
        {
            float[] normal = new float[3];

            // Racunanje normale na ravan odredjenu sa tri tacke definisane u CCW smeru
            normal[0] = (y1 - y2) * (z2 - z3) - (y2 - y3) * (z1 - z2);
            normal[1] = (x2 - x3) * (z1 - z2) - (x1 - x2) * (z2 - z3);
            normal[2] = (x1 - x2) * (y2 - y3) - (x2 - x3) * (y1 - y2);

            // Duzina vektora normale
            float len = (float)(Math.Sqrt((normal[0] * normal[0]) + (normal[1] * normal[1]) + (normal[2] * normal[2])));

            // Izbegava se deljenje sa nulom
            if (len == 0.0f)
            {
                len = 1.0f;
            }

            // Normalizacija vektora normale
            normal[0] /= len;
            normal[1] /= len;
            normal[2] /= len;

            return normal;
        }

        #endregion
    }

}
