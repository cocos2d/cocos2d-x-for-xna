using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            CCRGBAProtocol protocol = m_pTarget as CCRGBAProtocol;
            if (protocol != null)
            {
                m_from = protocol.Color;
            }
        }

        public override void update(float dt)
        {
            CCRGBAProtocol protocol = m_pTarget as CCRGBAProtocol;
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
