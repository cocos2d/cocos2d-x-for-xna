using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerAutoremove : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            schedule(autoremove, 0.5f);
            schedule(tick, 0.5f);
            accum = 0;
        }

        public override string title()
        {
            return "ScheduleAutoremove";
        }

        public override string subtitle()
        {
            return "1 scheduler will be autoremoved in 3 seconds. See console";
        }

        public void autoremove(float dt)
        {
            accum += dt;
            CCLog.Log("Time: {0:G2}", accum);

            if (accum > 3)
            {
                unschedule(autoremove);
                CCLog.Log("scheduler removed");
            }
        }

        public void tick(float dt)
        {
            CCLog.Log("This scheduler should not be removed");
        }

        private float accum;
    }
}
