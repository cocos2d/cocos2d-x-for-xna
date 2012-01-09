using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug886Layer : BugsTestBaseLayer
    {
        public override bool init()
        {
            if (base.init())
            {
                // ask director the the window size
                //		CGSize size = [[CCDirector sharedDirector] winSize];

                CCSprite sprite = CCSprite.spriteWithFile("Images/bugs/bug886");
                sprite.anchorPoint = new CCPoint(0, 0);
                sprite.position = new CCPoint(0, 0);
                sprite.scaleX = 0.6f;
                addChild(sprite);

                CCSprite sprite2 = CCSprite.spriteWithFile("Images/bugs/bug886");
                sprite2.anchorPoint = new CCPoint(0, 0);
                sprite2.scaleX = 0.6f;
                sprite2.position = new CCPoint(sprite.contentSize.width * 0.6f + 10, 0);
                addChild(sprite2);

                return true;
            }

            return false;
        }
    }
}
