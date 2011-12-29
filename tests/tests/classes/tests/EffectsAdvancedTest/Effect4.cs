using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Effect4 : EffectAdvanceTextLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval lens = CCLens3D.actionWithPosition(new CCPoint(100, 180), 150, new ccGridSize(32, 24), 10);
            //id move = [MoveBy::actionWithDuration:5 position:ccp(400,0)];

            /**
            @todo we only support CCNode run actions now.
            */
            // 	CCActionInterval* move = CCJumpBy::actionWithDuration(5, ccp(380,0), 100, 4);
            // 	CCActionInterval* move_back = move->reverse();
            // 	CCActionInterval* seq = (CCActionInterval *)(CCSequence::actions( move, move_back, NULL));
            //  CCActionManager::sharedManager()->addAction(seq, lens, false);

            runAction(lens);
        }

        public override string title()
        {
            return "Jumpy Lens3D";
        }
    }
}
