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
    public class CCSkewBy : CCSkewTo
    {
        public override bool initWithDuration(float t, float sx, float sy)
        {
            bool bRet = false;

            if (base.initWithDuration(t, sx, sy))
            {
                m_fSkewX = sx;
                m_fSkewY = sy;

                bRet = true;
            }

            return bRet;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_fDeltaX = m_fSkewX;
            m_fDeltaY = m_fSkewY;
            m_fEndSkewX = m_fStartSkewX + m_fDeltaX;
            m_fEndSkewY = m_fStartSkewY + m_fDeltaY;
        }

        public override CCFiniteTimeAction reverse()
        {
            return actionWithDuration(m_fDuration, -m_fSkewX, -m_fSkewY);
        }

        public new static CCSkewBy actionWithDuration(float t, float deltaSkewX, float deltaSkewY)
        {
            CCSkewBy pSkewBy = new CCSkewBy();
            if (pSkewBy != null)
            {
                if (pSkewBy.initWithDuration(t, deltaSkewX, deltaSkewY))
                {
                    //pSkewBy->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pSkewBy);
                }
            }

            return pSkewBy;
        }
    }
}
