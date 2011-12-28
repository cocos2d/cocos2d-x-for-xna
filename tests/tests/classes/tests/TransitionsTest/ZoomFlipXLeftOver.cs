using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZoomFlipXLeftOver : CCTransitionZoomFlipX
    {
        public static CCTransitionScene transitionWithDuration(float t, CCScene s)
        {
            return CCTransitionZoomFlipX.transitionWithDuration(t, s, tOrientation.kOrientationLeftOver);
        }
    }
}
