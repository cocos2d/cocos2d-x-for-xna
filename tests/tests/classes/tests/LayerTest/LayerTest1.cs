using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerTest1 : LayerTest
    {
        int kTagLayer = 1;
        int kCCMenuTouchPriority = -128;

        public override void onEnter()
        {
            base.onEnter();

            this.isTouchEnabled = true;

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLayerColor layer = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(0xFF, 0x00, 0x00, 0x80), 200, 200);

            layer.isRelativeAnchorPoint = true;
            layer.position = (new CCPoint(s.width / 2, s.height / 2));
            addChild(layer, 1, kTagLayer);
        }

        public override string title()
        {
            return "ColorLayer resize (tap & move)";
        }

        public override void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, kCCMenuTouchPriority + 1, true);
        }

        public void updateSize(CCTouch touch)
        {
            CCPoint touchLocation = touch.locationInView(touch.view());
            touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCSize newSize = new CCSize(Math.Abs(touchLocation.x - s.width / 2) * 2, Math.Abs(touchLocation.y - s.height / 2) * 2);
            CCLayerColor l = (CCLayerColor)getChildByTag(kTagLayer);
            l.contentSize = newSize;
        }

        public override bool ccTouchBegan(CCTouch touche, CCEvent events)
        {
            updateSize(touche);
            return false;
        }

        public override void ccTouchMoved(CCTouch touche, CCEvent events)
        {
            updateSize(touche);
        }

        public override void ccTouchEnded(CCTouch touche, CCEvent events)
        {
            updateSize(touche);
        }
    }
}
