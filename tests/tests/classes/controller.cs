using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{

    public class TestController : CCLayer
    {
        static int LINE_SPACE = 40;
        static CCPoint s_tCurPos = new CCPoint(0.0f, 0.0f);

        public TestController()
        {
            CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;

            m_tBeginPos = new CCPoint(0.0f, 0.0f);
            CCSize s = CCDirector.sharedDirector().getWinSize();
            // add close menu
            CCMenuItemImage pCloseItem = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathClose,
                TestResource.s_pPathClose,
                this,
                new SEL_MenuHandler(closeCallback));
            pCloseItem.anchorPoint = new CCPoint(0, 0);

            CCMenu pMenu = CCMenu.menuWithItems(pCloseItem);
            pMenu.position = new CCPoint(s.width - 40, 10);

            // add menu items for tests
            m_pItemMenu = CCMenu.menuWithItems(null);
            for (int i = 0; i < (int)(TestCases.TESTS_COUNT); ++i)
            {
                // todo, CCMenuItemLabel hasn't been implemented, use CCMenuItemImage instead
                CCLabelTTF label = CCLabelTTF.labelWithString(Tests.g_aTestNames[i], "Arial", 24);
                CCMenuItemLabel pMenuItem = CCMenuItemLabel.itemWithLabel(label, this, new SEL_MenuHandler(menuCallback));
                //CCMenuItemImage pMenuItem = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathGrossini, TestResource.s_pPathGrossini, this, new SEL_MenuHandler(menuCallback));

                m_pItemMenu.addChild(pMenuItem, i + 10000);
                pMenuItem.position = new CCPoint(s.width / 2, (s.height - (i + 1) * LINE_SPACE));
            }

            m_pItemMenu.contentSize = new CCSize(s.width, ((int)(TestCases.TESTS_COUNT) + 1) * LINE_SPACE);
            m_pItemMenu.position = (s_tCurPos);
            addChild(m_pItemMenu);

            isTouchEnabled = true;

            addChild(pMenu, 1);
        }

        ~TestController()
        {
        }

        public void menuCallback(CCObject pSender)
        {
            // get the userdata, it's the index of the menu item clicked
            CCMenuItem pMenuItem = (CCMenuItem)(pSender);
            int nIdx = pMenuItem.zOrder - 10000;

            // create the test scene and run it
            TestScene pScene = CreateTestScene(nIdx);
            if (pScene != null)
            {
                pScene.runThisTest();
            }
        }

        public void closeCallback(CCObject pSender)
        {
            CCDirector.sharedDirector().end();
            CCApplication.sharedApplication().Game.Exit();
        }

        public override void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            m_tBeginPos = touch.locationInView(touch.view());
            m_tBeginPos = CCDirector.sharedDirector().convertToGL(m_tBeginPos);
        }

        public override void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            CCPoint touchLocation = touch.locationInView(touch.view());
            touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);
            float nMoveY = touchLocation.y - m_tBeginPos.y;

            CCPoint curPos = m_pItemMenu.position;
            CCPoint nextPos = new CCPoint(curPos.x, curPos.y + nMoveY);
            CCSize winSize = CCDirector.sharedDirector().getWinSize();
            if (nextPos.y < 0.0f)
            {
                m_pItemMenu.position = new CCPoint(0, 0);
                return;
            }

            if (nextPos.y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - winSize.height))
            {
                m_pItemMenu.position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - winSize.height)));
                return;
            }

            m_pItemMenu.position = nextPos;
            m_tBeginPos = touchLocation;
            s_tCurPos = nextPos;
        }

        public static TestScene CreateTestScene(int nIdx)
        {
            CCDirector.sharedDirector().purgeCachedData();

            TestScene pScene = null;

            switch (nIdx)
            {
                case (int)TestCases.TEST_ACTIONS:
                    pScene = new ActionsTestScene(); break;
                case (int)TestCases.TEST_TRANSITIONS:
                    pScene = new TransitionsTestScene(); break;
                case (int)TestCases.TEST_PROGRESS_ACTIONS:
                    pScene = new ProgressActionsTestScene(); break;
                //    case TEST_EFFECTS:
                //        pScene = new EffectTestScene(); break;
                case (int)TestCases.TEST_CLICK_AND_MOVE:
                    pScene = new ClickAndMoveTest(); break;
                case (int)TestCases.TEST_ROTATE_WORLD:
                        pScene = new RotateWorldTestScene(); break;
                case (int)TestCases.TEST_PARTICLE:
                    pScene = new ParticleTestScene(); break;
                case (int)TestCases.TEST_EASE_ACTIONS:
                    pScene = new EaseActionsTestScene(); break;
                case (int)TestCases.TEST_MOTION_STREAK:
                    pScene = new MotionStreakTestScene(); break;
                case (int)TestCases.TEST_DRAW_PRIMITIVES:
                    pScene = new DrawPrimitivesTestScene(); break;
                case (int)TestCases.TEST_COCOSNODE:
                    pScene = new CocosNodeTestScene(); break;
                case (int)TestCases.TEST_TOUCHES:
                    pScene = new PongScene(); break;
                case (int)TestCases.TEST_MENU:
                    pScene = new MenuTestScene(); break;
                case (int)TestCases.TEST_ACTION_MANAGER:
                    pScene = new ActionManagerTestScene(); break;
                case (int)TestCases.TEST_LAYER:
                    pScene = new LayerTestScene(); break;
                case (int)TestCases.TEST_SCENE:
                    pScene = new SceneTestScene(); break;
                case (int)TestCases.TEST_PARALLAX:
                    pScene = new ParallaxTestScene(); break;
                case (int)TestCases.TEST_TILE_MAP:
                    pScene = new TileMapTestScene(); break;
                case (int)TestCases.TEST_INTERVAL:
                    pScene = new IntervalTestScene(); break;
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
                case (int)TestCases.TEST_LABEL:
                    pScene = new AtlasTestScene(); break;
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                //    case TEST_TEXT_INPUT:
                //        pScene = new TextInputTestScene(); break;
                //#endif
                case (int)TestCases.TEST_SPRITE:
                    pScene = new SpriteTestScene(); break;
                case (int)TestCases.TEST_SCHEDULER:
                    pScene = new SchedulerTestScene(); break;
                case (int)TestCases.TEST_RENDERTEXTURE:
                    pScene = new RenderTextureScene(); break;
                case (int)TestCases.TEST_TEXTURE2D:
                    pScene = new TextureTestScene(); break;
                //    case TEST_BOX2D:
                //        pScene = new Box2DTestScene(); break;
                //    case TEST_BOX2DBED:
                //        pScene = new Box2dTestBedScene(); break;
                //case TEST_EFFECT_ADVANCE:
                //    pScene = new EffectAdvanceScene(); break;
                case (int)TestCases.TEST_HIRES:
                    pScene = new HiResTestScene(); break;
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_WIN32)
                //    case TEST_ACCELEROMRTER:
                //        pScene = new AccelerometerTestScene(); break;
                //#endif
                //    case TEST_KEYPAD:
                //        pScene = new KeypadTestScene(); break;
                case (int)TestCases.TEST_COCOSDENSHION:
                    pScene = new CocosDenshionTestScene(); break;
                //case TEST_PERFORMANCE:
                //    pScene = new PerformanceTestScene(); break;
                //case TEST_ZWOPTEX:
                //    pScene = new ZwoptexTestScene(); break;
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                //    case TEST_CURL:
                //        pScene = new CurlTestScene(); break;
                case (int)TestCases.TEST_USERDEFAULT:
                    pScene = new UserDefaultTestScene(); break;
                //#endif
                case (int)TestCases.TEST_DIRECTOR:
                    pScene = new DirectorTestScene(); break;
                //    case TEST_BUGS:
                //        pScene = new BugsTestScene(); break;
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                //    case TEST_FONTS:
                //        pScene = new FontTestScene(); break;
                //    case TEST_CURRENT_LANGUAGE:
                //        pScene = new CurrentLanguageTestScene(); break;
                //        break;
                //#endif

                default:
                    break;
            }

            return pScene;
        }

        private CCPoint m_tBeginPos;
        private CCMenu m_pItemMenu;
    }
}