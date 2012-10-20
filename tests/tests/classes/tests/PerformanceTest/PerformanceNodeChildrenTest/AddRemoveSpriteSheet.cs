using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class AddRemoveSpriteSheet : NodeChildrenMainScene
    {
        public override void updateQuantityOfNodes()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            Random ran = new Random();
            // increase nodes
            if (currentQuantityOfNodes < quantityOfNodes)
            {
                for (int i = 0; i < (quantityOfNodes - currentQuantityOfNodes); i++)
                {
                    CCSprite sprite = CCSprite.spriteWithTexture(batchNode.Texture, new CCRect(0, 0, 32, 32));
                    batchNode.addChild(sprite);
                    sprite.position = new CCPoint(ran.Next() * s.width, ran.Next() * s.height);
                    sprite.visible = false;
                }
            }
            // decrease nodes
            else if (currentQuantityOfNodes > quantityOfNodes)
            {
                for (int i = 0; i < (currentQuantityOfNodes - quantityOfNodes); i++)
                {
                    int index = currentQuantityOfNodes - i - 1;
                    batchNode.removeChildAtIndex(index, true);
                }
            }

            currentQuantityOfNodes = quantityOfNodes;
        }

        public override void initWithQuantityOfNodes(int nNodes)
        {
            batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/spritesheet1");
            addChild(batchNode);
            NodeChildrenMainScene nodeChildrenMainScene = new NodeChildrenMainScene();
            nodeChildrenMainScene.initWithQuantityOfNodes(nNodes);

            //#if CC_ENABLE_PROFILERS
            //    _profilingTimer = CCProfiler::timerWithName(profilerName().c_str(), this);
            //#endif
            CCNode ccnode = new CCNode();
            ccnode.scheduleUpdate();
        }

        public override void update(float dt)
        {
            throw new NotFiniteNumberException();
        }

        public virtual string profilerName()
        {
            return "none";
        }

        protected CCSpriteBatchNode batchNode;
        //#if CC_ENABLE_PROFILERS
        //    CCProfilingTimer* _profilingTimer;
        //#endif
    }
}
