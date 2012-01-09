using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class BugsTestMainLayer : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();
            m_pItmeMenu = CCMenu.menuWithItems(null);
            CCMenuItemFont.FontName = "Arial";
            CCMenuItemFont.FontSize = 24;
            for (int i = 0; i < BugsTestScene.MAX_COUNT; ++i)
            {
                CCMenuItemFont pItem = CCMenuItemFont.itemFromString(BugsTestScene.testsName[i], this,
                                                            menuCallback);
                pItem.position = new CCPoint(s.width / 2, s.height - (i + 1) * BugsTestScene.LINE_SPACE);
                m_pItmeMenu.addChild(pItem, BugsTestScene.kItemTagBasic + i);
            }

            m_pItmeMenu.position = BugsTestScene.s_tCurPos;
            addChild(m_pItmeMenu);
            isTouchEnabled = true;
        }

        public void menuCallback(CCObject pSender)
        {
            CCMenuItemFont pItem = (CCMenuItemFont)pSender;
            int nIndex = pItem.zOrder - BugsTestScene.kItemTagBasic;

            CCScene pScene = CCScene.node();
            CCLayer pLayer = null;

            switch (nIndex)
            {
                case 0:
                    pLayer = new Bug350Layer();
                    pLayer.init();
                    break;
                case 1:
                    pLayer = new Bug422Layer();
                    pLayer.init();
                    break;
                case 2:
                    pLayer = new Bug458Layer();
                    pLayer.init();
                    break;
                case 3:
                    pLayer = new Bug624Layer();
                    pLayer.init();
                    break;
                case 4:
                    pLayer = new Bug886Layer();
                    pLayer.init();
                    break;
                case 5:
                    pLayer = new Bug899Layer();
                    pLayer.init();
                    break;
                case 6:
                    pLayer = new Bug914Layer();
                    pLayer.init();
                    break;
                case 7:
                    pLayer = new Bug1159Layer();
                    pLayer.init();
                    break;
                case 8:
                    pLayer = new Bug1174Layer();
                    pLayer.init();
                    break;
                default:
                    break;
            }
            pScene.addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(pScene);
        }

        public override void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {
            foreach (var it in pTouches)
            {
                CCTouch touch = it;
                m_tBeginPos = touch.locationInView(touch.view());
                m_tBeginPos = CCDirector.sharedDirector().convertToGL(m_tBeginPos);
            }

        }

        public override void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        {
            foreach (var it in pTouches)
            {
                CCTouch touch = it;

                CCPoint touchLocation = touch.locationInView(touch.view());
                touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);
                float nMoveY = touchLocation.y - m_tBeginPos.y;

                CCPoint curPos = m_pItmeMenu.position;
                CCPoint nextPos = new CCPoint(curPos.x, curPos.y + nMoveY);
                CCSize winSize = CCDirector.sharedDirector().getWinSize();
                if (nextPos.y < 0.0f)
                {
                    m_pItmeMenu.position = new CCPoint(0, 0);
                    return;
                }

                if (nextPos.y > ((BugsTestScene.MAX_COUNT + 1) * BugsTestScene.LINE_SPACE - winSize.height))
                {
                    m_pItmeMenu.position = new CCPoint(0, ((BugsTestScene.MAX_COUNT + 1) * BugsTestScene.LINE_SPACE - winSize.height));
                    return;
                }

                m_pItmeMenu.position = nextPos;
                m_tBeginPos = touchLocation;
                BugsTestScene.s_tCurPos = nextPos;
            }

        }

        protected CCPoint m_tBeginPos;
        protected CCMenu m_pItmeMenu;
    }
}
