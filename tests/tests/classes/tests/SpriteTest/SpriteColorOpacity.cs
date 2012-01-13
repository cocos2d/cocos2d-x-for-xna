using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteColorOpacity : SpriteTestDemo
    {
        public SpriteColorOpacity()
        {
            CCSprite sprite1 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
            CCSprite sprite2 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            CCSprite sprite3 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 2, 121 * 1, 85, 121));
            CCSprite sprite4 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 1, 85, 121));

            CCSprite sprite5 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 0, 121 * 1, 85, 121));
            CCSprite sprite6 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 1, 121 * 1, 85, 121));
            CCSprite sprite7 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 2, 121 * 1, 85, 121));
            CCSprite sprite8 = CCSprite.spriteWithFile("Images/grossini_dance_atlas", new CCRect(85 * 3, 121 * 1, 85, 121));

            CCSize s = CCDirector.sharedDirector().getWinSize();
            sprite1.position = new CCPoint((s.width / 5) * 1, (s.height / 3) * 1);
            sprite2.position = new CCPoint((s.width / 5) * 2, (s.height / 3) * 1);
            sprite3.position = new CCPoint((s.width / 5) * 3, (s.height / 3) * 1);
            sprite4.position = new CCPoint((s.width / 5) * 4, (s.height / 3) * 1);
            sprite5.position = new CCPoint((s.width / 5) * 1, (s.height / 3) * 2);
            sprite6.position = new CCPoint((s.width / 5) * 2, (s.height / 3) * 2);
            sprite7.position = new CCPoint((s.width / 5) * 3, (s.height / 3) * 2);
            sprite8.position = new CCPoint((s.width / 5) * 4, (s.height / 3) * 2);

            CCActionInterval action = CCFadeIn.actionWithDuration(2);
            CCActionInterval action_back = (CCActionInterval)action.reverse();
            CCAction fade = CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(action, action_back)));

            CCActionInterval tintred = CCTintBy.actionWithDuration(2, 0, -255, -255);
            CCActionInterval tintred_back = (CCActionInterval)tintred.reverse();
            CCAction red = CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(tintred, tintred_back)));

            CCActionInterval tintgreen = CCTintBy.actionWithDuration(2, -255, 0, -255);
            CCActionInterval tintgreen_back = (CCActionInterval)tintgreen.reverse();
            CCAction green = CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(tintgreen, tintgreen_back)));

            CCActionInterval tintblue = CCTintBy.actionWithDuration(2, -255, -255, 0);
            CCActionInterval tintblue_back = (CCActionInterval)tintblue.reverse();
            CCAction blue = CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(tintblue, tintblue_back)));

            sprite5.runAction(red);
            sprite6.runAction(green);
            sprite7.runAction(blue);
            sprite8.runAction(fade);

            // late add: test dirtyColor and dirtyPosition
            addChild(sprite1, 0, (int)kTagSprite.kTagSprite1);
            addChild(sprite2, 0, (int)kTagSprite.kTagSprite2);
            addChild(sprite3, 0, (int)kTagSprite.kTagSprite3);
            addChild(sprite4, 0, (int)kTagSprite.kTagSprite4);
            addChild(sprite5, 0, (int)kTagSprite.kTagSprite5);
            addChild(sprite6, 0, (int)kTagSprite.kTagSprite6);
            addChild(sprite7, 0, (int)kTagSprite.kTagSprite7);
            addChild(sprite8, 0, (int)kTagSprite.kTagSprite8);

            schedule(removeAndAddSprite, 2);
        }

        public void removeAndAddSprite(float dt)
        {
            CCSprite sprite = (CCSprite)(getChildByTag((int)kTagSprite.kTagSprite5));

            removeChild(sprite, false);
            addChild(sprite, 0, (int)kTagSprite.kTagSprite5);
        }

        public override string title()
        {
            return "Sprite: Color & Opacity";
        }
    }
}
