using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class StressTest1 : TestCocosNodeDemo
    {
        void shouldNotCrash(float dt)
        {
            unschedule((shouldNotCrash));

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // if the node has timers, it crashes
            CCNode explosion = CCParticleSun.node();
            ((CCParticleSun)explosion).Texture = (CCTextureCache.sharedTextureCache().addImage("Images/fire"));

            // if it doesn't, it works Ok.
            //	CocosNode *explosion = [Sprite spriteWithFile:@"grossinis_sister2.png");

            explosion.position = new CCPoint(s.width / 2, s.height / 2);

            runAction(CCSequence.actions(
                                    CCRotateBy.actionWithDuration(2, 360),
                                    CCCallFuncN.actionWithTarget(this, (removeMe))
                                    ));

            addChild(explosion);
        }

        void removeMe(CCNode node)
        {
            m_pParent.removeChild(node, true);
            nextCallback(this);
        }

        public StressTest1()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);

            sp1.position = (new CCPoint(s.width / 2, s.height / 2));

            schedule((shouldNotCrash), 1.0f);
        }

        public override string title()
        {
            return "stress test #1: no crashes";
        }
    }
}
