/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
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
    /** @brief Fades an object that implements the CCRGBAProtocol protocol. It modifies the opacity from the current value to a custom one.
    @warning This action doesn't support "reverse"
    */
    public class CCFadeTo : CCActionInterval
    {
        public bool initWithDuration(float duration, byte opacity)
        {
            if (base.initWithDuration(duration))
            {
                m_toOpacity = opacity;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFadeTo pCopy = null;
            if(pZone != null && pZone.m_pCopyObject != null) 
            {
                //in case of being called at sub class
                pCopy = (CCFadeTo)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFadeTo();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithDuration(m_fDuration, m_toOpacity);
	
            return pCopy;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);

            ICCRGBAProtocol pRGBAProtocol = pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                m_fromOpacity = pRGBAProtocol.Opacity;
            }
        }

        public override void update(float time)
        {
            ICCRGBAProtocol pRGBAProtocol = m_pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte)(m_fromOpacity + (m_toOpacity - m_fromOpacity) * time);
            }
        }

        /** creates an action with duration and opacity */
        public static CCFadeTo actionWithDuration(float duration, byte opacity)
        {
            CCFadeTo pFadeTo = new CCFadeTo();
            pFadeTo.initWithDuration(duration, opacity);

             return pFadeTo;
        }


    protected byte m_toOpacity;
    protected byte m_fromOpacity;

    }
}