using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class EffectAdvanceTextLayer : CCLayer
    {
        protected CCTextureAtlas m_atlas;
        protected string m_strTitle;

        public EffectAdvanceTextLayer()
        {

        }

        protected CCSprite grossini;
        protected CCSprite tamara;
        public override void onEnter()
        {
            base.onEnter();

            float x, y;

            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;

            CCSprite bg = CCSprite.spriteWithFile("Images/background3");
            addChild(bg, 0, EffectAdvanceScene.kTagBackground);
            bg.position = new CCPoint(x / 2, y / 2);

            grossini = CCSprite.spriteWithFile("Images/grossinis_sister2");
            bg.addChild(grossini, 1, EffectAdvanceScene.kTagSprite1);
            grossini.position = new CCPoint(x / 3.0f, 200);
            CCActionInterval sc = CCScaleBy.actionWithDuration(2, 5);
            CCFiniteTimeAction sc_back = sc.reverse();
            grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(sc, sc_back))));

            tamara = CCSprite.spriteWithFile("Images/grossinis_sister1");
            bg.addChild(tamara, 1, EffectAdvanceScene.kTagSprite2);
            tamara.position = new CCPoint(2 * x / 3.0f, 200);
            CCActionInterval sc2 = CCScaleBy.actionWithDuration(2, 5);
            CCFiniteTimeAction sc2_back = sc2.reverse();
            tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(sc2, sc2_back))));

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 28);

            label.position = new CCPoint(x / 2, y - 80);
            addChild(label);
            label.tag = EffectAdvanceScene.kTagLabel;

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 16);
                addChild(l, 101);
                l.position = new CCPoint(size.width / 2, size.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage("Images/b1", "Images/b2", this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage("Images/r1", "Images/r2", this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage("Images/f1", "Images/f2", this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0, 0);
            item1.position = new CCPoint(size.width / 2 - 100, 30);
            item2.position = new CCPoint(size.width / 2, 30);
            item3.position = new CCPoint(size.width / 2 + 100, 30);

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

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new EffectAdvanceScene();
            s.addChild(EffectAdvanceScene.restartEffectAdvanceAction());

            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new EffectAdvanceScene();
            s.addChild(EffectAdvanceScene.nextEffectAdvanceAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new EffectAdvanceScene();
            s.addChild(EffectAdvanceScene.backEffectAdvanceAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
    }
}
