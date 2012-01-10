using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeChildrenZ : SpriteTestDemo
    {
        public SpriteBatchNodeChildrenZ()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // parents
            CCSpriteBatchNode batch;
            CCSprite sprite1, sprite2, sprite3;


            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");

            // test 1
            batch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(s.width / 3, s.height / 2));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(-20, 30));

            batch.addChild(sprite1);
            sprite1.addChild(sprite2, 2);
            sprite1.addChild(sprite3, -2);

            // test 2
            batch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(2 * s.width / 3, s.height / 2));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(-20, 30));

            batch.addChild(sprite1);
            sprite1.addChild(sprite2, -2);
            sprite1.addChild(sprite3, 2);

            // test 3
            batch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(s.width / 2 - 90, s.height / 4));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(s.width / 2 - 60, s.height / 4));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(s.width / 2 - 30, s.height / 4));

            batch.addChild(sprite1, 10);
            batch.addChild(sprite2, -10);
            batch.addChild(sprite3, -5);

            // test 4
            batch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(s.width / 2 + 30, s.height / 4));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(s.width / 2 + 60, s.height / 4));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(s.width / 2 + 90, s.height / 4));

            batch.addChild(sprite1, -10);
            batch.addChild(sprite2, -5);
            batch.addChild(sprite3, -2);
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite/BatchNode + child + scale + rot";
        }
    }
}
