using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipAngularRightOver : CCTransitionFlipAngular
    {
        public static CCTransitionScene transitionWithDuration(float t, CCScene s)
        {
            return CCTransitionFlipAngular.transitionWithDuration(t, s, tOrientation.kOrientationRightOver);
        }
    }
}
