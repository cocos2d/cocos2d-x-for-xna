using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Test2 : TestCocosNodeDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);
            CCSprite sp3 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp4 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (new CCPoint(100, s.height / 2));
            sp2.position = (new CCPoint(380, s.height / 2));
            addChild(sp1);
            addChild(sp2);

            sp3.scale = (0.25f);
            sp4.scale = (0.25f);

            sp1.addChild(sp3);
            sp2.addChild(sp4);

            CCActionInterval a1 = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval a2 = CCScaleBy.actionWithDuration(2, 2);

            CCAction action1 = CCRepeatForever.actionWithAction(
                                                            (CCActionInterval)(CCSequence.actions(a1, a2, a2.reverse()))
                                                        );
            CCAction action2 = CCRepeatForever.actionWithAction(
                                                            (CCActionInterval)(CCSequence.actions(
                                                                                                (CCActionInterval)(a1.copy()),
                                                                                                (CCActionInterval)(a2.copy()),
                                                                                                a2.reverse()))
                                                        );

            sp2.anchorPoint = (new CCPoint(0, 0));

            sp1.runAction(action1);
            sp2.runAction(action2);
        }

        public override string title()
        {
            return "anchorPoint and children";
        }
    }
}
