using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeAnchorPoint : SpriteTestDemo
    {
        public SpriteBatchNodeAnchorPoint()
        {
            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 1);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.sharedDirector().getWinSize();


            CCActionInterval rotate = CCRotateBy.actionWithDuration(10, 360);
            CCAction action = CCRepeatForever.actionWithAction(rotate);
            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * i, 121 * 1, 85, 121));
                sprite.position = (new CCPoint(s.width / 4 * (i + 1), s.height / 2));

                CCSprite point = CCSprite.spriteWithFile("Images/r1");
                point.scale = 0.25f;
                point.position = sprite.position;
                addChild(point, 1);

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

                point.position = sprite.position;

                CCAction copy = (CCAction)(action.copy());
                sprite.runAction(copy);
                batch.addChild(sprite, i);
            }
        }

        public override string title()
        {
            return "SpriteBatchNode: anchor point";
        }
    }
}
