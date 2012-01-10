using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Sprite6 : SpriteTestDemo
    {
        int kTagSpriteBatchNode = 1;
        public Sprite6()
        {
            // small capacity. Testing resizing
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 1);
            addChild(batch, 0, kTagSpriteBatchNode);
            batch.isRelativeAnchorPoint = false;

            CCSize s = CCDirector.sharedDirector().getWinSize();

            batch.anchorPoint = new CCPoint(0.5f, 0.5f);
            batch.contentSize = (new CCSize(s.width, s.height));


            // SpriteBatchNode actions
            CCActionInterval rotate = CCRotateBy.actionWithDuration(5, 360);
            CCAction action = CCRepeatForever.actionWithAction(rotate);

            // SpriteBatchNode actions
            CCActionInterval rotate_back = (CCActionInterval)rotate.reverse();
            CCActionInterval rotate_seq = (CCActionInterval)(CCSequence.actions(rotate, rotate_back));
            CCAction rotate_forever = CCRepeatForever.actionWithAction(rotate_seq);

            CCActionInterval scale = CCScaleBy.actionWithDuration(5, 1.5f);
            CCActionInterval scale_back = (CCActionInterval)scale.reverse();
            CCActionInterval scale_seq = (CCActionInterval)(CCSequence.actions(scale, scale_back));
            CCAction scale_forever = CCRepeatForever.actionWithAction(scale_seq);

            float step = s.width / 4;

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * i, 121 * 1, 85, 121));
                sprite.position = (new CCPoint((i + 1) * step, s.height / 2));

                sprite.runAction((CCAction)(action.copy()));
                batch.addChild(sprite, i);
            }

            batch.runAction(scale_forever);
            batch.runAction(rotate_forever);
        }

        public override string title()
        {
            return "SpriteBatchNode transformation";
        }
    }
}
