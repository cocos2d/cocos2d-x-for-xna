using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Effect5 : EffectAdvanceTextLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            //CCDirector::sharedDirector()->setProjection(CCDirectorProjection2D);

            CCActionInterval effect = CCLiquid.actionWithWaves(1, 20, new ccGridSize(32, 24), 2);

            CCActionInterval stopEffect = (CCActionInterval)(CCSequence.actions(
                                                 effect,
                                                 CCDelayTime.actionWithDuration(2),
                                                 CCStopGrid.action(),
                //					 [DelayTime::actionWithDuration:2],
                //					 [[effect copy] autorelease],
                                                 null));

            CCNode bg = getChildByTag(EffectAdvanceScene.kTagBackground);
            bg.runAction(stopEffect);
        }

        public override void onExit()
        {
            base.onExit();

            CCDirector.sharedDirector().Projection = ccDirectorProjection.CCDirectorProjection3D;
        }

        public override string title()
        {
            return "Test Stop-Copy-Restar";
        }
    }
}
