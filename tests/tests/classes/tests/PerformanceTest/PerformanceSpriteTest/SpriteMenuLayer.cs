using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteMenuLayer : PerformBasicLayer
    {
        public SpriteMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {

        }

        public override void showCurrentTest()
        {
            throw new NotFiniteNumberException();
        }
    }
}
