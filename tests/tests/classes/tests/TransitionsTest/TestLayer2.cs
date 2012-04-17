using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TestLayer2 : CCLayer
    {
        public TestLayer2()
        {
            float x, y;

            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;

            CCSprite bg1 = CCSprite.spriteWithFile(TransitionsTestScene.s_back2);
            bg1.position = new CCPoint(size.width / 2, size.height / 2);
            addChild(bg1, -1);

            CCLabelTTF title = CCLabelTTF.labelWithString((TransitionsTestScene.transitions[TransitionsTestScene.s_nSceneIdx]), "Arial", 32);
            addChild(title);
            title.Color = new ccColor3B(255, 32, 32);
            title.position = new CCPoint(x / 2, y - 100);

            CCLabelTTF label = CCLabelTTF.labelWithString("SCENE 2", "Arial", 38);
            label.Color = new ccColor3B(16, 16, 255);
            label.position = new CCPoint(x / 2, y / 2);
            addChild(label);

            // menu
            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TransitionsTestScene.s_pPathB1, TransitionsTestScene.s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TransitionsTestScene.s_pPathR1, TransitionsTestScene.s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TransitionsTestScene.s_pPathF1, TransitionsTestScene.s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(x / 2 - 100, 30);
            item2.position = new CCPoint(x / 2, 30);
            item3.position = new CCPoint(x / 2 + 100, 30);

            addChild(menu, 1);
            schedule(step, 1.0f);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.addChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);

            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void nextCallback(CCObject pSender)
        {
            TransitionsTestScene.s_nSceneIdx++;
            TransitionsTestScene.s_nSceneIdx = TransitionsTestScene.s_nSceneIdx % TransitionsTestScene.MAX_LAYER;

            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.addChild(pLayer);
            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);

            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void backCallback(CCObject pSender)
        {
            TransitionsTestScene.s_nSceneIdx--;
            int total = TransitionsTestScene.MAX_LAYER;
            if (TransitionsTestScene.s_nSceneIdx < 0)
                TransitionsTestScene.s_nSceneIdx += total;

            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.addChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            
            if (pScene != null)
            {
                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void step(float dt) { }
    }
}
