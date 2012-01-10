using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteOffsetAnchorRotation : SpriteTestDemo
    {
        public SpriteOffsetAnchorRotation()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
            cache.addSpriteFramesWithFile("animations/grossini");
            cache.addSpriteFramesWithFile("animations/grossini_gray", "animations/images/grossini_gray");

            for (int i = 0; i < 3; i++)
            {
                //
                // Animation using Sprite BatchNode
                //
                CCSprite sprite = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
                sprite.position = (new CCPoint(s.width / 4 * (i + 1), s.height / 2));

                CCSprite point = CCSprite.spriteWithFile("Images/r1");
                point.scale = 0.25f;
                point.position = (sprite.position);
                addChild(point, 1);

                switch (i)
                {
                    case 0:
                        sprite.anchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprite.anchorPoint = new CCPoint(0.5f, 0.5f);
                        break;
                    case 2:
                        sprite.anchorPoint = new CCPoint(1, 1);
                        break;
                }

                point.position = sprite.position;

                List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>(14);
                string str = "";
                for (int j = 0; j < 14; j++)
                {
                    string temp = "";
                    if (j + 1 < 10)
                    {
                        temp = "0" + (j + 1);
                    }
                    else
                    {
                        temp = (j + 1).ToString();
                    }
                    str = string.Format("grossini_dance_{0}.png", temp);
                    CCSpriteFrame frame = cache.spriteFrameByName(str);
                    animFrames.Add(frame);
                }

                CCAnimation animation = CCAnimation.animationWithFrames(animFrames);
                sprite.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithAnimation(animation, false)));
                sprite.runAction(CCRepeatForever.actionWithAction(CCRotateBy.actionWithDuration(10, 360)));

                addChild(sprite, 0);

                //animFrames.release();    // win32 : memory leak    2010-0415
            }
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
            cache.removeSpriteFramesFromFile("animations/grossini");
            cache.removeSpriteFramesFromFile("animations/grossini_gray");
        }

        public override string title()
        {
            return "Sprite offset + anchor + rot";
        }
    }
}
