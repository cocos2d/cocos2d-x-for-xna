using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteProgressToRadial : SpriteDemo
    {
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathBlock = "Images/blocks";

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCProgressTo to1 = CCProgressTo.actionWithDuration(2, 100);
            CCProgressTo to2 = CCProgressTo.actionWithDuration(2, 100);

            CCProgressTimer left = CCProgressTimer.progressWithFile(s_pPathSister1);
            left.setType(CCProgressTimer.CCProgressTimerType.kCCProgressTimerTypeRadialCW);
            addChild(left);
            left.position = new CCPoint(100, s.height / 2);
            left.runAction(CCRepeatForever.actionWithAction(to1));

            CCProgressTimer right = CCProgressTimer.progressWithFile(s_pPathBlock);
            right.setType(CCProgressTimer.CCProgressTimerType.kCCProgressTimerTypeRadialCCW);
            addChild(right);
            right.position = new CCPoint(s.width - 100, s.height / 2);
            right.runAction(CCRepeatForever.actionWithAction(to2));
        }

        public override string subtitle()
        {
            return "ProgressTo Radial";
        }
    }
}
