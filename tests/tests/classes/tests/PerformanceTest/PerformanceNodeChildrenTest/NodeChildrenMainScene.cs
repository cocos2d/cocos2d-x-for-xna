using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class NodeChildrenMainScene : CCScene
    {
        public virtual void initWithQuantityOfNodes(uint nNodes)
        {
            throw new NotFiniteNumberException();
        }

        public virtual string title()
        {
            throw new NotFiniteNumberException();
        }

        public virtual string subtitle()
        {
            throw new NotFiniteNumberException();
        }

        public virtual void updateQuantityOfNodes()
        {
            throw new NotFiniteNumberException();
        }

        public void onDecrease(CCObject pSender)
        {
            throw new NotFiniteNumberException();
        }

        public void onIncrease(CCObject pSender)
        {
            throw new NotFiniteNumberException();
        }

        public void updateQuantityLabel()
        {
            throw new NotFiniteNumberException();
        }

        public int getQuantityOfNodes()
        {
            return quantityOfNodes;
        }

        protected int lastRenderedCount;
        protected int quantityOfNodes;
        protected int currentQuantityOfNodes;
    }
}
