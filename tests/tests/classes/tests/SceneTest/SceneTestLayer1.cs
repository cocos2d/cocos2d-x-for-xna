using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;
using System.Diagnostics;

namespace tests
{
    public class SceneTestLayer1 : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";

        public SceneTestLayer1()
        {
            CCMenuItemFont item1 = CCMenuItemFont.itemFromString("Test pushScene", this, onPushScene);
            CCMenuItemFont item2 = CCMenuItemFont.itemFromString("Test pushScene w/transition", this, onPushSceneTran);
            CCMenuItemFont item3 = CCMenuItemFont.itemFromString("Quit", this, onQuit);

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

        public override void onEnter()
        {
            Debug.WriteLine("SceneTestLayer1#onEnter");
            base.onEnter();
        }

        public override void onEnterTransitionDidFinish()
        {
            Debug.WriteLine("SceneTestLayer1#onEnterTransitionDidFinish");
            base.onEnterTransitionDidFinish();
        }

        public void testDealloc(float dt)
        {
            //UXLOG("SceneTestLayer1:testDealloc");    
        }

        public void onPushScene(CCObject pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.addChild(pLayer, 0);
            CCDirector.sharedDirector().pushScene(scene);
        }

        public void onPushSceneTran(CCObject pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.addChild(pLayer, 0);

            CCDirector.sharedDirector().pushScene(scene);
            //(CCTransitionSlideInT.transitionWithDuration(1f, scene));
        }

        public void onQuit(CCObject pSender) 
        {
            //getCocosApp()->exit();
            //CCDirector::sharedDirector()->popScene();

            //// HA HA... no more terminate on sdk v3.0
            //// http://developer.apple.com/iphone/library/qa/qa2008/qa1561.html
            //if( [[UIApplication sharedApplication] respondsToSelector:@selector(terminate)] )
            //	[[UIApplication sharedApplication] performSelector:@selector(terminate)];
        }
    }
}
