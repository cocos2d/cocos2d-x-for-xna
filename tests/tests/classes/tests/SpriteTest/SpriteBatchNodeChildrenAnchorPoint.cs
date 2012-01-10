using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests.classes.tests.SpriteTest
{
    public class SpriteBatchNodeChildrenAnchorPoint : SpriteTestDemo
    {
        public SpriteBatchNodeChildrenAnchorPoint()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");

            CCNode aParent;
            CCSprite sprite1, sprite2, sprite3, sprite4, point;
            //
            // SpriteBatchNode
            //
            // parents

            aParent = CCSpriteBatchNode.batchNodeWithFile("animations/grossini", 50);
            addChild(aParent, 0);

            // anchor (0,0)
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_08");
            sprite1.position = (new CCPoint(s.width / 4, s.height / 2));
            sprite1.anchorPoint = (new CCPoint(0, 0));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03");
            sprite3.position = (new CCPoint(-20, 30));

            sprite4 = CCSprite.spriteWithSpriteFrameName("grossini_dance_04");
            sprite4.position = (new CCPoint(0, 0));
            sprite4.scale = 0.5f;

            aParent.addChild(sprite1);
            sprite1.addChild(sprite2, -2);
            sprite1.addChild(sprite3, -2);
            sprite1.addChild(sprite4, 3);

            point = CCSprite.spriteWithFile("Images/r1");
            point.scale = 0.25f;
            point.position = sprite1.position;
            addChild(point, 10);


            // anchor (0.5, 0.5)
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_08");
            sprite1.position = (new CCPoint(s.width / 2, s.height / 2));
            sprite1.anchorPoint = (new CCPoint(0.5f, 0.5f));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03");
            sprite3.position = (new CCPoint(-20, 30));

            sprite4 = CCSprite.spriteWithSpriteFrameName("grossini_dance_04");
            sprite4.position = (new CCPoint(0, 0));
            sprite4.scale = 0.5f;

            aParent.addChild(sprite1);
            sprite1.addChild(sprite2, -2);
            sprite1.addChild(sprite3, -2);
            sprite1.addChild(sprite4, 3);

            point = CCSprite.spriteWithFile("Images/r1");
            point.scale = 0.25f;
            point.position = sprite1.position;
            addChild(point, 10);


            // anchor (1,1)
            sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_08");
            sprite1.position = (new CCPoint(s.width / 2 + s.width / 4, s.height / 2));
            sprite1.anchorPoint = (new CCPoint(1, 1));

            sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02");
            sprite2.position = (new CCPoint(20, 30));

            sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03");
            sprite3.position = (new CCPoint(-20, 30));

            sprite4 = CCSprite.spriteWithSpriteFrameName("grossini_dance_04");
            sprite4.position = (new CCPoint(0, 0));
            sprite4.scale = 0.5f;

            aParent.addChild(sprite1);
            sprite1.addChild(sprite2, -2);
            sprite1.addChild(sprite3, -2);
            sprite1.addChild(sprite4, 3);

            point = CCSprite.spriteWithFile("Images/r1");
            point.scale = 0.25f;
            point.position = sprite1.position;
            addChild(point, 10);
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeUnusedSpriteFrames();
        }

        public override string title()
        {
            return "SpriteBatchNode: children + anchor";
        }
    }
}
