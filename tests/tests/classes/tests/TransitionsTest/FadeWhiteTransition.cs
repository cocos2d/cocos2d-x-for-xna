using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeWhiteTransition : CCTransitionFade
    {
        static ccColor3B ccWHITE = new ccColor3B(255, 255, 255);

        public static CCTransitionScene transitionWithDuration(float t, CCScene s)
        {
            return CCTransitionFade.transitionWithDuration(t, s, ccWHITE);
        }
    }
}
