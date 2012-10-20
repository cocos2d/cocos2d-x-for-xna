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
            CCLayer pLayer = null;
            switch (m_nCurCase)
            {
                case 0:
                    pLayer = new TouchesPerformTest1(true, PerformanceTouchesTest.TEST_COUNT, m_nCurCase);
                    break;
                case 1:
                    pLayer = new TouchesPerformTest2(true, PerformanceTouchesTest.TEST_COUNT, m_nCurCase);
                    break;
            }
            PerformanceTouchesTest.s_nTouchCurCase = m_nCurCase;

            if (pLayer != null)
            {
                CCScene pScene = CCScene.node();
                pScene.addChild(pLayer);

                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // add title
            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 50);
            CCNode ccnode = new CCNode();
            ccnode.scheduleUpdate();

            m_plabel = CCLabelBMFont.labelWithString("00.0", "fonts/arial16.fnt");
            m_plabel.position = new CCPoint(s.width / 2, s.height / 2);
            addChild(m_plabel);

            elapsedTime = 0;
            numberOfTouchesB = numberOfTouchesM = numberOfTouchesE = numberOfTouchesC = 0;
        }

        public virtual string title()
        {
            return "No title";
        }

        public override void update(float dt)
        {
            elapsedTime += dt;

            if (elapsedTime > 1.0f)
            {
                float frameRateB = numberOfTouchesB / elapsedTime;
                float frameRateM = numberOfTouchesM / elapsedTime;
                float frameRateE = numberOfTouchesE / elapsedTime;
                float frameRateC = numberOfTouchesC / elapsedTime;
                elapsedTime = 0;
                numberOfTouchesB = numberOfTouchesM = numberOfTouchesE = numberOfTouchesC = 0;

                //char str[32] = {0};
                string str;
                //sprintf(str, "%.1f %.1f %.1f %.1f", frameRateB, frameRateM, frameRateE, frameRateC);
                str = string.Format("{0:1f},{1:1f},{2:1f},{3:1f}", frameRateB, frameRateM, frameRateE, frameRateC);
                m_plabel.setString(str);
            }
        }

        protected CCLabelBMFont m_plabel;
        protected int numberOfTouchesB;
        protected int numberOfTouchesM;
        protected int numberOfTouchesE;
        protected int numberOfTouchesC;
        protected float elapsedTime;
    }
}
