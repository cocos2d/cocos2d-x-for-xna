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

using System;
namespace cocos2d
{
    /** @brief An action that moves the target with a cubic Bezier curve by a certain distance. */
    public class CCBezierBy : CCActionInterval
    {
        public bool initWithDuration(float t, ccBezierConfig c)
        {
            if (base.initWithDuration(t))
            {
                m_sConfig = c;
                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCBezierBy ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCBezierBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCBezierBy();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            ret.initWithDuration(m_fDuration, m_sConfig);

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
                float xa = 0;
                float xb = m_sConfig.controlPoint_1.x;
                float xc = m_sConfig.controlPoint_2.x;
                float xd = m_sConfig.endPosition.x;

                float ya = 0;
                float yb = m_sConfig.controlPoint_1.y;
                float yc = m_sConfig.controlPoint_2.y;
                float yd = m_sConfig.endPosition.y;

                float x = bezierat(xa, xb, xc, xd, dt);
                float y = bezierat(ya, yb, yc, yd, dt);
                m_pTarget.position = CCPointExtension.ccpAdd(m_startPosition, CCPointExtension.ccp(x, y));
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            ccBezierConfig r;

	        r.endPosition = CCPointExtension.ccpNeg(m_sConfig.endPosition);
	        r.controlPoint_1 = CCPointExtension.ccpAdd(m_sConfig.controlPoint_2, CCPointExtension.ccpNeg(m_sConfig.endPosition));
	        r.controlPoint_2 = CCPointExtension.ccpAdd(m_sConfig.controlPoint_1, CCPointExtension.ccpNeg(m_sConfig.endPosition));

	        CCBezierBy action = CCBezierBy.actionWithDuration(m_fDuration, r);
	        return action;
        }

        public static CCBezierBy actionWithDuration(float t, ccBezierConfig c)
        {
            CCBezierBy ret = new CCBezierBy();
            ret.initWithDuration(t, c);

            return ret;
        }

        // Bezier cubic formula:
        //	((1 - t) + t)3 = 1 
        // Expands to¡­ 
        //   (1 - t)3 + 3t(1-t)2 + 3t2(1 - t) + t3 = 1 
        protected float bezierat(float a, float b, float c, float d, float t)
        {

            return (float)((Math.Pow(1 - t, 3) * a +
                            3 * t * (Math.Pow(1 - t, 2)) * b +
                            3 * Math.Pow(t, 2) * (1 - t) * c +
                            Math.Pow(t, 3) * d));
        }

        protected ccBezierConfig m_sConfig;
        protected CCPoint m_startPosition;
    }

    public struct ccBezierConfig
    {
        //! end position of the bezier
	    public CCPoint endPosition;
	    //! Bezier control point 1
	    public CCPoint controlPoint_1;
	    //! Bezier control point 2
	    public CCPoint controlPoint_2;
    }
}
