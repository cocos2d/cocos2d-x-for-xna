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
    /** @brief Tints a CCNode that implements the CCNodeRGB protocol from current tint to a custom one.
     @warning This action doesn't support "reverse"
     @since v0.7.2
    */
    public class CCTintTo : CCActionInterval
    {
        public bool initWithDuration(float duration, byte red, byte green, byte blue)
        {
            if (base.initWithDuration(duration))
            {
                m_to = new ccColor3B(red, green, blue);
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCTintTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCTintTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCTintTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_to.r, m_to.g, m_to.b);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            ICCRGBAProtocol protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                m_from = protocol.Color;
            }
        }

        public override void update(float dt)
        {
            ICCRGBAProtocol protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                protocol.Color =  new ccColor3B((byte)(m_from.r + (m_to.r - m_from.r) * dt),
                    (byte)(m_from.g + (m_to.g - m_from.g) * dt),
                    (byte)(m_from.b + (m_to.b - m_from.b) * dt));
            }
        }

        public static CCTintTo actionWithDuration(float duration, byte red, byte green, byte blue)
        {
            CCTintTo ret = new CCTintTo();
            ret.initWithDuration(duration, red, green, blue);

            return ret;
        }

        protected ccColor3B m_to;
        protected ccColor3B m_from;
    }
}
