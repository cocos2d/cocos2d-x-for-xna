/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

http://www.cocos2d-x.org

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
namespace cocos2d
{
    /** 
    @brief Flips the sprite vertically
    @since v0.99.0
    */
    public class CCFlipY : CCActionInstant
    {
        public CCFlipY()
        {
            m_bFlipY = false;
        }

        ~CCFlipY()
        {

        }

        public static CCFlipY actionWithFlipY(bool y)
        {
            CCFlipY pRet = new CCFlipY();

            if (pRet != null && pRet.initWithFlipY(y))
            {
                return pRet;
            }

            return null;
        }

        public bool initWithFlipY(bool y)
        {
            m_bFlipY = y;
            return true;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            ((CCSprite)(pTarget)).IsFlipY = m_bFlipY;
        }

        public override CCFiniteTimeAction reverse()
        {
            return actionWithFlipY(!m_bFlipY);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFlipY pRet = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCFlipY)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCFlipY();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);
            pRet.initWithFlipY(m_bFlipY);
            return pRet;
        }

        private bool m_bFlipY;
    }
}