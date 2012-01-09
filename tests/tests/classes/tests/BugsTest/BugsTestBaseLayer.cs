using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class BugsTestBaseLayer : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCMenuItemFont.FontName = "Arial";
            CCMenuItemFont.FontSize = 24;
            CCMenuItemFont pMainItem = CCMenuItemFont.itemFromString("Back", this,
                backCallback);
            pMainItem.position = new CCPoint(s.width - 50, 25);
            CCMenu pMenu = CCMenu.menuWithItems(pMainItem, null);
            pMenu.position = new CCPoint(0, 0);
            addChild(pMenu);
        }

        public void backCallback(CCObject pSender)
        {
            CCDirector.sharedDirector().enableRetinaDisplay(false);
            BugsTestScene pScene = new BugsTestScene();
            pScene.runThisTest();
        }
    }
}
