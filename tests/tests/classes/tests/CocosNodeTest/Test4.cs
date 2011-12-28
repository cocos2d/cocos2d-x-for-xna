using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Test4 : TestCocosNodeDemo
    {
        public Test4()
        {
            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (new CCPoint(100, 160));
            sp2.position = (new CCPoint(380, 160));

            addChild(sp1, 0, 2);
            addChild(sp2, 0, 3);

            schedule(new SEL_SCHEDULE(this.delay2), 2.0f);
            schedule(new SEL_SCHEDULE(this.delay4), 4.0f);
        }

        public void delay2(float dt)
        {
            CCSprite node = (CCSprite)(getChildByTag(2));
            CCAction action1 = CCRotateBy.actionWithDuration(1, 360);
            node.runAction(action1);
        }

        public void delay4(float dt)
        {
            unschedule(new SEL_SCHEDULE(this.delay4));
            removeChildByTag(3, false);
        }

        public override string title()
        {
            return "tags";
        }
    }
}
