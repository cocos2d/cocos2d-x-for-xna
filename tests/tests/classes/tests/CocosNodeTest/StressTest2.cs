using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class StressTest2 : TestCocosNodeDemo
    {
        public StressTest2()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLayer sublayer = CCLayer.node();

            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            sp1.position = (new CCPoint(80, s.height / 2));

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_ease_inout3 = CCEaseInOut.actionWithAction((CCActionInterval)(move.copy()), 2.0f);
            CCActionInterval move_ease_inout_back3 = (CCActionInterval)move_ease_inout3.reverse();
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_inout3, move_ease_inout_back3);
            sp1.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq3));
            sublayer.addChild(sp1, 1);

            CCParticleFire fire = CCParticleFire.node();
            fire.Texture = (CCTextureCache.sharedTextureCache().addImage("Images/fire"));
            fire.position = (new CCPoint(80, s.height / 2 - 50));

            CCActionInterval copy_seq3 = (CCActionInterval)(seq3.copy());

            fire.runAction(CCRepeatForever.actionWithAction(copy_seq3));
            sublayer.addChild(fire, 2);

            schedule((shouldNotLeak), 6.0f);

            addChild(sublayer, 0, CocosNodeTestStaticLibrary.kTagSprite1);
        }

        void shouldNotLeak(float dt)
        {
            unschedule((shouldNotLeak));
            CCLayer sublayer = (CCLayer)getChildByTag(CocosNodeTestStaticLibrary.kTagSprite1);
            sublayer.removeAllChildrenWithCleanup(true);
        }

        public override string title()
        {
            return "stress test #2: no leaks";
        }
    }
}
