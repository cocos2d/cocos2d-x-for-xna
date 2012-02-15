/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.


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
    /** 
    @brief An interval action is an action that takes place within a certain period of time.
    It has an start time, and a finish time. The finish time is the parameter
    duration plus the start time.

    These CCActionInterval actions have some interesting properties, like:
    - They can run normally (default)
    - They can run reversed with the reverse method
    - They can run with the time altered with the Accelerate, AccelDeccel and Speed actions.

    For example, you can simulate a Ping Pong effect running the action normally and
    then running it again in Reverse mode.

    Example:

    CCAction *pingPongAction = CCSequence::actions(action, action->reverse(), NULL);
    */
    public class CCActionInterval : CCFiniteTimeAction
    {
        /** initializes the action */
        public bool initWithDuration(float d)
        {
            m_fDuration = d;

            // prevent division by 0
            // This comparison could be in step:, but it might decrease the performance
            // by 3% in heavy based action games.
            if (m_fDuration == 0)
            {
                m_fDuration = (float)ccMacros.FLT_EPSILON;
            }

            m_elapsed = 0;
            m_bFirstTick = true;

            return true;
        }

        /** returns true if the action has finished */
        public override bool isDone()
        {
            return m_elapsed >= m_fDuration;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCActionInterval ret = null;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCActionInterval)(tmpZone.m_pCopyObject);
            }
            else
            {
                // action's base class , must be called using __super::copyWithZone(), after overriding from derived class
                Debug.Assert(false);

                ret = new CCActionInterval();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration);

            return ret;
        }

        public override void step(float dt)
        {
            if (m_bFirstTick)
            {
                m_bFirstTick = false;
                m_elapsed = 0;
            }
            else
            {
                m_elapsed += dt;
            }

            update(Math.Min(1, m_elapsed / m_fDuration));
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_elapsed = 0.0f;
            m_bFirstTick = true;
        }

        /// <summary>
        /// C# cannot return type of sub class if it override father function.
        /// In c++ this function return CCActionInterval, I don't know if it
        /// will be a problem. 
        /// Fix me if needed.
        /// </summary>
        /// <returns></returns>
        public override CCFiniteTimeAction reverse()
        {
            throw new NotImplementedException();
        }

        public void setAmplitudeRate(float amp)
        {
            Debug.Assert(false); 
        }

        public float getAmplitudeRate()
        {
            Debug.Assert(false);

            return 0;
        }

        public static CCActionInterval actionWithDuration(float d)
        {
            CCActionInterval ret = new CCActionInterval();
            ret.initWithDuration(d);

            return ret;
        }

        #region properties

        /** how many seconds had elapsed since the actions started to run. */
        protected float m_elapsed;
        public float elapsed
        {
            // read only
            get
            {
                return m_elapsed;
            }
        }

        #endregion

        protected bool m_bFirstTick;
    }
}
