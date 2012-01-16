using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PerformanceTouchesTest
    {

        public static int TEST_COUNT = 2;
        public static int s_nTouchCurCase = 0;

        public static void runTouchesTest()
        {
            s_nTouchCurCase = 0;
            CCScene pScene = CCScene.node();
            CCLayer pLayer = new TouchesPerformTest1(true, TEST_COUNT, s_nTouchCurCase);

            pScene.addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(pScene);
        }
    }
}
