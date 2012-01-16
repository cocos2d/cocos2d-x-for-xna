using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TouchesPerformTest1 : TouchesMainScene
    {

        public TouchesPerformTest1(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void onEnter()
        {
            base.onEnter();
            isTouchEnabled = true;
        }

        public override string title()
        {
            return "Targeted touches";
        }

        public override void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, true);
        }

        public override bool ccTouchBegan(CCTouch touch, CCEvent events)
        {
            numberOfTouchesB++;
            return true;
        }

        public override void ccTouchMoved(CCTouch touch, CCEvent events)
        {
            numberOfTouchesM++;
        }

        public override void ccTouchEnded(CCTouch touch, CCEvent events)
        {
            numberOfTouchesE++;
        }

        public override void ccTouchCancelled(CCTouch touch, CCEvent events)
        {
            numberOfTouchesC++;
        }
    }
}
