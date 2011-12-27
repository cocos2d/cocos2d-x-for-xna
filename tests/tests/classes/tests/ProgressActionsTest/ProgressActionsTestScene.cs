using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ProgressActionsTestScene : TestScene
    {
        public static int sceneIdx = -1;
        public static int MAX_LAYER = 3;

        public static CCLayer createLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new SpriteProgressToRadial();
                case 1: return new SpriteProgressToHorizontal();
                case 2: return new SpriteProgressToVertical();
            }

            return null;
        }

        public static CCLayer nextAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createLayer(sceneIdx);
            //pLayer->autorelease();

            return pLayer;
        }

        public static CCLayer backAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createLayer(sceneIdx);
            //pLayer->autorelease();

            return pLayer;
        }

        public static CCLayer restartAction()
        {
            CCLayer pLayer = createLayer(sceneIdx);
            //pLayer->autorelease();

            return pLayer;
        }

        public override void runThisTest()
        {
            addChild(nextAction());
            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
