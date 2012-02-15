/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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
    public class CCReuseGrid : CCActionInstant
    {

        /// <summary>
        /// initializes an action with the number of times that the current grid will be reused
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public bool initWithTimes(int times)
        {
            m_nTimes = times;
            return true;
        }

        public virtual void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);

            if (m_pTarget.Grid != null && m_pTarget.Grid.Active != null)
            {
                m_pTarget.Grid.ReuseGrid = m_pTarget.Grid.ReuseGrid + m_nTimes;
            }
        }

        /// <summary>
        /// creates an action with the number of times that the current grid will be reused
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static CCReuseGrid actionWithTimes(int times)
        {
            CCReuseGrid pAction = new CCReuseGrid();
            if (pAction != null)
            {
                if (pAction.initWithTimes(times))
                {
                    //pAction->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pAction);
                }
            }

            return pAction;
        }

        protected int m_nTimes;
    }
}
