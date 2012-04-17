/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
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
    /// <summary>
    /// CCWaves actio
    /// </summary>
    public class CCWaves : CCGrid3DAction
    {
        public float getAmplitude()
        {
            return m_fAmplitude;
        }

        public void setAmplitude(float fAmplitude)
        {
            m_fAmplitude = fAmplitude;
        }

        public float getAmplitudeRate()
        {
            return m_fAmplitudeRate;
        }

        public void setAmplitudeRate(float fAmplitudeRate)
        {
            m_fAmplitudeRate = fAmplitudeRate;
        }

        /// <summary>
        /// initializes the action with amplitude, horizontal sin, vertical sin, a grid and duration
        /// </summary>
        public bool initWithWaves(int wav, float amp, bool h, bool v, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;
                m_bHorizontal = h;
                m_bVertical = v;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCWaves pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCWaves)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCWaves();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithWaves(m_nWaves, m_fAmplitude, m_bHorizontal, m_bVertical, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x + 1; ++i)
            {
                for (j = 0; j < m_sGridSize.y + 1; ++j)
                {
                    ccVertex3F v = originalVertex(new ccGridSize(i, j));

                    if (m_bVertical)
                    {
                        v.x = (v.x + ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 + v.y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    if (m_bHorizontal)
                    {
                        v.y = (v.y + ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 + v.x * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }

        /// <summary>
        /// initializes the action with amplitude, horizontal sin, vertical sin, a grid and duration
        /// </summary>
        public static CCWaves actionWithWaves(int wav, float amp, bool h, bool v, ccGridSize gridSize,
            float duration)
        {
            CCWaves pAction = new CCWaves();

            if (pAction.initWithWaves(wav, amp, h, v, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nWaves;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected bool m_bVertical;
        protected bool m_bHorizontal;
    }
}
