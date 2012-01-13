using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteMainScene : CCScene
    {

        public virtual string title()
        {
            throw new NotFiniteNumberException();
        }

        public void initWithSubTest(int nSubTest, int nNodes)
        {
            throw new NotFiniteNumberException();
        }

        public void updateNodes()
        {
            throw new NotFiniteNumberException();
        }

        public void testNCallback(CCObject pSender)
        {
            throw new NotFiniteNumberException();
        }

        public void onIncrease(CCObject pSender)
        {
            throw new NotFiniteNumberException();
        }

        public void onDecrease(CCObject pSender)
        {
            throw new NotFiniteNumberException();
        }

        public virtual void doTest(CCSprite sprite)
        {
            throw new NotFiniteNumberException();
        }

        public int getSubTestNum()
        { return subtestNumber; }

        public int getNodesNum()
        { return quantityNodes; }


        protected int lastRenderedCount;
        protected int quantityNodes;
        protected SubTest m_pSubTest;
        protected int subtestNumber;
    }
}
