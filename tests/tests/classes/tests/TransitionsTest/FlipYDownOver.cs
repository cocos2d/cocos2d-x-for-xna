using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipYDownOver : CCTransitionFlipY
    {
        public static CCTransitionScene transitionWithDuration(float t, CCScene s)
        {
            return CCTransitionFlipY.transitionWithDuration(t, s, tOrientation.kOrientationDownOver);
        }
    }
}
