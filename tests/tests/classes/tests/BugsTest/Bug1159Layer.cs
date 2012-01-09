using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Bug1159Layer : BugsTestBaseLayer
    {
        public override bool init()
        {
            if (base.init())
            {
                CCSize s = CCDirector.sharedDirector().getWinSize();

                CCLayerColor background = CCLayerColor.layerWithColor(new ccColor4B(255, 0, 255, 255));
                addChild(background);

                CCLayerColor sprite_a = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(255, 0, 0, 255), 700, 700);
                sprite_a.anchorPoint = new CCPoint(0.5f, 0.5f);
                sprite_a.isRelativeAnchorPoint = true;
                sprite_a.position = new CCPoint(0.0f, s.height / 2);
                addChild(sprite_a);

                sprite_a.runAction(CCRepeatForever.actionWithAction((CCActionInterval)CCSequence.actions(
                                                                       CCMoveTo.actionWithDuration(1.0f, new CCPoint(1024.0f, 384.0f)),
                                                                       CCMoveTo.actionWithDuration(1.0f, new CCPoint(0.0f, 384.0f)))));

                CCLayerColor sprite_b = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(0, 0, 255, 255), 400, 400);
                sprite_b.anchorPoint = new CCPoint(0.5f, 0.5f);
                sprite_b.isRelativeAnchorPoint = true;
                sprite_b.position = new CCPoint(s.width / 2, s.height / 2);
                addChild(sprite_b);

                CCMenuItemLabel label = CCMenuItemLabel.itemWithLabel(CCLabelTTF.labelWithString("Flip Me", "Helvetica", 24), this, callBack);
                CCMenu menu = CCMenu.menuWithItems(label);
                menu.position = new CCPoint(s.width - 200.0f, 50.0f);
                addChild(menu);

                return true;
            }

            return false;
        }

        public static CCScene scene()
        {
            CCScene pScene = CCScene.node();
            //Bug1159Layer layer = Bug1159Layer.node();
            //pScene.addChild(layer);

            return pScene;
        }

        public void callBack(CCObject pSender)
        {
            CCDirector.sharedDirector().replaceScene(CCTransitionPageTurn.transitionWithDuration(1.0f, Bug1159Layer.scene(), false));
        }

        //LAYER_NODE_FUNC(Bug1159Layer);
    }
}
