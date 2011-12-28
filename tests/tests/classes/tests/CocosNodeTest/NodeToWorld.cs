using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class NodeToWorld : TestCocosNodeDemo
    {
        public NodeToWorld()
        {
            //
            // This code tests that nodeToParent works OK:
            //  - It tests different anchor Points
            //  - It tests different children anchor points

            CCSprite back = CCSprite.spriteWithFile(TestResource.s_back3);
            addChild(back, -10);
            back.anchorPoint = (new CCPoint(0, 0));
            CCSize backSize = back.contentSize;

            CCMenuItem item = CCMenuItemImage.itemFromNormalImage(TestResource.s_PlayNormal, TestResource.s_PlaySelect);
            CCMenu menu = CCMenu.menuWithItems(item);
            menu.alignItemsVertically();
            menu.position = (new CCPoint(backSize.width / 2, backSize.height / 2));
            back.addChild(menu);

            CCActionInterval rot = CCRotateBy.actionWithDuration(5, 360);
            CCAction fe = CCRepeatForever.actionWithAction(rot);
            item.runAction(fe);

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(200, 0));
            CCActionInterval move_back = (CCActionInterval)move.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(move, move_back);
            CCAction fe2 = CCRepeatForever.actionWithAction((CCActionInterval)seq);
            back.runAction(fe2);
        }

        public override string title()
        {
            return "nodeToParent transform";
        }
    }
}
