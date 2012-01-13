using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;

namespace tests
{
    public class SpriteBatchNodeZOrder : SpriteTestDemo
    {
        int m_dir;
        public SpriteBatchNodeZOrder()
        {
            m_dir = 1;

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 1);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            float step = s.width / 11;
            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                sprite.position = (new CCPoint((i + 1) * step, s.height / 2));
                batch.addChild(sprite, i);
            }

            for (int i = 5; i < 10; i++)
            {
                CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                sprite.position = new CCPoint((i + 1) * step, s.height / 2);
                batch.addChild(sprite, 14 - i);
            }

            CCSprite sprite1 = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 3, 121 * 0, 85, 121));
            batch.addChild(sprite1, -1, (int)kTagSprite.kTagSprite1);
            sprite1.position = (new CCPoint(s.width / 2, s.height / 2 - 20));
            sprite1.scale = 6;
            sprite1.Color = new ccColor3B(Color.Red);

            schedule(reorderSprite, 1);
        }

        public void reorderSprite(float dt)
        {
            CCSpriteBatchNode batch = (CCSpriteBatchNode)(getChildByTag((int)kTags.kTagSpriteBatchNode));
            CCSprite sprite = (CCSprite)(batch.getChildByTag((int)kTagSprite.kTagSprite1));

            int z = sprite.zOrder;

            if (z < -1)
                m_dir = 1;
            if (z > 10)
                m_dir = -1;

            z += m_dir * 3;

            batch.reorderChild(sprite, z);
        }

        public override string title()
        {
            return "SpriteBatchNode: Z order";
        }
    }
}
