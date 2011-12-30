using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RotateWorldTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = RotateWorldMainLayer.node();
            addChild(pLayer);
            runAction(CCRotateBy.actionWithDuration(4, -360));
            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
