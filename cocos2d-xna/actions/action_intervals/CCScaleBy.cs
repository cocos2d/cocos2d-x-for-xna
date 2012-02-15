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
    public class CCScaleBy : CCScaleTo
    {
        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_fDeltaX = m_fStartScaleX * m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fStartScaleY * m_fEndScaleY - m_fStartScaleY;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCScaleBy.actionWithDuration(m_fDuration, 1 / m_fEndScaleX, 1 / m_fEndScaleY);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCScaleTo pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCScaleBy)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCScaleBy();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(m_fDuration, m_fEndScaleX, m_fEndScaleY);

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        /// <summary>
        /// creates the action with the same scale factor for X and Y
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public new static CCScaleBy actionWithDuration(float duration, float s)
        {
            CCScaleBy pScaleBy = new CCScaleBy();
            pScaleBy.initWithDuration(duration, s);
            //pScaleBy->autorelease();

            return pScaleBy;
        }

        /// <summary>
        ///  creates the action with and X factor and a Y factor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <returns></returns>
        public new static CCScaleBy actionWithDuration(float duration, float sx, float sy)
        {
            CCScaleBy pScaleBy = new CCScaleBy();
            pScaleBy.initWithDuration(duration, sx, sy);
            //pScaleBy->autorelease();

            return pScaleBy;
        }
    }
}
