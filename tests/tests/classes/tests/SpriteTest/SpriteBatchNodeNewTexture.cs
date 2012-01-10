using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeNewTexture : SpriteTestDemo
    {
        CCTexture2D m_texture1;
        CCTexture2D m_texture2;
        Random rand = new Random();

        public SpriteBatchNodeNewTexture()
        {
            isTouchEnabled = true;

            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 50);
            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            m_texture1 = batch.Texture;
            m_texture2 = CCTextureCache.sharedTextureCache().addImage("Images/grossini_dance_atlas-mono");

            for (int i = 0; i < 30; i++)
                addNewSprite();
        }

        public void addNewSprite()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCPoint p = new CCPoint((float)(rand.NextDouble() * s.width), (float)(rand.NextDouble() * s.height));

            CCSpriteBatchNode batch = (CCSpriteBatchNode)getChildByTag((int)kTags.kTagSpriteBatchNode);

            int idx = (int)(rand.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(x, y, 85, 121));
            batch.addChild(sprite);

            sprite.position = (new CCPoint(p.x, p.y));

            CCActionInterval action;
            float random = (float)rand.NextDouble();

            if (random < 0.20)
                action = CCScaleBy.actionWithDuration(3, 2);
            else if (random < 0.40)
                action = CCRotateBy.actionWithDuration(3, 360);
            else if (random < 0.60)
                action = CCBlink.actionWithDuration(1, 3);
            else if (random < 0.8)
                action = CCTintBy.actionWithDuration(2, 0, -255, -255);
            else
                action = CCFadeOut.actionWithDuration(2);
            CCActionInterval action_back = (CCActionInterval)action.reverse();
            CCActionInterval seq = (CCActionInterval)(CCSequence.actions(action, action_back));

            sprite.runAction(CCRepeatForever.actionWithAction(seq));
        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
            base.ccTouchesEnded(touches, event_);
        }

        public override string title()
        {
            return "SpriteBatchNode new texture (tap)";
        }
    }
}
