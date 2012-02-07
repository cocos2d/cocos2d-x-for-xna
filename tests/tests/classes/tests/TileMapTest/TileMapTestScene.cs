using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileMapTestScene : TestScene
    {
        public static int sceneIdx = -1;
        public static int MAX_LAYER = 22;

        public static int kTagTileMap = 1;

        public static CCLayer restartTileMapAction()
        {
            CCLayer pLayer = createTileMapLayer(TileMapTestScene.sceneIdx);
            return pLayer;
        }
        public static CCLayer nextTileMapAction()
        {
            TileMapTestScene.sceneIdx++;
            TileMapTestScene.sceneIdx = TileMapTestScene.sceneIdx % TileMapTestScene.MAX_LAYER;

            CCLayer pLayer = createTileMapLayer(TileMapTestScene.sceneIdx);
            return pLayer;
        }
        public static CCLayer backTileMapAction()
        {
            sceneIdx--;
            int total = TileMapTestScene.MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createTileMapLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer createTileMapLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new TMXIsoZorder();
                case 1: return new TMXOrthoZorder();
                case 2: return new TMXIsoVertexZ();
                case 3: return new TMXOrthoVertexZ();
                case 4: return new TMXOrthoTest();
                case 5: return new TMXOrthoTest2();
                case 6: return new TMXOrthoTest3();
                case 7: return new TMXOrthoTest4();
                case 8: return new TMXIsoTest();
                case 9: return new TMXIsoTest1();
                case 10: return new TMXIsoTest2();
                case 11: return new TMXUncompressedTest();
                case 12: return new TMXHexTest();
                case 13: return new TMXReadWriteTest();
                case 14: return new TMXTilesetTest();
                case 15: return new TMXOrthoObjectsTest();
                case 16: return new TMXIsoObjectsTest();
                case 17: return new TMXResizeTest();
                case 18: return new TMXIsoMoveLayer();
                case 19: return new TMXOrthoMoveLayer();
                //case 20: return new TileMapTest();
                //case 20: return new TileMapEditTest();
                case 20: return new TMXBug987();
                case 21: return new TMXBug787();
                case 22: return new TMXGIDObjectsTest();
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
            //CCDirector.sharedDirector().setDepthTest(true);
            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
