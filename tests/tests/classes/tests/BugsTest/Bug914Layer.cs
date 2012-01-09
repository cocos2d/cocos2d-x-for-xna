using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug914Layer : BugsTestBaseLayer
    {
        public static CCScene scene()
        {
            // 'scene' is an autorelease object.
            CCScene pScene = CCScene.node();
            // 'layer' is an autorelease object.
            //Bug914Layer layer = Bug914Layer.node();

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
                for (int i = 0; i < 5; i++)
                {
                    layer = CCLayerColor.layerWithColor(new ccColor4B((byte)(i*20), (byte)(i*20), (byte)(i*20),255));
                    layer.contentSize = new CCSize(i * 100, i * 100);
                    layer.position = new CCPoint(size.width / 2, size.height / 2);
                    layer.anchorPoint = new CCPoint(0.5f, 0.5f);
                    layer.isRelativeAnchorPoint = true;
                    addChild(layer, -1 - i);
                    
                }

                // create and initialize a Label
                CCLabelTTF label = CCLabelTTF.labelWithString("Hello World", "Marker Felt", 64);
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
            Debug.WriteLine("Number of touches: %d", touches.Count);
        }

        public void ccTouchesBegan(List<CCTouch> touches, CCEvent eventn)
        {
            ccTouchesMoved(touches, eventn);
        }

        public void restart(CCObject sender)
        {
            CCDirector.sharedDirector().replaceScene(Bug914Layer.scene());
        }

        //LAYER_NODE_FUNC(Bug914Layer);
    }
}
