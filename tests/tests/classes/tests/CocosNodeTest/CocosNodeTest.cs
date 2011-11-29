using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using tests.classes.tests.Layer;

namespace tests
{
    public class CocosNodeTestStaticLibrary
    {
        public const int kTagSprite1 = 1;
        public const int kTagSprite2 = 2;
        public const int kTagSprite3 = 3;
        public const int kTagSlider = 4;

        public static int sceneIdx = -1;

        public const int MAX_LAYER = 12;
        public const int SID_DELAY2 = 1;
        public const int SID_DELAY4 = 2;

        public static CCLayer createCocosNodeLayer(int nIndex)
        {
            switch (nIndex)
            {
                //case 0: return new CameraCenterTest();
                case 1: return new Test2();
                case 2: return new Test4();
                case 3: return new Test5();
                //case 4: return new Test6();
                //case 5: return new StressTest1();
                //case 6: return new StressTest2();
                //case 7: return new NodeToWorld();
                //case 8: return new SchedulerTest1();
                //case 9: return new CameraOrbitTest();
                //case 10: return new CameraZoomTest();
                //case 11: return new ConvertToNode();
            }

            return null;
        }

        public static CCLayer nextCocosNodeAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backCocosNodeAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartCocosNodeAction()
        {
            CCLayer pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }
    }



    class TestCocosNodeDemo : CCLayer
    {
        public TestCocosNodeDemo() { }
        ~TestCocosNodeDemo() { }

        public virtual string title()
        {
            return "No title";
        }
        public virtual string subtitle()
        {
            return "";
        }
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            addChild(label, 1);
            label.position = (CCF.CCPointMake(s.width / 2, s.height - 50));

