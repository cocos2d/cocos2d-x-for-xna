using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ActionManagerTest : CCLayer
    {
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        protected CCTextureAtlas m_atlas;

        protected string m_strTitle;

        public ActionManagerTest() { }

        public virtual string title()
        {
            return "No title";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            addChild(label, 1);
            label.position = (new CCPoint(s.width / 2, s.height - 50));

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(s_pPathB1, s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(s_pPathR1, s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(s_pPathF1, s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = (new CCPoint(0, 0));
            item1.position = (new CCPoint(s.width / 2 - 100, 30));
            item2.position = (new CCPoint(s.width / 2, 30));
            item3.position = (new CCPoint(s.width / 2 + 100, 30));

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.addChild(restartActionManagerAction());

            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.addChild(nextActionManagerAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.addChild(backActionManagerAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public static int sceneIdx = -1;
        public static int MAX_LAYER = 4;

        public static CCLayer backActionManagerAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer createActionManagerLayer(int nIndex)
        {
            switch (nIndex)
            {
                // case 0: return new CrashTest();
                case 0: return new LogicTest();
                case 1: return new PauseTest();
                case 2: return new RemoveTest();
                case 3: return new ResumeTest();
            }

            return null;
        }

        public static CCLayer nextActionManagerAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartActionManagerAction()
        {
            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }
    }
}
