using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class SpriteFlip : SpriteTestDemo
    {
        public SpriteFlip()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite1 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.position = (new CCPoint(s.width / 2 - 100, s.height / 2));
            addChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.position = (new CCPoint(s.width / 2 + 100, s.height / 2));
            addChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            schedule(flipSprites, 1);
        }

        public void flipSprites(float dt)
        {
            CCSprite sprite1 = (CCSprite)(getChildByTag((int)kTagSprite.kTagSprite1));
            CCSprite sprite2 = (CCSprite)(getChildByTag((int)kTagSprite.kTagSprite2));

            bool x = sprite1.IsFlipX;
            bool y = sprite2.IsFlipY;
            Debug.WriteLine("Pre: {0}", sprite1.contentSize.height);
            sprite1.IsFlipX = (!x);
            sprite2.IsFlipY = (!y);
            Debug.WriteLine("Post: {0}", sprite1.contentSize.height);
        }

        public override string title()
        {
            return "Sprite Flip X & Y";
        }
    }
}
