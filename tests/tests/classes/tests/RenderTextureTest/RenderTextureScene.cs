using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RenderTextureScene : TestScene
    {
        public static int sceneIdx = -1;
        public static int MAX_LAYER = 4;

        public static CCLayer createTestCase(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new RenderTextureTest();
                case 1: return new RenderTextureIssue937();
                case 2: return new RenderTextureZbuffer();
                case 3: return new RenderTextureSave();
            }

            return null;
        }

        public static CCLayer nextTestCase()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createTestCase(sceneIdx);
            //pLayer->autorelease();

            return pLayer;
        }

        public static CCLayer backTestCase()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createTestCase(sceneIdx);
            //pLayer->autorelease();

            return pLayer;
        }

        public static CCLayer restartTestCase()
        {
            CCLayer pLayer = createTestCase(sceneIdx);
            //pLayer->autorelease();

            return pLayer;
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextTestCase();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
