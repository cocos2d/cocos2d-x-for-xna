using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class SpriteBatchNodeFlip : SpriteTestDemo
    {
        public SpriteBatchNodeFlip()
        {
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 10);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite1 = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.position = (new CCPoint(s.width / 2 - 100, s.height / 2));
            batch.addChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.position = new CCPoint(s.width / 2 + 100, s.height / 2);
            batch.addChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            schedule(flipSprites, 1);
        }

        public void flipSprites(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(getChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite1 = (CCSprite)(batch.getChildByTag((int)kTagSprite.kTagSprite1));
            CCSprite sprite2 = (CCSprite)(batch.getChildByTag((int)kTagSprite.kTagSprite2));

            bool x = sprite1.IsFlipX;
            bool y = sprite2.IsFlipY;

            Debug.WriteLine("Pre: {0}", sprite1.contentSize.height);
            sprite1.IsFlipX = !x;
            sprite2.IsFlipY = !y;
            Debug.WriteLine("Post: {0}", sprite1.contentSize.height);
        }

        public override string title()
        {
            return "SpriteBatchNode Flip X & Y";
        }
    }
}
