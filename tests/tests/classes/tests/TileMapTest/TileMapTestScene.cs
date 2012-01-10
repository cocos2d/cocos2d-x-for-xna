using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileMapTestScene : TestScene
    {
        static int sceneIdx = -1;
        static readonly int MAX_LAYER = 3;

        public static CCLayer restartTileMapAction()
        {
            CCLayer pLayer = createTileMapLayer(sceneIdx);
            return pLayer;
        } 
        public static CCLayer nextTileMapAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createTileMapLayer(sceneIdx);
            return pLayer;
        }
        public static CCLayer backTileMapAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createTileMapLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer createTileMapLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 10: return new TMXBug787();
                case 2: return new TMXOrthoTest2();
                case 1: return new TMXIsoTest1();
                case 0: return new TMXHexTest();
                case 9: return new TMXOrthoTest();
                case 5: return new TMXIsoMoveLayer();
                case 6: return new TMXOrthoMoveLayer();
                case 7: return new TMXBug987();
                case 8: return new TMXIsoTest();
                case 4: return new TMXIsoTest2();
                case 12: return new TMXOrthoZorder();//1
                case 111111: return new TMXGIDObjectsTest();
                case 1111: return new TMXIsoObjectsTest();
                case 11111: return new TMXOrthoObjectsTest();
                //case 3: return new TMXOrthoVertexZ();
                //case 6: return new TMXOrthoTest3();
                //case 7: return new TMXOrthoTest4();
                //case 11: return new TMXUncompressedTest();
                //case 17: return new TMXResizeTest();
                //case 20: return new TileMapTest();
                //case 13: return new TMXReadWriteTest();
                //case 14: return new TMXTilesetTest();
            }

            return null;
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextTileMapAction();
            addChild(pLayer);

            // fix bug #486, #419. 
            // "test" is the default value in CCDirector::setGLDefaultValues()
            // but TransitionTest may setDepthTest(false), we should revert it here
            CCDirector.sharedDirector().setDepthTest(true);
            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
