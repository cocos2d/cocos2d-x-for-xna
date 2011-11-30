using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Sprite1 : SpriteTestDemo
    {
        public Sprite1()
        {
            isTouchEnabled = true;

            CCSize s = CCDirector.sharedDirector().getWinSize();
            addNewSpriteWithCoords(new CCPoint(s.width / 2, s.height / 2));
        }

        public override string title()
        {
            return "Sprite (tap screen)";
        }

        public void addNewSpriteWithCoords(CCPoint p)
        {
            int idx = (int)(ccMacros.CCRANDOM_0_1() * 1400.0f / 100.0f);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;

            CCSprite sprite = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(x, y, 85, 121));
            addChild(sprite);

            sprite.position = p;

            CCActionInterval action;
            float random = ccMacros.CCRANDOM_0_1();

            if (random < 0.20)
                action = CCScaleBy.actionWithDuration(3, 2);
            else if (random < 0.40)
                action = CCRotateBy.actionWithDuration(3, 360);
            else if (random < 0.60)
                action = CCBlink.actionWithDuration(1, 3);
            else if (random < 0.8)
                action = CCTintBy.actionWithDuration(2, 0, -255, -255);
            else
                action = CCFadeOut.actionWithDuration(2);
            object obj = action.reverse();
            CCActionInterval action_back = (CCActionInterval)action.reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.actions(action, action_back));

            sprite.runAction(CCRepeatForever.actionWithAction(seq));
        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent eventArgs)
        {
            foreach (CCTouch touch in touches)
            {
                CCPoint location = touch.locationInView(touch.view());
                location = CCDirector.sharedDirector().convertToGL(location);
                addNewSpriteWithCoords(location);
            }
        }
    }
}
