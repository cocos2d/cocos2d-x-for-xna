using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerUpdateAndCustom : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();
            scheduleUpdate();
            schedule(tick);
            schedule(stopSelectors, 0.4f);
        }

        public override string title()
        {
            return "Schedule Update + custom selector";
        }

        public virtual string subtitle()
        {
            return "Update + custom selector at the same time. Stops in 4s. See console";
        }

        public void update(float dt)
        {
            CCLog.Log("update called:{0}", dt);
        }

        public void tick(float dt)
        {
            CCLog.Log("custom selector called:{0}", dt);
        }

        public void stopSelectors(float dt)
        {
            base.unscheduleAllSelectors();
        }
    }
}
