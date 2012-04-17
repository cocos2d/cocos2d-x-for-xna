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
    /// <summary>
    /// @brief CCShuffleTiles action
    /// Shuffle the tiles in random order
    /// </summary>
    public class CCShuffleTiles : CCTiledGrid3DAction
    {
        /// <summary>
        /// initializes the action with a random seed, the grid size and the duration
        /// </summary>
        public bool initWithSeed(int s, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_nSeed = s;
                m_pTilesOrder = null;
                m_pTiles = null;

                return true;
            }

            return false;
        }

        public void shuffle(ref int[] pArray, int nLen)
        {
            int i;
            Random random = new Random();
            for (i = nLen - 1; i >= 0; i--)
            {
                int j = random.Next() % (i + 1);
                int v = pArray[i];
                pArray[i] = pArray[j];
                pArray[j] = v;
            }
        }

        public ccGridSize getDelta(ccGridSize pos)
        {
            CCPoint pos2 = new CCPoint();

            int idx = pos.x * m_sGridSize.y + pos.y;

            pos2.x = (float)(m_pTilesOrder[idx] / (int)m_sGridSize.y);
            pos2.y = (float)(m_pTilesOrder[idx] % (int)m_sGridSize.y);

            return new ccGridSize((int)(pos2.x - pos.x), (int)(pos2.y - pos.y));
        }

        public void placeTile(ccGridSize pos, Tile t)
        {
            ccQuad3 coords = originalTile(pos);

            CCPoint step = m_pTarget.Grid.Step;
            coords.bl.x += (int)(t.position.x * step.x);
            coords.bl.y += (int)(t.position.y * step.y);

            coords.br.x += (int)(t.position.x * step.x);
            coords.br.y += (int)(t.position.y * step.y);

            coords.tl.x += (int)(t.position.x * step.x);
            coords.tl.y += (int)(t.position.y * step.y);

            coords.tr.x += (int)(t.position.x * step.x);
            coords.tr.y += (int)(t.position.y * step.y);

            setTile(pos, coords);
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);

            if (m_nSeed != -1)
            {
                m_nSeed = new Random().Next();
            }

            m_nTilesCount = m_sGridSize.x * m_sGridSize.y;
            m_pTilesOrder = new int[m_nTilesCount];
            int i, j;
            int k;

            /**
             * Use k to loop. Because m_nTilesCount is unsigned int,
             * and i is used later for int.
             */
            for (k = 0; k < m_nTilesCount; ++k)
            {
                m_pTilesOrder[k] = k;
            }

            shuffle(ref m_pTilesOrder, m_nTilesCount);

            m_pTiles = new Tile[m_nTilesCount];

            int f = 0;
            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    m_pTiles[f] = new Tile();
                    m_pTiles[f].position = new CCPoint((float)i, (float)j);
                    m_pTiles[f].startPosition = new CCPoint((float)i, (float)j);
                    m_pTiles[f].delta = getDelta(new ccGridSize(i, j));

                    f++;
                }
            }
        }

        public override void update(float time)
        {
            int i, j;

            int f = 0;
            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    var item = m_pTiles[f];
                    item.position = new CCPoint((float)(item.delta.x * time), (float)(item.delta.y * time));
                    placeTile(new ccGridSize(i, j), item);

                    f++;
                }
            }
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCShuffleTiles pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCShuffleTiles)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShuffleTiles();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithSeed(m_nSeed, m_sGridSize, m_fDuration);

            pNewZone = null;
            return pCopy;
        }

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public static CCShuffleTiles actionWithSeed(int s, ccGridSize gridSize, float duration)
        {
            CCShuffleTiles pAction = new CCShuffleTiles();

            if (pAction.initWithSeed(s, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nSeed;
        protected int m_nTilesCount;
        protected int[] m_pTilesOrder;
        protected Tile[] m_pTiles;
    }
}
