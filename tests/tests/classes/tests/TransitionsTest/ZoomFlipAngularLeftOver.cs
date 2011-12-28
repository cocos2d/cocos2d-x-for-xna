using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZoomFlipAngularLeftOver : CCTransitionZoomFlipAngular
    {
        public static CCTransitionScene transitionWithDuration(float t, CCScene s)
        {
            return CCTransitionZoomFlipAngular.transitionWithDuration(t, s, tOrientation.kOrientationLeftOver);
        }
    }
}
