using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteHybrid : SpriteTestDemo
    {
        Random rand = new Random();
        bool m_usingSpriteBatchNode;

        public SpriteHybrid()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // parents
            CCNode parent1 = CCNode.node();
            CCSpriteBatchNode parent2 = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);

            addChild(parent1, 0, (int)kTags.kTagNode);
            addChild(parent2, 0, (int)kTags.kTagSpriteBatchNode);


            // IMPORTANT:
            // The sprite frames will be cached AND RETAINED, and they won't be released unless you call
            //     CCSpriteFrameCache::sharedSpriteFrameCache()->removeUnusedSpriteFrames);
            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");


            // create 250 sprites
            // only show 80% of them
            for (int i = 0; i < 250; i++)
            {
                int spriteIdx = (int)(rand.NextDouble() * 14);
                string str = "";
                string temp = "";
                if (spriteIdx+1<10)
                {
                    temp = "0" + (spriteIdx+1);
                }
                else
                {
                    temp = (spriteIdx+1).ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str);
                CCSprite sprite = CCSprite.spriteWithSpriteFrame(frame);
                parent1.addChild(sprite, i, i);

                float x = -1000;
                float y = -1000;
                if (rand.NextDouble() < 0.2f)
                {
                    x = (float)(rand.NextDouble() * s.width);
                    y = (float)(rand.NextDouble() * s.height);
                }
                sprite.position = (new CCPoint(x, y));

                CCActionInterval action = CCRotateBy.actionWithDuration(4, 360);
                sprite.runAction(CCRepeatForever.actionWithAction(action));
            }

            m_usingSpriteBatchNode = false;

            schedule(reparentSprite, 2);
        }

        public void reparentSprite(float dt)
        {
            CCNode p1 = getChildByTag((int)kTags.kTagNode);
            CCNode p2 = getChildByTag((int)kTags.kTagSpriteBatchNode);

            List<CCNode> retArray = new List<CCNode>(250);

            if (m_usingSpriteBatchNode)
                CC_SWAP(p1, p2);


            ////----UXLOG("New parent is: %x", p2);

            CCNode node;
            List<CCNode> children = p1.children;
            foreach (var item in children)
            {
                if (item == null)
                {
                    break;
                }
                retArray.Add(item);
            }

            int i = 0;
            p1.removeAllChildrenWithCleanup(false);

            foreach (var item in retArray)
            {
                if (item == null)
                {
                    break;
                }
                p2.addChild(item, i, i);
                i++;
            }
            m_usingSpriteBatchNode = !m_usingSpriteBatchNode;
        }

        public override string title()
        {
            return "HybrCCSprite* sprite Test";
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeSpriteFramesFromFile("animations/grossini");
        }

        public void CC_SWAP(CCNode p1, CCNode p2)
        {
            CCNode tmp = p1;
            p1 = p2;
            p2 = tmp;
        }
    }
}
