using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeOffsetAnchorFlip : SpriteTestDemo
    {
        public SpriteBatchNodeOffsetAnchorFlip()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            for (int i = 0; i < 3; i++)
            {
                CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
                cache.addSpriteFramesWithFile("animations/grossini");
                cache.addSpriteFramesWithFile("animations/grossini_gray", "animations/images/grossini_gray");

                //
                // Animation using Sprite batch
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
                        sprite.anchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.position = sprite.position;

                CCSpriteBatchNode spritebatch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini");
                addChild(spritebatch);

                List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>();
                string tmp = "";
                for (int j = 0; j < 14; j++)
                {
                    string temp = "";
                    if ( i + 1<10)
                    {
                        temp = "0" + (i+1);
                    }
                    else
                    {
                        temp = (i + 1).ToString();
                    }
                    tmp = string.Format("grossini_dance_{0}.png", temp);
                    CCSpriteFrame frame = cache.spriteFrameByName(tmp);
                    animFrames.Add(frame);
                }

                CCAnimation animation = CCAnimation.animationWithFrames(animFrames);
                sprite.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithDuration(2.8f, animation, false)));

                animFrames = null;

                CCFlipY flip = CCFlipY.actionWithFlipY(true);
                CCFlipY flip_back = CCFlipY.actionWithFlipY(false);
                CCDelayTime delay = CCDelayTime.actionWithDuration(1);
                CCFiniteTimeAction seq = CCSequence.actions((CCFiniteTimeAction)delay, (CCFiniteTimeAction)flip, (CCFiniteTimeAction)delay.copyWithZone(null), (CCFiniteTimeAction)flip_back);
                sprite.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));

                spritebatch.addChild(sprite, i);
            }
        }

        public override string title()
        {
            return "SpriteBatchNode offset + anchor + flip";
        }
        public override string subtitle()
        {
            return "issue #1078";
        }
    }
}
