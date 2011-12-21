using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ParallaxDemo : CCLayer
    {
        string s_pPathB1 = "Images/b1.png";
        string s_pPathB2 = "Images/b2.png";
        string s_pPathR1 = "Images/r1.png";
        string s_pPathR2 = "Images/r2.png";
        string s_pPathF1 = "Images/f1.png";
        string s_pPathF2 = "Images/f2.png";

        protected CCTextureAtlas m_atlas;

        public ParallaxDemo()
        {
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 28);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 50);

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
            CCScene s = new ParallaxTestScene();
            s.addChild(ParallaxTestScene.restartParallaxAction());

            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.addChild(ParallaxTestScene.nextParallaxAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.addChild(ParallaxTestScene.backParallaxAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
    }
}
