using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{

    public class CrashTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";

        public override string title()
        {
            return "Test 1. Should not crash";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSprite child = CCSprite.spriteWithFile(s_pPathGrossini);
            child.position = (new CCPoint(200, 200));
            addChild(child, 1);

            //Sum of all action's duration is 1.5 second.
            child.runAction(CCRotateBy.actionWithDuration(1.5f, 90));
            child.runAction(CCSequence.actions(
                                                    CCDelayTime.actionWithDuration(1.4f),
                                                    CCFadeOut.actionWithDuration(1.1f))
                            );

            //After 1.5 second, self will be removed.
            runAction(CCSequence.actions(
                                            CCDelayTime.actionWithDuration(1.4f),
                                            CCCallFunc.actionWithTarget(this, (removeThis)))
                     );
        }

        public void removeThis()
        {
            m_pParent.removeChild(this, true);

            nextCallback(this);
        }
    }
}
