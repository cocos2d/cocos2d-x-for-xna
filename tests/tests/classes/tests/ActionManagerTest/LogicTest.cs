using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LogicTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";

        public override string title()
        {
            return "Logic test";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSprite grossini = CCSprite.spriteWithFile(s_pPathGrossini);
            addChild(grossini, 0, 2);
            grossini.position = (new CCPoint(200, 200));

            grossini.runAction(CCSequence.actions(
                                                        CCMoveBy.actionWithDuration(1, new CCPoint(150, 0)),
                                                        CCCallFuncN.actionWithTarget(this, bugMe))
                                );
        }

        public void bugMe(CCNode node)
        {
            node.stopAllActions(); //After this stop next action not working, if remove this stop everything is working
            node.runAction(CCScaleTo.actionWithDuration(2, 2));
        }
    }
}
