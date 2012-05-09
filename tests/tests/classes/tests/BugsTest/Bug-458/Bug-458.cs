using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug458Layer : BugsTestBaseLayer
    {
        public override bool init()
        {
            if (base.init())
            {
                // ask director the the window size
                CCSize size = CCDirector.sharedDirector().getWinSize();

                QuestionContainerSprite question = new QuestionContainerSprite();
                QuestionContainerSprite question2 = new QuestionContainerSprite();
                question.init();
                question2.init();

                //		[question setContentSize:CGSizeMake(50,50)];
                //		[question2 setContentSize:CGSizeMake(50,50)];

                CCMenuItemSprite sprite = CCMenuItemSprite.itemFromNormalSprite(question2, question, this, selectAnswer);
                CCLayerColor layer = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(0, 0, 255, 255), 100, 100);


                CCLayerColor layer2 = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(255, 0, 0, 255), 100, 100);
                CCMenuItemSprite sprite2 = CCMenuItemSprite.itemFromNormalSprite(layer, layer2, this, selectAnswer);
                CCMenu menu = CCMenu.menuWithItems(sprite, sprite2, null);
                menu.alignItemsVerticallyWithPadding(100);
                menu.position = new CCPoint(size.width / 2, size.height / 2);

                // add the label as a child to this Layer
                addChild(menu);

                return true;
            }
            return false;
        }

        public void selectAnswer(CCObject sender)
        {
            CCLog.Log("Selected");
        }
    }
}
