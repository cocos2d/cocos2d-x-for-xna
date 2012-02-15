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

namespace cocos2d
{
    public class CCJumpBy : CCActionInterval
    {
        public bool initWithDuration(float duration, CCPoint position, float height, uint jumps)
        {
            if (base.initWithDuration(duration))
            {
                m_delta = position;
                m_height = height;
                m_nJumps = jumps;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCJumpBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCJumpBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCJumpBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_delta, m_height, m_nJumps);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_startPosition = target.position;
        }

        public override void update(float dt)
        {
            if (m_pTarget != null)
            {
                // Is % equal to fmodf()???
                float frac = (dt * m_nJumps) % 1.0f;
                float y = m_height * 4 * frac * (1 - frac);
                y += m_delta.y * dt;
                float x = m_delta.x * dt;
                m_pTarget.position = CCPointExtension.ccp(m_startPosition.x + x, m_startPosition.y + y);
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCJumpBy.actionWithDuration(m_fDuration, CCPointExtension.ccp(-m_delta.x, -m_delta.y), m_height, m_nJumps);
        }

        public static CCJumpBy actionWithDuration(float duration, CCPoint position, float height, uint jumps)
        {
            CCJumpBy ret = new CCJumpBy();
            ret.initWithDuration(duration, position, height, jumps);

            return ret;
        }

        protected CCPoint m_startPosition;
        protected CCPoint m_delta;
        protected float m_height;
        protected uint m_nJumps;
    }
}
