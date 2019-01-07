using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PF1S2._1
{
    class Animation
    {

        private World m_world = null;
        private DispatcherTimer leftCarTimer;
        private DispatcherTimer rightCarTimer;
        private bool animationOn = false;

        public Animation(World world)
        {
            m_world = world;
        }

        public bool AnimationOn { get => animationOn; set => animationOn = value; }

        public void LeftCarAnimation(object sender, EventArgs e)
        {
            m_world.LeftTranslateZ -= 2.0f;
            if (m_world.LeftTranslateZ <= -30.0f)
            {
                leftCarTimer.Stop();
                //animation = false;
            }

        }

        public void RightCarAnimation(object sender, EventArgs e)
        {
            m_world.RightTranslateZ -= 2.0f;
            if (m_world.RightTranslateZ <= -30.0f)
            {
                rightCarTimer.Stop();
                animationOn = false;
            }

        }

        public void StartAnimation()
        {
            m_world.LeftTranslateZ = 11.4f;
            m_world.RightTranslateZ = 11.4f;
            m_world.RightTranslateX = 2.2f;
            m_world.LeftRotateY = 270.0f;

            animationOn = true;
            leftCarTimer = new DispatcherTimer();
            leftCarTimer.Interval = TimeSpan.FromMilliseconds(5);
            leftCarTimer.Tick += new EventHandler(LeftCarAnimation);

            rightCarTimer = new DispatcherTimer();
            rightCarTimer.Interval = TimeSpan.FromMilliseconds(60);
            rightCarTimer.Tick += new EventHandler(RightCarAnimation);

            leftCarTimer.Start();
            rightCarTimer.Start();
        }
    }
}
