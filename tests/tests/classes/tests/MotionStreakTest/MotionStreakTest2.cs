using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class MotionStreakTest2 : MotionStreakTest
    {
        string s_streak = "Images/streak";
        protected CCNode m_root;
        protected CCNode m_target;
        protected CCMotionStreak m_streak;

        public override void onEnter()
        {
            base.onEnter();
            this.isTouchEnabled = true;

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // create the streak object and add it to the scene
            m_streak = CCMotionStreak.streakWithFade(3, 3, 64, new ccColor3B(255, 255, 255), s_streak);
            addChild(m_streak);

            m_streak.position = (new CCPoint(s.width / 2, s.height / 2));
        }

        public override void ccTouchesMoved(List<CCTouch> touches, CCEvent event_)
        {
            var it = touches.FirstOrDefault();
            CCTouch touch = (CCTouch)(it);

            CCPoint touchLocation = touch.locationInView(touch.view());
            touchLocation = CCDirector.sharedDirector().convertToGL(touchLocation);

            m_streak.position = touchLocation;
        }

        public override string title()
        {
            return "MotionStreak test";
        }
    }
}
