using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class DispatcherTest : BugsTestBaseLayer
    {
        public DispatcherTest()
        {
        }

        public override bool init()
        {
            if (base.init())
            {
                CCSize size = CCDirector.sharedDirector().getWinSize();
                PauseCrashTestScene n = new PauseCrashTestScene();
                n.position = new CCPoint(0, 0);
                n.contentSize = new CCSize(size);
                this.addChild(n);
                n.init();
                return (true);
            }
            return (false);
        }
    }

    public delegate void ButtonClickDelegate(CCObject sender);

    public class Button : CCLayer
    {
        private ButtonClickDelegate _onClick;
        private CCNode _parent;
        private string _label;

        public Button(string label, CCNode parent, ButtonClickDelegate d)
        {
            _label = label;
            _parent = parent;
            _onClick = d;

            CCLabelTTF ttf = CCLabelTTF.labelWithString(label, "Arial", 32f);
            CCMenuItemLabel item = CCMenuItemLabel.itemWithLabel(ttf, null, new SEL_MenuHandler(d));
            CCMenu pMenu = CCMenu.menuWithItems(item);
            pMenu.position = new CCPoint(0, 0);
            addChild(pMenu);
        }
    }

    public class PauseCrashTestScene : CCLayer
    {
        private CCSize size;
        private CCLayerColor pauseLayer;
        private Button resumeButton;    // Button is just a custom class. Its just a layer with one menu button.

        public override bool init()
        {
            #region Default Code
            if (!base.init())
            {
                return false;
            }

            this.m_bIsTouchEnabled = true;
            size = CCDirector.sharedDirector().getWinSize();

            this.AddCloseButton();
            this.AddLabel();
            this.AddSpriteBackground();
            #endregion

            // create new Layer. Add a Button to it.
            pauseLayer = CCLayerColor.layerWithColorWidthHeight(new ccColor4B(100, 0, 0, 150), 800, 480);
            resumeButton = new Button("Tap to Resume", this, this.resumeButtonClicked);
            resumeButton.position = new CCPoint(400, 240);
            pauseLayer.addChild(resumeButton);

            this.isTouchEnabled = true;
            return true;
        }

        private void AddSpriteBackground()
        {
            CCSprite pSprite = CCSprite.spriteWithFile("Images\\HelloWorld");
            pSprite.position = new CCPoint(size.width / 2, size.height / 2);
            this.addChild(pSprite, 0);
        }

        private void AddLabel()
        {
            CCLabelTTF pLabel = CCLabelTTF.labelWithString("Images\\Hello World", "Arial", 24);
            pLabel.position = new CCPoint(size.width / 2, size.height - 50);
            this.addChild(pLabel, 1);
        }

        private void AddCloseButton()
        {
            CCMenuItemImage pCloseItem = CCMenuItemImage.itemFromNormalImage(
                                                "Images\\close",
                                                "Images\\close",
                                                this,
                                                new SEL_MenuHandler(menuCloseCallback));
            pCloseItem.position = new CCPoint(CCDirector.sharedDirector().getWinSize().width - 20, 20);

            CCMenu pMenu = CCMenu.menuWithItems(pCloseItem);
            pMenu.position = new CCPoint(0, 0);
            this.addChild(pMenu, 1);
        }

        // lauch a Pause screen.
        public virtual void menuCloseCallback(CCObject pSender)
        {
            this.isTouchEnabled = false;    // causes game crash when added
            CCScene scene = CCDirector.sharedDirector().runningScene;
//            CCScene scene = (CCScene)this.parent;
            scene.addChild(pauseLayer, 200);
            CCDirector.sharedDirector().pause();
        }

        public virtual void resumeButtonClicked(CCObject pSender)
        {
            CCDirector.sharedDirector().resume();
            CCScene scene = CCDirector.sharedDirector().runningScene;
//            CCScene scene = (CCScene)this.parent;
            scene.removeChild(pauseLayer, true);
            this.isTouchEnabled = true;  // causes game crash when added
        }


        public override void onEnter()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, true);
            base.onEnter();
        }

        public override void onExit()
        {
            CCTouchDispatcher.sharedDispatcher().removeDelegate(this);
        }

        public override bool ccTouchBegan(CCTouch touch, CCEvent event_)
        {
            //return base.ccTouchBegan(touch, event_);

            // do something
            return true;
        }

        public override void ccTouchEnded(CCTouch touch, CCEvent event_)
        {
            // base.ccTouchEnded(touch, event_);
            // do something.
        }
    }
}
