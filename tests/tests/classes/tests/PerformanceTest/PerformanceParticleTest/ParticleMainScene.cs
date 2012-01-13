using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ParticleMainScene : CCScene
    {
        public virtual void initWithSubTest(int subtest, int particles)
        {
            throw new NotFiniteNumberException();
        }

        public virtual string title()
        {
            throw new NotFiniteNumberException();
        }

        public void step(float dt)
        {
            throw new NotFiniteNumberException();
        }

        public void createParticleSystem()
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

        public void testNCallback(CCObject pSender)
        {
            throw new NotFiniteNumberException();
        }

        public void updateQuantityLabel()
        {
            throw new NotFiniteNumberException();
        }
        public int getSubTestNum()
        { return subtestNumber; }

        public int getParticlesNum()
        { return quantityParticles; }

        public virtual void doTest()
        {
            throw new NotFiniteNumberException();
        }


        protected int lastRenderedCount;
        protected int quantityParticles;
        protected int subtestNumber;
    }
}