            string strSubtitle = subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Thonburi", 16);
                addChild(l, 1);
                l.position = (CCF.CCPointMake(s.width / 2, s.height - 80));
            }

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(this.backCallback));
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(this.restartCallback));
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(this.nextCallback));

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = (CCF.CCPointZero);
            item1.position = (CCF.CCPointMake(s.width / 2 - 100, 30));
            item2.position = (CCF.CCPointMake(s.width / 2, 30));
            item3.position = (CCF.CCPointMake(s.width / 2 + 100, 30));

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene();//CCScene.node();
            s.addChild(CocosNodeTestStaticLibrary.restartCocosNodeAction());

            CCDirector.sharedDirector().replaceScene(s);
        }
        public void nextCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene();//CCScene.node();
            s.addChild(CocosNodeTestStaticLibrary.nextCocosNodeAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
        public void backCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene();//CCScene.node();
            s.addChild(CocosNodeTestStaticLibrary.backCocosNodeAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
    }

    class Test2 : TestCocosNodeDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);
            CCSprite sp3 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp4 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (CCF.CCPointMake(100, s.height / 2));
            sp2.position = (CCF.CCPointMake(380, s.height / 2));
            addChild(sp1);
            addChild(sp2);

            sp3.scale = (0.25f);
            sp4.scale = (0.25f);

            sp1.addChild(sp3);
            sp2.addChild(sp4);

            CCActionInterval a1 = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval a2 = CCScaleBy.actionWithDuration(2, 2);

            CCAction action1 = CCRepeatForever.actionWithAction(
                                                            (CCActionInterval)(CCSequence.actions(a1, a2, a2.reverse()))
                                                        );
            CCAction action2 = CCRepeatForever.actionWithAction(
                                                            (CCActionInterval)(CCSequence.actions(
                                                                                                (CCActionInterval)(a1.copy()),
                                                                                                (CCActionInterval)(a2.copy()),
                                                                                                a2.reverse()))
                                                        );

            sp2.anchorPoint = (CCF.CCPointMake(0, 0));

            sp1.runAction(action1);
            sp2.runAction(action2);
        }
        public override string title()
        {
            return "anchorPoint and children";
        }
    }

    class Test4 : TestCocosNodeDemo
    {
        public Test4()
        {
            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (CCF.CCPointMake(100, 160));
            sp2.position = (CCF.CCPointMake(380, 160));

            addChild(sp1, 0, 2);
            addChild(sp2, 0, 3);

            schedule(new SEL_SCHEDULE(this.delay2), 2.0f);
            schedule(new SEL_SCHEDULE(this.delay4), 4.0f);
        }
        public void delay2(float dt)
        {
            CCSprite node = (CCSprite)(getChildByTag(2));
            CCAction action1 = CCRotateBy.actionWithDuration(1, 360);
            node.runAction(action1);
        }
        public void delay4(float dt)
        {
            unschedule(new SEL_SCHEDULE(this.delay4));
            removeChildByTag(3, false);
        }

        public override string title()
        {
            return "tags";
        }
    }

    class Test5 : TestCocosNodeDemo
    {
        public Test5()
        {
            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (CCF.CCPointMake(100, 160));
            sp2.position = (CCF.CCPointMake(380, 160));

            CCRotateBy rot = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval rot_back = rot.reverse() as CCActionInterval;
            CCAction forever = CCRepeatForever.actionWithAction(
                                                            (CCActionInterval)(CCSequence.actions(rot, rot_back))
                                                        );
            CCAction forever2 = (CCAction)(forever.copy());
            forever.tag = (101);
            forever2.tag = (102);

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);

            sp1.runAction(forever);
            sp2.runAction(forever2);

            schedule(new SEL_SCHEDULE(this.addAndRemove), 2.0f);
        }
        public void addAndRemove(float dt)
        {
            CCNode sp1 = getChildByTag(CocosNodeTestStaticLibrary.kTagSprite1);
            CCNode sp2 = getChildByTag(CocosNodeTestStaticLibrary.kTagSprite2);

            removeChild(sp1, false);
            removeChild(sp2, true);

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
        }

        public override string title()
        {
            return "remove and cleanup";
        }
    }

    class Test6 : TestCocosNodeDemo
    {
        public Test6()
        {
            CCSprite sp1 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            CCSprite sp11 = CCSprite.spriteWithFile(TestResource.s_pPathSister1);

            CCSprite sp2 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);
            CCSprite sp21 = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            sp1.position = (CCF.CCPointMake(100, 160));
            sp2.position = (CCF.CCPointMake(380, 160));

            CCActionInterval rot = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval rot_back = rot.reverse() as CCActionInterval;
            CCAction forever1 = CCRepeatForever.actionWithAction(
                                                                    (CCActionInterval)(CCSequence.actions(rot, rot_back)));
            CCAction forever11 = (CCAction)(forever1.copy());

            CCAction forever2 = (CCAction)(forever1.copy());
            CCAction forever21 = (CCAction)(forever1.copy());

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            sp1.addChild(sp11);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);
            sp2.addChild(sp21);

            sp1.runAction(forever1);
            sp11.runAction(forever11);
            sp2.runAction(forever2);
            sp21.runAction(forever21);

            schedule(new SEL_SCHEDULE(this.addAndRemove), 2.0f);
        }
        public void addAndRemove(float dt)
        {
            CCNode sp1 = getChildByTag(CocosNodeTestStaticLibrary.kTagSprite1);
            CCNode sp2 = getChildByTag(CocosNodeTestStaticLibrary.kTagSprite2);

            removeChild(sp1, false);
            removeChild(sp2, true);

            addChild(sp1, 0, CocosNodeTestStaticLibrary.kTagSprite1);
            addChild(sp2, 0, CocosNodeTestStaticLibrary.kTagSprite2);

        }

        public override string title()
        {
            return "remove/cleanup with children";
        }
    }

#if incomplated
    class StressTest1 : TestCocosNodeDemo
    {
        void shouldNotCrash(float dt);
        void removeMe(CCNode node);

        public StressTest1();

        public override string title();
    }

    class StressTest2 : TestCocosNodeDemo
    {
        void shouldNotLeak(float dt);

        public StressTest2();

        public override string title();
    }

    class SchedulerTest1 : TestCocosNodeDemo
    {
        public SchedulerTest1();
        void doSomething(float dt);

        public override string title();
    }

    class NodeToWorld : TestCocosNodeDemo
    {
        public NodeToWorld();
        public override string title();
    }

    class CameraOrbitTest : TestCocosNodeDemo
    {
        public CameraOrbitTest();

        public override void onEnter();
        public override void onExit();
        public override string title();
    }

    class CameraZoomTest : TestCocosNodeDemo
    {
        float m_z;

        public CameraZoomTest();
        public void update(float dt);

        public override void onEnter();
        public override void onExit();

        public override string title();
    }

    class CameraCenterTest : TestCocosNodeDemo
    {
        public CameraCenterTest();
        public override string title();
        public override string subtitle();
    }

    class ConvertToNode : TestCocosNodeDemo
    {
        public ConvertToNode();
        public override void ccTouchesEnded(CCSet touches, CCEvent @event);
        public override string title();
        public override string subtitle();
    }

#endif


    class CocosNodeTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = CocosNodeTestStaticLibrary.nextCocosNodeAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }
    }
}
