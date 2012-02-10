/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
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

namespace cocos2d
{
    /** @brief Repeats an action a number of times.
     * To repeat an action forever use the CCRepeatForever action.
     */
    public class CCRepeat : CCActionInterval
    {
        /** initializes a CCRepeat action. Times is an unsigned integer between 1 and pow(2,30) */
        public bool initWithAction(CCFiniteTimeAction action, uint times)
        {
            float d = action.duration * times;

            if (base.initWithDuration(d))
            {
                m_uTimes = times;
                m_pInnerAction = action;

                m_uTotal = 0;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRepeat ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRepeat;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRepeat();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCFiniteTimeAction param = m_pInnerAction.copy() as CCFiniteTimeAction;
            if (param == null)
            {
                return null;
            }
            ret.initWithAction(param, m_uTimes);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            m_uTotal = 0;
            base.startWithTarget(target);
            m_pInnerAction.startWithTarget(target);
        }

        public override void stop()
        {
            m_pInnerAction.stop();
            base.stop();
        }

        // issue #80. Instead of hooking step:, hook update: since it can be called by any 
        // container action like Repeat, Sequence, AccelDeccel, etc..
        public override void update(float dt)
        {
            float t = dt * m_uTimes;
            if (t > m_uTotal + 1)
            {
                m_pInnerAction.update(1.0f);
                m_uTotal++;
                m_pInnerAction.stop();
                m_pInnerAction.startWithTarget(m_pTarget);

                // repeat is over?
                if (m_uTotal == m_uTimes)
                {
                    // so, set it in the original position
                    m_pInnerAction.update(0);
                }
                else
                {
                    // no ? start next repeat with the right update
                    // to prevent jerk (issue #390)
                    m_pInnerAction.update(t - m_uTotal);
                }
            }
            else
            {
                float r = t % 1.0f;

                // fix last repeat position
                // else it could be 0.
                if (dt == 1.0f)
                {
                    r = 1.0f;
                    m_uTotal++; // this is the added line
                }

                m_pInnerAction.update(r > 1 ? 1 : r);
            }
        }

        public override bool isDone()
        {
            return m_uTotal == m_uTimes;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCRepeat.actionWithAction(m_pInnerAction.reverse(), m_uTimes);
        }

        /** creates a CCRepeat action. Times is an unsigned integer between 1 and pow(2,30) */
        public static CCRepeat actionWithAction(CCFiniteTimeAction action, uint times)
        {
            CCRepeat ret = new CCRepeat();
            ret.initWithAction(action, times);

            return ret;
        }

        protected CCFiniteTimeAction m_pInnerAction;
        public CCFiniteTimeAction InnerAction
        {
            get
            {
                return m_pInnerAction;
            }
            set
            {
                m_pInnerAction = value;
            }
        }

        protected uint m_uTimes;
        protected uint m_uTotal;
    }
}
