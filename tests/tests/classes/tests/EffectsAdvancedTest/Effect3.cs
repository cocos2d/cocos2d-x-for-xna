using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Effect3 : EffectAdvanceTextLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCNode bg = getChildByTag(EffectAdvanceScene.kTagBackground);
            CCNode target1 = bg.getChildByTag(EffectAdvanceScene.kTagSprite1);
            CCNode target2 = bg.getChildByTag(EffectAdvanceScene.kTagSprite2);

            CCActionInterval waves = CCWaves.actionWithWaves(5, 20, true, false, new ccGridSize(15, 10), 5);
            CCActionInterval shaky = CCShaky3D.actionWithRange(4, false, new ccGridSize(15, 10), 5);

            target1.runAction(CCRepeatForever.actionWithAction(waves));
            target2.runAction(CCRepeatForever.actionWithAction(shaky));

            //// moving background. Testing issue #244
            //CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(200, 0));
            //bg.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(move))));
        }

        public override string title()
        {
            return "Effects on 2 sprites";
        }
    }
}
