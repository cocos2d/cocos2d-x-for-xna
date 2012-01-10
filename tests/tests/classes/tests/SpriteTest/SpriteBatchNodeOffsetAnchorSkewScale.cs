using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests.classes.tests.SpriteTest
{
    public class SpriteBatchNodeOffsetAnchorSkewScale : SpriteTestDemo
    {
        public SpriteBatchNodeOffsetAnchorSkewScale()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            for (int i = 0; i < 3; i++)
            {
                CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
                cache.addSpriteFramesWithFile("animations/grossini");
                cache.addSpriteFramesWithFile("animations/grossini_gray", "animations/grossini_gray");

                //
                // Animation using Sprite batch
                //
                CCSprite sprite = CCSprite.spriteWithSpriteFrameName("grossini_dance_01");
                sprite.position = new CCPoint(s.width / 4 * (i + 1), s.height / 2);

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

                CCSpriteBatchNode spritebatch = CCSpriteBatchNode.batchNodeWithFile("animations/grossini");
                addChild(spritebatch);

                List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>();
                string tmp = "";
                for (int j = 0; j < 14; j++)
                {
                    tmp = string.Format("grossini_dance_%02d.png", j + 1);
                    CCSpriteFrame frame = cache.spriteFrameByName(tmp);
                    animFrames.Add(frame);
                }

                CCAnimation animation = CCAnimation.animationWithFrames(animFrames);
                sprite.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithDuration(2.8f, animation, false)));

                animFrames = null;

                // skew
                CCSkewBy skewX = CCSkewBy.actionWithDuration(2, 45, 0);
                CCActionInterval skewX_back = (CCActionInterval)skewX.reverse();
                CCSkewBy skewY = CCSkewBy.actionWithDuration(2, 0, 45);
                CCActionInterval skewY_back = (CCActionInterval)skewY.reverse();

                CCFiniteTimeAction seq_skew = CCSequence.actions(skewX, skewX_back, skewY, skewY_back);
                sprite.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq_skew));

                // scale 
                CCScaleBy scale = CCScaleBy.actionWithDuration(2, 2);
                CCActionInterval scale_back = (CCActionInterval)scale.reverse();
                CCFiniteTimeAction seq_scale = CCSequence.actions(scale, scale_back);
                sprite.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq_scale));

                spritebatch.addChild(sprite, i);
            }
        }

        public override string title()
        {
            return "SpriteBatchNode anchor + skew + scale";
        }
    }
}
