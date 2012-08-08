using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerUpdate : SchedulerTestLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            TestNode d = new TestNode();
            string pStr = "---";
            d.initWithString(pStr, 50);
            addChild(d);

            TestNode b = new TestNode();
            pStr = "3rd";
            b.initWithString(pStr, 0);
            addChild(b);

            TestNode a = new TestNode();
            pStr = "1st";
            a.initWithString(pStr, -10);
            addChild(a);

            TestNode c = new TestNode();
            pStr = "4th";
            c.initWithString(pStr, 10);
            addChild(c);

            TestNode e = new TestNode();
            pStr = "5th";
            e.initWithString(pStr, 20);
            addChild(e);

            TestNode f = new TestNode();
            pStr = "2nd";
            f.initWithString(pStr, -5);
            addChild(f);

            schedule(removeUpdates, 4.0f);
        }

        public override string title()
        {
            return "Schedule update with priority";
        }

        public override string subtitle()
        {
            return "3 scheduled updates. Priority should work. Stops in 4s. See console";
        }

        void removeUpdates(float dt)
        {
            List<CCNode> children = this.children;

            foreach (var item in children)
            {
                if (item == null)
                {
                    break;
                }
                item.unsheduleAllSelectors();
            }
        }
    }
}
