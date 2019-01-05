using SharpGL.SceneGraph;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PF1S2._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        World m_world = null;

        public MainWindow()
        {
            InitializeComponent();
            //kreiranje OpenGL sveta
            try
            {
                m_world = new World(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "3D Models\\Formula"), "Formula1.3DS", "Formula2.obj", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Neuspesno kreirana instanca OpenGL sveta", "GRESKA", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            //iscrtavanje sveta
            m_world.Draw(args.OpenGL);
        }

        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            m_world.Initialize(args.OpenGL);
        }

        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            m_world.Resize(args.OpenGL, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.I: m_world.RotationX -= 5.0f; break;
                case Key.K: m_world.RotationX += 5.0f; break;
                case Key.J: m_world.RotationY -= 5.0f; break;
                case Key.L: m_world.RotationY += 5.0f; break;
                case Key.Q: m_world.CenterZoom -= 2.0f; break;
                case Key.A: m_world.CenterZoom += 2.0f; break;
                case Key.F4: Application.Current.Shutdown(); break;
            }
        }
    }
}
