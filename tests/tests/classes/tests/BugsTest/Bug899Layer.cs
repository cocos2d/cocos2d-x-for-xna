using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug899Layer : BugsTestBaseLayer
    {
        public override bool init()
        {
            CCDirector.sharedDirector().enableRetinaDisplay(true);
            if (base.init())
            {
                CCSprite bg = CCSprite.spriteWithFile("Images/bugs/RetinaDisplay");
                addChild(bg, 0);
                bg.anchorPoint = new CCPoint(0, 0);

                return true;
            }
            return false;
        }
    }
}
