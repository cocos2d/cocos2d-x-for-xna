using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerTest2 : LayerTest
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer1 = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(255, 255, 0, 80), 100, 300);
            layer1.position = (new CCPoint(s.width / 3, s.height / 2));
            layer1.isRelativeAnchorPoint = true;
            addChild(layer1, 1);

            CCLayerColor layer2 = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(0, 0, 255, 255), 100, 300);
            layer2.position = (new CCPoint((s.width / 3) * 2, s.height / 2));
            layer2.isRelativeAnchorPoint = true;
            addChild(layer2, 1);

            CCActionInterval actionTint = CCTintBy.actionWithDuration(2, -255, -127, 0);
            CCActionInterval actionTintBack = (CCActionInterval)actionTint.reverse();
            CCActionInterval seq1 = (CCActionInterval)CCSequence.actions(actionTint, actionTintBack);
            layer1.runAction(seq1);

            CCActionInterval actionFade = CCFadeOut.actionWithDuration(2.0f);
            CCActionInterval actionFadeBack = (CCActionInterval)actionFade.reverse();
            CCActionInterval seq2 = (CCActionInterval)CCSequence.actions(actionFade, actionFadeBack);
            layer2.runAction(seq2);
        }

        public virtual string title()
        {
            return "ColorLayer: fade and tint";
        }
    }
}
