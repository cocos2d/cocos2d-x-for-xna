using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNode1 : SpriteTestDemo
    {
        public Random rand = new Random();
        public SpriteBatchNode1()
        {
            isTouchEnabled = true;

            CCSpriteBatchNode BatchNode = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 50);
            addChild(BatchNode, 0, (int)kTags.kTagSpriteBatchNode);

            CCSize s = CCDirector.sharedDirector().getWinSize();
            addNewSpriteWithCoords(new CCPoint(s.width / 2, s.height / 2));
        }

        public void addNewSpriteWithCoords(CCPoint p)
        {
            CCSpriteBatchNode BatchNode = (CCSpriteBatchNode)getChildByTag((int)kTags.kTagSpriteBatchNode);

            int idx = (int)(rand.NextDouble() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCSprite sprite = CCSprite.spriteWithTexture(BatchNode.Texture, new CCRect(x, y, 85, 121));
            BatchNode.addChild(sprite);

            sprite.position = (new CCPoint(p.x, p.y));

            CCActionInterval action = null;
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
            foreach (CCTouch item in touches)
            {
                if (item == null)
                {
                    break;
                }
                CCPoint location = item.locationInView(item.view());

                location = CCDirector.sharedDirector().convertToGL(location);

                addNewSpriteWithCoords(location);
            }
            base.ccTouchesEnded(touches, event_);
        }

        public override string title()
        {
            return "SpriteBatchNode (tap screen)";
        }
    }
}
