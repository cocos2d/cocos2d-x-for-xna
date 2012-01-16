using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ParticleMenuLayer : PerformBasicLayer
    {
        public ParticleMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
            throw new NotFiniteNumberException();
        }

        public override void showCurrentTest()
        {
            CCNode ccnode = new CCNode();
            ParticleMainScene pScene = (ParticleMainScene)ccnode.parent;
            int subTest = pScene.getSubTestNum();
            int parNum = pScene.getParticlesNum();

            ParticleMainScene pNewScene = null;

            switch (m_nCurCase)
            {
                case 0:
                    pNewScene = new ParticlePerformTest1();
                    break;
                case 1:
                    pNewScene = new ParticlePerformTest2();
                    break;
                case 2:
                    pNewScene = new ParticlePerformTest3();
                    break;
                case 3:
                    pNewScene = new ParticlePerformTest4();
                    break;
            }

            PerformanceParticleTest.s_nParCurIdx = m_nCurCase;
            if (pNewScene != null)
            {
                pNewScene.initWithSubTest(subTest, parNum);

                CCDirector.sharedDirector().replaceScene(pNewScene);
            }
        }
    }
}
