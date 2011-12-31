using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class EaseSpriteDemo : CCLayer
    {
        protected CCSprite m_grossini;
        protected CCSprite m_tamara;
        protected CCSprite m_kathia;

        protected String m_strTitle;

        public EaseSpriteDemo() { }

        public virtual String title()
        {
            return "No title";
        }
        public override void onEnter()
        {
            base.onEnter();
            m_grossini = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            m_tamara = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            m_kathia = CCSprite.spriteWithFile(TestResource.s_pPathSister2);

            addChild(m_grossini, 3);
            addChild(m_kathia, 2);
            addChild(m_tamara, 1);

            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_grossini.position = new CCPoint(60, 50);
            m_kathia.position = new CCPoint(60, 150);
            m_tamara.position = new CCPoint(60, 250);

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 32);
            addChild(label);
            label.position = new CCPoint(s.width / 2, s.height - 50);

            CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, nextCallback);

            CCMenu menu = CCMenu.menuWithItems(item1, item2, item3);
            menu.position = CCPoint.Zero;
            item1.position = new CCPoint(s.width / 2 - 100, 30);
            item2.position = new CCPoint(s.width / 2, 30);
            item3.position = new CCPoint(s.width / 2 + 100, 30);

            addChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.addChild(EaseTest.restartEaseAction());
            CCDirector.sharedDirector().replaceScene(s);
        }
        public void nextCallback(CCObject pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.addChild(EaseTest.nextEaseAction());
            CCDirector.sharedDirector().replaceScene(s);
            ;
        }
        public void backCallback(CCObject pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.addChild(EaseTest.backEaseAction());
            CCDirector.sharedDirector().replaceScene(s);
        }

        public void positionForTwo()
        {
            m_grossini.position = new CCPoint(60, 120);
            m_tamara.position = new CCPoint(60, 220);
            m_kathia.visible = false;
        }
    }

    public class SpriteEase : EaseSpriteDemo
    {

        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = (CCActionInterval)move.reverse();

            CCActionInterval move_ease_in = CCEaseIn.actionWithAction(move.copy() as CCActionInterval, 3);
            CCActionInterval move_ease_in_back = move_ease_in.reverse() as CCActionInterval;

            CCActionInterval move_ease_out = CCEaseOut.actionWithAction(move.copy() as CCActionInterval, 3);
            CCActionInterval move_ease_out_back = move_ease_out.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);

            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_in, move_ease_in_back);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_out, move_ease_out_back);

            CCAction a2 = m_grossini.runAction(CCRepeatForever.actionWithAction(seq1 as CCActionInterval));
            a2.tag = 1;

            CCAction a1 = m_tamara.runAction(CCRepeatForever.actionWithAction(seq2 as CCActionInterval));
            a1.tag = 1;
            CCAction a = m_kathia.runAction(CCRepeatForever.actionWithAction(seq3 as CCActionInterval));
            a.tag = 1;

            schedule(new SEL_SCHEDULE(testStopAction), 6.0f);
        }

        public override String title()
        {
            return "EaseIn - EaseOut - Stop";
        }

        public void testStopAction(float dt)
        {
            unschedule(new SEL_SCHEDULE(testStopAction));
            m_kathia.stopActionByTag(1);
            m_tamara.stopActionByTag(1);
            m_grossini.stopActionByTag(1);
        }
    }

    public class SpriteEaseInOut : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));


            CCActionInterval move_ease_inout1 = CCEaseInOut.actionWithAction(move.copy() as CCActionInterval, 2.0f);
            CCActionInterval move_ease_inout_back1 = move_ease_inout1.reverse() as CCActionInterval;

            CCActionInterval move_ease_inout2 = CCEaseInOut.actionWithAction(move.copy() as CCActionInterval, 3.0f);
            CCActionInterval move_ease_inout_back2 = move_ease_inout2.reverse() as CCActionInterval;

            CCActionInterval move_ease_inout3 = CCEaseInOut.actionWithAction(move.copy() as CCActionInterval, 4.0f);
            CCActionInterval move_ease_inout_back3 = move_ease_inout3.reverse() as CCActionInterval;


            CCFiniteTimeAction seq1 = CCSequence.actions(move_ease_inout1, move_ease_inout_back1);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_inout2, move_ease_inout_back2);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_inout3, move_ease_inout_back3);

            m_tamara.runAction(CCRepeatForever.actionWithAction(seq1 as CCActionInterval));
            m_kathia.runAction(CCRepeatForever.actionWithAction(seq2 as CCActionInterval));
            m_grossini.runAction(CCRepeatForever.actionWithAction(seq3 as CCActionInterval));
        }
        public override String title()
        {
            return "EaseInOut and rates";
        }
    }

    public class SpriteEaseExponential : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease_in = CCEaseExponentialIn.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_in_back = move_ease_in.reverse() as CCActionInterval;

            CCActionInterval move_ease_out = CCEaseExponentialOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_out_back = move_ease_out.reverse() as CCActionInterval;


            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_in, move_ease_in_back);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_out, move_ease_out_back);


            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
            m_kathia.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq3));
        }
        public override String title()
        {
            return "ExpIn - ExpOut actions";
        }
    }

    public class SpriteEaseExponentialInOut : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease = CCEaseExponentialInOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_back = move_ease.reverse() as CCActionInterval;	//-. reverse()

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease, move_ease_back);

            this.positionForTwo();

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
        }
        public override String title()
        {
            return "EaseExponentialInOut action";
        }
    }

    public class SpriteEaseSine : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease_in = CCEaseSineIn.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_in_back = move_ease_in.reverse() as CCActionInterval;

            CCActionInterval move_ease_out = CCEaseSineOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_out_back = move_ease_out.reverse() as CCActionInterval;


            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_in, move_ease_in_back);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_out, move_ease_out_back);


            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
            m_kathia.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq3));
        }
        public override String title()
        {
            return "EaseSineIn - EaseSineOut";
        }
    }

    public class SpriteEaseSineInOut : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease = CCEaseSineInOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_back = move_ease.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease, move_ease_back);

            this.positionForTwo();

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
        }
        public override String title()
        {
            return "EaseSineInOut action";
        }
    }

    public class SpriteEaseElastic : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease_in = CCEaseElasticIn.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_in_back = move_ease_in.reverse() as CCActionInterval;

            CCActionInterval move_ease_out = CCEaseElasticOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_out_back = move_ease_out.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_in, move_ease_in_back);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_out, move_ease_out_back);

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
            m_kathia.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq3));
        }
        public override String title()
        {
            return "Elastic In - Out actions";
        }
    }
    /*
        public class SpriteEaseElasticInOut : EaseSpriteDemo
        {
            public override void onEnter()
            {
                base.onEnter();
	
                CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350,0));

                CCActionInterval move_ease_inout1 = CCEaseElasticInOut.actionWithAction((CCActionInterval)(move.copy().autorelease()), 0.3f);
                CCActionInterval move_ease_inout_back1 = move_ease_inout1.reverse() as CCActionInterval;
	
                CCActionInterval move_ease_inout2 = CCEaseElasticInOut.actionWithAction((CCActionInterval)(move.copy().autorelease()), 0.45f);
                CCActionInterval move_ease_inout_back2 = move_ease_inout2.reverse() as CCActionInterval;
	
                CCActionInterval move_ease_inout3 = CCEaseElasticInOut.actionWithAction((CCActionInterval)(move.copy().autorelease()), 0.6f);
                CCActionInterval move_ease_inout_back3 = move_ease_inout3.reverse() as CCActionInterval;
	
	
                CCFiniteTimeAction seq1 = CCSequence.actions( move_ease_inout1, move_ease_inout_back1);
                CCFiniteTimeAction seq2 = CCSequence.actions( move_ease_inout2, move_ease_inout_back2);
                CCFiniteTimeAction seq3 = CCSequence.actions( move_ease_inout3, move_ease_inout_back3);
	
                m_tamara.runAction( CCRepeatForever.actionWithAction((CCActionInterval)seq1));
                m_kathia.runAction( CCRepeatForever.actionWithAction((CCActionInterval)seq2));
                m_grossini.runAction( CCRepeatForever.actionWithAction((CCActionInterval)seq3)); 
            }
            public override String title()
            {
                return "EaseElasticInOut action";
            }
        }
        */
    public class SpriteEaseBounce : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease_in = CCEaseBounceIn.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_in_back = move_ease_in.reverse() as CCActionInterval;

            CCActionInterval move_ease_out = CCEaseBounceOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_out_back = move_ease_out.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_in, move_ease_in_back);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_out, move_ease_out_back);

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
            m_kathia.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq3));
        }
        public override String title()
        {
            return "Bounce In - Out actions";
        }
    }

    public class SpriteEaseBounceInOut : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease = CCEaseBounceInOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_back = move_ease.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease, move_ease_back);

            this.positionForTwo();

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
        }
        public override String title()
        {
            return "EaseBounceInOut action";
        }
    }

    public class SpriteEaseBack : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();
            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease_in = CCEaseBackIn.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_in_back = move_ease_in.reverse() as CCActionInterval;

            CCActionInterval move_ease_out = CCEaseBackOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_out_back = move_ease_out.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease_in, move_ease_in_back);
            CCFiniteTimeAction seq3 = CCSequence.actions(move_ease_out, move_ease_out_back);

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
            m_kathia.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq3));
        }
        public override String title()
        {
            return "Back In - Out actions";
        }
    }

    public class SpriteEaseBackInOut : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();
            CCActionInterval move = CCMoveBy.actionWithDuration(3, new CCPoint(350, 0));
            CCActionInterval move_back = move.reverse() as CCActionInterval;

            CCActionInterval move_ease = CCEaseBackInOut.actionWithAction((CCActionInterval)(move.copy()));
            CCActionInterval move_ease_back = move_ease.reverse() as CCActionInterval;

            CCFiniteTimeAction seq1 = CCSequence.actions(move, move_back);
            CCFiniteTimeAction seq2 = CCSequence.actions(move_ease, move_ease_back);

            this.positionForTwo();

            m_grossini.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq1));
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq2));
        }
        public override String title()
        {
            return "EaseBackInOut action";
        }
    }

    public class SpeedTest : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            // rotate and jump
            CCActionInterval jump1 = CCJumpBy.actionWithDuration(4, new CCPoint(-400, 0), 100, 4);
            CCActionInterval jump2 = (CCActionInterval)jump1.reverse();
            CCActionInterval rot1 = CCRotateBy.actionWithDuration(4, 360*2);
            CCActionInterval rot2 = (CCActionInterval)rot1.reverse();

            CCFiniteTimeAction seq3_1 = CCSequence.actions(jump2, jump1);
            CCFiniteTimeAction seq3_2 = CCSequence.actions(rot1, rot2);
            CCFiniteTimeAction spawn = CCSpawn.actions(seq3_1, seq3_2);
            CCSpeed action = CCSpeed.actionWithAction(CCRepeatForever.actionWithAction((CCActionInterval)spawn), 1.0f);


            action.tag = EaseTest.kTagAction1;

            CCAction action2 = (CCAction)(action.copy());
            CCAction action3 = (CCAction)(action.copy());

            action2.tag = EaseTest.kTagAction1;
            action3.tag = EaseTest.kTagAction1;

            m_grossini.runAction(action2);
            //m_grossini.runAction(CCRepeat.actionWithAction(CCSequence.actions(jump2, jump1), 5));
            m_tamara.runAction(action3);
            m_kathia.runAction(action);
            
            this.schedule(new SEL_SCHEDULE(altertime), 1.0f);//:@selector(altertime:) interval:1.0f];
        }

        public void altertime(float dt)
        {
            CCSpeed action1 = (CCSpeed)(m_grossini.getActionByTag(EaseTest.kTagAction1));
            CCSpeed action2 = (CCSpeed)(m_tamara.getActionByTag(EaseTest.kTagAction1));
            CCSpeed action3 = (CCSpeed)(m_kathia.getActionByTag(EaseTest.kTagAction1));

            Random rand = new Random();

            action1.speed = rand.Next(1000000) / 1000000.0f * 2;
            action2.speed = rand.Next(1000000) / 1000000.0f * 2;
            action3.speed = rand.Next(1000000) / 1000000.0f * 2;
        }

        public override String title()
        {
            return "Speed action";
        }
    }

    public class SchedulerTest : EaseSpriteDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            // rotate and jump
            CCActionInterval jump1 = CCJumpBy.actionWithDuration(4, new CCPoint(-400, 0), 100, 4);
            CCActionInterval jump2 = jump1.reverse() as CCActionInterval;
            CCActionInterval rot1 = CCRotateBy.actionWithDuration(4, 360 * 2);
            CCActionInterval rot2 = rot1.reverse() as CCActionInterval;

            CCFiniteTimeAction seq3_1 = CCSequence.actions(jump2, jump1);
            CCFiniteTimeAction seq3_2 = CCSequence.actions(rot1, rot2);
            CCFiniteTimeAction spawn = CCSpawn.actions(seq3_1, seq3_2);
            CCFiniteTimeAction action = CCRepeatForever.actionWithAction((CCActionInterval)spawn);

            CCRepeatForever action2 = (CCRepeatForever)(action.copy());
            CCRepeatForever action3 = (CCRepeatForever)(action.copy());


            m_grossini.runAction(CCSpeed.actionWithAction((CCActionInterval)action, 0.5f));
            m_tamara.runAction(CCSpeed.actionWithAction((CCActionInterval)action2, 1.5f));
            m_kathia.runAction(CCSpeed.actionWithAction((CCActionInterval)action3, 1.0f));

            CCParticleSystem emitter = CCParticleFireworks.node();
            emitter.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
            addChild(emitter);
        }
        public override String title()
        {
            return "Scheduler scaleTime Test";
        }
    }

    public class EaseActionsTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = EaseTest.nextEaseAction();
            addChild(pLayer);

            CCDirector.sharedDirector().replaceScene(this);
        }
    }

    /// <summary>
    /// 这里放的是-x原来的全局函数
    /// </summary>

    public static class EaseTest
    {
        static int sceneIdx = -1;
        public const int MAX_LAYER = 14;
        public const int kTagAction1 = 1;
        public const int kTagAction2 = 2;
        public const int kTagSlider = 1;

        public static CCLayer createEaseLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new SpriteEase();
                case 1: return new SpriteEaseInOut();
                case 2: return new SpriteEaseExponential();
                case 3: return new SpriteEaseExponentialInOut();
                case 4: return new SpriteEaseSine();
                case 5: return new SpriteEaseSineInOut();
                case 6: return new SpriteEaseElastic();
                //case 7: return new SpriteEaseElasticInOut();
                case 7: return new SpriteEaseBounce();
                case 8: return new SpriteEaseBounceInOut();
                case 9: return new SpriteEaseBack();
                case 10: return new SpriteEaseBackInOut();
                case 11: return new SpeedTest();
                case 12: return new SchedulerTest();
            }
            return null;
        }
        public static CCLayer nextEaseAction()
        {
            sceneIdx++;
            sceneIdx %= MAX_LAYER;

            CCLayer pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }
        public static CCLayer backEaseAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0) sceneIdx += total;
            CCLayer pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }
        public static CCLayer restartEaseAction()
        {
            CCLayer pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }
    }
}
