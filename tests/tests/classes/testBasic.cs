using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public abstract class TestScene : CCScene
    {
        public TestScene()
        {
            m_bPortrait = false;
            base.init();
        }
        
        public TestScene(bool bPortrait)
        {
            m_bPortrait = bPortrait;
            if (m_bPortrait)
            {
                CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeRight;
            }
    
            base.init();
        }

        ~TestScene()
        {
        }

        public override void onEnter()
        {
            base.onEnter();

            ////add the menu item for back to main menu
            //CCLabelTTF label = CCLabelTTF.labelWithString("MainMenu", "Arial", 20);
            //CCMenuItemLabel pMenuItem = CCMenuItemLabel.itemWithLabel(label, this, new SEL_MenuHandler(MainMenuCallback));

            //CCMenu pMenu =CCMenu.menuWithItems(pMenuItem);
            //CCSize s = CCDirector.sharedDirector().getWinSize();
            //pMenu.position = new CCPoint(0.0f, 0.0f);
            //pMenuItem.position = new CCPoint( s.width - 50, 25);

            //addChild(pMenu, 1);
        }

        public virtual void MainMenuCallback(CCObject pSender)
        {
            CCScene pScene = CCScene.node();
            CCLayer pLayer = new TestController();

            pScene.addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(pScene);
        }

        public abstract void runThisTest();

        protected bool m_bPortrait; // indicate if this test case requires portrait mode
    }
}