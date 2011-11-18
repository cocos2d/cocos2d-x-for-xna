using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCScaleTo : CCActionInterval
    {
        /** initializes the action with the same scale factor for X and Y */
        public bool initWithDuration(float duration, float s)
        {
            throw new NotImplementedException();
        }

        /** initializes the action with and X factor and a Y factor */
        public bool initWithDuration(float duration, float sx, float sy)
        {
            throw new NotImplementedException();
        }

        public virtual CCObject copyWithZone(CCZone pZone)
        {
            throw new NotImplementedException();
        }
        public virtual void startWithTarget(CCNode pTarget)
        {
            throw new NotImplementedException();
        }
        public virtual void update(float time)
        {
            throw new NotImplementedException();
        }

        /** creates the action with the same scale factor for X and Y */
        public static CCScaleTo actionWithDuration(float duration, float s)
        {
            throw new NotImplementedException();
        }

        /** creates the action with and X factor and a Y factor */
        public static CCScaleTo actionWithDuration(float duration, float sx, float sy)
        {
            throw new NotImplementedException();
        }

        protected float m_fScaleX;
        protected float m_fScaleY;
        protected float m_fStartScaleX;
        protected float m_fStartScaleY;
        protected float m_fEndScaleX;
        protected float m_fEndScaleY;
        protected float m_fDeltaX;
        protected float m_fDeltaY;
    }
}
