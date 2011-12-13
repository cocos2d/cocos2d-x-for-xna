using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileMapTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = TileMapTest.nextTileMapAction();
            addChild(pLayer);

            // fix bug #486, #419. 
            // "test" is the default value in CCDirector::setGLDefaultValues()
            // but TransitionTest may setDepthTest(false), we should revert it here
            CCDirector.sharedDirector().setDepthTest(true);
            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
