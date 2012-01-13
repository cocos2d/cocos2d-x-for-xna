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
           :base(bControlMenuVisible, nMaxCases, nCurCase)
        {
            throw new NotFiniteNumberException();
        }

        public override void showCurrentTest()
        {
            throw new NotFiniteNumberException();
        }
    }
}
