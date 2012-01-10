using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZwoptexTest : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 26);
            addChild(label, 1);
            label.position = (new CCPoint(s.width / 2, s.height - 50));

            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubTitle, "Arial", 16);
                addChild(l, 1);
                l.position = (new CCPoint(s.width / 2, s.height - 80));
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, (backCallback));
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, (restartCallback));
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, (nextCallback));

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = (new CCPoint(0, 0));
            item1.position = (new CCPoint(s.width / 2 - 100, 30));
            item2.position = (new CCPoint(s.width / 2, 30));
            item3.position = (new CCPoint(s.width / 2 + 100, 30));
            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = ZwoptexTestScene.node();
            s.addChild(restartZwoptexTest());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = ZwoptexTestScene.node();
            s.addChild(nextZwoptexTest());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = ZwoptexTestScene.node();
            s.addChild(backZwoptexTest());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }


        public static int MAX_LAYER = 1;

        public static int sceneIdx = -1;



        public static CCLayer createZwoptexLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new ZwoptexGenericTest();
            }

            return null;
        }

        public static CCLayer nextZwoptexTest()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createZwoptexLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backZwoptexTest()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createZwoptexLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartZwoptexTest()
        {
            CCLayer pLayer = createZwoptexLayer(sceneIdx);

            return pLayer;
        }
    }
}
