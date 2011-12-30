using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RotateWorldMainLayer : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();

            float x, y;

            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;

            CCNode blue = CCLayerColor.layerWithColor(new ccColor4B(0, 0, 255, 255));
            CCNode red = CCLayerColor.layerWithColor(new ccColor4B(255, 0, 0, 255));
            CCNode green = CCLayerColor.layerWithColor(new ccColor4B(0, 255, 0, 255));
            CCNode white = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 255, 255));

            blue.scale = (0.5f);
            blue.position = (new CCPoint(-x / 4, -y / 4));
            blue.addChild(SpriteLayer.node());

            red.scale = (0.5f);
            red.position = (new CCPoint(x / 4, -y / 4));

            green.scale = (0.5f);
            green.position = (new CCPoint(-x / 4, y / 4));
            green.addChild(TestLayer.node());

            white.scale = (0.5f);
            white.position = (new CCPoint(x / 4, y / 4));

            addChild(blue, -1);
            addChild(white);
            addChild(green);
            addChild(red);

            CCAction rot = CCRotateBy.actionWithDuration(8, 720);

            blue.runAction(rot);
            red.runAction((CCAction)(rot.copy()));
            green.runAction((CCAction)(rot.copy()));
            white.runAction((CCAction)(rot.copy()));
        }

        public static new RotateWorldMainLayer node()
        {
            RotateWorldMainLayer pNode = new RotateWorldMainLayer();
            return pNode;
        }
    }
}
