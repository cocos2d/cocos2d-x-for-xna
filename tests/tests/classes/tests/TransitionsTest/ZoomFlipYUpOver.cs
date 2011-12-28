using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZoomFlipYUpOver : CCTransitionZoomFlipY
    {
        public static CCTransitionScene transitionWithDuration(float t, CCScene s)
        {
            return CCTransitionZoomFlipY.transitionWithDuration(t, s, tOrientation.kOrientationUpOver);
        }
    }
}
