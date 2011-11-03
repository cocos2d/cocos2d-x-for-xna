using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace HelloCocos2d
{
    public class HelloCocos2dScene : CCLayer
    {
        /// <summary>
        ///  Here's a difference. Method 'init' in cocos2d-x returns bool, instead of returning 'id' in cocos2d-iphone
        /// </summary>
        public bool init()
        {
            //////////////////////////////
            // 1. super init first
            if (!base.init())
            {
                return false;
            }

            /////////////////////////////
            // 2. add a menu item with "X" image, which is clicked to quit the program
            //    you may modify it.

            // add a "close" icon to exit the progress. it's an autorelease object
            //CCMenuItemImage *pCloseItem = CCMenuItemImage::itemFromNormalImage(
            //                                    "CloseNormal.png",
            //                                    "CloseSelected.png",
            //                                    this,
            //                                    menu_selector(HelloWorld::menuCloseCallback) );
            //pCloseItem->setPosition( ccp(CCDirector::sharedDirector()->getWinSize().width - 20, 20) );

            // create menu, it's an autorelease object
            //CCMenu* pMenu = CCMenu::menuWithItems(pCloseItem, NULL);
            //pMenu->setPosition( CCPointZero );
            //this->addChild(pMenu, 1);

            /////////////////////////////
            // 3. add your codes below...

            // add a label shows "Hello World"
            // create and initialize a label
            //CCLabelTTF* pLabel = CCLabelTTF::labelWithString("Hello World", "Arial", 24);
            // ask director the window size
            // CCSize size = CCDirector.sharedDirector().getWinSize();

            // position the label on the center of the screen
            //pLabel->setPosition( ccp(size.width / 2, size.height - 50) );

            // add the label as a child to this layer
            //this->addChild(pLabel, 1);

            // add "HelloWorld" splash screen"
            // CCSprite pSprite = CCSprite.spriteWithFile("HelloWorld");

            // position the sprite on the center of the screen
            // pSprite.position = new CCPoint(size.width / 2, size.height / 2);

            // add the sprite as a child to this layer
            // this.addChild(pSprite, 0);

            return true;
        }

        /// <summary>
        /// there's no 'id' in cpp, so we recommand to return the exactly class pointer
        /// </summary>
        public static CCScene scene()
        {
            // 'scene' is an autorelease object
            CCScene scene = CCScene.node();

            // 'layer' is an autorelease object
            CCLayer layer = HelloCocos2dScene.node();

            // add layer as a child to scene
            scene.addChild(layer);


            CCSize size = CCDirector.sharedDirector().getWinSize();

            // position the label on the center of the screen
            //pLabel->setPosition( ccp(size.width / 2, size.height - 50) );

            // add the label as a child to this layer
            //this->addChild(pLabel, 1);

            // add "HelloWorld" splash screen"
            CCSprite pSprite = CCSprite.spriteWithFile("HelloWorld");

            // position the sprite on the center of the screen
            pSprite.position = new CCPoint(size.width / 2, size.height / 2);

            // add the sprite as a child to this layer
            layer.addChild(pSprite, 0);

            // return the scene
            return scene;
        }

        public static new CCLayer node()
        {
            HelloCocos2dScene ret = new HelloCocos2dScene();
            if (ret.init())
            {
                return ret;
            }
            else
            {
                ret = null;
            }

            return ret;
        }
        /// <summary>
        /// a selector callback
        /// </summary>
        /// <param name="pSender"></param>
        public virtual void menuCloseCallback(CCObject pSender)
        {
            CCDirector.sharedDirector().end();
        }
    }
}
