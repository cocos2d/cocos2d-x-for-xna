/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 Fulcrum Mobile Network, Inc

http://www.cocos2d-x.org
http://www.openxlive.com

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
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCSkewTo : CCActionInterval
    {
        public CCSkewTo() { }
        
        public virtual bool initWithDuration(float t, float sx, float sy)
        {
            bool bRet = false;

            if (base.initWithDuration(t))
            {
                m_fEndSkewX = sx;
                m_fEndSkewY = sy;

                bRet = true;
            }

            return bRet;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCSkewTo pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCSkewTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCSkewTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(m_fDuration, m_fEndSkewX, m_fEndSkewY);

            //CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override void startWithTarget(CCNode pTarget)
        {

            base.startWithTarget(pTarget);

            m_fStartSkewX = pTarget.skewX;

            if (m_fStartSkewX > 0)
            {
                m_fStartSkewX = m_fStartSkewX % 180f;
            }
            else
            {
                m_fStartSkewX = m_fStartSkewX % -180f;
            }

            m_fDeltaX = m_fEndSkewX - m_fStartSkewX;

            if (m_fDeltaX > 180)
            {
                m_fDeltaX -= 360;
            }
            if (m_fDeltaX < -180)
            {
                m_fDeltaX += 360;
            }

            m_fStartSkewY = pTarget.skewY;

            if (m_fStartSkewY > 0)
            {
                m_fStartSkewY = m_fStartSkewY % 360f;
            }
            else
            {
                m_fStartSkewY = m_fStartSkewY % -360f;
            }

            m_fDeltaY = m_fEndSkewY - m_fStartSkewY;

            if (m_fDeltaY > 180)
            {
                m_fDeltaY -= 360;
            }
            if (m_fDeltaY < -180)
            {
                m_fDeltaY += 360;
            }
        }

        public override void update(float time)
        {
            m_pTarget.skewX = m_fStartSkewX + m_fDeltaX * time;
            m_pTarget.skewY = m_fStartSkewY + m_fDeltaY * time;
        }

        public static CCSkewTo actionWithDuration(float t, float sx, float sy)
        {

            CCSkewTo pSkewTo = new CCSkewTo();

            if (pSkewTo != null)
            {
                if (pSkewTo.initWithDuration(t, sx, sy))
                {
                    //pSkewTo->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pSkewTo);
                }
            }

            return pSkewTo;
        }

        protected float m_fSkewX;
        protected float m_fSkewY;
        protected float m_fStartSkewX;
        protected float m_fStartSkewY;
        protected float m_fEndSkewX;
        protected float m_fEndSkewY;
        protected float m_fDeltaX;
        protected float m_fDeltaY;
    }
}
