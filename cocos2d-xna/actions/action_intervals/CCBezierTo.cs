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
    /** @brief An action that moves the target with a cubic Bezier curve to a destination point.
     @since v0.8.2
     */
    public class CCBezierTo : CCBezierBy
    {
        public static CCBezierTo actionWithDuration(float t, ccBezierConfig c)
        {
            CCBezierTo ret = new CCBezierTo();
            ret.initWithDuration(t, c);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_sConfig.controlPoint_1 = CCPointExtension.ccpSub(m_sConfig.controlPoint_1, m_startPosition);
            m_sConfig.controlPoint_2 = CCPointExtension.ccpSub(m_sConfig.controlPoint_2, m_startPosition);
            m_sConfig.endPosition = CCPointExtension.ccpSub(m_sConfig.endPosition, m_startPosition);
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCBezierTo ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCBezierTo;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCBezierTo();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_sConfig);

            return ret;
        }
    }
}
