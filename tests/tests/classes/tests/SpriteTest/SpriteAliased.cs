using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteAliased : SpriteTestDemo
    {
        public SpriteAliased()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite1 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.position = (new CCPoint(s.width / 2 - 100, s.height / 2));
            addChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.position = (new CCPoint(s.width / 2 + 100, s.height / 2));
            addChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

            CCActionInterval scale = CCScaleBy.actionWithDuration(2, 5);
            CCActionInterval scale_back = (CCActionInterval)scale.reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.actions(scale, scale_back));
            CCAction repeat = CCRepeatForever.actionWithAction(seq);

            CCAction repeat2 = (CCAction)(repeat.copy());

            sprite1.runAction(repeat);
            sprite2.runAction(repeat2);
        }

        public override void onEnter()
        {
            base.onEnter();

            //
            // IMPORTANT:
            // This change will affect every sprite that uses the same texture
            // So sprite1 and sprite2 will be affected by this change
            //
            CCSprite sprite = (CCSprite)getChildByTag((int)kTagSprite.kTagSprite1);
            sprite.Texture.setAliasTexParameters();
        }

        public override void onExit()
        {
            // restore the tex parameter to AntiAliased.
            CCSprite sprite = (CCSprite)getChildByTag((int)kTagSprite.kTagSprite1);
            sprite.Texture.setAntiAliasTexParameters();
            base.onExit();
        }

        public override string title()
        {
            return "Sprite Aliased";
        }
    }
}
