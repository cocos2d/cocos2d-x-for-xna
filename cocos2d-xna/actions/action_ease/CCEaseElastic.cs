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
    public class CCEaseElastic : CCActionEase
    {
        protected float m_fPeriod;

        /// <summary>
        /// get period of the wave in radians. default is 0.3
        /// set period of the wave in radians.
        /// </summary>
        public float Period 
        {
            get { return m_fPeriod; }
            set { m_fPeriod = value; } 
        }

	    /// <summary>
	    /// Initializes the action with the inner action and the period in radians (default is 0.3) 
	    /// </summary>
	    /// <param name="pAction"></param>
	    /// <param name="fPeriod"></param>
	    /// <returns></returns>
        public bool initWithAction(CCActionInterval pAction, float fPeriod) 
        { 
            if (base.initWithAction(pAction))
		    {
			    m_fPeriod = fPeriod;
			    return true;
		    }

		    return false;
        }

	    /// <summary>
	    /// initializes the action
	    /// </summary>
	    /// <param name="pAction"></param>
	    /// <returns></returns>
        public new bool initWithAction(CCActionInterval pAction) 
        {
            return initWithAction(pAction, 0.3f);
        }

        public override CCFiniteTimeAction reverse() 
        {
            //assert(0);
            return null;
        }

        public override CCObject copyWithZone(CCZone pZone) 
        {
            CCZone pNewZone = null;
            CCEaseElastic pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy =pZone.m_pCopyObject as  CCEaseElastic;
            }
            else
            {
                pCopy = new CCEaseElastic();
                pZone = pNewZone = new CCZone(pCopy);
            }

            pCopy.initWithAction((CCActionInterval)(m_pOther.copy()), m_fPeriod);

            return pCopy;
        }

	    /// <summary>
        ///  creates the action
	    /// </summary>
	    /// <param name="pAction"></param>
	    /// <returns></returns>
        public new static CCEaseElastic actionWithAction(CCActionInterval pAction) 
        {
            CCEaseElastic pRet = new CCEaseElastic();

            if (pRet != null)
            {
                if (pRet.initWithAction(pAction))
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

	    /// <summary>
        /// Creates the action with the inner action and the period in radians (default is 0.3)
	    /// </summary>
	    /// <param name="pAction"></param>
	    /// <param name="fPeriod"></param>
	    /// <returns></returns>
        public static CCEaseElastic actionWithAction(CCActionInterval pAction, float fPeriod) 
        {
            CCEaseElastic pRet = new CCEaseElastic();

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
