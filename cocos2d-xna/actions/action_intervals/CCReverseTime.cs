/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
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

using System.Diagnostics;
namespace cocos2d
{
    /** @brief Executes an action in reverse order, from time=duration to time=0
 
     @warning Use this action carefully. This action is not
     sequenceable. Use it as the default "reversed" method
     of your own actions, but using it outside the "reversed"
     scope is not recommended.
    */
    public class CCReverseTime : CCActionInterval
    {
        public bool initWithAction(CCFiniteTimeAction action)
        {
            Debug.Assert(action != null);
            Debug.Assert(action != m_pOther);

            if (base.initWithDuration(action.duration))
            {
                m_pOther = action;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCReverseTime ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCReverseTime;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCReverseTime();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithAction(m_pOther.copy() as CCFiniteTimeAction);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_pOther.startWithTarget(target);
        }

        public override void stop()
        {
            m_pOther.stop();
            base.stop();
        }

        public override void update(float dt)
        {
            if (m_pOther != null)
            {
                m_pOther.update(1 - dt);
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            return m_pOther.copy() as CCFiniteTimeAction;
        }

        public static CCReverseTime actionWithAction(CCFiniteTimeAction action)
        {
            CCReverseTime ret = new CCReverseTime();
            ret.initWithAction(action);

            return ret;
        }

        protected CCFiniteTimeAction m_pOther;
    }
}
