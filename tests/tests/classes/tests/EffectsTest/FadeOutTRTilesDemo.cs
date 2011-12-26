using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeOutTRTilesDemo : CCFadeOutTRTiles
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFadeOutTRTiles fadeout = CCFadeOutTRTiles.actionWithSize(new ccGridSize(16, 12), t);
            CCActionInterval back = fadeout.reverse();
            CCDelayTime delay = CCDelayTime.actionWithDuration(0.5f);

            return (CCActionInterval)(CCSequence.actions(fadeout, delay, back));
        }
    }
}
