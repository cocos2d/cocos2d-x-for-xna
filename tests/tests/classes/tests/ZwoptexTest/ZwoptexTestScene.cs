using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZwoptexTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = ZwoptexTest.nextZwoptexTest();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }

        public static ZwoptexTestScene node()
        {
            ZwoptexTestScene pRet = new ZwoptexTestScene();
            if (pRet != null && pRet.init())
            {
                return pRet;
            }
            else
            {
                pRet = null;
                return null;
            }
        }
    }
}
