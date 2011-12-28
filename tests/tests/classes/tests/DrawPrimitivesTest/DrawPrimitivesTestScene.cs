using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class DrawPrimitivesTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = new DrawPrimitivesTest();
            addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
