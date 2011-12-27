using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RenderTextureTestDemo : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 28);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Thonburi", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage("Images/b1.png", "Images/b2.png", this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage("Images/r1.png", "Images/r2.png", this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage("Images/f1.png", "Images/f2.png", this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }

        public virtual string title()
        {
            return "Render Texture Test";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new RenderTextureScene();
            s.addChild(RenderTextureScene.restartTestCase());

            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new RenderTextureScene();
            s.addChild(RenderTextureScene.nextTestCase());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new RenderTextureScene();
            s.addChild(RenderTextureScene.backTestCase());
            CCDirector.sharedDirector().replaceScene(s);
            //s->release();
        }
    }
}
