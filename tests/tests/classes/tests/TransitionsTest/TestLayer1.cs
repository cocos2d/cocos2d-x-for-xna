using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{

    public class TestLayer1 : CCLayer
    {
        static int s_nSceneIdx = 0;
        int MAX_LAYER = 37;
        float TRANSITION_DURATION = 1.2f;
        string s_back1 = "Images/background1";
        static string[] transitions = new string[]  {
    "JumpZoomTransition",
    "FadeTransition",
    "FadeWhiteTransition",
    "FlipXLeftOver",
    "FlipXRightOver",
    "FlipYUpOver",
    "FlipYDownOver",
    "FlipAngularLeftOver",
    "FlipAngularRightOver",
    "ZoomFlipXLeftOver",
    "ZoomFlipXRightOver",
    "ZoomFlipYUpOver",
    "ZoomFlipYDownOver",
    "ZoomFlipAngularLeftOver",
    "ZoomFlipAngularRightOver",
    "ShrinkGrowTransition",
    "RotoZoomTransition",
    "MoveInLTransition",
    "MoveInRTransition",
    "MoveInTTransition",
    "MoveInBTransition",
    "SlideInLTransition",
    "SlideInRTransition",
    "SlideInTTransition",
    "SlideInBTransition",

    "CCTransitionCrossFade",
    "CCTransitionRadialCCW",
    "CCTransitionRadialCW",
    "PageTransitionForward",
    "PageTransitionBackward",
    "FadeTRTransition",
    "FadeBLTransition",
    "FadeUpTransition",
    "FadeDownTransition",
    "TurnOffTilesTransition",
    "SplitRowsTransition",
    "SplitColsTransition",
};
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        public TestLayer1()
        {
            float x, y;

            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;

            CCSprite bg1 = CCSprite.spriteWithFile(s_back1);
            bg1.position = (new CCPoint(size.width / 2, size.height / 2));
            addChild(bg1, -1);

            CCLabelTTF title = CCLabelTTF.labelWithString((transitions[s_nSceneIdx]), "Arial", 32);
            addChild(title);
            title.Color = new ccColor3B(255, 32, 32);
            title.position = new CCPoint(x / 2, y - 100);

            CCLabelTTF label = CCLabelTTF.labelWithString("SCENE 1", "Arial", 38);
            label.Color = (new ccColor3B(16, 16, 255));
            label.position = (new CCPoint(x / 2, y / 2));
            addChild(label);

            // menu
            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(s_pPathB1, s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(s_pPathR1, s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(s_pPathF1, s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(size.width / 2 - 100, 30);
            item2.position = new CCPoint(size.width / 2, 30);
            item3.position = new CCPoint(size.width / 2 + 100, 30);

            addChild(menu, 1);
            schedule(step, 1.0f);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.addChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(s_nSceneIdx, TRANSITION_DURATION, s);
            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void nextCallback(CCObject pSender)
        {
            s_nSceneIdx++;
            s_nSceneIdx = s_nSceneIdx % MAX_LAYER;

            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.addChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(s_nSceneIdx, TRANSITION_DURATION, s);
            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void backCallback(CCObject pSender)
        {
            s_nSceneIdx--;
            int total = MAX_LAYER;
            if (s_nSceneIdx < 0)
                s_nSceneIdx += total;

            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.addChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(s_nSceneIdx, TRANSITION_DURATION, s);
            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void step(float dt)
        {

        }
    }
}
