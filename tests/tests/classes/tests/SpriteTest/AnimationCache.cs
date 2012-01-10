using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class AnimationCache : SpriteTestDemo
    {
        public AnimationCache()
        {
            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");
            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini_gray");
            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini_blue");

            //
            // create animation "dance"
            //
            List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>(15);
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                string temp = "";
                if (i<10)
                {
                    temp = "0" + i;
                }
                else
                {
                    temp = i.ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.animationWithFrames(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.sharedAnimationCache().addAnimation(animation, "dance");

            //
            // create animation "dance gray"
            //
            animFrames.Clear();

            for (int i = 1; i < 15; i++)
            {
                string temp = "";
                if (i < 10)
                {
                    temp = "0" + i;
                }
                else
                {
                    temp = i.ToString();
                }
                str = String.Format("grossini_dance_gray_{0}.png", temp);
                CCSpriteFrame frame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str);
                animFrames.Add(frame);
            }

            animation = CCAnimation.animationWithFrames(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.sharedAnimationCache().addAnimation(animation, "dance_gray");

            //
            // create animation "dance blue"
            //
            animFrames.Clear();

            for (int i = 1; i < 4; i++)
            {
                str = String.Format("grossini_blue_0{0}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str);
                animFrames.Add(frame);
            }

            animation = CCAnimation.animationWithFrames(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.sharedAnimationCache().addAnimation(animation, "dance_blue");


            CCAnimationCache animCache = CCAnimationCache.sharedAnimationCache();

            CCAnimation normal = animCache.animationByName("dance");
            CCAnimation dance_grey = animCache.animationByName("dance_gray");
            CCAnimation dance_blue = animCache.animationByName("dance_blue");

            CCAnimate animN = CCAnimate.actionWithAnimation(normal);
            CCAnimate animG = CCAnimate.actionWithAnimation(dance_grey);
            CCAnimate animB = CCAnimate.actionWithAnimation(dance_blue);

            CCFiniteTimeAction seq = CCSequence.actions(animN, animG, animB);

            // create an sprite without texture
            CCSprite grossini = new CCSprite();
            grossini.init();

            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            grossini.position = (new CCPoint(winSize.width / 2, winSize.height / 2));
            addChild(grossini);

            // run the animation
            grossini.runAction(seq);
        }

        public override string title()
        {
            return "AnimationCache";
        }

        public override string subtitle()
        {
            return "Sprite should be animated";
        }
    }
}
