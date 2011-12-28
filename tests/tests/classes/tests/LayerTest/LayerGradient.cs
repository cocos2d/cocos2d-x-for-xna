using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;

namespace tests
{
    public class LayerGradient : LayerTest
    {
        int kTagLayer = 1;
        public LayerGradient()
        {
            CCLayerGradient layer1 = CCLayerGradient.layerWithColor(new ccColor4B(255, 0, 0, 255), new ccColor4B(0, 255, 0, 255), new CCPoint(0.9f, 0.9f));
            addChild(layer1, 0, kTagLayer);

            this.isTouchEnabled = true;

            CCLabelTTF label1 = CCLabelTTF.labelWithString("Compressed Interpolation: Enabled", "Arial", 26);
            CCLabelTTF label2 = CCLabelTTF.labelWithString("Compressed Interpolation: Disabled", "Arial", 26);
            CCMenuItemLabel item1 = CCMenuItemLabel.itemWithLabel(label1);
            CCMenuItemLabel item2 = CCMenuItemLabel.itemWithLabel(label2);
            CCMenuItemToggle item = CCMenuItemToggle.itemWithTarget(this, (toggleItem), item1, item2);

            CCMenu menu = CCMenu.menuWithItems(item);
            addChild(menu);
            CCSize s = CCDirector.sharedDirector().getWinSize();
            menu.position = (new CCPoint(s.width / 2, 100));
        }

        public override void ccTouchesMoved(List<cocos2d.CCTouch> touches, cocos2d.CCEvent event_)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            var it = touches.FirstOrDefault();
            CCTouch touch = (CCTouch)(it);
            CCPoint start = touch.locationInView(touch.view());
            start = CCDirector.sharedDirector().convertToGL(start);

            CCPoint diff = new CCPoint(s.width / 2 - start.x, s.height / 2 - start.y);
            diff = CCPointExtension.ccpNormalize(diff);

            CCLayerGradient gradient = (CCLayerGradient)getChildByTag(1);
            gradient.Vector = diff;
        }


        public override string title()
        {
            return "LayerGradient";
        }

        public override string subtitle()
        {
            return "Touch the screen and move your finger";
        }

        public void toggleItem(object sender)
        {
            CCLayerGradient gradient = (CCLayerGradient)getChildByTag(kTagLayer);
            gradient.IsCompressedInterpolation = !gradient.IsCompressedInterpolation;
        }
    }
}
