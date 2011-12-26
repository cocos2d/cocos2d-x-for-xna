using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipY3DDemo : CCFlipY3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFlipX3D flipx = CCFlipX3D.actionWithDuration(t);
            CCActionInterval flipx_back = flipx.reverse();
            CCDelayTime delay = CCDelayTime.actionWithDuration(2);

            return (CCActionInterval)(CCSequence.actions(flipx, delay, flipx_back));
        }
    }
}
