using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeChildren2 : SpriteTestDemo
    {
        public int CC_HONOR_PARENT_TRANSFORM_TRANSLATE = 1 << 0;
        public SpriteBatchNodeChildren2()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // parents
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);
            batch.Texture.generateMipmap();

            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");


            CCSprite sprite11 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite11.position = (new CCPoint(s.width / 3, s.height / 2));

            CCSprite sprite12 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite12.position = (new CCPoint(20, 30));
            sprite12.scale = 0.2f;

            CCSprite sprite13 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite13.position = (new CCPoint(-20, 30));
            sprite13.scale = 0.2f;

            batch.addChild(sprite11);
            sprite11.addChild(sprite12, -2);
            sprite11.addChild(sprite13, 2);

            // don't rotate with it's parent
            sprite12.honorParentTransform = ((ccHonorParentTransform)(sprite12.honorParentTransform & ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_TRANSLATE));

            // don't scale and rotate with it's parent
            sprite13.honorParentTransform = ((ccHonorParentTransform)(sprite13.honorParentTransform & (ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_SCALE | ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_ROTATE)));

            CCActionInterval action = CCMoveBy.actionWithDuration(2, new CCPoint(200, 0));
            CCActionInterval action_back = (CCActionInterval)action.reverse();
            CCActionInterval action_rot = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval action_s = CCScaleBy.actionWithDuration(2, 2);
            CCActionInterval action_s_back = (CCActionInterval)action_s.reverse();

            sprite11.runAction(CCRepeatForever.actionWithAction(action_rot));
            sprite11.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(action, action_back))));
            sprite11.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(action_s, action_s_back))));

            //
            // another set of parent / children
            //

            CCSprite sprite21 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite21.position = (new CCPoint(2 * s.width / 3, s.height / 2 - 50));

            CCSprite sprite22 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite22.position = (new CCPoint(20, 30));
            sprite22.scale = 0.8f;

            CCSprite sprite23 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite23.position = (new CCPoint(-20, 30));
            sprite23.scale = 0.8f;

            batch.addChild(sprite21);
            sprite21.addChild(sprite22, -2);
            sprite21.addChild(sprite23, 2);

            // don't rotate with it's parent
            sprite22.honorParentTransform = ((ccHonorParentTransform)(sprite22.honorParentTransform & ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_TRANSLATE));

            // don't scale and rotate with it's parent
            sprite23.honorParentTransform = ((ccHonorParentTransform)(sprite23.honorParentTransform & ccHonorParentTransform.CC_HONOR_PARENT_TRANSFORM_SCALE));

            sprite21.runAction(CCRepeatForever.actionWithAction(CCRotateBy.actionWithDuration(1, 360)));
            sprite21.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(CCScaleTo.actionWithDuration(0.5f, 5.0f), CCScaleTo.actionWithDuration(0.5f, 1)))));
        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeUnusedSpriteFrames();
        }

        public override string title()
        {
            return "SpriteBatchNode HonorTransform";
        }
    }
}
