using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public enum ActionTest
    {
        ACTION_MANUAL_LAYER = 0,
        ACTION_MOVE_LAYER,
        ACTION_SCALE_LAYER,
        ACTION_ROTATE_LAYER,
	    ACTION_SKEW_LAYER,
	    ACTION_SKEWROTATE_LAYER,
        ACTION_JUMP_LAYER,
        ACTION_BEZIER_LAYER,
        ACTION_BLINK_LAYER,
        ACTION_FADE_LAYER,
        ACTION_TINT_LAYER,
        ACTION_ANIMATE_LAYER,
        ACTION_SEQUENCE_LAYER,
        ACTION_SEQUENCE2_LAYER,
        ACTION_SPAWN_LAYER,
        ACTION_REVERSE,
        ACTION_DELAYTIME_LAYER,
        ACTION_REPEAT_LAYER,
        ACTION_REPEATEFOREVER_LAYER,
        ACTION_ROTATETOREPEATE_LAYER,
        ACTION_ROTATEJERK_LAYER,
        ACTION_CALLFUNC_LAYER,
        ACTION_CALLFUNCND_LAYER,
        ACTION_REVERSESEQUENCE_LAYER,
        ACTION_REVERSESEQUENCE2_LAYER,
        ACTION_ORBIT_LAYER,
        ACTION_FLLOW_LAYER,
        ACTION_LAYER_COUNT,
    };


    // the class inherit from TestScene
    // every Scene each test used must inherit from TestScene,
    // make sure the test have the menu item for back to main menu
    public class ActionsTestScene : TestScene
    {
        public static int s_nActionIdx = -1;

        public static CCLayer CreateLayer(int nIndex)
        {
            CCLayer pLayer = null;

            switch (nIndex)
            {
            //case (int)ActionTest.ACTION_MANUAL_LAYER:
            //    pLayer = new ActionManual(); break;
            //case (int)ActionTest.ACTION_MOVE_LAYER:
            //    pLayer = new ActionMove(); break;
            //case (int)ActionTest.ACTION_SCALE_LAYER:
            //    pLayer = new ActionScale(); break;
            //case (int)ActionTest.ACTION_ROTATE_LAYER:
            //    pLayer = new ActionRotate(); break;
            //case (int)ActionTest.ACTION_SKEW_LAYER:
            //    pLayer = new ActionSkew(); break;
            //case (int)ActionTest.ACTION_SKEWROTATE_LAYER:
            //    pLayer = new ActionSkewRotateScale(); break;
            //case (int)ActionTest.ACTION_JUMP_LAYER:
            //    pLayer = new ActionJump(); break;
            //case (int)ActionTest.ACTION_BEZIER_LAYER:
            //    pLayer = new ActionBezier(); break;
            //case (int)ActionTest.ACTION_BLINK_LAYER:
            //    pLayer = new ActionBlink(); break;
            //case (int)ActionTest.ACTION_FADE_LAYER:
            //    pLayer = new ActionFade(); break;
            //case (int)ActionTest.ACTION_TINT_LAYER:
            //    pLayer = new ActionTint(); break;
            //case (int)ActionTest.ACTION_ANIMATE_LAYER:
            //    pLayer = new ActionAnimate(); break;
            //case (int)ActionTest.ACTION_SEQUENCE_LAYER:
            //    pLayer = new ActionSequence(); break;
            //case (int)ActionTest.ACTION_SEQUENCE2_LAYER:
            //    pLayer = new ActionSequence2(); break;
            //case (int)ActionTest.ACTION_SPAWN_LAYER:
            //    pLayer = new ActionSpawn(); break;
            //case (int)ActionTest.ACTION_REVERSE:
            //    pLayer = new ActionReverse(); break;
            //case (int)ActionTest.ACTION_DELAYTIME_LAYER:
            //    pLayer = new ActionDelayTime(); break;
            //case (int)ActionTest.ACTION_REPEAT_LAYER:
            //    pLayer = new ActionRepeat(); break;
            //case (int)ActionTest.ACTION_REPEATEFOREVER_LAYER:
            //    pLayer = new ActionRepeatForever(); break;
            //case (int)ActionTest.ACTION_ROTATETOREPEATE_LAYER:
            //    pLayer = new ActionRotateToRepeat(); break;
            //case (int)ActionTest.ACTION_ROTATEJERK_LAYER:
            //    pLayer = new ActionRotateJerk(); break;    
            //case (int)ActionTest.ACTION_CALLFUNC_LAYER:
            //    pLayer = new ActionCallFunc(); break;
            //case (int)ActionTest.ACTION_CALLFUNCND_LAYER:
            //    pLayer = new ActionCallFuncND(); break;
            //case (int)ActionTest.ACTION_REVERSESEQUENCE_LAYER:
            //    pLayer = new ActionReverseSequence(); break;
            //case (int)ActionTest.ACTION_REVERSESEQUENCE2_LAYER:
            //    pLayer = new ActionReverseSequence2(); break;
            //case (int)ActionTest.ACTION_ORBIT_LAYER:
            //    pLayer = new ActionOrbit(); break;
            //case (int)ActionTest.ACTION_FLLOW_LAYER:
            //    pLayer = new ActionFollow(); break;
            default:
                break;
            }

            return pLayer;
        }

        public static CCLayer NextAction()
        {
            ++s_nActionIdx;
            s_nActionIdx = s_nActionIdx % (int)ActionTest.ACTION_LAYER_COUNT;

            CCLayer pLayer = CreateLayer(s_nActionIdx);

            return pLayer;
        }

        public static CCLayer BackAction()
        {
            --s_nActionIdx;
            if( s_nActionIdx < 0 )
                s_nActionIdx += (int)ActionTest.ACTION_LAYER_COUNT;	

            CCLayer pLayer = CreateLayer(s_nActionIdx);

            return pLayer;
        }

        public static CCLayer RestartAction()
        {
            CCLayer pLayer = CreateLayer(s_nActionIdx);

            return pLayer;
        }


        public override void runThisTest()
        {
            s_nActionIdx = -1;
            addChild(NextAction());

            CCDirector.sharedDirector().replaceScene(this);
        }
    }

    public class ActionsDemo : CCLayer
    {
        public virtual string title()
        {
            return "ActionsTest";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void onEnter()
        {
            base.onEnter();

            // Or you can create an sprite using a filename. only PNG is supported now. Probably TIFF too
            m_grossini = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);

            m_tamara = CCSprite.spriteWithFile(TestResource.s_pPathSister1); 

            m_kathia = CCSprite.spriteWithFile(TestResource.s_pPathSister2);
            
            addChild(m_grossini, 1);
            addChild(m_tamara, 2);
            addChild(m_kathia, 3);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_grossini.position = new CCPoint(s.width/2, s.height/3);
            m_tamara.position = new CCPoint(s.width/2, 2*s.height/3);
            m_kathia.position = new CCPoint(s.width/2, s.height/2); 

            // add title and subtitle
            string str = title();
            string pTitle = str;
            CCLabelTTF label = CCLabelTTF.labelWithString(pTitle, "Arial", 18);
            addChild(label, 1);
            label.position = new CCPoint(s.width/2, s.height - 30);

            string strSubtitle = subtitle();
            if( ! strSubtitle.Equals("") ) 
            {
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Thonburi", 22);
                addChild(l, 1);
                l.position = new CCPoint(s.width/2, s.height - 60);
            }	

            // add menu
            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(backCallback) );
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(restartCallback) );
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(nextCallback) );

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);

            menu.position = new CCPoint(0,0);
            item1.position = new CCPoint( s.width/2 - 100,30 );
            item2.position = new CCPoint( s.width/2, 30 );
            item3.position = new CCPoint( s.width/2 + 100,30 );

            addChild(menu, 1);
        }

        public override void onExit()
        {
            base.onExit();
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new ActionsTestScene();
            s.addChild( ActionsTestScene.RestartAction() );
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new ActionsTestScene();
            s.addChild( ActionsTestScene.NextAction() );
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new ActionsTestScene();
            s.addChild( ActionsTestScene.BackAction() );
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void centerSprites(uint numberOfSprites)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            if( numberOfSprites == 1 ) 
            {
                m_tamara.visible = false;
                m_kathia.visible = false;
                m_grossini.position = new CCPoint(s.width/2, s.height/2);
            }
            else if( numberOfSprites == 2 ) 
            {		
                m_kathia.position = new CCPoint(s.width/3, s.height/2);
                m_tamara.position = new CCPoint(2*s.width/3, s.height/2);
                m_grossini.visible = false;
            } 
            else if( numberOfSprites == 3 ) 
            {
                m_grossini.position = new CCPoint(s.width/2, s.height/2);
                m_tamara.position = new CCPoint(s.width/4, s.height/2);
                m_kathia.position = new CCPoint(3 * s.width/4, s.height/2);
            }
        }

        public void alignSpritesLeft(uint numberOfSprites)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            if( numberOfSprites == 1 ) 
            {
                m_tamara.visible = false;
                m_kathia.visible = false;
                m_grossini.position = new CCPoint(60, s.height/2);
            } 
            else if( numberOfSprites == 2 ) 
            {		
                m_kathia.position = new CCPoint(60, s.height/3);
                m_tamara.position = new CCPoint(60, 2*s.height/3);
                m_grossini.visible = false;
            } 
            else if( numberOfSprites == 3 ) 
            {
                m_grossini.position = new CCPoint(60, s.height/2);
                m_tamara.position = new CCPoint(60, 2*s.height/3);
                m_kathia.position = new CCPoint(60, s.height/3);
            }
        }

        protected CCSprite m_grossini;
        protected CCSprite m_tamara;
        protected CCSprite m_kathia;
    };

    //class ActionManual : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionMove : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionScale : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionSkew : public ActionsDemo
    //{
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionSkewRotateScale : public ActionsDemo
    //{
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionRotate : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionJump : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionBezier : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionBlink : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionFade : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionTint : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionAnimate : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionSequence : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionSequence2 : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();

    //    void callback1();
    //    void callback2(CCNode* sender);
    //    void callback3(CCNode* sender, void* data);
    //};

    //class ActionSpawn : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionReverse : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionRepeat : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionDelayTime : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionReverseSequence : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionReverseSequence2 : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionOrbit : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionRepeatForever : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();

    //    void repeatForever(CCNode* pTarget);
    //};

    //class ActionRotateToRepeat : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionRotateJerk : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};

    //class ActionCallFunc : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();

    //    void callback1();
    //    void callback2(CCNode* pTarget);
    //    void callback3(CCNode* pTarget, void* data);
    //};

    //class ActionCallFuncND : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //    virtual std::string subtitle();
    //};

    //class ActionFollow : public ActionsDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string subtitle();
    //};



}