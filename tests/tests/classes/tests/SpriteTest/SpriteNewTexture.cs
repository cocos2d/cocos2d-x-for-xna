using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteNewTexture : SpriteTestDemo
    {
        bool m_usingTexture1;
        CCTexture2D m_texture1;
        CCTexture2D m_texture2;

        public SpriteNewTexture()
        {
            base.isTouchEnabled = true;

            CCNode node = CCNode.node();
            addChild(node, 0, (int)kTags.kTagSpriteBatchNode);

            m_texture1 = CCTextureCache.sharedTextureCache().addImage("Images/grossini_dance_atlas");
            m_texture2 = CCTextureCache.sharedTextureCache().addImage("Images/grossini_dance_atlas-mono");

            m_usingTexture1 = true;

            for (int i = 0; i < 30; i++)
                addNewSprite();
        }

        public static Random rand = new Random();

        public void addNewSprite()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCPoint p = new CCPoint((float)(rand.NextDouble() * s.width), (float)(rand.NextDouble() * s.height));

            int idx = (int)(rand.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCNode node = getChildByTag((int)kTags.kTagSpriteBatchNode);
            CCSprite sprite = CCSprite.spriteWithTexture(m_texture1, new CCRect(x, y, 85, 121));
            node.addChild(sprite);

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
            CCNode node = getChildByTag((int)kTags.kTagSpriteBatchNode);

            List<CCNode> children = node.children;
            CCSprite sprite;
            if (m_usingTexture1)                          //--> win32 : Let's it make just simple sentence
            {
                foreach (var item in children)
                {
                    sprite = (CCSprite)item;
                    if (sprite == null)
                        break;

                    sprite.Texture = m_texture2;
                }

                m_usingTexture1 = false;
            }
            else
            {
                foreach (var item in children)
                {
                    sprite = (CCSprite)item;
                    if (sprite == null)
                        break;

                    sprite.Texture = m_texture1;
                }

                m_usingTexture1 = true;
            }
        }

        public override string title()
        {
            return "Sprite New texture (tap)";
        }
    }
}
