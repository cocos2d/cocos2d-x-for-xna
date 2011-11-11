/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
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
    public class CCTintBy : CCActionInterval
    {
        public bool initWithDuration(float duration, short deltaRed, short deltaGreen, short deltaBlue)
        {
            if (base.initWithDuration(duration))
            {
                m_deltaR = deltaRed;
                m_deltaG = deltaGreen;
                m_deltaB = deltaBlue;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCTintBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCTintBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCTintBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_deltaR, m_deltaG, m_deltaB);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);

            CCRGBAProtocol protocol = target as CCRGBAProtocol;
            if (protocol != null)
            {
                ccColor3B color = protocol.Color;
                m_fromR = color.r;
                m_fromG = color.g;
                m_fromB = color.b;
            }
        }

        public override void update(float dt)
        {
            CCRGBAProtocol protocol = m_pTarget as CCRGBAProtocol;
            if (protocol != null)
            {
                protocol.Color = new ccColor3B((byte)(m_fromR + m_deltaR * dt),
                                               (byte)(m_fromG + m_deltaG * dt),
                                               (byte)(m_fromB + m_deltaB * dt));
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCTintBy.actionWithDuration(m_fDuration, (short)-m_deltaR, (short)-m_deltaG, (short)-m_deltaB) as CCFiniteTimeAction;
        }

        public static CCTintBy actionWithDuration(float duration, short deltaRed, short deltaGreen, short deltaBlue)
        {
            CCTintBy ret = new CCTintBy();
            ret.initWithDuration(duration, deltaRed, deltaGreen, deltaBlue);

            return ret;
        }

        protected short m_deltaR;
        protected short m_deltaG;
        protected short m_deltaB;

        protected short m_fromR;
        protected short m_fromG;
        protected short m_fromB;
    }
}
