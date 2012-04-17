/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// @brief Progress from a percentage to another percentage
    /// @since v0.99.1
    /// </summary>
    public class CCProgressFromTo : CCActionInterval
    {
        /// <summary>
        /// Initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public bool initWithDuration(float duration, float fFromPercentage, float fToPercentage)
        {
            // if (CCActionInterval::initWithDuration(duration))
            if (initWithDuration(duration))
            {
                m_fTo = fToPercentage;
                m_fFrom = fFromPercentage;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCProgressFromTo pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCProgressFromTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCProgressFromTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            // CCActionInterval::copyWithZone(pZone);
            copyWithZone(pZone);
            pCopy.initWithDuration(m_fDuration, m_fFrom, m_fTo);

            // CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCProgressFromTo.actionWithDuration(m_fDuration, m_fTo, m_fFrom);
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
        }

        public override void update(float time)
        {
            ((CCProgressTimer)(m_pTarget)).Percentage = m_fFrom + (m_fTo - m_fFrom) * time;
        }

        /// <summary>
        /// Creates and initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public static CCProgressFromTo actionWithDuration(float duration, float fFromPercentage, float fToPercentage)
        {
            CCProgressFromTo pProgressFromTo = new CCProgressFromTo();
            pProgressFromTo.initWithDuration(duration, fFromPercentage, fToPercentage);

            return pProgressFromTo;
        }

        protected float m_fTo;
        protected float m_fFrom;
    }
}
