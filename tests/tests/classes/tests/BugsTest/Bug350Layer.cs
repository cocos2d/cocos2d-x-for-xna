using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug350Layer : BugsTestBaseLayer
    {
        public virtual bool init()
        {
            if (base.init())
            {
                CCSize size = CCDirector.sharedDirector().getWinSize();
                CCSprite background = CCSprite.spriteWithFile("Hello");
                background.position = new CCPoint(size.width / 2, size.height / 2);
                addChild(background);
                return true;
            }

            return false;
        }
    }
}
