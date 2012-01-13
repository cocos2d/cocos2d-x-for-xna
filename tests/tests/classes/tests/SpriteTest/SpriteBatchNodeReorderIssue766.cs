using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeReorderIssue766 : SpriteTestDemo
    {

        public SpriteBatchNodeReorderIssue766()
        {
            batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/piece", 15);
            addChild(batchNode, 1, 0);

            sprite1 = makeSpriteZ(2);
            sprite1.position = (new CCPoint(200, 160));

            sprite2 = makeSpriteZ(3);
            sprite2.position = (new CCPoint(264, 160));

            sprite3 = makeSpriteZ(4);
            sprite3.position = (new CCPoint(328, 160));

            schedule(reorderSprite, 2);
        }

        public override string title()
        {
            return "SpriteBatchNode: reorder issue #766";
        }

        public override string subtitle()
        {
            return "In 2 seconds 1 sprite will be reordered";
        }
        public void reorderSprite(float dt)
        {
            batchNode.reorderChild(sprite1, 4);
        }

        public CCSprite makeSpriteZ(int aZ)
        {
            CCSprite sprite = CCSprite.spriteWithBatchNode(batchNode, new CCRect(128, 0, 64, 64));
            batchNode.addChild(sprite, aZ + 1, 0);

            //children
            CCSprite spriteShadow = CCSprite.spriteWithBatchNode(batchNode, new CCRect(0, 0, 64, 64));
            spriteShadow.Opacity = 128;
            sprite.addChild(spriteShadow, aZ, 3);

            CCSprite spriteTop = CCSprite.spriteWithBatchNode(batchNode, new CCRect(64, 0, 64, 64));
            sprite.addChild(spriteTop, aZ + 2, 3);

            return sprite;
        }

        private CCSpriteBatchNode batchNode;
        private CCSprite sprite1;
        private CCSprite sprite2;
        private CCSprite sprite3;
    }
}
