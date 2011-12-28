using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerTestLayer : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            addChild(label);
            label.position = (new CCPoint(s.width / 2, s.height - 50));

            string subTitle = subtitle();
            if (!string.IsNullOrEmpty(subTitle))
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(subTitle, "Arial", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage("Images/b1", "Images/b2", this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage("Images/r1", "Images/r2", this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage("Images/f1", "Images/f2", this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);
            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public void backCallback(CCObject pSender)
        {
            CCScene pScene = new SchedulerTestScene();
            CCLayer pLayer = SchedulerTestScene.backSchedulerTest();

            pScene.addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(pScene);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene pScene = new SchedulerTestScene();
            CCLayer pLayer = SchedulerTestScene.nextSchedulerTest();

            pScene.addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(pScene);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene pScene = new SchedulerTestScene();
            CCLayer pLayer = SchedulerTestScene.restartSchedulerTest();

            pScene.addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(pScene);
        }
    }
}
