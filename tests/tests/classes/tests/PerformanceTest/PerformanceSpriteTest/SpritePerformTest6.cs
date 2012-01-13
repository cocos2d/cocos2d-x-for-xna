using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpritePerformTest6 : SpriteMainScene
    {

        public override void doTest(CCSprite sprite)
        {
            throw new NotFiniteNumberException();
        }

        public override string title()
        {
            throw new NotFiniteNumberException();
        }
    }
}
