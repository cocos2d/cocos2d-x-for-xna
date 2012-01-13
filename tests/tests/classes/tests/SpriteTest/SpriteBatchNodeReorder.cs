using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class SpriteBatchNodeReorder : SpriteTestDemo
    {
        public SpriteBatchNodeReorder()
        {
            List<Object> a = new List<Object>(10);
            CCSpriteBatchNode asmtest = CCSpriteBatchNode.batchNodeWithFile("animations/images/ghosts");

            for (int i = 0; i < 10; i++)
            {
                CCSprite s1 = CCSprite.spriteWithBatchNode(asmtest, new CCRect(0, 0, 50, 50));
                a.Add(s1);
                asmtest.addChild(s1, 10);
            }

            for (int i = 0; i < 10; i++)
            {
                if (i != 5)
                    asmtest.reorderChild((CCNode)(a[i]), 9);
            }

            int prev = -1;
            List<CCNode> children = asmtest.children;
            CCSprite child;
            foreach (var item in children)
            {
                child = (CCSprite)item;
                if (child == null)
                    break;

                int currentIndex = child.atlasIndex;
                Debug.Assert(prev == currentIndex - 1, "Child order failed");
                ////----UXLOG("children %x - atlasIndex:%d", child, currentIndex);
                prev = currentIndex;
            }

            prev = -1;
            List<CCSprite> sChildren = asmtest.Descendants;
            foreach (var item in sChildren)
            {
                child = (CCSprite)item;
                if (child == null)
                    break;

                int currentIndex = child.atlasIndex;
                Debug.Assert(prev == currentIndex - 1, "Child order failed");
                ////----UXLOG("descendant %x - atlasIndex:%d", child, currentIndex);
                prev = currentIndex;
            }

            //a.release();       //memory leak : 2010-0415
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder #1";
        }

        public string subtitle()
        {
            return "Should not crash";
        }
    }
}
