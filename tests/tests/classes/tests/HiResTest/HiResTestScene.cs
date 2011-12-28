using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class HiResTestScene : TestScene
    {
        public override void runThisTest()
        {
            sm_bRitinaDisplay = CCDirector.sharedDirector().isRetinaDisplay();

            CCLayer pLayer = HiResDemo.nextHiResAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }

        public override void MainMenuCallback(CCObject pSender)
        {
            CCDirector.sharedDirector().enableRetinaDisplay(sm_bRitinaDisplay);
            base.MainMenuCallback(pSender);
        }

        public static bool sm_bRitinaDisplay;
    }
}
