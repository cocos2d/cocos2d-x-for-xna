using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TextureMenuLayer : PerformBasicLayer
    {
        public TextureMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            throw new NotFiniteNumberException();
        }

        public override void onEnter()
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

        public virtual void performTests()
        {
            throw new NotFiniteNumberException();
        }
    }
}
