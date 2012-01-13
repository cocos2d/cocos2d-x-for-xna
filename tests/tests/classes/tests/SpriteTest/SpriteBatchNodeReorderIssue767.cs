using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeReorderIssue767 : SpriteTestDemo
    {
        public SpriteBatchNodeReorderIssue767()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/ghosts", "animations/images/ghosts");
            CCNode aParent;
            CCSprite l1, l2a, l2b, l3a1, l3a2, l3b1, l3b2;

            //
            // SpriteBatchNode: 3 levels of children
            //
            aParent = CCSpriteBatchNode.batchNodeWithFile("animations/images/ghosts");
            addChild(aParent, 0, (int)kTagSprite.kTagSprite1);

            // parent
            l1 = CCSprite.spriteWithSpriteFrameName("father.gif");
            l1.position = (new CCPoint(s.width / 2, s.height / 2));
            aParent.addChild(l1, 0, (int)kTagSprite.kTagSprite2);
            CCSize l1Size = l1.contentSize;

            // child left
            l2a = CCSprite.spriteWithSpriteFrameName("sister1.gif");
            l2a.position = (new CCPoint(-25 + l1Size.width / 2, 0 + l1Size.height / 2));
            l1.addChild(l2a, -1, (int)kTags.kTagSpriteLeft);
            CCSize l2aSize = l2a.contentSize;

            // child right
            l2b = CCSprite.spriteWithSpriteFrameName("sister2.gif");
            l2b.position = new CCPoint(+25 + l1Size.width / 2, 0 + l1Size.height / 2);
            l1.addChild(l2b, 1, (int)kTags.kTagSpriteRight);
            CCSize l2bSize = l2a.contentSize;


            // child left bottom
            l3a1 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3a1.scale = 0.65f;
            l3a1.position = new CCPoint(0 + l2aSize.width / 2, -50 + l2aSize.height / 2);
            l2a.addChild(l3a1, -1);

            // child left top
            l3a2 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3a2.scale = 0.65f;
            l3a2.position = (new CCPoint(0 + l2aSize.width / 2, +50 + l2aSize.height / 2));
            l2a.addChild(l3a2, 1);

            // child right bottom
            l3b1 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3b1.scale = 0.65f;
            l3b1.position = (new CCPoint(0 + l2bSize.width / 2, -50 + l2bSize.height / 2));
            l2b.addChild(l3b1, -1);

            // child right top
            l3b2 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3b2.scale = 0.65f;
            l3b2.position = (new CCPoint(0 + l2bSize.width / 2, +50 + l2bSize.height / 2));
            l2b.addChild(l3b2, 1);

            schedule(reorderSprites, 1);
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder issue #767";
        }

        public override string subtitle()
        {
            return "Should not crash";
        }

        public void reorderSprites(float dt)
        {
            CCSpriteBatchNode spritebatch = (CCSpriteBatchNode)getChildByTag((int)kTagSprite.kTagSprite1);
            CCSprite father = (CCSprite)spritebatch.getChildByTag((int)kTagSprite.kTagSprite2);
            CCSprite left = (CCSprite)father.getChildByTag((int)kTags.kTagSpriteLeft);
            CCSprite right = (CCSprite)father.getChildByTag((int)kTags.kTagSpriteRight);

            int newZLeft = 1;

            if (left.zOrder == 1)
                newZLeft = -1;

            father.reorderChild(left, newZLeft);
            father.reorderChild(right, -newZLeft);
        }
    }
}
