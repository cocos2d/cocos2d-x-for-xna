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
    /** @brief CCLiquid action */
    public class CCLiquid : CCGrid3DAction
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

        /** initializes the action with amplitude, a grid and duration */
        public bool initWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            // if (CCGrid3DAction::initWithSize(gridSize, duration))
            if (initWithSize(gridSize, duration))
            {
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCLiquid pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCLiquid)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCLiquid();
                pZone = pNewZone = new CCZone(pCopy);
            }

            // CCGrid3DAction::copyWithZone(pZone);
            copyWithZone(pZone);
            pCopy.initWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, m_fDuration);

            // CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override void update(float time)
        {
            int i, j;

            for (i = 1; i < m_sGridSize.x; ++i)
            {
                for (j = 1; j < m_sGridSize.y; ++j)
                {
                    ccVertex3F v = originalVertex(new ccGridSize(i, j));
                    v.x = (v.x + ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 + v.x * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    v.y = (v.y + ((float)Math.Sin(time * (float)Math.PI * m_nWaves * 2 + v.y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }

        /** creates the action with amplitude, a grid and duration */
        public static CCLiquid actionWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
        {
            CCLiquid pAction = new CCLiquid();

            if (pAction != null)
            {
                if (pAction.initWithWaves(wav, amp, gridSize, duration))
                {
                    // pAction->autorelease();
                }
                else
                {
                    // CC_SAFE_RELEASE_NULL(pAction);
                }
            }

            return pAction;
        }

        protected int m_nWaves;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
    }
}
