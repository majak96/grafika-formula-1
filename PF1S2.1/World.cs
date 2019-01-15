using SharpGL;
using SharpGL.SceneGraph.Primitives;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AssimpSample;
using System.Windows.Threading;

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

        private float m_ambientBlue = 0.0f;
        private float m_ambientRed = 0.0f;
        private float m_ambientGreen = 0.0f;

        private float m_rightTranslateX = 2.2f;
        private float m_leftRotateY = 270.0f;
        private float m_leftTranslateZ = 11.4f;
        private float m_rightTranslateZ = 11.4f;
        private float m_lightTranslate = 14.0f;

        private float m_cameraZ = 15.0f;
        private float m_cameraY = 7.0f;
        private float m_cameraX = 0.0f;
        private float m_pointZ = -12.0f;

        private enum Textures { Asphalt = 0, Metal = 1, Gravel = 2 };
        private uint[] m_textures = null;
        private string[] m_textureImages = { "..//..//Textures//asfalt.jpg", "..//..//Textures//metal.jpg", "..//..//Textures//gravel.jpg" };
        private int m_textureCount = Enum.GetNames(typeof(Textures)).Length;

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

        public float AmbientRed
        {
            get { return m_ambientRed; }
            set { m_ambientRed = value; }
        }

        public float AmbientGreen
        {
            get { return m_ambientGreen; }
            set { m_ambientGreen = value; }
        }

        public float AmbientBlue
        {
            get { return m_ambientBlue; }
            set { m_ambientBlue = value; }
        }

        public float RightTranslateX
        {
            get { return m_rightTranslateX; }
            set { m_rightTranslateX = value; }
        }

        public float LeftTranslateZ
        {
            get { return m_leftTranslateZ; }
            set { m_leftTranslateZ = value; }
        }

        public float RightTranslateZ
        {
            get { return m_rightTranslateZ; }
            set { m_rightTranslateZ = value; }
        }

        public float LeftRotateY
        {
            get { return m_leftRotateY; }
            set { m_leftRotateY = value; }
        }

        public float LightTranslate
        {
            get { return m_lightTranslate; }
            set { m_lightTranslate = value; }
        }

        public float CameraZ
        {
            get { return m_cameraZ; }
            set { m_cameraZ = value; }
        }

        public float CameraY
        {
            get { return m_cameraY; }
            set { m_cameraY = value; }
        }
        public float CameraX
        {
            get { return m_cameraX; }
            set { m_cameraX = value; }
        }

        public float PointZ
        {
            get { return m_pointZ; }
            set { m_pointZ = value; }
        }
        #endregion

        public World(String scenePath1, String scenePath2, String sceneFileName1, String sceneFileName2, int width, int height, OpenGL gl)
        {
            this.m_scene1 = new AssimpScene(scenePath1, sceneFileName1, gl);
            this.m_scene2 = new AssimpScene(scenePath2, sceneFileName2, gl);
            this.m_width = width;
            this.m_height = height;

            m_textures = new uint[m_textureCount];
        }

        public void Initialize(OpenGL gl)
        {
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            gl.ShadeModel(OpenGL.GL_FLAT);

            gl.Enable(OpenGL.GL_DEPTH_TEST); //testiranje dubine
            gl.Enable(OpenGL.GL_CULL_FACE); //sakrivanje nevidljivih povrsina

            SetupLightning(gl); //podesavanje svetla

            SetupTexture(gl); //podesavanje teksture

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

            //ChangeAmbientLightning(gl);

            gl.LoadIdentity();

            //pozicioniranje kamere - da gleda na scenu spreda i odgore (6)
            gl.LookAt(m_cameraX, m_cameraY, m_cameraZ, 0.0f, 0.0f, m_pointZ, 0.0f, 1.0f, 0.0f);
            //gl.PushMatrix();
            //gl.LookAt(0.0f, 60.0f, 0.0f, 0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f);

            gl.Translate(0.0f, 0.0f, -11.0f);

            gl.Rotate(m_xRotation, 1.0f, 0.0f, 0.0f);
            gl.Rotate(m_yRotation, 0.0f, 1.0f, 0.0f);

            UpdateReflektorskoSvetlo(gl);

            DrawPodloga(gl);

            DrawStaza(gl);

            DrawLinija(gl);

            DrawOgrade(gl);
         
            DrawFormula1(gl);

            DrawFormula2(gl);

            DrawText(gl);

            gl.Flush();
        }

        public void DrawPodloga(OpenGL gl)
        {
            //gl.Scale(1.0f, 1.0f, 2.0f);

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Gravel]); //tekstura sljunka (5)

            gl.MatrixMode(OpenGL.GL_TEXTURE); //skaliranje pomocu Texture matrice (5)

            gl.LoadIdentity();
            gl.Scale(3f, 3f, 3f); 

            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            gl.Begin(OpenGL.GL_QUADS);
            //gl.Color(0.36f, 0.25f, 0.20f); //braon podloga

            gl.Normal(0.0f, 0.1f, 0.0f); //normala (2)

            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(-10.0f, 0.0f, 15.0f);
            gl.TexCoord(0.0f, 10.0f);
            gl.Vertex(10.0f, 0.0f, 15.0f);
            gl.TexCoord(10.0f, 10.0f);
            gl.Vertex(10.0f, 0.0f, -35.0f);
            gl.TexCoord(10.0f, 0.0f);
            gl.Vertex(-10.0f, 0.0f, -35.0f);

            gl.End();

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawStaza(OpenGL gl)
        {
            gl.Translate(0.0f, 0.01f, 0.0f); //staza je malo iznad podloge

            gl.Enable(OpenGL.GL_TEXTURE_2D);

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Asphalt]); //tekstura asfalta (4)

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(0.0f, 0.0f, 0.0f);

            gl.Normal(0.0f, 0.1f, 0.0f); //normala (2)

            gl.TexCoord(0.0f, 0.0f);
            gl.Vertex(-5.0f, 0.0f, 15.0f);
            gl.TexCoord(0.0f, 4.0f);
            gl.Vertex(5.0f, 0.0f, 15.0f);
            gl.TexCoord(4.0f, 4.0f);
            gl.Vertex(5.0f, 0.0f, -35.0f);
            gl.TexCoord(4.0f, 0.0f);
            gl.Vertex(-5.0f, 0.0f, -35.0f);

            gl.End();

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawLinija(OpenGL gl)
        {
            gl.Translate(0.0f, 0.01f, 0.0f); //linija je malo iznad staze

            gl.Begin(OpenGL.GL_QUADS);
            gl.Color(1.0f, 1.0f, 1.0f); //bela linija

            gl.Normal(0.0f, 0.1f, 0.0f); //normala

            gl.Vertex(-0.10f, 0.0f, 15.0f);
            gl.Vertex(0.10f, 0.0f, 15.0f);
            gl.Vertex(0.10f, 0.0f, -35.0f);
            gl.Vertex(-0.10f, 0.0f, -35.0f);

            gl.End();
        }

        public void DrawOgrade(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[(int)Textures.Metal]); //tekstura metalne zastitne ograde (4)

            //leva ograda
            gl.PushMatrix();
            gl.Color(0.55f, 0.13f, 0.13f); //crvena kocka
            gl.Translate(-6f, 0.5f, -10.0f);
            gl.Scale(0.10f, 0.5f, 25.0f);

            Cube cube1 = new Cube();
            cube1.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();

            //desna ograda
            gl.PushMatrix();
            gl.Color(0.55f, 0.13f, 0.13f); //crvena kocka
            gl.Translate(6f, 0.5f, -10.0f);
            gl.Scale(0.10f, 0.5f, 25.0f);

            Cube cube2 = new Cube();
            cube2.Render(gl, SharpGL.SceneGraph.Core.RenderMode.Render);

            gl.PopMatrix();

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public void DrawFormula1(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_3D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE); //način stapanja teksture sa materijalom (10)
            
            //desna formula
            gl.PushMatrix();
            gl.Translate(m_rightTranslateX, -0.1f, m_rightTranslateZ);
            gl.Scale(0.037f, 0.037f, 0.040f);
            //gl.Rotate(0.0f, 0.0f, 0.0f);

            m_scene1.Draw();

            gl.PopMatrix();

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL); //način stapanja teksture sa materijalom
            gl.Disable(OpenGL.GL_TEXTURE_3D);
        }

        public void DrawFormula2(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_3D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE); //način stapanja teksture sa materijalom (10)

            //leva formula
            gl.PushMatrix();
            gl.Translate(-2.2f, 0.1f, m_leftTranslateZ);
            gl.Rotate(0.0f, m_leftRotateY, 0.0f);
            gl.Scale(0.015f, 0.015f, 0.015f);
          
            m_scene2.Draw();

            gl.PopMatrix();

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL); //način stapanja teksture sa materijalom
            gl.Disable(OpenGL.GL_TEXTURE_3D);
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

        #region setup

        public void RefreshScene()
        {
            LeftTranslateZ = 11.4f;
            RightTranslateZ = 11.4f;
            RightTranslateX = 2.2f;
            LeftRotateY = 270.0f;
            CameraZ = 15.0f;
            CameraY = 7.0f;
            CameraX = 0.0f;
            PointZ = -12.0f;
            m_xRotation = 0.0f;
            m_yRotation = 0.0f;
            m_lightTranslate = 14.0f;
    }

        /*SETTING UP TEXTURE*/
        public void SetupTexture(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_DECAL); //način stapanja teksture sa materijalom (3)

            gl.GenTextures(m_textureCount, m_textures);
            for (int i = 0; i < m_textureCount; ++i)
            {
                gl.BindTexture(OpenGL.GL_TEXTURE_2D, m_textures[i]);

                Bitmap image = new Bitmap(m_textureImages[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                gl.Build2DMipmaps(OpenGL.GL_TEXTURE_2D, (int)OpenGL.GL_RGBA8, image.Width, image.Height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, bitmapdata.Scan0);


                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR); //filteri za teksture (3)
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR); //linearno filtriranje

                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT); //wrapping (3)
                gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT); //po obema osama

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        /*SETTING UP LIGHTING*/ 
        public void SetupLightning(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_LIGHTING);

            SetupTackastoSvetlo(gl);
            SetupReflektorskoSvetlo(gl);

            gl.Enable(OpenGL.GL_COLOR_MATERIAL); //color tracking mehanizam (1)
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE); //ambijentalna i difuzna komponenta (1)

            gl.Enable(OpenGL.GL_NORMALIZE); //automatska normalizacija (2)

        }
        public void SetupTackastoSvetlo(OpenGL gl)
        {
            float[] ambientColor = { 0.1f, 0.1f, 0.0f, 1.0f };
            float[] diffuseColor = { 0.97f, 0.98f, 0.61f, 1.0f }; //tackasti izvor svetlo-zute boje (2)

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, ambientColor);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, diffuseColor);

            //specijalni slucaj reflektorskog izvora (2)
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPOT_CUTOFF, 180.0f); //tackasti izvor -> cutoff = 180 stepeni

            gl.Enable(OpenGL.GL_LIGHT0);

            //gore levo u odnosu na centar scene (2)
            float[] lightPosition = { -10.0f, 6.0f, -26.0f, 1.0f };

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPosition);

        }

        public void SetupReflektorskoSvetlo(OpenGL gl)
        {
            float[] ambientColor = { m_ambientRed, m_ambientGreen, m_ambientBlue, 1.0f };
            float[] diffuseColor = { 1.0f, 1.0f, 1.0f, 1.0f }; //reflektorski izvor bele boje (9)

            float[] direction = { 0.0f, -1.0f, 0.0f }; //smer ka dole 

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambientColor);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, diffuseColor);

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction); //smer svetla
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_CUTOFF, 45.0f); //reflektorsko svetlo, cutoff = 45 stepeni (9)

            gl.Enable(OpenGL.GL_LIGHT1);

            //iznad automobila
            float[] lightPosition = { 0.0f, 6.0f, m_lightTranslate, 1.0f };

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightPosition);

        }

        public void UpdateReflektorskoSvetlo(OpenGL gl)
        {
            float[] ambientColor = { m_ambientRed, m_ambientGreen, m_ambientBlue, 1.0f};
            float[] direction = { 0.0f, -1.0f, 0.0f }; //smer ka dole 
            float[] lightPosition = { 0.0f, 6.0f, m_lightTranslate, 1.0f };

            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, ambientColor);
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPOT_DIRECTION, direction); //smer svetla
            gl.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, lightPosition);

        }

        #endregion

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
