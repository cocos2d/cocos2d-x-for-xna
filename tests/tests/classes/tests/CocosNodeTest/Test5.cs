using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Test5 : TestCocosNodeDemo
    {
        public Test5()
        {
            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (new CCPoint(100, 160));
            sp2.position = (new CCPoint(380, 160));

            CCRotateBy rot = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval rot_back = rot.reverse() as CCActionInterval;
            CCAction forever = CCRepeatForever.actionWithAction(
                                                            (CCActionInterval)(CCSequence.actions(rot, rot_back))
                                                        );
            CCAction forever2 = (CCAction)(forever.copy());
            forever.tag = (101);
            forever2.tag = (102);

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);

            sp1.runAction(forever);
            sp2.runAction(forever2);

            schedule(new SEL_SCHEDULE(this.addAndRemove), 2.0f);
        }

        public void addAndRemove(float dt)
        {
            CCNode sp1 = getChildByTag(CocosNodeTestStaticLibrary.kTagSprite1);
            CCNode sp2 = getChildByTag(CocosNodeTestStaticLibrary.kTagSprite2);

            removeChild(sp1, false);
            removeChild(sp2, true);

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
        }

        public override string title()
        {
            return "remove and cleanup";
        }
    }
}
