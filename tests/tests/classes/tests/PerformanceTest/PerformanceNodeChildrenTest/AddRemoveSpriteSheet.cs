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
            throw new NotFiniteNumberException();
        }

        public override void initWithQuantityOfNodes(uint nNodes)
        {
            throw new NotFiniteNumberException();
        }

        public override void update(float dt)
        {
            throw new NotFiniteNumberException();
        }

        public virtual string profilerName()
        {
            throw new NotFiniteNumberException();
        }

        protected CCSpriteBatchNode batchNode;
//#if CC_ENABLE_PROFILERS
//    CCProfilingTimer* _profilingTimer;
//#endif
    }
}
