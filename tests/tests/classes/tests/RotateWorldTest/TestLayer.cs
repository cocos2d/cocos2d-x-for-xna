using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TestLayer : CCLayer
    {
        public override void onEnter()
        {
            base.onEnter();
            float x, y;
            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;
            //CCMutableArray *array = [UIFont familyNames];
            //for( CCString *s in array )
            //	NSLog( s );
            CCLabelTTF label = CCLabelTTF.labelWithString("cocos2d", "Arial", 64);
            label.position = new CCPoint(x / 2, y / 2);
            addChild(label);
        }

        public static new TestLayer node()
        {
            TestLayer pNode = new TestLayer();
            return pNode;
        }
    }
}
