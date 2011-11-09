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
    /** @brief Places the node in a certain position
    */
    public class CCPlace : CCActionInstant
    {
        public CCPlace()
        {
            m_tPosition = new CCPoint();
        }

        ~CCPlace()
        {

        }

        public static CCPlace actionWithPosition(CCPoint pos) 
        {
	        CCPlace pRet = new CCPlace();

	        if (pRet != null && pRet.initWithPosition(pos)) 
            {
		        return pRet;
	        }

	        return null;
        }

        public bool initWithPosition(CCPoint pos) 
        {
	        m_tPosition = pos;
	        return true;
        }

        public override CCObject copyWithZone(CCZone pZone) 
        {
	        CCZone pNewZone = null;
	        CCPlace pRet = null;

	        if (pZone != null && pZone.m_pCopyObject != null) 
            {
		        pRet = (CCPlace) (pZone.m_pCopyObject);
	        } else {
		        pRet = new CCPlace();
		        pZone = pNewZone = new CCZone(pRet);
	        }

	        base.copyWithZone(pZone);
	        pRet.initWithPosition(m_tPosition);
	        return pRet;
        }

        public override void startWithTarget(CCNode pTarget) 
        {
	        base.startWithTarget(pTarget);
	        m_pTarget.position = m_tPosition;
        }

        private CCPoint m_tPosition;
    }
}