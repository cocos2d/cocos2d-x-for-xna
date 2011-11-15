using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{

    //static TestScene* CreateTestScene(int nIdx)
    //{
    //    CCDirector::sharedDirector()->purgeCachedData();

    //    TestScene* pScene = NULL;

    //    switch (nIdx)
    //    {
    //    case TEST_ACTIONS:
    //        pScene = new ActionsTestScene(); break;
    //    case TEST_TRANSITIONS:
    //        pScene = new TransitionsTestScene(); break;
    //    case TEST_PROGRESS_ACTIONS:
    //        pScene = new ProgressActionsTestScene(); break;
    //    case TEST_EFFECTS:
    //        pScene = new EffectTestScene(); break;
    //    case TEST_CLICK_AND_MOVE:
    //        pScene = new ClickAndMoveTestScene(); break;
    //    case TEST_ROTATE_WORLD:
    //        pScene = new RotateWorldTestScene(); break;
    //    case TEST_PARTICLE:
    //        pScene = new ParticleTestScene(); break;
    //    case TEST_EASE_ACTIONS:
    //        pScene = new EaseActionsTestScene(); break;
    //    case TEST_MOTION_STREAK:
    //        pScene = new MotionStreakTestScene(); break;
    //    case TEST_DRAW_PRIMITIVES:
    //        pScene = new DrawPrimitivesTestScene(); break;
    //    case TEST_COCOSNODE:
    //        pScene = new CocosNodeTestScene(); break;
    //    case TEST_TOUCHES:
    //        pScene = new PongScene(); break;
    //    case TEST_MENU:
    //        pScene = new MenuTestScene(); break;
    //    case TEST_ACTION_MANAGER:
    //        pScene = new ActionManagerTestScene(); break;
    //    case TEST_LAYER:
    //        pScene = new LayerTestScene(); break;
    //    case TEST_SCENE:
    //        pScene = new SceneTestScene(); break;
    //    case TEST_PARALLAX:
    //        pScene = new ParallaxTestScene(); break;
    //    case TEST_TILE_MAP:
    //        pScene = new TileMapTestScene(); break;
    //    case TEST_INTERVAL:
    //        pScene = new IntervalTestScene(); break;
    //    case TEST_CHIPMUNK:
    //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
    //        pScene = new ChipmunkTestScene(); break;
    //#else
    //#ifdef AIRPLAYUSECHIPMUNK
    //#if	(AIRPLAYUSECHIPMUNK == 1)
    //        pScene = new ChipmunkTestScene(); break;
    //#endif
    //#endif
    //#endif
    //    case TEST_LABEL:
    //        pScene = new AtlasTestScene(); break;
    //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
    //    case TEST_TEXT_INPUT:
    //        pScene = new TextInputTestScene(); break;
    //#endif
    //    case TEST_SPRITE:
    //        pScene = new SpriteTestScene(); break;
    //    case TEST_SCHEDULER:
    //        pScene = new SchedulerTestScene(); break;
    //    case TEST_RENDERTEXTURE:
    //        pScene = new RenderTextureScene(); break;
    //    case TEST_TEXTURE2D:
    //        pScene = new TextureTestScene(); break;
    //    case TEST_BOX2D:
    //        pScene = new Box2DTestScene(); break;
    //    case TEST_BOX2DBED:
    //        pScene = new Box2dTestBedScene(); break;
    //    case TEST_EFFECT_ADVANCE:
    //        pScene = new EffectAdvanceScene(); break;
    //    case TEST_HIRES:
    //        pScene = new HiResTestScene(); break;
    //#if (CC_TARGET_PLATFORM != CC_PLATFORM_WIN32)
    //    case TEST_ACCELEROMRTER:
    //        pScene = new AccelerometerTestScene(); break;
    //#endif
    //    case TEST_KEYPAD:
    //        pScene = new KeypadTestScene(); break;
    //    case TEST_COCOSDENSHION:
    //        pScene = new CocosDenshionTestScene(); break;
    //    case TEST_PERFORMANCE:
    //        pScene = new PerformanceTestScene(); break;
    //    case TEST_ZWOPTEX:
    //        pScene = new ZwoptexTestScene(); break;
    //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
    //    case TEST_CURL:
    //        pScene = new CurlTestScene(); break;
    //    case TEST_USERDEFAULT:
    //        pScene = new UserDefaultTestScene(); break;
    //#endif
    //    case TEST_DIRECTOR:
    //        pScene = new DirectorTestScene(); break;
    //    case TEST_BUGS:
    //        pScene = new BugsTestScene(); break;
    //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
    //    case TEST_FONTS:
    //        pScene = new FontTestScene(); break;
    //    case TEST_CURRENT_LANGUAGE:
    //        pScene = new CurrentLanguageTestScene(); break;
    //        break;
    //#endif
	
    //    default:
    //        break;
    //    }

    //    return pScene;
    //}

    public class TestController : CCLayer
    {
        static int LINE_SPACE = 40;
        static CCPoint s_tCurPos = new CCPoint(0.0f, 0.0f);
        
        public TestController()
        {
            m_tBeginPos = new CCPoint(0.0f, 0.0f);

            // add close menu
            CCMenuItemImage pCloseItem = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathClose, TestResource.s_pPathClose, this, new SEL_MenuHandler(closeCallback));
            CCMenu pMenu =CCMenu.menuWithItems(pCloseItem);
            CCSize s = CCDirector.sharedDirector().getWinSize();

            pMenu.position = new CCPoint(0.0f, 0.0f);
            pCloseItem.position = new CCPoint( s.width - 30, s.height - 30);

            //// add menu items for tests
            //m_pItmeMenu = CCMenu.menuWithItems(null);
            //for (int i = 0; i < (int)(TestCases.TESTS_COUNT); ++i)
            //{
            //    CCLabelTTF label = CCLabelTTF.labelWithString(Tests.g_aTestNames[i], "Arial", 24);
            //    CCMenuItemLabel pMenuItem = CCMenuItemLabel.itemWithLabel(label, this, new SEL_MenuHandler(menuCallback));

            //    m_pItmeMenu.addChild(pMenuItem, i + 10000);
            //    pMenuItem.position = new CCPoint( s.width / 2, (s.height - (i + 1) * LINE_SPACE) );
            //}

            //m_pItmeMenu.contentSize = new CCSize(s.width, ((int)(TestCases.TESTS_COUNT) + 1) * LINE_SPACE);
            //m_pItmeMenu.position = (s_tCurPos);
            //addChild(m_pItmeMenu);

            isTouchEnabled = true;

            addChild(pMenu, 1);
        }

        ~TestController()
        {
        }

        public void menuCallback(CCObject  pSender)
        {
            //// get the userdata, it's the index of the menu item clicked
            //CCMenuItem pMenuItem = (CCMenuItem )(pSender);
            //int nIdx = pMenuItem.zOrder - 10000;

            //// create the test scene and run it
            //TestScene pScene = CreateTestScene(nIdx);
            //if (pScene)
            //{
            //    pScene.runThisTest();
            //}
        }

        public void closeCallback(CCObject pSender)
        {
            CCDirector.sharedDirector().end();
        }

        public override void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {
            //CCSetIterator it = pTouches->begin();
            //CCTouch* touch = (CCTouch*)(*it);

            //m_tBeginPos = touch->locationInView( touch->view() );	
            //m_tBeginPos = CCDirector::sharedDirector()->convertToGL( m_tBeginPos );
        }

        public override void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        {
            //CCSetIterator it = pTouches->begin();
            //CCTouch* touch = (CCTouch*)(*it);

            //CCPoint touchLocation = touch->locationInView( touch->view() );	
            //touchLocation = CCDirector::sharedDirector()->convertToGL( touchLocation );
            //float nMoveY = touchLocation.y - m_tBeginPos.y;

            //CCPoint curPos  = m_pItmeMenu->getPosition();
            //CCPoint nextPos = ccp(curPos.x, curPos.y + nMoveY);
            //CCSize winSize = CCDirector::sharedDirector()->getWinSize();
            //if (nextPos.y < 0.0f)
            //{
            //    m_pItmeMenu->setPosition(CCPointZero);
            //    return;
            //}

            //if (nextPos.y > ((TESTS_COUNT + 1)* LINE_SPACE - winSize.height))
            //{
            //    m_pItmeMenu->setPosition(ccp(0, ((TESTS_COUNT + 1)* LINE_SPACE - winSize.height)));
            //    return;
            //}

            //m_pItmeMenu->setPosition(nextPos);
            //m_tBeginPos = touchLocation;
            //s_tCurPos   = nextPos;
        }

        private CCPoint m_tBeginPos;
        private CCMenu m_pItmeMenu;
    }
}