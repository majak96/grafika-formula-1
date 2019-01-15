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
        Animation animation = null;

        public MainWindow()
        {
            InitializeComponent();
            //kreiranje OpenGL sveta
            
            try
            {
                m_world = new World(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ModelFormula1"),
                                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ModelFormula2"),
                                    "rio.obj", "Car_Obj.obj", (int)openGLControl.Width, (int)openGLControl.Height, openGLControl.OpenGL);

                animation = new Animation(m_world);
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
            if (!animation.AnimationOn)
            {
                switch (e.Key)
                {
                    //rotacija za 5 stepeni oko horizontalne ose (8)
                    case Key.I: 
                        if (m_world.RotationX >= 0.0f) //da se nikada ne vidi donja strana
                            m_world.RotationX -= 5.0f;
                        break;
                    case Key.K: 
                        if (m_world.RotationX <= 90.0f) //da se nikada ne vidi donja strana
                            m_world.RotationX += 5.0f;
                        break;
                    //rotacija za 5 stepeni oko vertikalne ose (8)
                    case Key.J: m_world.RotationY -= 5.0f; break;
                    case Key.L: m_world.RotationY += 5.0f; break;
                    //priblizavanje i udaljavanje od centra scene
                    case Key.OemPlus: m_world.CameraZ -= 2.0f; break;
                    case Key.OemMinus: m_world.CameraZ += 2.0f; break;
                    //animacija (11)
                    case Key.V:
                        animation.StartAnimation(); break;
                    case Key.F4: Application.Current.Shutdown(); break;
                }
            }
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //izbor boje ambijentalne komponente reflektorskog svetlosnog izvora (7)
            if (m_world != null &&  !animation.AnimationOn)
            {
                m_world.AmbientRed = (float)slColorR.Value;
                m_world.AmbientGreen = (float)slColorG.Value;
                m_world.AmbientBlue = (float)slColorB.Value;
            }

        }

        private void TranslationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //transliranje desnog bolida po hoorizontalnoj osi (7)
            if (m_world != null && !animation.AnimationOn)
            {
                m_world.RightTranslateX = (float)translate.Value;
            }
        }

        private void RotationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //rotiranje levog bolida oko vertikalne ose (7)
            if (m_world != null && !animation.AnimationOn)
            {
                m_world.LeftRotateY = (float)rotate.Value;
            }
        }

        private void ButtonClick_Refresh(object sender, RoutedEventArgs e)
        {
            if (m_world != null && !animation.AnimationOn)
            {
                m_world.RefreshScene();
            }
        }
    }
}
