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
    /** @brief Moves a CCNode object to the position x,y. x and y are absolute coordinates by modifying it's position attribute.*/   
    public class CCMoveTo : CCActionInterval
    {
        public bool initWithDuration(float duration, CCPoint position)
        {
            if (base.initWithDuration(duration))
            {
                m_endPosition = position;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCMoveTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCMoveTo)tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCMoveTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);
            ret.initWithDuration(m_fDuration, m_endPosition);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_startPosition = target.position;
            m_delta = CCPointExtension.ccpSub(m_endPosition, m_startPosition);
        }

        public override void update(float dt)
        {
            if (m_pTarget != null)
            {
                m_pTarget.position = CCPointExtension.ccp(m_startPosition.x + m_delta.x * dt,
                    m_startPosition.y + m_delta.y * dt);
            }
        }

        /** creates the action */
        public static CCMoveTo actionWithDuration(float duration, CCPoint position)
        {
            CCMoveTo moveTo = new CCMoveTo();
            moveTo.initWithDuration(duration, position);

            return moveTo;
        }

        protected CCPoint m_endPosition = new CCPoint(0f, 0f);
        protected CCPoint m_startPosition = new CCPoint(0f, 0f);
        protected CCPoint m_delta = new CCPoint(0f, 0f);
    }
}
