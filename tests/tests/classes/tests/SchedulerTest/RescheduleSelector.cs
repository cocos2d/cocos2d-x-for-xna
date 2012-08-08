using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RescheduleSelector : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            m_fInterval = 1.0f;
            m_nTicks = 0;
            schedule(schedUpdate, m_fInterval);
        }

        public override string title()
        {
            return "RescheduleSelector";
        }

        public override string subtitle()
        {
            return "Interval is 1 second, then 2, then 3...";
        }

        public void schedUpdate(float dt)
        {
            m_nTicks++;
            CCLog.Log("schedUpdate: {0:G2}", dt);
            if (m_nTicks > 3)
            {
                m_nTests++;
                if (m_nTests == 5)
                {
                    CCLog.Log("Test completed");
                }
                else
                {
                    m_fInterval += 1.0f;
                    schedule(schedUpdate, m_fInterval);
                    m_nTicks = 0;
                }
            }
        }
        private float m_fInterval;
        private int m_nTicks;
        private int m_nTests;
    }
}
