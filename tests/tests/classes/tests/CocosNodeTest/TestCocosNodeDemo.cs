using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TestCocosNodeDemo : CCLayer
    {
        public TestCocosNodeDemo() { }

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
            addChild(label, 1);
            label.position = (new CCPoint(s.width / 2, s.height - 50));

            string strSubtitle = subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 16);
                addChild(l, 1);
                l.position = (new CCPoint(s.width / 2, s.height - 80));
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(this.backCallback));
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(this.restartCallback));
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(this.nextCallback));

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene();//CCScene.node();
            s.addChild(CocosNodeTestScene.restartCocosNodeAction());

            CCDirector.sharedDirector().replaceScene(s);
        }
        public void nextCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene();//CCScene.node();
            s.addChild(CocosNodeTestScene.nextCocosNodeAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
        public void backCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene();//CCScene.node();
            s.addChild(CocosNodeTestScene.backCocosNodeAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
    }
}
