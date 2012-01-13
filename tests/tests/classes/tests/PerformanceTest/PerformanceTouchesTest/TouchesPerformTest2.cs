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

        public override void ccTouchesBegan(List<CCTouch> touches, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }

        public override void ccTouchesMoved(List<CCTouch> touches, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }

        public override void ccTouchesCancelled(List<CCTouch> touches, CCEvent events)
        {
            throw new NotFiniteNumberException();
        }
    }
}
