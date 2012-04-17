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
using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// @brief Base class for Grid actions
    /// </summary>
    public class CCGridAction : CCActionInterval
    {
        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCGridAction pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCGridAction)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCGridAction();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithSize(m_sGridSize, m_fDuration);

            if (pNewZone != null)
            {
                pNewZone = null;
            }
            return pCopy;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);

            CCGridBase newgrid = this.getGrid();

            CCNode t = m_pTarget;
            CCGridBase targetGrid = t.Grid;

            if (targetGrid != null && targetGrid.ReuseGrid > 0)
            {
                if (targetGrid.Active && targetGrid.GridSize.x == m_sGridSize.x
                    && targetGrid.GridSize.y == m_sGridSize.y /*&& dynamic_cast<CCGridBase*>(targetGrid) != NULL*/)
                {
                    targetGrid.reuse();
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else
            {
                if (targetGrid != null && targetGrid.Active)
                {
                    targetGrid.Active = false;
                }

                t.Grid = newgrid;
                t.Grid.Active = true;
            }
        }
        public override CCFiniteTimeAction reverse()
        {
            return CCReverseTime.actionWithAction(this);
        }

        /// <summary>
        /// returns the grid
        /// </summary>
        public virtual CCGridBase getGrid()
        {
            // Abstract class needs implementation
            Debug.Assert(false);

            return null;
        }

        /// <summary>
        /// creates the action with size and duration
        /// </summary>
        public static CCGridAction actionWithSize(ccGridSize gridSize, float duration)
        {
            CCGridAction pAction = new CCGridAction();
            if (pAction.initWithSize(gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        /// <summary>
        /// initializes the action with size and duration
        /// </summary>
        public virtual bool initWithSize(ccGridSize gridSize, float duration)
        {
            if (base.initWithDuration(duration))
            {
                m_sGridSize = gridSize;

                return true;
            }

            return false;
        }

        protected ccGridSize m_sGridSize;
    }
}
