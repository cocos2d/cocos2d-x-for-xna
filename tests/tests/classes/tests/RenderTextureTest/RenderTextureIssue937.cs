using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RenderTextureIssue937 : RenderTextureTestDemo
    {
        public RenderTextureIssue937()
        {
            /*
            *     1    2
            * A: A1   A2
            *
            * B: B1   B2
            *
            *  A1: premulti sprite
            *  A2: premulti render
            *
            *  B1: non-premulti sprite
            *  B2: non-premulti render
            */
            CCLayerColor background = CCLayerColor.layerWithColor(new ccColor4B(200, 200, 200, 255));
            addChild(background);

            CCSprite spr_premulti = CCSprite.spriteWithFile("Images/fire.png");
            spr_premulti.position = new CCPoint(16, 48);

            CCSprite spr_nonpremulti = CCSprite.spriteWithFile("Images/fire.png");
            spr_nonpremulti.position = new CCPoint(16, 16);


            /* A2 & B2 setup */
            CCRenderTexture rend = CCRenderTexture.renderTextureWithWidthAndHeight(32, 64);

            if (null == rend)
            {
                return;
            }

            // It's possible to modify the RenderTexture blending function by
            //		[[rend sprite] setBlendFunc:(ccBlendFunc) {GL_ONE, GL_ONE_MINUS_SRC_ALPHA}];
            rend.begin();
            spr_premulti.visit();
            spr_nonpremulti.visit();
            rend.end();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            /* A1: setup */
            spr_premulti.position = new CCPoint(s.width / 2 - 16, s.height / 2 + 16);
            /* B1: setup */
            spr_nonpremulti.position = new CCPoint(s.width / 2 - 16, s.height / 2 - 16);

            rend.position = new CCPoint(s.width / 2 + 16, s.height / 2);

            addChild(spr_nonpremulti);
            addChild(spr_premulti);
            addChild(rend);
        }

        public override string title()
        {
            return "Testing issue #937";
        }

        public override string subtitle()
        {
            return "All images should be equal...";
        }
    }
}
