using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests.classes
{
    public class TextureTest : TextureMenuLayer
    {

        public TextureTest(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void performTests()
        {
            throw new NotFiniteNumberException();
        }

        public override string title()
        {
            throw new NotFiniteNumberException();
        }

        public override string subtitle()
        {
            throw new NotFiniteNumberException();
        }

        public void performTestsPNG(string filename)
        {
            throw new NotFiniteNumberException();
        }

        public static CCScene scene()
        {
            throw new NotFiniteNumberException();
        }
    }
}
