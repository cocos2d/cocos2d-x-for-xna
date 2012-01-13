using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZwoptexGenericTest : ZwoptexTest
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("zwoptex/grossini");
            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("zwoptex/grossini-generic");

            CCLayerColor layer1 = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(255, 0, 0, 255), 85, 121);
            layer1.position = (new CCPoint(s.width / 2 - 80 - (85.0f * 0.5f), s.height / 2 - (121.0f * 0.5f)));
            addChild(layer1);

            sprite1 = CCSprite.spriteWithSpriteFrame(CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName("grossini_dance_01.png"));
            sprite1.position = (new CCPoint(s.width / 2 - 80, s.height / 2));
            addChild(sprite1);

            sprite1.IsFlipX = (false);
            sprite1.IsFlipY = (false);

            CCLayerColor layer2 = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(255, 0, 0, 255), 85, 121);
            layer2.position = (new CCPoint(s.width / 2 + 80 - (85.0f * 0.5f), s.height / 2 - (121.0f * 0.5f)));
            addChild(layer2);

            sprite2 = CCSprite.spriteWithSpriteFrame(CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName("grossini_dance_generic_01.png"));
            sprite2.position = (new CCPoint(s.width / 2 + 80, s.height / 2));
            addChild(sprite2);

            sprite2.IsFlipX = (false);
            sprite2.IsFlipY = (false);

            schedule((startIn05Secs), 1.0f);

            counter = 0;
        }

        static int spriteFrameIndex = 0;
        public void flipSprites(float dt)
        {
            counter++;

            bool fx = false;
            bool fy = false;
            int i = counter % 4;

            switch (i)
            {
                case 0:
                    fx = false;
                    fy = false;
                    break;
                case 1:
                    fx = true;
                    fy = false;
                    break;
                case 2:
                    fx = false;
                    fy = true;
                    break;
                case 3:
                    fx = true;
                    fy = true;
                    break;
            }
            sprite1.IsFlipX = (fx);
            sprite2.IsFlipX = (fx);
            sprite1.IsFlipY = (fy);
            sprite2.IsFlipY = (fy);
            string temp = "";
            if (++spriteFrameIndex > 14)
            {
               
                spriteFrameIndex = 1;
            }
            if (spriteFrameIndex < 10)
            {
                temp = "0" + spriteFrameIndex;
            }
            else
            {
                temp = spriteFrameIndex.ToString();
            }
            string str1 = "";
            string str2 = "";
            str1 = string.Format("grossini_dance_{0}.png", temp);
            str2 = string.Format("grossini_dance_generic_{0}.png", temp);
            sprite1.setDisplayFrame(CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str1));
            sprite2.setDisplayFrame(CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str2));
        }

        public void startIn05Secs(float dt)
        {
            schedule((flipSprites), 0.5f);
        }

        public override string title() 
        {
            return "Zwoptex Tests";
        }

        public override string subtitle() 
        {
            return "Coordinate Formats, Rotation, Trimming, flipX/Y";
        }

        protected CCSprite sprite1;
        protected CCSprite sprite2;
        protected int counter;
    }
}
