using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerSchedulesAndRemove : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();
            schedule(tick1, 0.5f);
            schedule(tick2, 1.0f);
            schedule(scheduleAndUnschedule, 4.0f);
        }

        public override string title()
        {
            return "Schedule from Schedule";
        }

        public override string subtitle()
        {
            return "Will unschedule and schedule selectors in 4s. See console";
        }

        public void tick1(float dt)
        {
            CCLog.Log("tick1");
        }

        public void tick2(float dt)
        {
            CCLog.Log("tick2");
        }

        public void tick3(float dt)
        {
            CCLog.Log("tick3");
        }

        public void tick4(float dt)
        {
            CCLog.Log("tick4");
        }

        public void scheduleAndUnschedule(float dt)
        {
            unschedule(tick1);
            unschedule(tick2);
            unschedule(scheduleAndUnschedule);

            schedule(tick3, 1.0f);
            schedule(tick4, 1.0f);
        }
    }
}
