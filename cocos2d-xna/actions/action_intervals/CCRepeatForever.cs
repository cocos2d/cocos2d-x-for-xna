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
    public class CCRepeatForever : CCActionInterval
    {
        public static CCRepeatForever actionWithAction(CCActionInterval action)
        {
            CCRepeatForever ret = new CCRepeatForever();
            ret.initWithAction(action);

            return ret;
        }

        public bool initWithAction(CCActionInterval action)
        {
            Debug.Assert(action != null);

            m_pInnerAction = action;
            return true;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRepeatForever ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRepeatForever;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRepeatForever();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCActionInterval param = m_pInnerAction.copy() as CCActionInterval;
            if (param == null)
            {
                return null;
            }
            ret.initWithAction(param);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_pInnerAction.startWithTarget(target);
        }

        public override void step(float dt)
        {
            m_pInnerAction.step(dt);
            if (m_pInnerAction.isDone())
            {
                float diff = dt + m_pInnerAction.duration - m_pInnerAction.elapsed;
                m_pInnerAction.startWithTarget(m_pTarget);
                m_pInnerAction.step(diff);
            }
        }

        public override bool isDone()
        {
            return false;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCRepeatForever.actionWithAction(m_pInnerAction.reverse() as CCActionInterval);
        }

        protected CCActionInterval m_pInnerAction;
        public CCActionInterval InnerAction
        {
            get { return m_pInnerAction; }
            set { m_pInnerAction = value; }
        }
    }
}
