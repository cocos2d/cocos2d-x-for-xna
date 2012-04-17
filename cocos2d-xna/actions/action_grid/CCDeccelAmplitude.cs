/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 openxlive.com

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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCDeccelAmplitude : CCActionInterval
    {
        /// <summary>
        ///  initializes the action with an inner action that has the amplitude property, and a duration time
        /// </summary>
        public bool initWithAction(CCAction pAction, float duration)
        {
            if (base.initWithDuration(duration))
            {
                m_fRate = 1.0f;
                m_pOther = pAction as CCActionInterval;

                return true;
            }

            return false;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_pOther.startWithTarget(pTarget);
        }

        public override void update(float time)
        {
            ((CCDeccelAmplitude)(m_pOther)).setAmplitudeRate((float)Math.Pow((1 - time), m_fRate));
            m_pOther.update(time);
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCDeccelAmplitude.actionWithAction(m_pOther.reverse(), m_fDuration);
        }

        /// <summary>
        /// creates the action with an inner action that has the amplitude property, and a duration time
        /// </summary>
        public static CCDeccelAmplitude actionWithAction(CCAction pAction, float duration)
        {
            CCDeccelAmplitude pRet = new CCDeccelAmplitude();

            if (pRet.initWithAction(pAction, duration))
            {
                return pRet;
            }

            return null;
        }

        protected float m_fRate;

        /// <summary>
        /// get or set amplitude rate
        /// </summary>
        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }

        protected CCActionInterval m_pOther;
    }
}
