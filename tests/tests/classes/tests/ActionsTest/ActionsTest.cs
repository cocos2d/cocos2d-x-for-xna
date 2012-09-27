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
        // ACTION_ANIMATE_LAYER,
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
        //ACTION_ORBIT_LAYER,
        //ACTION_FLLOW_LAYER,
        ACTION_TWEEN,
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
                case (int)ActionTest.ACTION_MANUAL_LAYER:
                    pLayer = new ActionManual(); break;
                case (int)ActionTest.ACTION_MOVE_LAYER:
                    pLayer = new ActionMove(); break;
                case (int)ActionTest.ACTION_SCALE_LAYER:
                    pLayer = new ActionScale(); break;
                case (int)ActionTest.ACTION_ROTATE_LAYER:
                    pLayer = new ActionRotate(); break;
                case (int)ActionTest.ACTION_SKEW_LAYER:
                    pLayer = new ActionSkew(); break;
                case (int)ActionTest.ACTION_SKEWROTATE_LAYER:
                    pLayer = new ActionSkewRotateScale(); break;
                case (int)ActionTest.ACTION_JUMP_LAYER:
                    pLayer = new ActionJump(); break;
                case (int)ActionTest.ACTION_BEZIER_LAYER:
                    pLayer = new ActionBezier(); break;
                case (int)ActionTest.ACTION_BLINK_LAYER:
                    pLayer = new ActionBlink(); break;
                case (int)ActionTest.ACTION_FADE_LAYER:
                    pLayer = new ActionFade(); break;
                case (int)ActionTest.ACTION_TINT_LAYER:
                    pLayer = new ActionTint(); break;
                //case (int)ActionTest.ACTION_ANIMATE_LAYER:
                //    pLayer = new ActionAnimate(); break;
                case (int)ActionTest.ACTION_SEQUENCE_LAYER:
                    pLayer = new ActionSequence(); break;
                case (int)ActionTest.ACTION_SEQUENCE2_LAYER:
                    pLayer = new ActionSequence2(); break;
                case (int)ActionTest.ACTION_SPAWN_LAYER:
                    pLayer = new ActionSpawn(); break;
                case (int)ActionTest.ACTION_REVERSE:
                    pLayer = new ActionReverse(); break;
                case (int)ActionTest.ACTION_DELAYTIME_LAYER:
                    pLayer = new ActionDelayTime(); break;
                case (int)ActionTest.ACTION_REPEAT_LAYER:
                    pLayer = new ActionRepeat(); break;
                case (int)ActionTest.ACTION_REPEATEFOREVER_LAYER:
                    pLayer = new ActionRepeatForever(); break;
                case (int)ActionTest.ACTION_ROTATETOREPEATE_LAYER:
                    pLayer = new ActionRotateToRepeat(); break;
                case (int)ActionTest.ACTION_ROTATEJERK_LAYER:
                    pLayer = new ActionRotateJerk(); break;
                case (int)ActionTest.ACTION_CALLFUNC_LAYER:
                    pLayer = new ActionCallFunc(); break;
                case (int)ActionTest.ACTION_CALLFUNCND_LAYER:
                    pLayer = new ActionCallFuncND(); break;
                case (int)ActionTest.ACTION_REVERSESEQUENCE_LAYER:
                    pLayer = new ActionReverseSequence(); break;
                case (int)ActionTest.ACTION_REVERSESEQUENCE2_LAYER:
                    pLayer = new ActionReverseSequence2(); break;
                //case (int)ActionTest.ACTION_ORBIT_LAYER:
                //    pLayer = new ActionOrbit(); break;
                //case (int)ActionTest.ACTION_FLLOW_LAYER:
                //    pLayer = new ActionFollow(); break;
                case (int)ActionTest.ACTION_TWEEN:
                    pLayer = new ActionTween(); break;
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
                CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Arial", 22);
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
            // todo: the situation that scale is negtive hasn't been supported
            // original code : m_tamara->setScaleY(-1.0f);
            m_tamara.scaleY = 1.0f;
            m_tamara.position = new CCPoint(150, 100);
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

    public class ActionTween : ActionsDemo
    {
        private void myTween(float dx, string key)
        {
            CCLog.Log("myTween: dx=" + dx + ", for key=" + key);

            m_tamara.position = new CCPoint(dx, m_tamara.position.y);
            switch (key)
            {
                case "tamara":
                    m_tamara.position = new CCPoint(dx, m_tamara.position.y);
                    break;
                case "grossini":
                    m_grossini.position = new CCPoint(dx, m_grossini.position.y);
                    break;
                case "kathia":
                    m_kathia.position = new CCPoint(dx, m_kathia.position.y);
                    break;
            }
        }

        public override void onEnter()
        {
            base.onEnter();
            centerSprites(3);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCActionInterval actionTween1 = CCActionTween.actionWithDuration(3, "tamara", 25f, 250f, new CCActionTweenDelegate(myTween));
            CCActionInterval actionTween2 = CCActionTween.actionWithDuration(3, "grossini", 150f, 260f, new CCActionTweenDelegate(myTween));
            CCActionInterval actionTween3 = CCActionTween.actionWithDuration(3, "kathia", 75f, 250f, new CCActionTweenDelegate(myTween));

            // source code: CCActionInterval* actionByBack = actionBy->reverse();
            CCFiniteTimeAction actionTween1Back = actionTween1.reverse();

            m_tamara.runAction(actionTween2);
            m_grossini.runAction(CCSequence.actions(actionTween1, actionTween1Back));
            m_kathia.runAction(actionTween3);
        }

        public override string subtitle()
        {
            return "CCActionTween";
        }
    }


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

            m_tamara.runAction(actionTo);
            m_grossini.runAction( CCSequence.actions(actionBy, actionByBack));
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
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack));
            m_kathia.runAction(CCSequence.actions(actionBy2, actionBy2.reverse()));
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

	        m_tamara.runAction(CCSequence.actions(actionTo, actionToBack));
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack));

            m_kathia.runAction(CCSequence.actions(actionBy2, actionBy2.reverse()));
        }

        public override string subtitle()
        {
	        return "SkewTo / SkewBy";
        }


    };

    public class ActionSkewRotateScale : ActionsDemo
    {
        static float markrside = 10.0f;

        public override void onEnter()
        {
            // todo: CCLayerColor hasn't been implemented

            base.onEnter();

            m_tamara.removeFromParentAndCleanup(true);
            m_grossini.removeFromParentAndCleanup(true);
            m_kathia.removeFromParentAndCleanup(true);

            CCSize boxSize = new CCSize(100.0f, 100.0f);

            CCLayerColor box = CCLayerColor.layerWithColor(new ccColor4B(255, 255, 0, 255));
            box.anchorPoint = new CCPoint(0, 0);
            box.position = new CCPoint(190, 110);
            box.contentSize = boxSize;

            CCLayerColor uL = CCLayerColor.layerWithColor(new ccColor4B(255, 0, 0, 255));
            box.addChild(uL);
            uL.contentSize = new CCSize(markrside, markrside);
            uL.position = new CCPoint(0.0f, boxSize.height - markrside);
            uL.anchorPoint = new CCPoint(0, 0);

            CCLayerColor uR = CCLayerColor.layerWithColor(new ccColor4B(0, 0, 255, 255));
            box.addChild(uR);
            uR.contentSize = new CCSize(markrside, markrside);
            uR.position = new CCPoint(boxSize.width - markrside, boxSize.height - markrside);
            uR.anchorPoint = new CCPoint(0, 0);
            addChild(box);

            CCActionInterval actionTo = CCSkewTo.actionWithDuration(2, 0.0f, 2.0f);
            CCActionInterval rotateTo = CCRotateTo.actionWithDuration(2, 61.0f);
            CCActionInterval actionScaleTo = CCScaleTo.actionWithDuration(2, -0.44f, 0.47f);

            CCActionInterval actionScaleToBack = CCScaleTo.actionWithDuration(2, 1.0f, 1.0f);
            CCActionInterval rotateToBack = CCRotateTo.actionWithDuration(2, 0);
            CCActionInterval actionToBack = CCSkewTo.actionWithDuration(2, 0, 0);

            box.runAction(CCSequence.actions(actionTo, actionToBack));
            box.runAction(CCSequence.actions(rotateTo, rotateToBack));
            box.runAction(CCSequence.actions(actionScaleTo, actionScaleToBack));
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
            m_tamara.runAction(CCSequence.actions(actionTo, actionTo0));

            CCActionInterval actionBy = CCRotateBy.actionWithDuration(2 , 360);
            CCFiniteTimeAction actionByBack = actionBy.reverse();
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack));

            // m_kathia->runAction( CCSequence::actions(actionTo2, actionTo0->copy()->autorelease(), NULL));
            m_kathia.runAction(CCSequence.actions(actionTo2, actionTo0));
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
            m_grossini.runAction(CCSequence.actions(actionBy, actionByBack));
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
            CCAction rep = CCRepeatForever.actionWithAction((CCActionInterval)CCSequence.actions( bezierForward, bezierBack));


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

    public class ActionBlink : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(2);

            CCActionInterval action1 = CCBlink.actionWithDuration(2, 10);
            CCActionInterval action2 = CCBlink.actionWithDuration(2, 5);

            m_tamara.runAction(action1);
            m_kathia.runAction(action2);
        }

        public override string subtitle()
        {
            return "Blink";
        }
    };

    public class ActionFade : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(2);

            m_tamara.Opacity = 0;
            CCActionInterval action1 = CCFadeIn.actionWithDuration(1.0f);
            CCFiniteTimeAction action1Back = action1.reverse();

            CCActionInterval action2 = CCFadeOut.actionWithDuration(1.0f);
            CCFiniteTimeAction action2Back = action2.reverse();

            m_tamara.runAction( CCSequence.actions(action1, action1Back));
            m_kathia.runAction( CCSequence.actions(action2, action2Back));
        }

        public override string subtitle()
        {
            return "FadeIn / FadeOut";
        }

    };

    public class ActionTint : ActionsDemo
    {

        public override void onEnter()
        {
            base.onEnter();

            centerSprites(2);

            CCActionInterval action1 = CCTintTo.actionWithDuration(2, 255, 0, 255);
            CCActionInterval action2 = CCTintBy.actionWithDuration(2, -127, -255, -127);
            CCFiniteTimeAction action2Back = action2.reverse();

            m_tamara.runAction(action1);
            m_kathia.runAction(CCSequence.actions( action2, action2Back));
        }

        public override string subtitle()
        {
            return "TintTo / TintBy";
        }
    };

    public class ActionAnimate : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(1);

            CCAnimation animation = CCAnimation.animation();
            string frameName;
            for( int i=1;i<15;i++)
            {
                if (i < 10)
                {
                    frameName = string.Format("Images/grossini_dance_0{0}", i);
                }
                else
                {
                    frameName = string.Format("Images/grossini_dance_{0}", i);
                }
                animation.addFrameWithFileName(frameName);
            }

            CCActionInterval action = CCAnimate.actionWithDuration(3.0f, animation, false);
            CCFiniteTimeAction action_back = action.reverse();

            m_grossini.runAction( CCSequence.actions( action, action_back));
        }

        public override string subtitle()
        {
            return "Animation";
        }
    
    };

    public class ActionSequence : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(1);

            CCFiniteTimeAction action = CCSequence.actions(
                CCMoveBy.actionWithDuration( 2, new CCPoint(240,0)),
                CCRotateBy.actionWithDuration(2,  540));

            m_grossini.runAction(action);
        }

        public override string subtitle()
        {
            return "Sequence: Move + Rotate";
        }

    };

    public class ActionSequence2 : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(1);

            m_grossini.visible = false;

            CCFiniteTimeAction action = CCSequence.actions(
                CCPlace.actionWithPosition(new CCPoint(200,200)),
                CCShow.action(),
                CCMoveBy.actionWithDuration(1, new CCPoint(100,0)),
                CCCallFunc.actionWithTarget(this, new SEL_CallFunc(callback1)),
                CCCallFuncN.actionWithTarget(this, new SEL_CallFuncN(callback2)),
                CCCallFuncND.actionWithTarget(this, new SEL_CallFuncND(callback3), (object)0xbebabeba));

            m_grossini.runAction(action);
        }

        public void callback1()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("callback 1 called", "Arial", 16);
            label.position = new CCPoint( s.width/4*1,s.height/2);

            addChild(label);
        }

        public void callback2(CCNode sender)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("callback 2 called", "Arial", 16);
            label.position = new CCPoint(s.width/4*2,s.height/2);

            addChild(label);
        }

        public void callback3(CCNode sender, object data)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("callback 3 called", "Arial", 16);
            label.position = new CCPoint( s.width/4*3,s.height/2);

            addChild(label);
        }

        public override string subtitle()
        {
            return "Sequence of InstantActions";
        }    
    };

    public class ActionCallFunc : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(3);

            CCFiniteTimeAction action = CCSequence.actions(
                CCMoveBy.actionWithDuration(2, new CCPoint(200,0)),
                CCCallFunc.actionWithTarget(this, new SEL_CallFunc(callback1)));

            CCFiniteTimeAction action2 = CCSequence.actions(
                CCScaleBy.actionWithDuration(2, 2),
                CCFadeOut.actionWithDuration(2),
                CCCallFuncN.actionWithTarget(this, new SEL_CallFuncN(callback2)));

            CCFiniteTimeAction action3 = CCSequence.actions(
                CCRotateBy.actionWithDuration(3 , 360),
                CCFadeOut.actionWithDuration(2),
                CCCallFuncND.actionWithTarget(this, new SEL_CallFuncND(callback3), (object)0xbebabeba));

            m_grossini.runAction(action);
            m_tamara.runAction(action2);
            m_kathia.runAction(action3);
        }


        public void callback1()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("callback 1 called", "Arial", 16);
            label.position = new CCPoint(s.width/4*1,s.height/2);

            addChild(label);
        }

        public void callback2(CCNode pSender)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("callback 2 called", "Arial", 16);
            label.position = new CCPoint(s.width/4*2,s.height/2);

            addChild(label);
        }

        public void callback3(CCNode pTarget, object data)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("callback 3 called", "Arial", 16);
            label.position = new CCPoint(s.width/4*3,s.height/2);
            addChild(label);
        }

        public override string subtitle()
        {
            return "Callbacks: CallFunc and friends";
        }
    };

    public class ActionCallFuncND : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(1);

            CCFiniteTimeAction action = CCSequence.actions(CCMoveBy.actionWithDuration(2.0f, new CCPoint(200, 0)));
                //CCCallFuncND::actionWithTarget(m_grossini, callfuncND_selector(ActionCallFuncND::removeFromParentAndCleanup), (void*)true),

            m_grossini.runAction(action);
        }

        public override string title()
        {
            return "CallFuncND + auto remove";
        }

        public override string subtitle()
        {
            return "CallFuncND + removeFromParentAndCleanup. Grossini dissapears in 2s";
        }
    };

    public class ActionSpawn : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(1);

            CCAction action = CCSpawn.actions(
                CCJumpBy.actionWithDuration(2, new CCPoint(300,0), 50, 4),
                CCRotateBy.actionWithDuration( 2,  720));

            m_grossini.runAction(action);
        }

        public override string subtitle()
        {
            return "Spawn: Jump + Rotate";
        }
    
    };

    public class ActionRepeatForever : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(1);

            CCFiniteTimeAction action = CCSequence.actions(
                CCDelayTime.actionWithDuration(1),
                CCCallFuncN.actionWithTarget(this, new SEL_CallFuncN(repeatForever)));

            m_grossini.runAction(action);
        }

        public void repeatForever(CCNode pSender)
        {
            CCRepeatForever repeat = CCRepeatForever.actionWithAction(CCRotateBy.actionWithDuration(1.0f, 360) );

            pSender.runAction(repeat);
        }

        public override string subtitle()
        {
            return "CallFuncN + RepeatForever";
        }

    };

    public class ActionRotateToRepeat : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(2);

            CCActionInterval act1 = CCRotateTo.actionWithDuration(1, 90);
            CCActionInterval act2 = CCRotateTo.actionWithDuration(1, 0);
            CCActionInterval seq = (CCActionInterval)(CCSequence.actions(act1, act2));
            CCAction rep1 = CCRepeatForever.actionWithAction(seq);
            CCActionInterval rep2 = CCRepeat.actionWithAction((CCFiniteTimeAction)(seq.copy()), 10);

            m_tamara.runAction(rep1);
            m_kathia.runAction(rep2);
        }

        public override string subtitle()
        {
            return "Repeat/RepeatForever + RotateTo";
        }
    
    };

    public class ActionRotateJerk : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(2);

            CCFiniteTimeAction seq = CCSequence.actions(
                CCRotateTo.actionWithDuration(0.5f, -20),
                CCRotateTo.actionWithDuration(0.5f, 20));

            CCActionInterval rep1 = CCRepeat.actionWithAction(seq, 10);
            CCAction rep2 = CCRepeatForever.actionWithAction((CCActionInterval)(seq.copy()));

            m_tamara.runAction(rep1);
            m_kathia.runAction(rep2);
        }

        public override string subtitle()
        {
            return "RepeatForever / Repeat + Rotate";
        }

    };

    public class ActionReverse : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(1);

            CCActionInterval jump = CCJumpBy.actionWithDuration(2, new CCPoint(300,0), 50, 4);
            CCFiniteTimeAction action = CCSequence.actions( jump, jump.reverse());

            m_grossini.runAction(action);
        }

        public override string subtitle()
        {
            return "Reverse an action";
        }

    };

    public class ActionDelayTime : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(1);

            CCActionInterval move = CCMoveBy.actionWithDuration(1, new CCPoint(150,0));
            CCFiniteTimeAction action = CCSequence.actions( move, CCDelayTime.actionWithDuration(2), move);

            m_grossini.runAction(action);
        }

        public override string subtitle()
        {
            return "DelayTime: m + delay + m";
        }

    };

    public class ActionReverseSequence : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(1);

            CCActionInterval move1 = CCMoveBy.actionWithDuration(1, new CCPoint(250,0));
            CCActionInterval move2 = CCMoveBy.actionWithDuration(1, new CCPoint(0,50));
            CCFiniteTimeAction seq = CCSequence.actions( move1, move2, move1.reverse());
            CCFiniteTimeAction action = CCSequence.actions( seq, seq.reverse());

            m_grossini.runAction(action);
        }

        public override string subtitle()
        {
            return "Reverse a sequence";
        }
    };

    public class ActionReverseSequence2 : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(2);


            // Test:
            //   Sequence should work both with IntervalAction and InstantActions
            CCActionInterval move1 = CCMoveBy.actionWithDuration(1, new CCPoint(250,0));
            CCActionInterval move2 = CCMoveBy.actionWithDuration(1, new CCPoint(0,50));
            CCToggleVisibility tog1 = new CCToggleVisibility();
            CCToggleVisibility tog2 = new CCToggleVisibility();
            CCFiniteTimeAction seq = CCSequence.actions( move1, tog1, move2, tog2, move1.reverse());
            CCActionInterval action = CCRepeat.actionWithAction((CCActionInterval)(CCSequence.actions(seq, seq.reverse())), 3);

            // Test:
            //   Also test that the reverse of Hide is Show, and vice-versa
            m_kathia.runAction(action);

            CCActionInterval move_tamara = CCMoveBy.actionWithDuration(1, new CCPoint(100,0));
            CCActionInterval move_tamara2 = CCMoveBy.actionWithDuration(1, new CCPoint(50,0));
            CCActionInstant hide = new CCHide();
            CCFiniteTimeAction seq_tamara = CCSequence.actions( move_tamara, hide, move_tamara2);
            CCFiniteTimeAction seq_back = seq_tamara.reverse();
            m_tamara.runAction( CCSequence.actions( seq_tamara, seq_back));
        }
        public override string subtitle()
        {
            return "Reverse sequence 2";
        }

    };

    public class ActionRepeat : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            alignSpritesLeft(2);

            CCActionInterval a1 = CCMoveBy.actionWithDuration(1, new CCPoint(150,0));
            CCActionInterval action1 = CCRepeat.actionWithAction(
                CCSequence.actions( CCPlace.actionWithPosition(new CCPoint(60,60)), a1) , 
                3); 
            CCAction action2 = CCRepeatForever.actionWithAction(
                (CCActionInterval)(CCSequence.actions((CCActionInterval)(a1.copy()), a1.reverse()))
                );

            m_kathia.runAction(action1);
            m_tamara.runAction(action2);
        }

        public override string subtitle()
        {
            return "Repeat / RepeatForever actions";
        }
    };

    public class ActionOrbit : ActionsDemo
    {
        public override void onEnter()
        {
            // todo : CCOrbitCamera hasn't been implement

            base.onEnter();

            centerSprites(3);

            CCOrbitCamera orbit1 = CCOrbitCamera.actionWithDuration(2, 1, 0, 0, 180, 0, 0);
            CCFiniteTimeAction action1 = CCSequence.actions(
                orbit1,
                orbit1.reverse());

            CCOrbitCamera orbit2 = CCOrbitCamera.actionWithDuration(2, 1, 0, 0, 180, -45, 0);
            CCFiniteTimeAction action2 = CCSequence.actions(
                orbit2,
                orbit2.reverse());

            CCOrbitCamera orbit3 = CCOrbitCamera.actionWithDuration(2, 1, 0, 0, 180, 90, 0);
            CCFiniteTimeAction action3 = CCSequence.actions(
                orbit3,
                orbit3.reverse());

            m_kathia.runAction(CCRepeatForever.actionWithAction((CCActionInterval)action1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)action2));
            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)action3));

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(100, -100));
            CCFiniteTimeAction move_back = move.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(move, move_back);
            CCAction rfe = CCRepeatForever.actionWithAction((CCActionInterval)seq);
            m_kathia.runAction(rfe);
            m_tamara.runAction((CCAction)(rfe.copy()));
            m_grossini.runAction((CCAction)(rfe.copy()));
        }

        public override string subtitle()
        {
            return "OrbitCamera action";
        }

    };

    public class ActionFollow : ActionsDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            centerSprites(1);
            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_grossini.position = new CCPoint(-200, s.height / 2);
            CCActionInterval move      = CCMoveBy.actionWithDuration(2, new CCPoint(s.width * 3, 0));
            CCFiniteTimeAction move_back = move.reverse();
            CCFiniteTimeAction seq     = CCSequence.actions(move, move_back);
            CCAction rep               = CCRepeatForever.actionWithAction((CCActionInterval)seq);

            m_grossini.runAction(rep);

            this.runAction(CCFollow.actionWithTarget(m_grossini, new CCRect(0, 0, s.width * 2 - 100, s.height)));
        }

        public override string subtitle()
        {
            return "Follow action";
        }
    };

}