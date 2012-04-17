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
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseElasticIn : CCEaseElastic
    {
        public override void update(float time) 
        {
            float newT = 0;

            if (time == 0 || time == 1)
            {
                newT = time;
            }
            else
            {
                float s = m_fPeriod / 4;
                time = time - 1;
                newT = -(float)(Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.Pi * 2.0f / m_fPeriod));
            }

            m_pOther.update(newT);
        }

        public override CCFiniteTimeAction reverse() 
        {
            return CCEaseElasticOut.actionWithAction((CCActionInterval)m_pOther.reverse(), m_fPeriod);
        }

        public override CCObject copyWithZone(CCZone pZone) 
        {
            CCZone pNewZone = null;
            CCEaseElasticIn pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy =pZone.m_pCopyObject as  CCEaseElasticIn;
            }
            else
            {
                pCopy = new CCEaseElasticIn();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fPeriod);

            return pCopy;
        }

	    /// <summary>
        /// creates the action
	    /// </summary>
	    /// <param name="pAction"></param>
	    /// <returns></returns>
        public new static CCEaseElasticIn actionWithAction(CCActionInterval pAction) 
        {
            CCEaseElasticIn pRet = new CCEaseElasticIn();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction))
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

        /// <summary>
        /// Creates the action with the inner action and the period in radians (default is 0.3) 
        /// </summary>
        /// <param name="pAction"></param>
        /// <param name="fPeriod"></param>
        /// <returns></returns>
        public new static CCEaseElasticIn actionWithAction(CCActionInterval pAction, float fPeriod) 
        {
            CCEaseElasticIn pRet = new CCEaseElasticIn();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction, fPeriod))
                {
                    //pRet->autorelease();
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
