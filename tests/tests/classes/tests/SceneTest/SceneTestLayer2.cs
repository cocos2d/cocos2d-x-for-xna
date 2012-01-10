using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;

namespace tests
{
    public class SceneTestLayer2 : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        float m_timeCounter;

        public SceneTestLayer2()
        {
            m_timeCounter = 0;

            CCMenuItemFont item1 = CCMenuItemFont.itemFromString("replaceScene", this, onReplaceScene);
            CCMenuItemFont item2 = CCMenuItemFont.itemFromString("replaceScene w/transition", this, onReplaceSceneTran);
            CCMenuItemFont item3 = CCMenuItemFont.itemFromString("Go Back", this, onGoBack);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);
            menu.alignItemsVertically();

            addChild(menu);

            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCSprite sprite = CCSprite.spriteWithFile(s_pPathGrossini);
            addChild(sprite);
            sprite.position = new CCPoint(s.width - 40, s.height / 2);
            CCActionInterval rotate = CCRotateBy.actionWithDuration(2, 360);
            CCAction repeat = CCRepeatForever.actionWithAction(rotate);
            sprite.runAction(repeat);

            schedule(testDealloc);
        }

        public void testDealloc(float dt)
        {
            //m_timeCounter += dt;
            //if( m_timeCounter > 10 )
            //	onReplaceScene(this);
        }

        public void onGoBack(CCObject pSender)
        {
            CCDirector.sharedDirector().popScene();
        }

        public void onReplaceScene(CCObject pSender)
        {
            CCScene pScene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer3();
            pScene.addChild(pLayer, 0);
            CCDirector.sharedDirector().replaceScene(pScene);
        }

        public void onReplaceSceneTran(CCObject pSender)
        {
            CCScene pScene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer3();
            pScene.addChild(pLayer, 0);
            CCDirector.sharedDirector().replaceScene(pScene);
            //(CCTransitionFlipX.transitionWithDuration(2, pScene));
        }

        //CREATE_NODE(SceneTestLayer2);
    }
}
