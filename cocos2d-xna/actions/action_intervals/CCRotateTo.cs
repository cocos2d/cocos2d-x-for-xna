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
    public class CCRotateTo : CCActionInterval
    {
        public bool initWithDuration(float duration, float fDeltaAngle)
        {
            if (base.initWithDuration(duration))
            {
                m_fDstAngle = fDeltaAngle;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRotateTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRotateTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRotateTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_fDstAngle);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);

            m_fStartAngle = target.rotation;

            if (m_fStartAngle > 0)
            {
                m_fStartAngle = m_fStartAngle % 350.0f;
            }
            else
            {
                m_fStartAngle = m_fStartAngle % -360.0f;
            }

            m_fDiffAngle = m_fDstAngle - m_fStartAngle;
            if (m_fDiffAngle > 180)
            {
                m_fDiffAngle -= 360;
            }

            if (m_fDiffAngle < -180)
            {
                m_fDiffAngle += 360;
            }
        }

        public override void update(float dt)
        {
            if (m_pTarget != null)
            {
                m_pTarget.rotation = m_fStartAngle + m_fDiffAngle * dt;
            }
        }

        public static CCRotateTo actionWithDuration(float duration, float fDeltaAngle)
        {
            CCRotateTo ret = new CCRotateTo();
            ret.initWithDuration(duration, fDeltaAngle);

            return ret;
        }

        protected float m_fDstAngle;
        protected float m_fStartAngle;
        protected float m_fDiffAngle;
    }
}
