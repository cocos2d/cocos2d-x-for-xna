using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeChildrenScale : SpriteTestDemo
    {
        public SpriteBatchNodeChildrenScale()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini_family");

            CCNode aParent;
            CCSprite sprite1, sprite2;
            CCActionInterval rot = CCRotateBy.actionWithDuration(10, 360);
            CCAction seq = CCRepeatForever.actionWithAction(rot);

            //
            // Children + Scale using Sprite
            // Test 1
            //
            aParent = CCNode.node();
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossinis_sister1");
            sprite1.position = new CCPoint(s.width / 4, s.height / 4);
            sprite1.scaleX = -0.5f;
            sprite1.scaleY = 2.0f;
            sprite1.runAction(seq);


            sprite2 = CCSprite.spriteWithSpriteFrameName("grossinis_sister2");
            sprite2.position = (new CCPoint(50, 0));

            addChild(aParent);
            aParent.addChild(sprite1);
            sprite1.addChild(sprite2);


            //
            // Children + Scale using SpriteBatchNode
            // Test 2
            //

            aParent = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini_family");
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossinis_sister1");
            sprite1.position = new CCPoint(3 * s.width / 4, s.height / 4);
            sprite1.scaleX = -0.5f;
            sprite1.scaleY = 2.0f;
            sprite1.runAction((CCAction)(seq.copy()));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossinis_sister2");
            sprite2.position = (new CCPoint(50, 0));

            addChild(aParent);
            aParent.addChild(sprite1);
            sprite1.addChild(sprite2);


            //
            // Children + Scale using Sprite
            // Test 3
            //

            aParent = CCNode.node();
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossinis_sister1");
            sprite1.position = (new CCPoint(s.width / 4, 2 * s.height / 3));
            sprite1.scaleX = (1.5f);
            sprite1.scaleY = -0.5f;
            sprite1.runAction((CCAction)(seq.copy()));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossinis_sister2");
            sprite2.position = (new CCPoint(50, 0));

            addChild(aParent);
            aParent.addChild(sprite1);
            sprite1.addChild(sprite2);

            //
            // Children + Scale using Sprite
            // Test 4
            //

            aParent = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini_family");
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossinis_sister1");
            sprite1.position = (new CCPoint(3 * s.width / 4, 2 * s.height / 3));
            sprite1.scaleX = 1.5f;
            sprite1.scaleY = -0.5f;
            sprite1.runAction((CCAction)(seq.copy()));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossinis_sister2");
            sprite2.position = (new CCPoint(50, 0));

            addChild(aParent);
            aParent.addChild(sprite1);
            sprite1.addChild(sprite2);
        }

        public override string title()
        {
            return "Sprite/BatchNode + child + scale + rot";
        }
    }
}
