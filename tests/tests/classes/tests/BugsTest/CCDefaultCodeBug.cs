using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class CCDefaultCodeBug : BugsTestBaseLayer
    {
        public static CCScene scene()
        {
            // 'scene' is an autorelease object.
            CCScene pScene = CCScene.node();
            // 'layer' is an autorelease object.

            // add layer as a child to scene
            //pScene.addChild(layer);

            // return the scene
            return pScene;
        }

        public override bool init()
        {
            // always call "super" init
            // Apple recommends to re-assign "self" with the "super" return value
            if (base.init())
            {
                isTouchEnabled = true;
                // ask director the the window size
                CCSize size = CCDirector.sharedDirector().getWinSize();
                CCLayerColor layer;
                CCUserDefault.sharedUserDefault().setBoolForKey("bool", true);
                for (int i = 0; i < 5; i++)
                {
                    ccColor4B c = new ccColor4B((byte)(i * 20), (byte)(i * 20), (byte)(i * 20), 255);
                    CCUserDefault.sharedUserDefault().setStringForKey("i" + i, c.ToString());
                }
                CCUserDefault.sharedUserDefault().flush();
                bool testValue = CCUserDefault.sharedUserDefault().getBoolForKey("bool", false);
                if(!testValue) {
                    CCLog.Log("CCUserDefault: Test failed b/c the 'bool' value was not true when it was expected.");
                }
                for (int i = 0; i < 5; i++)
                {
                    string cstr = CCUserDefault.sharedUserDefault().getStringForKey("i" + i, null);
                    if (cstr == null)
                    {
                        CCLog.Log("CCUserDefault: The color for iteration #" + i + " is null.");
                        continue;
                    }
                    ccColor4B c = new ccColor4B((byte)(i * 20), (byte)(i * 20), (byte)(i * 20), 255);
                    layer = CCLayerColor.layerWithColor(c);
                    layer.contentSize = new CCSize(i * 100, i * 100);
                    layer.position = new CCPoint(size.width / 2, size.height / 2);
                    layer.anchorPoint = new CCPoint(0.5f, 0.5f);
                    layer.isRelativeAnchorPoint = true;
                    addChild(layer, -1 - i);

                }

                // create and initialize a Label
                CCLabelTTF label = CCLabelTTF.labelWithString("ccUserDefault Test", "Arial", 14);
                CCMenuItem item1 = CCMenuItemFont.itemFromString("restart", this, restart);

                CCMenu menu = CCMenu.menuWithItems(item1);
                menu.alignItemsVertically();
                menu.position = new CCPoint(size.width / 2, 100);
                addChild(menu);

                // position the label on the center of the screen
                label.position = new CCPoint(size.width / 2, size.height / 2);

                // add the label as a child to this Layer
                addChild(label);
                return true;
            }
            return false;
        }

        public void ccTouchesMoved(List<CCTouch> touches, CCEvent eventn)
        {
            CCLog.Log("Number of touches: {0}", touches.Count);
        }

        public void ccTouchesBegan(List<CCTouch> touches, CCEvent eventn)
        {
            ccTouchesMoved(touches, eventn);
        }

        public void restart(CCObject sender)
        {
            CCDirector.sharedDirector().replaceScene(CCDefaultCodeBug.scene());
        }

    }
}
