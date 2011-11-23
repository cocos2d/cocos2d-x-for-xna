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
            case (int)ActionTest.ACTION_MOVE_LAYER:
                pLayer = new ActionMove(); break;
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

    public class ActionManual : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_tamara.scaleX = 2.5f;
            m_tamara.scaleY = -1.0f;
            m_tamara.position = new CCPoint(100,70);
            m_tamara.Opacity = 128;

            m_grossini.rotation = 120;
            m_grossini.position = new CCPoint(s.width/2, s.height/2);
            m_grossini.Color = new ccColor3B(255,0,0);

            m_kathia.position = new CCPoint(s.width-100, s.height/2);
            m_kathia.Color = new ccColor3B(0, 0, 255);// ccTypes.ccBLUE
        }

        public override string subtitle()
        {
            return "Manual Transformation";
        }
    };

    public class ActionMove : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(3);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCActionInterval actionTo = CCMoveTo.actionWithDuration(2, new CCPoint(s.width-40, s.height-40));
            CCActionInterval actionBy = CCMoveBy.actionWithDuration(2, new CCPoint(80,80));

            // source code: CCActionInterval* actionByBack = actionBy->reverse();
            CCFiniteTimeAction actionByBack = actionBy.reverse();

            m_tamara.runAction( actionTo);
            m_grossini.runAction( CCSequence.actions(actionBy, actionByBack, null));
            m_kathia.runAction(CCMoveTo.actionWithDuration(1, new CCPoint(40,40)));
        }

        public override string subtitle()
        {
            return "MoveTo / MoveBy";
        }
    }

    public class ActionScale : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(3);

            CCActionInterval actionTo = CCScaleTo.actionWithDuration( 2, 0.5f);
            CCActionInterval actionBy = CCScaleBy.actionWithDuration(2 ,  2);
            CCActionInterval actionBy2 = CCScaleBy.actionWithDuration(2, 0.25f, 4.5f);
            CCFiniteTimeAction actionByBack = actionBy.reverse();

            m_tamara.runAction(actionTo);
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack, null));
            m_kathia.runAction(CCSequence.actions(actionBy2, actionBy2.reverse(), null));
        }

        public override string subtitle()
        {
            return "ScaleTo / ScaleBy";
        }
    };

    public class ActionSkew : ActionsDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        centerSprites(3);

	        CCActionInterval actionTo = CCSkewTo.actionWithDuration(2, 37.2f, -37.2f);
	        CCActionInterval actionToBack = CCSkewTo.actionWithDuration(2, 0, 0);
	        CCActionInterval actionBy = CCSkewBy.actionWithDuration(2, 0.0f, -90.0f);
	        CCActionInterval actionBy2 = CCSkewBy.actionWithDuration(2, 45.0f, 45.0f);
	        CCFiniteTimeAction actionByBack = actionBy.reverse();

	        m_tamara.runAction(CCSequence.actions(actionTo, actionToBack, null));
	        m_grossini.runAction(CCSequence.actions(actionBy, actionByBack, null));

	        m_kathia.runAction(CCSequence.actions(actionBy2, actionBy2.reverse(), null));
        }

        public override string subtitle()
        {
	        return "SkewTo / SkewBy";
        }


    };

    public class ActionSkewRotateScale : ActionsDemo
    {
        public override void onEnter()
        {
            // todo: CCLayerColor hasn't been implemented

            //base.onEnter();

            //m_tamara.removeFromParentAndCleanup(true);
            //m_grossini.removeFromParentAndCleanup(true);
            //m_kathia.removeFromParentAndCleanup(true);

            //CCSize boxSize = new CCSize(100.0f, 100.0f);

            //CCLayerColor box = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 0, 255));
            //box.anchorPoint = new CCPoint(0, 0);
            //box.position = new CCPoint(190, 110);
            //box.contentSize = boxSize;

            //static float markrside = 10.0f;
            //CCLayerColor *uL = CCLayerColor::layerWithColor(ccc4(255, 0, 0, 255));
            //box->addChild(uL);
            //uL->setContentSize(CCSizeMake(markrside, markrside));
            //uL->setPosition(ccp(0.f, boxSize.height - markrside));
            //uL->setAnchorPoint(ccp(0, 0));

            //CCLayerColor *uR = CCLayerColor::layerWithColor(ccc4(0, 0, 255, 255));
            //box->addChild(uR);
            //uR->setContentSize(CCSizeMake(markrside, markrside));
            //uR->setPosition(ccp(boxSize.width - markrside, boxSize.height - markrside));
            //uR->setAnchorPoint(ccp(0, 0));
            //addChild(box);

            //CCActionInterval *actionTo = CCSkewTo::actionWithDuration(2, 0.f, 2.f);
            //CCActionInterval *rotateTo = CCRotateTo::actionWithDuration(2, 61.0f);
            //CCActionInterval *actionScaleTo = CCScaleTo::actionWithDuration(2, -0.44f, 0.47f);

            //CCActionInterval *actionScaleToBack = CCScaleTo::actionWithDuration(2, 1.0f, 1.0f);
            //CCActionInterval *rotateToBack = CCRotateTo::actionWithDuration(2, 0);
            //CCActionInterval *actionToBack = CCSkewTo::actionWithDuration(2, 0, 0);

            //box->runAction(CCSequence::actions(actionTo, actionToBack, NULL));
            //box->runAction(CCSequence::actions(rotateTo, rotateToBack, NULL));
            //box->runAction(CCSequence::actions(actionScaleTo, actionScaleToBack, NULL));
        }

        public override string subtitle()
        {
	        return "Skew + Rotate + Scale";
        }

    };

    public class ActionRotate : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(3);

            CCActionInterval actionTo = CCRotateTo.actionWithDuration(2, 45);
            CCActionInterval actionTo2 = CCRotateTo.actionWithDuration(2, -45);
            CCActionInterval actionTo0 = CCRotateTo.actionWithDuration(2 , 0);
            m_tamara.runAction(CCSequence.actions(actionTo, actionTo0, null));

            CCActionInterval actionBy = CCRotateBy.actionWithDuration(2 , 360);
            CCFiniteTimeAction actionByBack = actionBy.reverse();
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack, null));

            // m_kathia->runAction( CCSequence::actions(actionTo2, actionTo0->copy()->autorelease(), NULL));
            m_kathia.runAction(CCSequence.actions(actionTo2, actionTo0, null));
        }

        public override string subtitle()
        {
            return "RotateTo / RotateBy";
        }
    };

    public class ActionJump : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(3);

            CCActionInterval actionTo = CCJumpTo.actionWithDuration(2, new CCPoint(300,300), 50, 4);
            CCActionInterval actionBy = CCJumpBy.actionWithDuration(2, new CCPoint(300, 0), 50, 4);
            CCActionInterval actionUp = CCJumpBy.actionWithDuration(2, new CCPoint(0,0), 80, 4);
            CCFiniteTimeAction actionByBack = actionBy.reverse();

            m_tamara.runAction(actionTo);
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack, null));
            m_kathia.runAction(CCRepeatForever.actionWithAction(actionUp));
        }

        public override string subtitle()
        {
            return "JumpTo / JumpBy";
        }
    };

    public class ActionBezier : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            //
            // startPosition can be any coordinate, but since the movement
            // is relative to the Bezier curve, make it (0,0)
            //

            centerSprites(3);

            // sprite 1
            ccBezierConfig bezier;
            bezier.controlPoint_1 = new CCPoint(0, s.height/2);
            bezier.controlPoint_2 = new CCPoint(300, -s.height/2);
            bezier.endPosition = new CCPoint(300,100);

            CCActionInterval bezierForward = CCBezierBy.actionWithDuration(3, bezier);
            CCFiniteTimeAction bezierBack = bezierForward.reverse();	
            CCAction rep = CCRepeatForever.actionWithAction((CCActionInterval)CCSequence.actions( bezierForward, bezierBack, null));


            // sprite 2
            m_tamara.position = new CCPoint(80,160);
            ccBezierConfig bezier2;
            bezier2.controlPoint_1 = new CCPoint(100, s.height/2);
            bezier2.controlPoint_2 = new CCPoint(200, -s.height/2);
            bezier2.endPosition = new CCPoint(240,160);

            CCActionInterval bezierTo1 = CCBezierTo.actionWithDuration(2, bezier2);	

            // sprite 3
            m_kathia.position = new CCPoint(400,160);
            CCActionInterval bezierTo2 = CCBezierTo.actionWithDuration(2, bezier2);

            m_grossini.runAction(rep);
            m_tamara.runAction(bezierTo1);
            m_kathia.runAction(bezierTo2);
        }

        public override string subtitle()
        {
            return "BezierBy / BezierTo";
        }
    };

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