using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeOutUpTilesDemo : CCFadeOutUpTiles
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFadeOutUpTiles fadeout = CCFadeOutUpTiles.actionWithSize(new ccGridSize(16, 12), t);
            CCActionInterval back = fadeout.reverse();
            CCDelayTime delay = CCDelayTime.actionWithDuration(0.5f);

            return (CCActionInterval)(CCSequence.actions(fadeout, delay, back));
        }
    }
}
