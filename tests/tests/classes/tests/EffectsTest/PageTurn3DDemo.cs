using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PageTurn3DDemo : CCPageTurn3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCDirector.sharedDirector().setDepthTest(true);
            return CCPageTurn3D.actionWithSize(new ccGridSize(15, 10), t);
        }
    }
}
