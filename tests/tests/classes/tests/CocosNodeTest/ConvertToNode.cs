using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ConvertToNode : TestCocosNodeDemo
    {
        public ConvertToNode()
        {
            this.isTouchEnabled = true;
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCRotateBy rotate = CCRotateBy.actionWithDuration(10, 360);
            CCRepeatForever action = CCRepeatForever.actionWithAction(rotate);
            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = CCSprite.spriteWithFile("Images/grossini");
                sprite.position = (new CCPoint(s.width / 4 * (i + 1), s.height / 2));

                CCSprite point = CCSprite.spriteWithFile("Images/r1");
                point.scale = 0.25f;
                point.position = sprite.position;
                addChild(point, 10, 100 + i);

                switch (i)
                {
                    case 0:
                        sprite.anchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprite.anchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.anchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.position = (sprite.position);

                CCRepeatForever copy = (CCRepeatForever)action.copy();
                sprite.runAction(copy);
                addChild(sprite, i);
            }
        }

        public override string title()
        {
            return "Convert To Node Space";
        }

        public override void ccTouchesEnded(List<cocos2d.CCTouch> touches, cocos2d.CCEvent event_)
        {
            base.ccTouchesEnded(touches, event_);
        }

        public override string subtitle()
        {
            return "testing convertToNodeSpace / AR. Touch and see console";
        }
    }
}
