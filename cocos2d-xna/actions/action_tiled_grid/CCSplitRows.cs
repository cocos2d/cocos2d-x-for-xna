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
    public class CCSplitRows : CCTiledGrid3DAction
    {
        /// <summary>
        /// initializes the action with the number of rows to split and the duration
        /// </summary>
        public bool initWithRows(int nRows, float duration)
        {
            m_nRows = nRows;

            return base.initWithSize(new ccGridSize(1, nRows), duration);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCSplitRows pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCSplitRows)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCSplitRows();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithRows(m_nRows, m_fDuration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void update(float time)
        {
            int j;

            for (j = 0; j < m_sGridSize.y; ++j)
            {
                ccQuad3 coords = originalTile(new ccGridSize(0, j));
                float direction = 1;

                if ((j % 2) == 0)
                {
                    direction = -1;
                }

                coords.bl.x += direction * m_winSize.width * time;
                coords.br.x += direction * m_winSize.width * time;
                coords.tl.x += direction * m_winSize.width * time;
                coords.tr.x += direction * m_winSize.width * time;

                setTile(new ccGridSize(0, j), coords);
            }
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_winSize = CCDirector.sharedDirector().winSizeInPixels;
        }

        /// <summary>
        ///  creates the action with the number of rows to split and the duration 
        /// </summary>
        public static CCSplitRows actionWithRows(int nRows, float duration)
        {
            CCSplitRows pAction = new CCSplitRows();
            if (pAction.initWithRows(nRows, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nRows;
        protected CCSize m_winSize;
    }
}
