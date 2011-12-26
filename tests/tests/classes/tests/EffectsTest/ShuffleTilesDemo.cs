using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ShuffleTilesDemo : CCShuffleTiles
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCShuffleTiles shuffle = CCShuffleTiles.actionWithSeed(25, new ccGridSize(16, 12), t);
            CCActionInterval shuffle_back = shuffle.reverse();
            CCDelayTime delay = CCDelayTime.actionWithDuration(2);

            return (CCActionInterval)(CCSequence.actions(shuffle, delay, shuffle_back));
        }
    }
}
