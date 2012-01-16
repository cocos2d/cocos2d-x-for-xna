using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TouchesPerformTest2 : TouchesMainScene
    {
        public TouchesPerformTest2(bool bControlMenuVisible, int nMaxCases, int nCurCase)
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
            return "Standard touches";
        }

        public override void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addStandardDelegate(this, 0);
        }

        public override void ccTouchesBegan(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesB += touches.Count;
        }

        public override void ccTouchesMoved(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesM += touches.Count;
        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesE += touches.Count;
        }

        public override void ccTouchesCancelled(List<CCTouch> touches, CCEvent events)
        {
            numberOfTouchesC += touches.Count;
        }
    }
}
