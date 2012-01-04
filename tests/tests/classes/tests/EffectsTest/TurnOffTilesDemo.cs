using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TurnOffTilesDemo : CCTurnOffTiles
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCTurnOffTiles fadeout = CCTurnOffTiles.actionWithSeed(25, new ccGridSize(48, 32), t);
            CCFiniteTimeAction back = fadeout.reverse();
            CCDelayTime delay = CCDelayTime.actionWithDuration(0.5f);

            return (CCActionInterval)(CCSequence.actions(fadeout, delay, back));
        }
    }
}
