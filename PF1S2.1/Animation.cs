using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PF1S2._1
{
    class Animation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private World m_world = null;
        private DispatcherTimer leftCarTimer;
        private DispatcherTimer rightCarTimer;
        private DispatcherTimer cameraTimer;

        private bool animationNotOn = true;

        public Animation(World world)
        {
            m_world = world;
        }

        public bool AnimationNotOn {
            get
            {
                return animationNotOn;
            }
            set {
                animationNotOn = value;
                  OnPropertyChanged("AnimationNotOn");
            }
        }

        public void LeftCarAnimation(object sender, EventArgs e)
        {
            m_world.LeftTranslateZ -= 2.0f;
            m_world.LightTranslate -= 2.0f;
            if (m_world.LeftTranslateZ <= -30.0f)
            {
                leftCarTimer.Stop();
                m_world.LeftTranslateZ -= 0.35f;
                //animation = false;
            }
        }

        public void RightCarAnimation(object sender, EventArgs e)
        {
            m_world.RightTranslateZ -= 2.0f;
            if (m_world.RightTranslateZ <= -30.0f)
            {
                rightCarTimer.Stop();
                cameraTimer.Stop();
                AnimationNotOn = true;
            }
       
        }

        public void CameraAnimation(object sender, EventArgs e)
        {
            if(m_world.CameraZ > (m_world.PointZ + 10.0f))
            {
                m_world.CameraZ -= 2.0f;
            }
            else
            {
                m_world.CameraY += 7.0f;
            }
        }

        public void StartAnimation()
        {
            m_world.RefreshScene();

            AnimationNotOn = false;
            leftCarTimer = new DispatcherTimer();
            leftCarTimer.Interval = TimeSpan.FromMilliseconds(5);
            leftCarTimer.Tick += new EventHandler(LeftCarAnimation);

            rightCarTimer = new DispatcherTimer();
            rightCarTimer.Interval = TimeSpan.FromMilliseconds(60);
            rightCarTimer.Tick += new EventHandler(RightCarAnimation);

            cameraTimer = new DispatcherTimer();
            cameraTimer.Interval = TimeSpan.FromMilliseconds(60);
            cameraTimer.Tick += new EventHandler(CameraAnimation);

            leftCarTimer.Start();
            rightCarTimer.Start();
            cameraTimer.Start();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
