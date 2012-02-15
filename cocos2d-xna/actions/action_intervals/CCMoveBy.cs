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
    /** @brief Moves a CCNode object x,y pixels by modifying it's position attribute.
     x and y are relative to the position of the object.
     Duration is is seconds.
    */ 
    public class CCMoveBy : CCMoveTo
    {
        public new bool initWithDuration(float duration, CCPoint position)
        {
            if (base.initWithDuration(duration))
            {
                m_delta = position;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCMoveBy ret = null;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCMoveBy;

                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCMoveBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_delta);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            CCPoint dTmp = m_delta;
            base.startWithTarget(target);
            m_delta = dTmp;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCMoveBy.actionWithDuration(m_fDuration, CCPointExtension.ccp(-m_delta.x, -m_delta.y));
        }

        public static new CCMoveBy actionWithDuration(float duration, CCPoint position)
        {
            CCMoveBy ret = new CCMoveBy();
            ret.initWithDuration(duration, position);

            return ret;
        }
    }
}
