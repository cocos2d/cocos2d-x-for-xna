using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using cocos2d;

namespace tests
{
    public class SchedulerPauseResume : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            schedule(tick1, 0.5f);
            schedule(tick2, 0.5f);
            schedule(pause, 0.5f);
        }

        public override string title()
        {
            return "SchedulerPauseResume";
        }

        public virtual string subtitle()
        {
            return "Scheduler should be paused after 3 seconds. See console";
        }

        public void tick1(float dt)
        {
            CCLog.Log("tick1");
        }

        public void tick2(float dt)
        {
            CCLog.Log("tick2");
        }

        public void pause(float dt)
        {
            CCLog.Log("Pausing target");
            CCScheduler.sharedScheduler().pauseTarget(this);
        }
    }
}
