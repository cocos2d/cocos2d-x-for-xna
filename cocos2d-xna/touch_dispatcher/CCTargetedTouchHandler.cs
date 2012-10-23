/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2009      Valentin Milea
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
    public class CCTargetedTouchHandler : CCTouchHandler
    {
        /// <summary>
        /// whether or not the touches are swallowed
        /// </summary>
        public bool IsSwallowsTouches 
        {
            get { return m_bSwallowsTouches; }
            set { m_bSwallowsTouches = value; }
        }

        /// <summary>
        /// MutableSet that contains the claimed touches 
        /// </summary>
        public List<CCTouch> ClaimedTouches
        {
            get
            {
                return m_pClaimedTouches;
            }
        }

        /// <summary>
        ///  initializes a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not
        /// </summary>
        public bool initWithDelegate(ICCTargetedTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            if (base.initWithDelegate(pDelegate, nPriority))
            {
                m_pClaimedTouches = new List<CCTouch>();
                m_bSwallowsTouches = bSwallow;

                return true;
            }

            return false;
        }

        /// <summary>
        /// allocates a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not 
        /// </summary>
        public static CCTargetedTouchHandler handlerWithDelegate(ICCTargetedTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            CCTargetedTouchHandler pHandler = new CCTargetedTouchHandler();
            pHandler.initWithDelegate(pDelegate, nPriority, bSwallow);
            return pHandler;
        }

        protected bool m_bSwallowsTouches;
        protected List<CCTouch> m_pClaimedTouches;
    }
}
