using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class HiResTest1 : HiResDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize size = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite = CCSprite.spriteWithFile("Images/grossini");
            addChild(sprite);
            sprite.position = (new CCPoint(size.width / 2, size.height / 2));
        }

        public override string title()
        {
            return "High resolution image test";
        }

        public override string subtitle()
        {
            return "Image without high resolution resource";
        }
    }
}
