using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class HiResDemo : CCLayer
    {
        public virtual string title() 
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            label.position = (new CCPoint(s.width / 2, s.height - 50));
            addChild(label, 1);

            string sSubTitle = subtitle();
            if (sSubTitle.Length > 0)
            {
                CCLabelTTF subLabel = CCLabelTTF.labelWithString(sSubTitle, "Arial", 16);
                subLabel.position = (new CCPoint(s.width / 2, s.height - 80));
                addChild(subLabel, 1);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCLayer pLayer = restartHiResAction();

            if (pLayer != null)
            {
                CCScene pScene = new HiResTestScene();
                pScene.addChild(pLayer);

                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void nextCallback(CCObject pSender)
        {
            CCLayer pLayer = nextHiResAction();

            if (pLayer != null)
            {
                CCScene pScene = new HiResTestScene();
                pScene.addChild(pLayer);

                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        public void backCallback(CCObject pSender)
        {
            CCLayer pLayer = backHiResAction();

            if (pLayer != null)
            {
                CCScene pScene = new HiResTestScene();
                pScene.addChild(pLayer);

                CCDirector.sharedDirector().replaceScene(pScene);
            }
        }

        static int MAX_LAYERS = 2;
        static int sceneIdx = -1;


        public static CCLayer createHiResLayer(int idx)
        {
            CCLayer pLayer = null;

            switch (idx)
            {
                case 0:
                    CCDirector.sharedDirector().enableRetinaDisplay(false);
                    pLayer = new HiResTest1();
                    break;
                case 1:
                    CCDirector.sharedDirector().enableRetinaDisplay(true);
                    pLayer = new HiResTest2();
                    break;
            }

            return pLayer;
        }

        public static CCLayer nextHiResAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYERS;

            CCLayer pLayer = createHiResLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartHiResAction()
        {
            CCLayer pLayer = createHiResLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backHiResAction()
        {
            sceneIdx--;
            if (sceneIdx < 0)
                sceneIdx += MAX_LAYERS;

            CCLayer pLayer = createHiResLayer(sceneIdx);
            return pLayer;
        }
    }
}
