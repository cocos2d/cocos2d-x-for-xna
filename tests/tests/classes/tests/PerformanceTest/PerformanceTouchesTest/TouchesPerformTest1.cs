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
            throw new NotFiniteNumberException();
        }

        public override string title()
        {
            throw new NotFiniteNumberException();
        }

        public override void registerWithTouchDispatcher()
        {
            throw new NotFiniteNumberException();
        }

        public override bool ccTouchBegan(CCTouch touch, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }

        public override void ccTouchMoved(CCTouch touch, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }

        public override void ccTouchEnded(CCTouch touch, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }

        public override void ccTouchCancelled(CCTouch touch, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }
    }
}
