using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace tests
{
    public class SchedulerAutoremove : SchedulerTestLayer
    {
        public virtual void onEnter()
        {
            base.onEnter();

            schedule(autoremove, 0.5f);
            schedule(tick, 0.5f);
            accum = 0;
        }

        public override string title()
        {
            return "Self-remove an scheduler";
        }

        public override string subtitle()
        {
            return "1 scheduler will be autoremoved in 3 seconds. See console";
        }

        public void autoremove(float dt)
        {
            accum += dt;
            Debug.WriteLine("Time: %f", accum);

            if (accum > 3)
            {
                unschedule(autoremove);
                Debug.WriteLine("scheduler removed");
            }
        }

        public void tick(float dt)
        {
            Debug.WriteLine("This scheduler should not be removed");
        }

        private float accum;
    }
}
