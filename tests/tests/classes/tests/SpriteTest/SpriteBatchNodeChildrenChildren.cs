using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeChildrenChildren : SpriteTestDemo
    {
        public SpriteBatchNodeChildrenChildren()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("animations/ghosts");

            CCSpriteBatchNode aParent;
            CCSprite l1, l2a, l2b, l3a1, l3a2, l3b1, l3b2;
            CCActionInterval rot = CCRotateBy.actionWithDuration(10, 360);
            CCAction seq = CCRepeatForever.actionWithAction(rot);

            CCActionInterval rot_back = (CCActionInterval)rot.reverse();
            CCAction rot_back_fe = CCRepeatForever.actionWithAction(rot_back);

            //
            // SpriteBatchNode: 3 levels of children
            //

            aParent = CCSpriteBatchNode.batchNodeWithFile("animations/images/ghosts");
            aParent.Texture.generateMipmap();
            addChild(aParent);

            // parent
            l1 = CCSprite.spriteWithSpriteFrameName("father.gif");
            l1.position = new CCPoint(s.width / 2, s.height / 2);
            l1.runAction((CCAction)(seq.copy()));
            aParent.addChild(l1);
            CCSize l1Size = l1.contentSize;

            // child left
            l2a = CCSprite.spriteWithSpriteFrameName("sister1.gif");
            l2a.position = (new CCPoint(-50 + l1Size.width / 2, 0 + l1Size.height / 2));
            l2a.runAction((CCAction)(rot_back_fe.copy()));
            l1.addChild(l2a);
            CCSize l2aSize = l2a.contentSize;

            // child right
            l2b = CCSprite.spriteWithSpriteFrameName("sister2.gif");
            l2b.position = (new CCPoint(+50 + l1Size.width / 2, 0 + l1Size.height / 2));
            l2b.runAction((CCAction)(rot_back_fe.copy()));
            l1.addChild(l2b);
            CCSize l2bSize = l2a.contentSize;


            // child left bottom
            l3a1 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3a1.scale = 0.45f;
            l3a1.position = (new CCPoint(0 + l2aSize.width / 2, -100 + l2aSize.height / 2));
            l2a.addChild(l3a1);

            // child left top
            l3a2 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3a2.scale = 0.45f;
            l3a1.position = (new CCPoint(0 + l2aSize.width / 2, +100 + l2aSize.height / 2));
            l2a.addChild(l3a2);

            // child right bottom
            l3b1 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3b1.scale = 0.45f;
            l3b1.IsFlipY = true;
            l3b1.position = new CCPoint(0 + l2bSize.width / 2, -100 + l2bSize.height / 2);
            l2b.addChild(l3b1);

            // child right top
            l3b2 = CCSprite.spriteWithSpriteFrameName("child1.gif");
            l3b2.scale = 0.45f;
            l3b2.IsFlipY = true;
            l3b1.position = new CCPoint(0 + l2bSize.width / 2, +100 + l2bSize.height / 2);
            l2b.addChild(l3b2);
        }

        public override string title()
        {
            return "SpriteBatchNode multiple levels of children";
        }
    }
}
