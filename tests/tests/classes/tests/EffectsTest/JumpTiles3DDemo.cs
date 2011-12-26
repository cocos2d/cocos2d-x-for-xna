using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class JumpTiles3DDemo : CCJumpTiles3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCSize size = CCDirector.sharedDirector().getWinSize();
            return CCJumpTiles3D.actionWithJumps(2, 30, new ccGridSize(15, 10), t);
        }
    }
}
