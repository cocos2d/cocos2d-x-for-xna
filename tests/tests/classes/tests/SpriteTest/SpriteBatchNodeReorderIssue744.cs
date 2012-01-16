using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeReorderIssue744 : SpriteTestDemo
    {
        public SpriteBatchNodeReorderIssue744()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // Testing issue #744
            // http://code.google.com/p/cocos2d-iphone/issues/detail?id=744
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 15);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSprite sprite = CCSprite.spriteWithBatchNode(batch, new CCRect(0, 0, 85, 121));
            sprite.position = (new CCPoint(s.width / 2, s.height / 2));
            batch.addChild(sprite, 3);
            batch.reorderChild(sprite, 1);
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder issue #744";
        }

        public override string subtitle()
        {
            return "Should not crash";
        }
    }
}
