using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteDemo : CCLayer
    {
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        public SpriteDemo()
        {
        }

        public virtual string title()
        {
            return "ProgressActionsTest";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 18);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 22);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(s_pPathB1, s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(s_pPathR1, s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(s_pPathF1, s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new ProgressActionsTestScene();
            s.addChild(ProgressActionsTestScene.restartAction());

            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new ProgressActionsTestScene();
            s.addChild(ProgressActionsTestScene.nextAction());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new ProgressActionsTestScene();
            s.addChild(ProgressActionsTestScene.backAction());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }
    }
}
