/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using System.Diagnostics;
namespace cocos2d
{
    /// <summary>
    /// @brief Changes the speed of an action, making it take longer (speed>1)
    ///  or less (speed<1) time.
    /// Useful to simulate 'slow motion' or 'fast forward' effect.
    /// @warning This action can't be Sequenceable because it is not an CCIntervalAction
    /// </summary>
    public class CCSpeed : CCAction
    {
        public CCSpeed() { }

        /// <summary>
        /// initializes the action
        /// </summary>
        public bool initWithAction(CCActionInterval action, float fRate)
        {
            Debug.Assert(action != null);

            m_pInnerAction = action;
            m_fSpeed = fRate;

            return true;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = null;
            CCSpeed ret = null;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCSpeed)tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCSpeed();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(zone);

            ret.initWithAction((CCActionInterval)m_pInnerAction.copy(), m_fSpeed);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_pInnerAction.startWithTarget(target);
        }

        public override void stop()
        {
            m_pInnerAction.stop();
            base.stop();
        }

        public override void step(float dt)
        {
            m_pInnerAction.step(dt * m_fSpeed);
        }

        public override bool isDone()
        {
            return m_pInnerAction.isDone();
        }

        public virtual CCActionInterval reverse()
        {
            return (CCActionInterval)(CCAction)CCSpeed.actionWithAction((CCActionInterval)m_pInnerAction.reverse(), m_fSpeed);
        }

        public static CCSpeed actionWithAction(CCActionInterval action, float fRate)
        {
            CCSpeed ret = new CCSpeed();

            if (ret != null && ret.initWithAction(action, fRate))
            {
                return ret;
            }

            return null;
        }

        #region Properties

        protected float m_fSpeed;
        public float speed
        {
            get
            {
                return m_fSpeed;
            }
            set
            {
                m_fSpeed = value;
            }
        }

        protected CCActionInterval m_pInnerAction;

        #endregion
    }
}
