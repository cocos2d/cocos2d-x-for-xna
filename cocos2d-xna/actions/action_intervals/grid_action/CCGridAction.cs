using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace cocos2d
{
    public class CCGridAction : CCActionInterval
    {
        public virtual CCObject copyWithZone(CCZone pZone)
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
        public virtual void startWithTarget(CCNode pTarget)
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
        public virtual CCActionInterval reverse()
        {
            return CCReverseTime.actionWithAction(this);
        }

        /** initializes the action with size and duration */
        public virtual bool initWithSize(ccGridSize gridSize, float duration)
        {
            if (base.initWithDuration(duration))
            {
                m_sGridSize = gridSize;

                return true;
            }

            return false;
        }
        /** returns the grid */
        public virtual CCGridBase getGrid()
        {
            // Abstract class needs implementation
            Debug.Assert(false);

            return null;
        }

        /** creates the action with size and duration */
        public static CCGridAction actionWithSize(ccGridSize gridSize, float duration)
        {
            CCGridAction pAction = new CCGridAction();
            if (pAction != null)
            {
                if (pAction.initWithSize(gridSize, duration))
                {
                    //pAction->autorelease();
                }
                else
                {
                    //CC_SAFE_DELETE(pAction);
                }
            }

            return pAction;
        }

        protected ccGridSize m_sGridSize;
    }
}
