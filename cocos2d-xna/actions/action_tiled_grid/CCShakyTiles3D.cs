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
    public class CCShakyTiles3D : CCTiledGrid3DAction
    {
        /// <summary>
        ///  initializes the action with a range, whether or not to shake Z vertices, a grid size, and duration
        /// </summary>
        public bool initWithRange(int nRange, bool bShakeZ, ccGridSize gridSize,
            float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nRandrange = nRange;
                m_bShakeZ = bShakeZ;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCShakyTiles3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCShakyTiles3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShakyTiles3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithRange(m_nRandrange, m_bShakeZ, m_sGridSize, m_fDuration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    ccQuad3 coords = originalTile(new ccGridSize(i, j));
                    Random rand = new Random();
                    // X
                    coords.bl.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    coords.br.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    coords.tl.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    coords.tr.x += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;

                    // Y
                    coords.bl.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    coords.br.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    coords.tl.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    coords.tr.y += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;

                    if (m_bShakeZ)
                    {
                        coords.bl.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.br.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tl.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.tr.z += (rand.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    setTile(new ccGridSize(i, j), coords);
                }
            }
        }

        /// <summary>
        /// creates the action with a range, whether or not to shake Z vertices, a grid size, and duration
        /// </summary>
        public static CCShakyTiles3D actionWithRange(int nRange, bool bShakeZ, ccGridSize gridSize, float duration)
        {
            CCShakyTiles3D pAction = new CCShakyTiles3D();

            if (pAction.initWithRange(nRange, bShakeZ, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nRandrange;
        protected bool m_bShakeZ;
    }
}
