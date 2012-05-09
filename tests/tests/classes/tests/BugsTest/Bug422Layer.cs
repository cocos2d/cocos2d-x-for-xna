using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug422Layer : BugsTestBaseLayer
    {
        public override bool init()
        {
            if (base.init())
            {
                reset();
                return true;
            }

            return false;
        }

        public void reset()
        {
            Random random = new Random();
            int localtag = 0;
            localtag++;

            // TO TRIGGER THE BUG:
            // remove the itself from parent from an action
            // The menu will be removed, but the instance will be alive
            // and then a new node will be allocated occupying the memory.
            // => CRASH BOOM BANG
            CCNode node = getChildByTag(localtag - 1);
            CCLog.Log("Menu: %p", node);
            removeChild(node, false);
            //	[self removeChildByTag:localtag-1 cleanup:NO];

            CCMenuItem item1 = CCMenuItemFont.itemFromString("One", this, menuCallback);
            CCLog.Log("MenuItemFont: %p", item1);
            CCMenuItem item2 = CCMenuItemFont.itemFromString("Two", this, menuCallback);
            CCMenu menu = CCMenu.menuWithItems(item1, item2);
            menu.alignItemsVertically();

            float x = random.Next() * 50;
            float y = random.Next() * 50;
            menu.position = CCPointExtension.ccpAdd(menu.position, new CCPoint(x, y));
            addChild(menu, 0, localtag);

            //[self check:self];
        }

        public void check(CCNode t)
        {
            //List<CCNode> array = t.children;
            //CCObject pChild = null;
            //foreach (var array in pChild)
            //{
            //     //CC_BREAK_IF(! pChild);
            //    CCNode pNode = (CCNode)pChild;
            //    //CCLog.Log("%p, rc: %d", pNode, pNode.retainCount());
            //    check(pNode);
            //}

            //CCArray *array = t->getChildren();
            //CCObject* pChild = NULL;
            //CCARRAY_FOREACH(array, pChild)
            //{
            //    CC_BREAK_IF(! pChild);
            //    CCNode* pNode = (CCNode*) pChild;
            //    CCLog("%p, rc: %d", pNode, pNode->retainCount());
            //    check(pNode);
            //}
            throw new NotFiniteNumberException();
            
        }

        public void menuCallback(CCObject sender)
        {
            reset();
        }
    }
}
