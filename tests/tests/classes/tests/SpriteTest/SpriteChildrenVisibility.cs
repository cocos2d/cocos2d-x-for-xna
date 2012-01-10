using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteChildrenVisibility : SpriteTestDemo
    {
        public SpriteChildrenVisibility()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");

            CCNode aParent;
            CCSprite sprite1, sprite2, sprite3;
            //
            // SpriteBatchNode
            //
            // parents
            aParent = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);
            aParent.position = (new CCPoint(s.width / 3, s.height / 2));
            addChild(aParent, 0);



            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(0, 0));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(-20, 30));

            aParent.addChild(sprite1);
            sprite1.addChild(sprite2, -2);
            sprite1.addChild(sprite3, 2);

            sprite1.runAction(CCBlink.actionWithDuration(5, 10));

            //
            // Sprite
            //
            aParent = CCNode.node();
            aParent.position = (new CCPoint(2 * s.width / 3, s.height / 2));
            addChild(aParent, 0);

            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(0, 0));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(-20, 30));

            aParent.addChild(sprite1);
            sprite1.addChild(sprite2, -2);
            sprite1.addChild(sprite3, 2);

            sprite1.runAction(CCBlink.actionWithDuration(5, 10));
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite & SpriteBatchNode Visibility";
        }
    }
}
