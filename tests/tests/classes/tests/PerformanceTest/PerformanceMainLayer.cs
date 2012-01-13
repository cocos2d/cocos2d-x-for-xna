using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PerformanceMainLayer : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCMenu pMenu = CCMenu.menuWithItems(null);
            pMenu.position = new CCPoint(0, 0);
            CCMenuItemFont.FontName = "Arial";
            CCMenuItemFont.FontSize = 24;
            for (int i = 0; i < PerformanceTestScene.MAX_COUNT; ++i)
            {
                CCMenuItemFont pItem = CCMenuItemFont.itemFromString(PerformanceTestScene.testsName[i], this, menuCallback);
                pItem.position = new CCPoint(s.width / 2, s.height - (i + 1) * PerformanceTestScene.LINE_SPACE);
                pMenu.addChild(pItem, PerformanceTestScene.kItemTagBasic + i);
            }

            addChild(pMenu);
        }

        public void menuCallback(CCObject pSender)
        {
            CCMenuItemFont pItem = (CCMenuItemFont)pSender;
            int nIndex = pItem.zOrder - PerformanceTestScene.kItemTagBasic;

            switch (nIndex)
            {
                case 0:
                    PerformanceNodeChildrenTest.runNodeChildrenTest();
                    break;
                case 1:
                    PerformanceParticleTest.runParticleTest();
                    break;
                case 2:
                    PerformanceSpriteTest.runSpriteTest();
                    break;
                case 3:
                    PerformanceTextureTest.runTextureTest();
                    break;
                case 4:
                    PerformanceTouchesTest.runTouchesTest();
                    break;
                default:
                    break;
            }
        }
    }
}
