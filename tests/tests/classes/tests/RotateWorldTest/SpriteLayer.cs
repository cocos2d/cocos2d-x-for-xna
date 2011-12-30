using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteLayer : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        string s_pPathSister1 = "Images/grossinis_sister1";
        string s_pPathSister2 = "Images/grossinis_sister2";

        public override void onEnter()
        {
            base.onEnter();

            float x, y;

            CCSize size = CCDirector.sharedDirector().getWinSize();
            x = size.width;
            y = size.height;

            CCSprite sprite = CCSprite.spriteWithFile(s_pPathGrossini);
            CCSprite spriteSister1 = CCSprite.spriteWithFile(s_pPathSister1);
            CCSprite spriteSister2 = CCSprite.spriteWithFile(s_pPathSister2);

            sprite.scale = (1.5f);
            spriteSister1.scale = (1.5f);
            spriteSister2.scale = (1.5f);

            sprite.position = (new CCPoint(x / 2, y / 2));
            spriteSister1.position = (new CCPoint(40, y / 2));
            spriteSister2.position = (new CCPoint(x - 40, y / 2));

            CCAction rot = CCRotateBy.actionWithDuration(16, -3600);

            addChild(sprite);
            addChild(spriteSister1);
            addChild(spriteSister2);

            sprite.runAction(rot);

            CCActionInterval jump1 = CCJumpBy.actionWithDuration(4, new CCPoint(-400, 0), 100, 4);
            CCActionInterval jump2 = (CCActionInterval)jump1.reverse();

            CCActionInterval rot1 = CCRotateBy.actionWithDuration(4, 360 * 2);
            CCActionInterval rot2 = (CCActionInterval)rot1.reverse();

            spriteSister1.runAction(CCRepeat.actionWithAction(CCSequence.actions(jump2, jump1), 5));
            spriteSister2.runAction(CCRepeat.actionWithAction(CCSequence.actions((CCFiniteTimeAction)(jump1.copy()), (CCFiniteTimeAction)(jump2.copy())), 5));

            spriteSister1.runAction(CCRepeat.actionWithAction(CCSequence.actions(rot1, rot2), 5));
            spriteSister2.runAction(CCRepeat.actionWithAction(CCSequence.actions((CCFiniteTimeAction)(rot2.copy()), (CCFiniteTimeAction)(rot1.copy())), 5));
        }

        public static new SpriteLayer node()
        {
            SpriteLayer pNode = new SpriteLayer();
            return pNode;
        }
    }
}
