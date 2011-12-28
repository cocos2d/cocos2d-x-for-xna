using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerTestBlend : LayerTest
    {
        int kTagLayer = 1;
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathSister2 = "Images/grossinis_sister2";
        public LayerTestBlend()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer1 = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 80));

            CCSprite sister1 = CCSprite.spriteWithFile(s_pPathSister1);
            CCSprite sister2 = CCSprite.spriteWithFile(s_pPathSister2);

            addChild(sister1);
            addChild(sister2);
            addChild(layer1, 100, kTagLayer);

            sister1.position = new CCPoint(160, s.height / 2);
            sister2.position = new CCPoint(320, s.height / 2);

            schedule(newBlend, 1.0f);
        }
        uint GL_ZERO = 0;

        public void newBlend(float dt)
        {
            CCLayerColor layer = (CCLayerColor)getChildByTag(kTagLayer);

            uint src;
            uint dst;

            if (layer.BlendFunc.dst == GL_ZERO)
            {
                src = 1;
                dst = 0x0303;
            }
            else
            {
                src = 0x0307;
                dst = GL_ZERO;
            }

            ccBlendFunc bf = new ccBlendFunc(src, dst);
            layer.BlendFunc = (bf);
        }

        public override string title()
        {
            return "ColorLayer: blend";
        }
    }
}
