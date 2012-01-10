using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeChildren : SpriteTestDemo
    {
        public SpriteBatchNodeChildren()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // parents
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini", 50);

            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/grossini");

            CCSprite sprite1 = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite1.position = (new CCPoint(s.width / 3, s.height / 2));

            CCSprite sprite2 = CCSprite.spriteWithSpriteFrameName("grossini_dance_02.png");
            sprite2.position = (new CCPoint(50, 50));

            CCSprite sprite3 = CCSprite.spriteWithSpriteFrameName("grossini_dance_03.png");
            sprite3.position = (new CCPoint(-50, -50));

            batch.addChild(sprite1);
            sprite1.addChild(sprite2);
            sprite1.addChild(sprite3);

            // BEGIN NEW CODE
            List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>();
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                string temp = "";
                if (i<10)
                {
                    temp = "0" + i;
                }
                else
                {
                    temp = i.ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = CCSpriteFrameCache.sharedSpriteFrameCache().spriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.animationWithFrames(animFrames, 0.2f);
            sprite1.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithAnimation(animation, false)));
            // END NEW CODE

            CCActionInterval action = CCMoveBy.actionWithDuration(2, new CCPoint(200, 0));
            CCActionInterval action_back = (CCActionInterval)action.reverse();
            CCActionInterval action_rot = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval action_s = CCScaleBy.actionWithDuration(2, 2);
            CCActionInterval action_s_back = (CCActionInterval)action_s.reverse();

            CCActionInterval seq2 = (CCActionInterval)action_rot.reverse();
            sprite2.runAction(CCRepeatForever.actionWithAction(seq2));

            sprite1.runAction((CCAction)(CCRepeatForever.actionWithAction(action_rot)));
            sprite1.runAction((CCAction)(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(action, action_back)))));
            sprite1.runAction((CCAction)(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(action_s, action_s_back)))));

        }

        public override void onExit()
        {
            base.onExit();
            CCSpriteFrameCache.sharedSpriteFrameCache().removeUnusedSpriteFrames();
        }

        public override string title()
        {
            return "SpriteBatchNode Grand Children";
        }
    }
}
