using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /** Cardinal Spline path.
     http://en.wikipedia.org/wiki/Cubic_Hermite_spline#Cardinal_spline
    @ingroup Actions
     */
    public class CCCardinalSplineTo : CCActionInterval
    {
        /** creates an action with a Cardinal Spline array of points and tension 
        @deprecated: This interface will be deprecated sooner or later.
        */
        public static new CCCardinalSplineTo actionWithDuration(float duration, CCPointArray points, float tension)
        {
            return CCCardinalSplineTo.create(duration, points, tension);
        }

        /** creates an action with a Cardinal Spline array of points and tension */
        public static CCCardinalSplineTo create(float duration, CCPointArray points, float tension)
        {
            CCCardinalSplineTo ret = new CCCardinalSplineTo();
            ret.initWithDuration(duration, points, tension);
            return ret;
        }

        public CCCardinalSplineTo()
        {
        }

        /** initializes the action with a duration and an array of points */
        public virtual bool initWithDuration(float duration, CCPointArray points, float tension)
        {
            if (points == null || points.count() == 0)
            {
                return (false);
            }
            if (base.initWithDuration(duration))
            {
                setPoints(points);
                m_fTension = tension;

                return true;
            }

            return false;
        }

        // super virtual functions
        public virtual CCCardinalSplineTo copyWithZone(CCZone pZone)
        {
            CCZone pNewZone;
            CCCardinalSplineTo pRet;
            if (pZone != null && pZone.m_pCopyObject != null) //in case of being called at sub class
            {
                pRet = (CCCardinalSplineTo)(pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCCardinalSplineTo();
                pZone = pNewZone = new CCZone(pRet);
            }

            base.copyWithZone(pZone);

            pRet.initWithDuration(duration, m_pPoints, m_fTension);

            return pRet;
        }
        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);

            m_fDeltaT = (float)1 / m_pPoints.count();
        }

        public override void update(float time)
        {
            int p;
            float lt;

            // border
            if (time == 1)
            {
                p = m_pPoints.count() - 1;
                lt = 1;
            }
            else
            {
                p = (int)(time / m_fDeltaT);
                lt = (time - m_fDeltaT * (float)p) / m_fDeltaT;
            }

            // Interpolate    
            CCPoint pp0 = m_pPoints.getControlPointAtIndex(p - 1);
            CCPoint pp1 = m_pPoints.getControlPointAtIndex(p + 0);
            CCPoint pp2 = m_pPoints.getControlPointAtIndex(p + 1);
            CCPoint pp3 = m_pPoints.getControlPointAtIndex(p + 2);

            CCPoint newPos = ccUtils.ccCardinalSplineAt(pp0, pp1, pp2, pp3, m_fTension, lt);

            updatePosition(newPos);
        }

        public virtual CCActionInterval reverse()
        {
            CCPointArray pReverse = m_pPoints.reverse();

            return CCCardinalSplineTo.create(m_fDuration, pReverse, m_fTension);
        }

        public virtual void updatePosition(CCPoint newPos)
        {
            m_pTarget.position = new CCPoint(newPos);
        }

        public virtual CCPointArray getPoints()
        {
            return m_pPoints;
        }
        public virtual void setPoints(CCPointArray points)
        {
            m_pPoints = points;
        }

        /** Array of control points */
        protected CCPointArray m_pPoints;
        protected float m_fDeltaT=0f;
        protected float m_fTension=0f;
    }
}
