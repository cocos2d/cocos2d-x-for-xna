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
    public class CCEaseIn : CCEaseRateAction
    {

        public override void update(float time)
        {
            m_pOther.update((float)Math.Pow(time, m_fRate));
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCEaseIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseIn;
            }
            else
            {
                pCopy = new CCEaseIn();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fRate);

            return pCopy;
        }
        /// <summary>
        /// Creates the action with the inner action and the rate parameter 
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fRate"></param>
        /// <returns></returns>
        public static new CCEaseIn actionWithAction(CCActionInterval pAction, float fRate)
        {
            CCEaseIn pRet = new CCEaseIn();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fRate))
                {
                    //pRet.autorelease();
                }
                else
                {
                    //CC_SAFE_RELEASE_NULL(pRet);
                }
            }
            return pRet;
        }
    }
}
