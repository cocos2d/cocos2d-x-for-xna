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
    public class CCSplitCols : CCTiledGrid3DAction
    {
        /// <summary>
        ///  initializes the action with the number of columns to split and the duration 
        /// </summary>
        public bool initWithCols(int nCols, float duration)
        {
            m_nCols = nCols;
            return base.initWithSize(new ccGridSize(nCols, 1), duration);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCSplitCols pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCSplitCols)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCSplitCols();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);
            pCopy.initWithCols(m_nCols, m_fDuration);

            //CC_SAFE_DELETE(pNewZone);
            pNewZone = null;
            return pCopy;
        }

        public override void update(float time)
        {
            int i;

            for (i = 0; i < m_sGridSize.x; ++i)
            {
                ccQuad3 coords = originalTile(new ccGridSize(i, 0));
                float direction = 1;

                if ((i % 2) == 0)
                {
                    direction = -1;
                }

                coords.bl.y += direction * m_winSize.height * time;
                coords.br.y += direction * m_winSize.height * time;
                coords.tl.y += direction * m_winSize.height * time;
                coords.tr.y += direction * m_winSize.height * time;

                setTile(new ccGridSize(i, 0), coords);
            }
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_winSize = CCDirector.sharedDirector().winSizeInPixels;
        }

        /// <summary>
        /// creates the action with the number of columns to split and the duration
        /// </summary>
        public static CCSplitCols actionWithCols(int nCols, float duration)
        {
            CCSplitCols pAction = new CCSplitCols();
            if (pAction.initWithCols(nCols, duration))
            {
                return pAction;
            }

            return null;
        }

        protected int m_nCols;
        protected CCSize m_winSize;
    }
}
