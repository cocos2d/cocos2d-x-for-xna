using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteTestDemo : CCLayer
    {
        protected string m_strTitle;

        public SpriteTestDemo()
        { }

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

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 28);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 16);
                //CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Thonburi", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage("Images/b1", "Images/b2", this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage("Images/r1", "Images/r2", this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage("Images/f1", "Images/f2", this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint();
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1); 
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new SpriteTestScene();
            s.addChild(SpriteTestScene.restartSpriteTestAction());

            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new SpriteTestScene();
            s.addChild(SpriteTestScene.nextSpriteTestAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new SpriteTestScene();
            s.addChild(SpriteTestScene.backSpriteTestAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
    }
}
