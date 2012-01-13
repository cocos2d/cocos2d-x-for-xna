using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PerformBasicLayer : CCLayer
    {
        public PerformBasicLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
        {
            throw new NotFiniteNumberException();
        }

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCMenuItemFont.FontName = "Arial";
            CCMenuItemFont.FontSize = 24;
            CCMenuItemFont pMainItem = CCMenuItemFont.itemFromString("Back", this, toMainLayer);
            pMainItem.position = new CCPoint(s.width - 50, 25);
            CCMenu pMenu = CCMenu.menuWithItems(pMainItem);
            pMenu.position = new CCPoint(0, 0);

            if (m_bControlMenuVisible)
            {
                CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage("Images/b1", "Images/b2", this, backCallback);
                CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage("Images/r1", "Images/r2", this, restartCallback);
                CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage("Images/f1", "Images/f2", this, nextCallback);
                item1.position = new CCPoint(s.width / 2 - 100, 30);
                item2.position = new CCPoint(s.width / 2, 30);
                item3.position = new CCPoint(s.width / 2 + 100, 30);

                pMenu.addChild(item1, PerformanceTestScene.kItemTagBasic);
                pMenu.addChild(item2, PerformanceTestScene.kItemTagBasic);
                pMenu.addChild(item3, PerformanceTestScene.kItemTagBasic);
            }
            addChild(pMenu);
        }

        public virtual void restartCallback(CCObject pSender)
        {
            showCurrentTest();
        }

        public virtual void nextCallback(CCObject pSender)
        {
            m_nCurCase++;
            m_nCurCase = m_nCurCase % m_nMaxCases;

            showCurrentTest();
        }

        public virtual void backCallback(CCObject pSender)
        {
            m_nCurCase--;
            if (m_nCurCase < 0)
                m_nCurCase += m_nMaxCases;

            showCurrentTest();
        }

        public virtual void showCurrentTest()
        {
            throw new NotFiniteNumberException();
        }

        public virtual void toMainLayer(CCObject pSender)
        {
            PerformanceTestScene pScene = new PerformanceTestScene();
            pScene.runThisTest();
        }

        protected bool m_bControlMenuVisible;
        protected int m_nMaxCases;
        public static int m_nCurCase;
        protected int nMaxCases = 0;
        protected int nCurCase = 0;
    }
}
