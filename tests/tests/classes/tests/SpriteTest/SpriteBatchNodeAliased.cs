using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeAliased : SpriteTestDemo
    {
        public SpriteBatchNodeAliased()
        {
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 10);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite1 = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite1.position = (new CCPoint(s.width / 2 - 100, s.height / 2));
            batch.addChild(sprite1, 0, (int)kTagSprite.kTagSprite1);

            CCSprite sprite2 = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 1, 121 * 1, 85, 121));
            sprite2.position = (new CCPoint(s.width / 2 + 100, s.height / 2));
            batch.addChild(sprite2, 0, (int)kTagSprite.kTagSprite2);

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
            CCSpriteBatchNode batch = (CCSpriteBatchNode)getChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.Texture.setAliasTexParameters();
        }

        public override void onExit()
        {
            // restore the tex parameter to AntiAliased.
            CCSpriteBatchNode batch = (CCSpriteBatchNode)getChildByTag((int)kTags.kTagSpriteBatchNode);
            batch.Texture.setAntiAliasTexParameters();
            base.onExit();
        }

        public override string title()
        {
            return "SpriteBatchNode Aliased";
        }
    }
}
