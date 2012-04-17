/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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
    public class CCJumpTiles3D : CCTiledGrid3DAction
    {
        protected int m_nJumps;

        protected float m_fAmplitude;
        /// <summary>
        /// amplitude of the sin
        /// </summary>
        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }
        protected float m_fAmplitudeRate;
        /// <summary>
        ///  amplitude rate 
        /// </summary>
        public float AmplitudeRate
        {
            get { return m_fAmplitudeRate; }
            set { m_fAmplitudeRate = value; }
        }

        /// <summary>
        /// initializes the action with the number of jumps, the sin amplitude, the grid size and the duration 
        /// </summary>
        public bool initWithJumps(int j, float amp, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nJumps = j;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCJumpTiles3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCJumpTiles3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCJumpTiles3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);
            pCopy.initWithJumps(m_nJumps, m_fAmplitude, m_sGridSize, m_fDuration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }
        public override void update(float time)
        {
            int i, j;

            float sinz = ((float)Math.Sin((float)Math.PI * time * m_nJumps * 2) * m_fAmplitude * m_fAmplitudeRate);
            float sinz2 = (float)(Math.Sin((float)Math.PI * (time * m_nJumps * 2 + 1)) * m_fAmplitude * m_fAmplitudeRate);

            for (i = 0; i < m_sGridSize.x; i++)
            {
                for (j = 0; j < m_sGridSize.y; j++)
                {
                    ccQuad3 coords = originalTile(new ccGridSize(i, j));

                    if (((i + j) % 2) == 0)
                    {
                        coords.bl.z += sinz;
                        coords.br.z += sinz;
                        coords.tl.z += sinz;
                        coords.tr.z += sinz;
                    }
                    else
                    {
                        coords.bl.z += sinz2;
                        coords.br.z += sinz2;
                        coords.tl.z += sinz2;
                        coords.tr.z += sinz2;
                    }

                    setTile(new ccGridSize(i, j), coords);
                }
            }
        }


        public static CCJumpTiles3D actionWithJumps(int j, float amp, ccGridSize gridSize, float duration)
        {
            CCJumpTiles3D pAction = new CCJumpTiles3D();

            if (pAction.initWithJumps(j, amp, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }
    }
}
