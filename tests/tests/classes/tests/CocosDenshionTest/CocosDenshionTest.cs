using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using cocos2d;
using CocosDenshion;

namespace tests 
{
    public class CocosDenshionTest : CCLayer
    {

    }


    public class CocosDenshionTestScene : TestScene
    {
        public override void runThisTest()
        {
	        CCLayer pLayer = new CocosDenshionTest();
	        addChild(pLayer);

	        CCDirector.sharedDirector().replaceScene(this);
        }
    }
}