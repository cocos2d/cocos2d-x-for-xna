using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Effect1 : EffectAdvanceTextLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCNode target = getChildByTag(EffectAdvanceScene.kTagBackground);

            // To reuse a grid the grid size and the grid type must be the same.
            // in this case:
            //     Lens3D is Grid3D and it's size is (15,10)
            //     Waves3D is Grid3D and it's size is (15,10)

            CCSize size = CCDirector.sharedDirector().getWinSize();
            CCActionInterval lens = CCLens3D.actionWithPosition(new CCPoint(size.width / 2, size.height / 2), 240, new ccGridSize(15, 10), 0.0f);
            CCActionInterval waves = CCWaves3D.actionWithWaves(18, 15, new ccGridSize(15, 10), 10);

            CCFiniteTimeAction reuse = CCReuseGrid.actionWithTimes(1);
            CCActionInterval delay = CCDelayTime.actionWithDuration(8);

            CCActionInterval orbit = CCOrbitCamera.actionWithDuration(5, 1, 2, 0, 180, 0, -90);
            CCFiniteTimeAction orbit_back = orbit.reverse();

            target.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(orbit, orbit_back))));
            target.runAction(CCSequence.actions(lens, delay, reuse, waves));
        }

        public override string title()
        {
            return "Lens + Waves3d and OrbitCamera";
        }
    }
}
