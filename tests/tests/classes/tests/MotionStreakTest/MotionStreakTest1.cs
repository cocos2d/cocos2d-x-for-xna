using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class MotionStreakTest1 : MotionStreakTest
    {
        string s_streak = "Images/streak";
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        protected CCNode m_root;
        protected CCNode m_target;
        protected CCMotionStreak m_streak;

        public override void onEnter()
        {
            base.onEnter();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // the root object just rotates around
            m_root = CCSprite.spriteWithFile(s_pPathR1);
            addChild(m_root, 1);
            m_root.position = (new CCPoint(s.width / 2, s.height / 2));

            // the target object is offset from root, and the streak is moved to follow it
            m_target = CCSprite.spriteWithFile(s_pPathR1);
            m_root.addChild(m_target);
            m_target.position = (new CCPoint(100, 0));

            // create the streak object and add it to the scene
            m_streak = CCMotionStreak.streakWithFade(2f, 3f, 32f, new ccColor3B(0, 255, 0), s_streak);
            addChild(m_streak);
            // schedule an update on each frame so we can syncronize the streak with the target
            schedule(onUpdate);

            CCActionInterval a1 = CCRotateBy.actionWithDuration(2, 360);

            CCAction action1 = CCRepeatForever.actionWithAction(a1);
            CCActionInterval motion = CCMoveBy.actionWithDuration(2, new CCPoint(100, 0));
            m_root.runAction(CCRepeatForever.actionWithAction((CCActionInterval)(CCSequence.actions(motion, motion.reverse()))));
            m_root.runAction(action1);
            CCActionInterval colorAction = CCRepeatForever.actionWithAction((CCActionInterval)CCSequence.actions(
        CCTintTo.actionWithDuration(0.2f, 255, 0, 0),
        CCTintTo.actionWithDuration(0.2f, 0, 255, 0),
        CCTintTo.actionWithDuration(0.2f, 0, 0, 255),
        CCTintTo.actionWithDuration(0.2f, 0, 255, 255),
        CCTintTo.actionWithDuration(0.2f, 255, 255, 0),
        CCTintTo.actionWithDuration(0.2f, 255, 0, 255),
        CCTintTo.actionWithDuration(0.2f, 255, 255, 255)
                ));
            m_streak.runAction(colorAction);
            /*
    CCActionInterval *colorAction = CCRepeatForever::create((CCActionInterval *)CCSequence::create(
        CCTintTo::create(0.2f, 255, 0, 0),
        CCTintTo::create(0.2f, 0, 255, 0),
        CCTintTo::create(0.2f, 0, 0, 255),
        CCTintTo::create(0.2f, 0, 255, 255),
        CCTintTo::create(0.2f, 255, 255, 0),
        CCTintTo::create(0.2f, 255, 0, 255),
        CCTintTo::create(0.2f, 255, 255, 255),
        NULL));

    streak->runAction(colorAction);
             */
        }

        public void onUpdate(float delta)
        {
            m_streak.position = (m_target.convertToWorldSpace(new CCPoint(0, 0)));
        }

        public override string title()
        {
            return "MotionStreak test 1";
        }
    }
}
