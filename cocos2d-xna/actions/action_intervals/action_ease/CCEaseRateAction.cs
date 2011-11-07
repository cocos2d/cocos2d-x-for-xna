using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCEaseRateAction:CCActionEase
    {
        protected float m_fRate;
        /// <summary>
        /// set rate value for the actions
        /// </summary>
        /// <param name="rate"></param>
        public void setRate(float rate) { m_fRate = rate; }
        /// <summary>
        /// get rate value for the actions
        /// </summary>
        /// <returns></returns>
        public float getRate() { return m_fRate; }

        /// <summary>
        /// Initializes the action with the inner action and the rate parameter
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public bool initWithAction(CCActionInterval pAction, float fRate) 
        {
            throw new NotImplementedException();
        }

        public virtual CCObject copyWithZone(CCZone pZone)
        {
            throw new NotImplementedException();
        }
        public virtual CCActionInterval reverse()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the action with the inner action and the rate parameter
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public static CCEaseRateAction actionWithAction(CCActionInterval pAction, float fRate)
        {
            throw new NotImplementedException();
        }
    }
}
