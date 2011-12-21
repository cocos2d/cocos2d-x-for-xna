using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ActionManagerTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = ActionManagerTest.nextActionManagerAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
