using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerUpdateFromCustom : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();
            schedule(schedUpdate, 2.0f);
        }

        public virtual string title()
        {
            return "Schedule Update in 2 sec";
        }

        public virtual string subtitle()
        {
            return "Update schedules in 2 secs. Stops 2 sec later. See console";
        }

        public void update(float dt)
        {
            CCLog.Log("update called:{0:G2}", dt);
        }

        public void schedUpdate(float dt)
        {
            unschedule(schedUpdate);
            base.sheduleUpdate();
            schedule(stopUpdate, 2.0f);
        }

        public void stopUpdate(float dt)
        {
            unscheduleUpdate();
            unschedule(stopUpdate);
        }
    }
}
