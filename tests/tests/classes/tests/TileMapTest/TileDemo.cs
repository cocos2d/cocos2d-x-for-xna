using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileDemo : CCLayer
    {

        protected CCLabelTTF m_label;
        protected CCLabelTTF m_subtitle;

        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new TileMapTestScene();
            s.addChild(TileMapTestScene.restartTileMapAction()); 

            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new TileMapTestScene();
            s.addChild(TileMapTestScene.nextTileMapAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new TileMapTestScene();
            s.addChild(TileMapTestScene. backTileMapAction() );
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, true);
        }

        public bool ccTouchBegan(CCTouch touch, CCEvent parmevent)
        {
            return true;
        }
        public void ccTouchEnded(CCTouch touch, CCEvent parmevent) { }
        public void ccTouchCancelled(CCTouch touch, CCEvent parmevent) { }
        public void ccTouchMoved(CCTouch touch, CCEvent parmevent)
        {
            CCPoint touchLocation = touch.locationInView(touch.view());
            CCPoint prevLocation = touch.previousLocationInView(touch.view());

            touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);
            prevLocation = CCDirector.sharedDirector().convertToGL(prevLocation);

            CCPoint diff = new CCPoint(touchLocation.x - prevLocation.x, touchLocation.y - prevLocation.y);

            CCNode node = getChildByTag(1);
            CCPoint currentPos = node.position;
            node.position = new CCPoint(currentPos.x + diff.x, currentPos.y + diff.y);
        }

        public TileDemo()
        {
            base.isTouchEnabled = true;

            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_label = CCLabelTTF.labelWithString("not", "Arial", 28);
            addChild(m_label, 1);
            m_label.position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle == null)
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 16);
                addChild(l, 1);
                l.position = new CCPoint(s.width / 2, s.height - 80);

                m_subtitle = l;
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(s_pPathB1, s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(s_pPathR1, s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(s_pPathF1, s_pPathF2, this, nextCallback);




            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);
            menu.position = new CCPoint(0, 0);
            addChild(menu, 1);
        }

        public virtual string title()
        {
            return "No tile";
        }

        public virtual string subtitle()
        {
            return "drag the screen";
        }

        public override void onEnter()
        {
            base.onEnter();

            m_label.setString(title());
        //    m_subtitle.setString(subtitle());
        }

    
    }
}
