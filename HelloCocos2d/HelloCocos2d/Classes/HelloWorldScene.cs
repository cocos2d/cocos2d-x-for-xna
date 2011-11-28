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
        public override bool init()
        {
            CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;
            if (!base.init())
            {
                return false;
            }

            this.m_bIsTouchEnabled = true;
            CCSize size = CCDirector.sharedDirector().getWinSize();

            CCMenuItemImage pCloseItem = CCMenuItemImage.itemFromNormalImage("CloseNormal", "CloseSelected", this, new SEL_MenuHandler(menuCloseCallback));
            pCloseItem.position = new CCPoint(0, 0);
            pCloseItem.anchorPoint = new CCPoint(0, 0);

            CCLabelTTF EffectsTest = CCLabelTTF.labelWithString("EffectsTest", "SpriteFont1", 30);
            EffectsTest.position = new CCPoint(size.width / 2, size.height / 2);

            CCMenuItemLabel EffectsTestMenu = CCMenuItemLabel.itemWithLabel(EffectsTest, this, menuCloseCallback);
            EffectsTestMenu.position = new CCPoint(size.width / 2, size.height / 2);
            EffectsTestMenu.anchorPoint = new CCPoint(0.0f, 0.0f);

            CCMenu pMenu = CCMenu.menuWithItems(new CCMenuItem[] { EffectsTestMenu, pCloseItem });
            //pMenu.position = new CCPoint(0.1f, 0.1f);
            this.addChild(pMenu, 1);

            //pSprite = CCSprite.spriteWithFile("HelloWorld");
            //pSprite.position = new CCPoint(size.width / 2, size.height / 2);
            //pSprite.anchorPoint = new CCPoint(0f, 0f);
            //this.addChild(pSprite, 0);

            ///@test action test
            //CCSize s = CCDirector.sharedDirector().getWinSize();
            //CCActionInterval actionTo = CCMoveTo.actionWithDuration(3, new CCPoint(s.width - 40, s.height - 40));
            //pSprite.runAction(actionTo);

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
            CCApplication.sharedApplication().Game.Exit();
        }

        public override void ccTouchesBegan(List<CCTouch> touches, CCEvent event_)
        {
            //CCSize s = CCDirector.sharedDirector().getWinSize();
            //CCActionInterval actionTo = CCMoveTo.actionWithDuration(3, new CCPoint(-s.width, -s.height));
            //pSprite.runAction(actionTo);
        }
    }
}
