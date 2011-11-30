using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;
using System.Diagnostics;

namespace tests
{
    public class DirectorTest : CCLayer
    {
        static int MAX_LAYER = 1;
        static int sceneIdx = -1;
        public static ccDeviceOrientation s_currentOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;

        public static CCLayer createTestCaseLayer(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        Director1 pRet = new Director1();
                        pRet.init();
                        return pRet;
                    }
                default:
                    return null;
            }
        }

        public static CCLayer nextDirectorTestCase()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            return createTestCaseLayer(sceneIdx);
        }

        public static CCLayer backDirectorTestCase()
        {
            sceneIdx--;
            if (sceneIdx < 0)
                sceneIdx += MAX_LAYER;

            return createTestCaseLayer(sceneIdx);
        }

        public static CCLayer restartDirectorTestCase()
        {
            return createTestCaseLayer(sceneIdx);
        }

        public override bool init()
        {
            bool bRet = false;
            do
            {
                if( !base.init())
                    break;

                CCSize s = CCDirector.sharedDirector().getWinSize();

                CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 26);
                addChild(label, 1);
                label.position = new CCPoint(s.width / 2, s.height - 50);

                string sSubtitle = subtitle();
                if (sSubtitle.Length > 0)
                {
                    CCLabelTTF l = CCLabelTTF.labelWithString(sSubtitle, "Arial", 16);
                    addChild(l, 1);
                    l.position = new CCPoint(s.width / 2, s.height - 80);
                }

                CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(backCallback));
                CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(restartCallback));
                CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(nextCallback));

                CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);
                menu.position = new CCPoint();
                item1.position = new CCPoint(s.width / 2 - 100, 30);
                item2.position = new CCPoint(s.width / 2, 30);
                item3.position = new CCPoint(s.width / 2 + 100, 30);

                bRet = true;
            } while (false);

            return bRet;
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new DirectorTestScene();
            s.addChild(restartDirectorTestCase());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new DirectorTestScene();
            s.addChild(nextDirectorTestCase());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new DirectorTestScene();
            s.addChild(backDirectorTestCase());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

    }

    public class Director1 : DirectorTest
    {

        public override bool init()
        {
            bool bRet = false;
            do
            {
                if (!base.init())
                    break;
                isTouchEnabled = true;
                CCSize s = CCDirector.sharedDirector().getWinSize();


                CCMenuItem item = CCMenuItemFont.itemFromString("Rotate Device", this, new SEL_MenuHandler(rotateDevice));
                CCMenu menu = CCMenu.menuWithItems(item);
                menu.position = new CCPoint(s.width / 2, s.height / 2);
                addChild(menu);

                bRet = true;
            } while (false);

            return bRet;
        }

        public void newOrientation()
        {
            switch (s_currentOrientation)
            {
                case ccDeviceOrientation.CCDeviceOrientationLandscapeLeft:
                    s_currentOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;
                    break;
                case ccDeviceOrientation.CCDeviceOrientationPortrait:
                    s_currentOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeRight;
                    break;
                case ccDeviceOrientation.CCDeviceOrientationLandscapeRight:
                    s_currentOrientation = ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown;
                    break;
                case ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown:
                    s_currentOrientation = ccDeviceOrientation.CCDeviceOrientationLandscapeLeft;
                    break;
            }
            CCDirector.sharedDirector().deviceOrientation = s_currentOrientation;
        }

        public void rotateDevice(CCObject pSender)
        {
            newOrientation();
            restartCallback(null);
        }


        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent eventarg)
        {

            foreach (CCTouch touch in touches)
            {
                if (touch != null)
                    break;
                CCPoint a = touch.locationInView(touch.view());

                CCDirector director = CCDirector.sharedDirector();
                CCPoint b = director.convertToUI(director.convertToGL(a));
                //CCLog("(%d,%d) == (%d,%d)", (int) a.x, (int)a.y, (int)b.x, (int)b.y );
                Debug.WriteLine("(%d,%d) == (%d,%d)", (int)a.x, (int)a.y, (int)b.x, (int)b.y);
            }
        }

        public override string title()
        {
            return "Testing conversion";
        }

        public override string subtitle()
        {
            return "Tap screen and see the debug console";
        }

    }

    public class DirectorTestScene : TestScene
    {

        public override void runThisTest()
        {
            DirectorTest.s_currentOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;
            CCLayer pLayer = DirectorTest.nextDirectorTestCase();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }

        public override void MainMenuCallback(CCObject pSender)
        {
            CCDirector.sharedDirector().deviceOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;
            base.MainMenuCallback(pSender);
        }

    }

}
