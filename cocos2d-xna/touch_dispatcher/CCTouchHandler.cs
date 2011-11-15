/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

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

namespace cocos2d
{
    /// <summary>
    ///  Object than contains the delegate and priority of the event handler.
    /// </summary>
    public class CCTouchHandler
    {
        /// <summary>
        /// delegate
        /// </summary>
        public ICCTouchDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        /// <summary>
        /// priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// enabled selectors 
        /// </summary>
        public int getEnabledSelectors { get; set; }

        /// <summary>
        /// initializes a TouchHandler with a delegate and a priority 
        /// </summary>
        public virtual bool initWithDelegate(ICCTouchDelegate pDelegate, int nPriority)
        {
            m_pDelegate = pDelegate;
            m_nPriority = nPriority;
            m_nEnabledSelectors = 0;

            return true;
        }

        /// <summary>
        /// allocates a TouchHandler with a delegate and a priority 
        /// </summary>
        public static CCTouchHandler handlerWithDelegate(ICCTouchDelegate pDelegate, int nPriority)
        {
            CCTouchHandler pHandler = new CCTouchHandler();

            if (pHandler.initWithDelegate(pDelegate, nPriority))
            {
                pHandler = null;
            }
            else
            {
                pHandler = null;
            }

            return pHandler;
        }

        protected ICCTouchDelegate m_pDelegate;
        protected int m_nPriority;
        protected int m_nEnabledSelectors;
    }
}
