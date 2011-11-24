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

            //////////////////////////////
            // 1. super init first
            if (!base.init())
            {
                return false;
            }

            this.m_bIsTouchEnabled = true;
            CCSize size = CCDirector.sharedDirector().getWinSize();

            //CCMenuItemImage pCloseItem = CCMenuItemImage.itemFromNormalImage(
            //                                    "CloseNormal",
            //                                    "CloseSelected",
            //                                    this,
            //                                    new SEL_MenuHandler(menuCloseCallback));
            //pCloseItem.position = new CCPoint(size.width / 2, 50);
            //pCloseItem.anchorPoint = new CCPoint();

            //this.addChild(pCloseItem, 0);

            //pSprite = CCSprite.spriteWithFile("HelloWorld");
            //pSprite.position = new CCPoint(size.width / 2, 0);
            //pSprite.anchorPoint = new CCPoint(1f, 0.8f);
            //this.addChild(pSprite, 0);

            //CCLabelTTF label = CCLabelTTF.labelWithString("ActionsTest", "SpriteFont1", 30);
            //label.position = new CCPoint(10, size.height - 70);
            //CCMenuItemLabel labelMenu = CCMenuItemLabel.itemWithLabel(label, this, menuCloseCallback);


            //CCLabelTTF label1 = CCLabelTTF.labelWithString("TransitionsTest", "SpriteFont1", 30);
            //label1.position = new CCPoint(10, size.height - 140);
            //CCMenuItemLabel labelMenu1 = CCMenuItemLabel.itemWithLabel(label1, this, menuCloseCallback);

            //CCLabelTTF ProgressActiionsTest = CCLabelTTF.labelWithString("ProgressActiionsTest", "SpriteFont1", 30);
            //ProgressActiionsTest.position = new CCPoint(10, size.height - 210);
            //CCMenuItemLabel ProgressActiionsTestMenu = CCMenuItemLabel.itemWithLabel(ProgressActiionsTest, this, menuCloseCallback);

            CCLabelTTF EffectsTest = CCLabelTTF.labelWithString("EffectsTest", "SpriteFont1", 30);
            //EffectsTest.position = new CCPoint(10, size.height - 280);
            EffectsTest.position = new CCPoint(size.width / 2, size.height  / 2);
            //EffectsTest.anchorPoint = new CCPoint(0.5f, 0.5f);
            CCMenuItemLabel EffectsTestMenu = CCMenuItemLabel.itemWithLabel(EffectsTest, this, menuCloseCallback);
            EffectsTestMenu.position = new CCPoint(size.width / 2, size.height / 2);
            EffectsTestMenu.anchorPoint = new CCPoint(0.5f, 0.5f);

            EffectsTest.anchorPoint = new CCPoint(0.5f, 0.5f);

            //CCLabelTTF ClickAndMoveTest = CCLabelTTF.labelWithString("ClickAndMoveTest", "SpriteFont1", 30);
            //ClickAndMoveTest.position = new CCPoint(10, size.height - 350);
            //CCMenuItemLabel ClickAndMoveTestMenu = CCMenuItemLabel.itemWithLabel(ClickAndMoveTest, this, menuCloseCallback);
            //ClickAndMoveTestMenu.position = new CCPoint(10, size.height - 350);

            //CCLabelTTF RotateWorldTest = CCLabelTTF.labelWithString("RotateWorldTest", "SpriteFont1", 30);
            //RotateWorldTest.position = new CCPoint(10, size.height - 420);
            //CCMenuItemLabel RotateWorldTestMenu = CCMenuItemLabel.itemWithLabel(RotateWorldTest, this, menuCloseCallback);

            //CCLabelTTF ParticleTest = CCLabelTTF.labelWithString("ParticleTest", "SpriteFont1", 30);
            //ParticleTest.position = new CCPoint(10, size.height - 500);
            //CCMenuItemLabel ParticleTestMenu = CCMenuItemLabel.itemWithLabel(ParticleTest, this, menuCloseCallback);

            //CCMenu pMenu = CCMenu.menuWithItems(new CCMenuItem[] { ParticleTestMenu, labelMenu, labelMenu1, ProgressActiionsTestMenu, EffectsTestMenu, ClickAndMoveTestMenu, RotateWorldTestMenu });
            CCMenu pMenu = CCMenu.menuWithItems(new CCMenuItem[] { EffectsTestMenu });
            pMenu.isTouchEnabled = true;
            pMenu.position = new CCPoint(size.width / 2, size.height / 2);
            pMenu.anchorPoint = new CCPoint(0.5f, 0.5f);
            this.addChild(pMenu, 1);

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
