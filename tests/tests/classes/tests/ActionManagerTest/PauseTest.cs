using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PauseTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";
        int kTagGrossini = 1;

        public override string title()
        {
            return "Pause Test";
        }

        public override void onEnter()
        {
            //
            // This test MUST be done in 'onEnter' and not on 'init'
            // otherwise the paused action will be resumed at 'onEnter' time
            //
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF l = CCLabelTTF.labelWithString("After 5 seconds grossini should move", "Arial", 16);
            addChild(l);
            l.position = (new CCPoint(s.width / 2, 245));


            //
            // Also, this test MUST be done, after [super onEnter]
            //
            CCSprite grossini = CCSprite.spriteWithFile(s_pPathGrossini);
            addChild(grossini, 0, kTagGrossini);
            grossini.position = (new CCPoint(200, 200));

            CCAction action = CCMoveBy.actionWithDuration(1, new CCPoint(150, 0));

            CCActionManager.sharedManager().addAction(action, grossini, true);

            schedule(unpause, 3);
        }

        public void unpause(float dt)
        {
            unschedule(unpause);
            CCNode node = getChildByTag(kTagGrossini);
            CCActionManager.sharedManager().resumeTarget(node);
        }
    }
}
