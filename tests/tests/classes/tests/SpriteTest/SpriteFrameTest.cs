using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteFrameTest : SpriteTestDemo
    {
        public SpriteFrameTest()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // IMPORTANT:
            // The sprite frames will be cached AND RETAINED, and they won't be released unless you call
            //     CCSpriteFrameCache::sharedSpriteFrameCache()->removeUnusedSpriteFrames);
            CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
            cache.addSpriteFramesWithFile("animations/grossini");
            cache.addSpriteFramesWithFile("animations/grossini_gray", "animations/images/grossini_gray");
            cache.addSpriteFramesWithFile("animations/grossini_blue", "animations/images/grossini_blue");

            //
            // Animation using Sprite BatchNode
            //
            m_pSprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            m_pSprite1.position = (new CCPoint(s.width / 2 - 80, s.height / 2));

            CCSpriteBatchNode spritebatch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini");
            spritebatch.addChild(m_pSprite1);
            addChild(spritebatch);

            List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>(15);

            string str = "";
            for (int i = 1; i < 15; i++)
            {
                string temp;
                if (i<10)
                {
                   temp= "0" + i;
                }
                else
                {
                    temp = i.ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = cache.spriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.animationWithFrames(animFrames);
            m_pSprite1.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithAnimation(animation, false)));

            // to test issue #732, uncomment the following line
            m_pSprite1.IsFlipX = false;
            m_pSprite1.IsFlipY = false;

            //
            // Animation using standard Sprite
            //
            m_pSprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            m_pSprite2.position = (new CCPoint(s.width / 2 + 80, s.height / 2));
            addChild(m_pSprite2);


            List<CCSpriteFrame> moreFrames = new List<CCSpriteFrame>(20);
            for (int i = 1; i < 15; i++)
            {
                string temp;
                if (i < 10)
                {
                    temp = "0" + i;
                }
                else
                {
                    temp = i.ToString();
                }
                str = string.Format("grossini_dance_gray_{0}.png", temp);
                CCSpriteFrame frame = cache.spriteFrameByName(str);
                moreFrames.Add(frame);
            }


            for (int i = 1; i < 5; i++)
            {
                str = string.Format("grossini_blue_0{0}.png", i);
                CCSpriteFrame frame = cache.spriteFrameByName(str);
                moreFrames.Add(frame);
            }

            // append frames from another batch
            moreFrames.AddRange(animFrames);
            CCAnimation animMixed = CCAnimation.animationWithFrames(moreFrames);


            m_pSprite2.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithAnimation(animMixed, false)));



            // to test issue #732, uncomment the following line
            m_pSprite2.IsFlipX = (false);
            m_pSprite2.IsFlipY = (false);

            schedule(startIn05Secs, 0.5f);
            m_nCounter = 0;
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
            cache.removeSpriteFramesFromFile("animations/grossini");
            cache.removeSpriteFramesFromFile("animations/grossini_gray");
            cache.removeSpriteFramesFromFile("animations/grossini_blue");
        }

        public override string title()
        {
            return "Sprite vs. SpriteBatchNode animation";
        }

        public override string subtitle()
        {
            return "Testing issue #792";
        }

        public void startIn05Secs(float dt)
        {
            unschedule(startIn05Secs);
            schedule(flipSprites, 1.0f);
        }

        public void flipSprites(float dt)
        {
            m_nCounter++;

            bool fx = false;
            bool fy = false;
            int i = m_nCounter % 4;

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

            m_pSprite1.IsFlipX = (fx);
            m_pSprite1.IsFlipY = (fy);
            m_pSprite2.IsFlipX = (fx);
            m_pSprite2.IsFlipY = (fy);
            //NSLog(@"flipX:%d, flipY:%d", fx, fy);
        }

        private CCSprite m_pSprite1;
        private CCSprite m_pSprite2;
        private int m_nCounter;
    }
}
