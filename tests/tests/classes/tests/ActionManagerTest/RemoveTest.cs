using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public enum KTag
    {
        kTagNode,
        kTagGrossini,
        kTagSequence,
    }
    public class RemoveTest : ActionManagerTest
    {
        int kTagGrossini = 1;
        string s_pPathGrossini = "Images/grossini";

        public override string title() 
        {
            return "Remove Test";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF l = CCLabelTTF.labelWithString("Should not crash", "Arial", 16);
            addChild(l);
            l.position = (new CCPoint(s.width / 2, 245));

            CCMoveBy pMove = CCMoveBy.actionWithDuration(2, new CCPoint(200, 0));
            CCCallFunc pCallback = CCCallFunc.actionWithTarget(this, stopAction);
            CCActionInterval pSequence = (CCActionInterval)CCSequence.actions(pMove, pCallback);
            pSequence.tag = (int)KTag.kTagSequence;

            CCSprite pChild = CCSprite.spriteWithFile(s_pPathGrossini);
            pChild.position = (new CCPoint(200, 200));

            addChild(pChild, 1, kTagGrossini);
            pChild.runAction(pSequence);
        }

        public void stopAction()
        {
            CCNode pSprite = getChildByTag(kTagGrossini);
            pSprite.stopActionByTag((int)KTag.kTagSequence);
        }
    }
}
