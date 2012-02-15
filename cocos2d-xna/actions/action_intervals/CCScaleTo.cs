/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
    public class CCScaleTo : CCActionInterval
    {
        /// <summary>
        /// initializes the action with the same scale factor for X and Y
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool initWithDuration(float duration, float s)
        {
            if (base.initWithDuration(duration))
            {
                m_fEndScaleX = s;
                m_fEndScaleY = s;

                return true;
            }

            return false;
        }

        /// <summary>
        /// initializes the action with and X factor and a Y factor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public bool initWithDuration(float duration, float sx, float sy)
        {
            if (base.initWithDuration(duration))
            {
                m_fEndScaleX = sx;
                m_fEndScaleY = sy;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCScaleTo pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCScaleTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCScaleTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(m_fDuration, m_fEndScaleX, m_fEndScaleY);

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }
        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_fStartScaleX = pTarget.scaleX;
            m_fStartScaleY = pTarget.scaleY;
            m_fDeltaX = m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fEndScaleY - m_fStartScaleY;
        }
        public override void update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.scaleX = m_fStartScaleX + m_fDeltaX * time;
                m_pTarget.scaleY = m_fStartScaleY + m_fDeltaY * time;
            }
        }

        /// <summary>
        /// creates the action with the same scale factor for X and Y
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static CCScaleTo actionWithDuration(float duration, float s)
        {
            CCScaleTo pScaleTo = new CCScaleTo();
            pScaleTo.initWithDuration(duration, s);
            //pScaleTo->autorelease();

            return pScaleTo;
        }

        /// <summary>
        /// creates the action with and X factor and a Y factor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public static CCScaleTo actionWithDuration(float duration, float sx, float sy)
        {
            CCScaleTo pScaleTo = new CCScaleTo();
            pScaleTo.initWithDuration(duration, sx, sy);
            //pScaleTo->autorelease();

            return pScaleTo;
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
