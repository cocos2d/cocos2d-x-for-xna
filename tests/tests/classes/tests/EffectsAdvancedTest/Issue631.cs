using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Issue631 : EffectAdvanceTextLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval effect = (CCActionInterval)(CCSequence.actions(CCDelayTime.actionWithDuration(2.0f), CCShaky3D.actionWithRange(16, false, new ccGridSize(5, 5), 5.0f)));

            // cleanup
            CCNode bg = getChildByTag(EffectAdvanceScene.kTagBackground);
            removeChild(bg, true);

            // background
            CCLayerColor layer = CCLayerColor.layerWithColor(new ccColor4B(255, 0, 0, 255));
            addChild(layer, -10);
            CCSprite sprite = CCSprite.spriteWithFile("Images/grossini");
            sprite.position = new CCPoint(50, 80);
            layer.addChild(sprite, 10);

            // foreground
            CCLayerColor layer2 = CCLayerColor.layerWithColor(new ccColor4B(0, 255, 0, 255));
            CCSprite fog = CCSprite.spriteWithFile("Images/Fog");

            ccBlendFunc bf = new ccBlendFunc { src = 0x0302, dst = 0x0303 };
            fog.BlendFunc = bf;
            layer2.addChild(fog, 1);
            addChild(layer2, 1);

            layer2.runAction(CCRepeatForever.actionWithAction(effect));
        }

        public override string title()
        {
            return "Testing Opacity";
        }

        public override string subtitle()
        {
            return "Effect image should be 100% opaque. Testing issue #631";
        }
    }
}
