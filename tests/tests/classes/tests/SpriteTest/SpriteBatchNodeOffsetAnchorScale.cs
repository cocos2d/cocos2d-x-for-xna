using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeOffsetAnchorScale : SpriteTestDemo
    {
        public SpriteBatchNodeOffsetAnchorScale()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            for (int i = 0; i < 3; i++)
            {
                CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
                cache.addSpriteFramesWithFile("animations/grossini");
                cache.addSpriteFramesWithFile("animations/grossini_gray", "animations/images/grossini_gray");

                //
                // Animation using Sprite BatchNode
                //
                CCSprite sprite = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
                sprite.position = (new CCPoint(s.width / 4 * (i + 1), s.height / 2));

                CCSprite point = CCSprite.spriteWithFile("Images/r1");
                point.scale = 0.25f;
                point.position = sprite.position;
                addChild(point, 200);

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

                CCSpriteBatchNode spritesheet = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini");
                addChild(spritesheet);

                List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>(14);
                string str = "";
                for (int k = 0; k < 14; k++)
                {
                    string temp = "";
                    if (k+1<10)
                    {
                        temp = "0" + (k + 1);
                    }
                    else
                    {
                        temp = (k + 1).ToString();
                    }
                    str = string.Format("grossini_dance_{0}.png", temp);
                    CCSpriteFrame frame = cache.spriteFrameByName(str);
                    animFrames.Add(frame);
                }

                CCAnimation animation = CCAnimation.animationWithFrames(animFrames);
                sprite.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithAnimation(animation, false)));

                CCActionInterval scale = CCScaleBy.actionWithDuration(2, 2);
                CCActionInterval scale_back = (CCActionInterval)scale.reverse();
                CCActionInterval seq_scale = (CCActionInterval)(CCSequence.actions(scale, scale_back));
                sprite.runAction(CCRepeatForever.actionWithAction(seq_scale));

                spritesheet.addChild(sprite, i);

                //animFrames->release();    // win32 : memory leak    2010-0415
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
            return "SpriteBatchNode offset + anchor + scale";
        }
    }
}
