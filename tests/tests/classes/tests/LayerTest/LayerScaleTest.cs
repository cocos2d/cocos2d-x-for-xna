using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerScaleTest : LayerTest
    {
        int kTagLayer = 1;
        int kCCMenuTouchPriority = -128;

        public override void onEnter()
        {
            base.onEnter();

            this.isTouchEnabled = true;

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(0xFF, 0x00, 0x00, 0x80), s.width * 0.75f, s.height * 0.75f);

            layer.isRelativeAnchorPoint = true;
            layer.position = (new CCPoint(s.width / 2, s.height / 2));
            addChild(layer, 1, kTagLayer);
            //
            // Add two labels using BM label class
            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.labelWithString("LABEL1", "fonts/fnt/konqa32");
            layer.addChild(label1);
            label1.position = new CCPoint(layer.contentSize.width / 2, layer.contentSize.height * 0.75f);
            CCLabelBMFont label2 = CCLabelBMFont.labelWithString("LABEL2", "fonts/fnt/konqa32");
            layer.addChild(label2);
            label2.position = new CCPoint(layer.contentSize.width / 2, layer.contentSize.height * 0.25f);
            //
            // Do the sequence of actions in the bug report
            float waitTime = 3f;
            float runTime = 12f;
            layer.visible = false;
            CCHide hide = CCHide.action();
            CCScaleTo scaleTo1 = CCScaleTo.actionWithDuration(0.0f, 0.0f);
            CCShow show = CCShow.action();
            CCDelayTime delay = CCDelayTime.actionWithDuration(waitTime);
            CCScaleTo scaleTo2 = CCScaleTo.actionWithDuration(runTime * 0.25f, 1.2f);
            CCScaleTo scaleTo3 = CCScaleTo.actionWithDuration(runTime * 0.25f, 0.95f);
            CCScaleTo scaleTo4 = CCScaleTo.actionWithDuration(runTime * 0.25f, 1.1f);
            CCScaleTo scaleTo5 = CCScaleTo.actionWithDuration(runTime * 0.25f, 1.0f);

            CCFiniteTimeAction seq = CCSequence.actions(hide, scaleTo1, show, delay, scaleTo2, scaleTo3, scaleTo4, scaleTo5);

            layer.runAction(seq);


        }

        public override string title()
        {
            return "Layer Scale With BM Font";
        }

    }
}
