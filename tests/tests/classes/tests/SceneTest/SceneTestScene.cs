using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SceneTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = new SceneTestLayer1();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
