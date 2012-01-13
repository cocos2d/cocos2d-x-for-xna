using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteFrameAliasNameTest : SpriteTestDemo
    {
        public override void onEnter()
        {
            base.onEnter();
            CCSize s = CCDirector.sharedDirector().getWinSize();

            // IMPORTANT:
            // The sprite frames will be cached AND RETAINED, and they won't be released unless you call
            //     [[CCSpriteFrameCache sharedSpriteFrameCache] removeUnusedSpriteFrames];
            //
            // CCSpriteFrameCache is a cache of CCSpriteFrames
            // CCSpriteFrames each contain a texture id and a rect (frame).

            CCSpriteFrameCache cache = CCSpriteFrameCache.sharedSpriteFrameCache();
            cache.addSpriteFramesWithFile("animations/grossini-aliases", "animations/images/grossini-aliases");

            //
            // Animation using Sprite batch
            //
            // A CCSpriteBatchNode can reference one and only one texture (one .png file)
            // Sprites that are contained in that texture can be instantiatied as CCSprites and then added to the CCSpriteBatchNode
            // All CCSprites added to a CCSpriteBatchNode are drawn in one OpenGL ES draw call
            // If the CCSprites are not added to a CCSpriteBatchNode then an OpenGL ES draw call will be needed for each one, which is less efficient
            //
            // When you animate a sprite, CCAnimation changes the frame of the sprite using setDisplayFrame: (this is why the animation must be in the same texture)
            // When setDisplayFrame: is used in the CCAnimation it changes the frame to one specified by the CCSpriteFrames that were added to the animation,
            // but texture id is still the same and so the sprite is still a child of the CCSpriteBatchNode, 
            // and therefore all the animation sprites are also drawn as part of the CCSpriteBatchNode
            //

            CCSprite sprite = CCSprite.spriteWithSpriteFrameName("grossini_dance_01.png");
            sprite.position = (new CCPoint(s.width * 0.5f, s.height * 0.5f));

            CCSpriteBatchNode spriteBatch = CCSpriteBatchNode.batchNodeWithFile("animations/images/grossini-aliases");
            spriteBatch.addChild(sprite);
            addChild(spriteBatch);

            List<CCSpriteFrame> animFrames = new List<CCSpriteFrame>(15);
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
                // Obtain frames by alias name
                str = string.Format("dance_{0}", temp);
                CCSpriteFrame frame = cache.spriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.animationWithFrames(animFrames);
            // 14 frames * 1sec = 14 seconds
            sprite.runAction(CCRepeatForever.actionWithAction(CCAnimate.actionWithDuration(14.0f, animation, false)));
        }
        public override string title()
        {
            return "SpriteFrame Alias Name";
        }
        public override string subtitle()
        {
            return "SpriteFrames are obtained using the alias name";
        }
    }
}
