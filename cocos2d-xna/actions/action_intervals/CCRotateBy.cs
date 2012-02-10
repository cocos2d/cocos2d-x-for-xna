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
    /** @brief Rotates a CCNode object clockwise a number of degrees by modifying it's rotation attribute. */
    public class CCRotateBy : CCActionInterval
    {
        public bool initWithDuration(float duration, float fDeltaAngle)
        {
            if (base.initWithDuration(duration))
            {
                m_fAngle = fDeltaAngle;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRotateBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRotateBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRotateBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_fAngle);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_fStartAngle = target.rotation;
        }

        public override void update(float dt)
        {
            // XXX: shall I add % 360
            if (m_pTarget != null)
            {
                m_pTarget.rotation = m_fStartAngle + m_fAngle * dt;
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCRotateBy.actionWithDuration(m_fDuration, -m_fAngle);
        }

        public static CCRotateBy actionWithDuration(float duration, float fDeltaAngle)
        {
            CCRotateBy ret = new CCRotateBy();
            ret.initWithDuration(duration, fDeltaAngle);

            return ret;
        }

        protected float m_fAngle;
        protected float m_fStartAngle;
    }
}
