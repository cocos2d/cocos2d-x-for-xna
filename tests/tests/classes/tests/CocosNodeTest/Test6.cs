using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Test6 : TestCocosNodeDemo
    {
        public Test6()
        {
            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp11 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);

            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);
            CCSprite sp21 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (new CCPoint(100, 160));
            sp2.position = (new CCPoint(380, 160));

            CCActionInterval rot = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval rot_back = rot.reverse() as CCActionInterval;
            CCAction forever1 = CCRepeatForever.actionWithAction(
                                                                    (CCActionInterval)(CCSequence.actions(rot, rot_back)));
            CCAction forever11 = (CCAction)(forever1.copy());

            CCAction forever2 = (CCAction)(forever1.copy());
            CCAction forever21 = (CCAction)(forever1.copy());

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            sp1.addChild(sp11);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
            sp2.addChild(sp21);

            sp1.runAction(forever1);
            sp11.runAction(forever11);
            sp2.runAction(forever2);
            sp21.runAction(forever21);

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
            return "remove/cleanup with children";
        }
    }
}
