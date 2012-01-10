using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteAnimationSplit : SpriteTestDemo
    {
        public SpriteAnimationSplit()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCTexture2D texture = CCTextureCache.sharedTextureCache().addImage("animations/images/dragon_animation");

            // manually add frames to the frame cache
            CCSpriteFrame frame0 = CCSpriteFrame.frameWithTexture(texture, new CCRect(132 * 0, 132 * 0, 132, 132));
            CCSpriteFrame frame1 = CCSpriteFrame.frameWithTexture(texture, new CCRect(132 * 1, 132 * 0, 132, 132));
            CCSpriteFrame frame2 = CCSpriteFrame.frameWithTexture(texture, new CCRect(132 * 2, 132 * 0, 132, 132));
            CCSpriteFrame frame3 = CCSpriteFrame.frameWithTexture(texture, new CCRect(132 * 3, 132 * 0, 132, 132));
            CCSpriteFrame frame4 = CCSpriteFrame.frameWithTexture(texture, new CCRect(132 * 0, 132 * 1, 132, 132));
            CCSpriteFrame frame5 = CCSpriteFrame.frameWithTexture(texture, new CCRect(132 * 1, 132 * 1, 132, 132));

            //
            // Animation using Sprite BatchNode
            //
            CCSprite sprite = CCSprite.spriteWithSpriteFrame(frame0);
            sprite.position = (new CCPoint(s.width / 2 - 80, s.height / 2));
            addChild(sprite);

            List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>(6);
            animFrames.Add(frame0);
            animFrames.Add(frame1);
            animFrames.Add(frame2);
            animFrames.Add(frame3);
            animFrames.Add(frame4);
            animFrames.Add(frame5);

            CCAnimation animation = CCAnimation.animationWithFrames(animFrames, 0.2f);
            CCAnimate animate = CCAnimate.actionWithAnimation(animation, false);
            CCActionInterval seq = (CCActionInterval)(CCSequence.actions(animate,
                               CCFlipX.actionWithFlipX(true),
                              (CCFiniteTimeAction)animate.copy(),
                               CCFlipX.actionWithFlipX(false)
                               ));

            sprite.runAction(CCRepeatForever.actionWithAction(seq));
            //animFrames->release();    // win32 : memory leak    2010-0415
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite: Animation + flip";
        }
    }
}
