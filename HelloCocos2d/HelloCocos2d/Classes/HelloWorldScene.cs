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

            this.m_bIsTouchEnabled = true;

            /////////////////////////////
            // 2. add a menu item with "X" image, which is clicked to quit the program
            //    you may modify it.

            // add a "close" icon to exit the progress. it's an autorelease object
            CCMenuItemImage pCloseItem = CCMenuItemImage.itemFromNormalImage(
                                                "CloseNormal",
                                                "CloseSelected",
                                                this,
                                                new SEL_MenuHandler(menuCloseCallback));

            pCloseItem.position = new CCPoint(CCDirector.sharedDirector().getWinSize().width - 20, 20);

            // create menu, it's an autorelease object
            CCMenu pMenu = CCMenu.menuWithItems(pCloseItem);
            pMenu.position = new CCPoint(0, 0);
            this.addChild(pMenu, 1);

            /////////////////////////////
            // 3. add your codes below...

            // add a label shows "Hello World"
            // create and initialize a label
            //CCLabelTTF* pLabel = CCLabelTTF::labelWithString("Hello World", "Arial", 24);
            // ask director the window size
            CCSize size = CCDirector.sharedDirector().getWinSize();

            // position the label on the center of the screen
            //pLabel->setPosition( ccp(size.width / 2, size.height - 50) );

            // add the label as a child to this layer
            //this->addChild(pLabel, 1);

            // add "HelloWorld" splash screen"
            pSprite = CCSprite.spriteWithFile("HelloWorld");

            // position the sprite on the center of the screen
            pSprite.position = new CCPoint(size.width / 2, size.height / 2);

            // add the sprite as a child to this layer
            this.addChild(pSprite, 0);

            ///@test action test
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCActionInterval actionTo = CCMoveTo.actionWithDuration(3, new CCPoint(s.width - 40, s.height - 40));
            pSprite.runAction(actionTo);

            return true;
        }
        CCSprite pSprite;
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

        public override void ccTouchesBegan(List<CCTouch> touches, CCEvent event_)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCActionInterval actionTo = CCMoveTo.actionWithDuration(3, new CCPoint(-s.width, -s.height));
            pSprite.runAction(actionTo);
        }
    }
}
