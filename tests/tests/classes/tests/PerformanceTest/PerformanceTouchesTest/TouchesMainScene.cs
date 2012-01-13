using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TouchesMainScene : PerformBasicLayer
    {

        public TouchesMainScene(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            throw new NotFiniteNumberException();
        }

        public override void onEnter()
        {
            throw new NotFiniteNumberException();
        }

        public virtual string title()
        {
            throw new NotFiniteNumberException();
        }

        public override void update(float dt)
        {
            throw new NotFiniteNumberException();
        }

        protected CCLabelBMFont m_plabel;
        protected int numberOfTouchesB;
        protected int numberOfTouchesM;
        protected int numberOfTouchesE;
        protected int numberOfTouchesC;
        protected float elapsedTime;
    }
}
