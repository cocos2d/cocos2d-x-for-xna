using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ClickAndMoveTest : TestScene
    {
        public static int kTagSprite = 1;
        public static string s_pPathGrossini = "Images/grossini";
        public override void runThisTest()
        {
            CCLayer pLayer = new MainLayer();
            //pLayer->autorelease();

            addChild(pLayer);
            CCDirector.sharedDirector().replaceScene(this);
        }

    }

    public class MainLayer : CCLayer
    {
        public MainLayer()
        {
            base.isTouchEnabled = true;

            CCSprite sprite = CCSprite.spriteWithFile(ClickAndMoveTest.s_pPathGrossini);

            CCLayer layer = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 0, 255));
            addChild(layer, -1);

            addChild(sprite, 0, ClickAndMoveTest.kTagSprite);
            sprite.position = new CCPoint(20, 150);

            sprite.runAction(CCJumpTo.actionWithDuration(4, new CCPoint(300, 48), 100, 4));

            layer.runAction(CCRepeatForever.actionWithAction(
                                                                (CCActionInterval)(CCSequence.actions(
                                                                                    CCFadeIn.actionWithDuration(1),
                                                                                    CCFadeOut.actionWithDuration(1)))
                                                                ));
        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            //base.ccTouchesEnded(touches, event_);
            object it = touches.First();
            CCTouch touch = (CCTouch)(it);

            CCPoint location = touch.locationInView(touch.view());
            CCPoint convertedLocation = CCDirector.sharedDirector().convertToGL(location);

            CCNode s = getChildByTag(ClickAndMoveTest.kTagSprite);
            s.stopAllActions();
            s.runAction(CCMoveTo.actionWithDuration(1, new CCPoint(convertedLocation.x, convertedLocation.y)));
            float o = convertedLocation.x - s.position.x;
            float a = convertedLocation.y - s.position.y;
            float at = (float)(Math.Atan(o / a) * 57.29577951f);

            if (a < 0)
            {
                if (o < 0)
                    at = 180 + Math.Abs(at);
                else
                    at = 180 - Math.Abs(at);
            }

            s.runAction(CCRotateTo.actionWithDuration(1, at));
        }
    }
}
