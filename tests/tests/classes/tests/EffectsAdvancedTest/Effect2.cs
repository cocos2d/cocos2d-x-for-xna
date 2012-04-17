using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Effect2 : EffectAdvanceTextLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCNode target = getChildByTag(EffectAdvanceScene.kTagBackground);

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     ShakyTiles is TiledGrid3D and it's size is (15,10)
            //     Shuffletiles is TiledGrid3D and it's size is (15,10)
            //	   TurnOfftiles is TiledGrid3D and it's size is (15,10)
            CCActionInterval shaky = CCShakyTiles3D.actionWithRange(4, false, new ccGridSize(15, 10), 5);
            CCActionInterval shuffle = CCShuffleTiles.actionWithSeed(0, new ccGridSize(15, 10), 3);
            CCActionInterval turnoff = CCTurnOffTiles.actionWithSeed(0, new ccGridSize(15, 10), 3);
            CCFiniteTimeAction turnon = turnoff.reverse();

            // reuse 2 times:
            //   1 for shuffle
            //   2 for turn off
            //   turnon tiles will use a new grid
            CCFiniteTimeAction reuse = CCReuseGrid.actionWithTimes(2);

            CCActionInterval delay = CCDelayTime.actionWithDuration(1);

            //	id orbit = [OrbitCamera::actionWithDuration:5 radius:1 deltaRadius:2 angleZ:0 deltaAngleZ:180 angleX:0 deltaAngleX:-90];
            //	id orbit_back = [orbit reverse];
            //
            //	[target runAction: [RepeatForever::actionWithAction: [Sequence actions: orbit, orbit_back, nil]]];
            target.runAction((CCActionInterval)(CCSequence.actions(shaky, delay, reuse, shuffle, delay, turnoff, turnon)));
        }

        public override string title()
        {
            return "ShakyTiles + ShuffleTiles + TurnOffTiles";
        }
    }
}
