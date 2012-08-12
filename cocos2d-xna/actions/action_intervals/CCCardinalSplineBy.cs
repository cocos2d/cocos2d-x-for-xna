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
    public class CCCardinalSplineBy : CCCardinalSplineTo
    {
        /** creates an action with a Cardinal Spline array of points and tension 
       @deprecated: This interface will be deprecated sooner or later.
       */
        public static CCCardinalSplineBy actionWithDuration(float duration, CCPointArray points, float tension)
        {
            return CCCardinalSplineBy.create(duration, points, tension);
        }

        /** creates an action with a Cardinal Spline array of points and tension */
        public static CCCardinalSplineBy create(float duration, CCPointArray points, float tension)
        {
            CCCardinalSplineBy ret = new CCCardinalSplineBy();
            ret.initWithDuration(duration, points, tension);
            return ret;
        }

        public CCCardinalSplineBy()
        {
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_startPosition = pTarget.position;
        }

        public virtual CCActionInterval reverse()
        {
            CCPointArray copyConfig = (CCPointArray)m_pPoints.copy();

            //
            // convert "absolutes" to "diffs"
            //
            CCPoint p = copyConfig.getControlPointAtIndex(0);
            for (int i = 1; i < copyConfig.count(); ++i)
            {
                CCPoint current = copyConfig.getControlPointAtIndex(i);
                CCPoint diff = current.Sub(p);
                copyConfig.replaceControlPoint(diff, i);

                p = current;
            }


            // convert to "diffs" to "reverse absolute"

            CCPointArray pReverse = copyConfig.reverse();

            // 1st element (which should be 0,0) should be here too

            p = pReverse.getControlPointAtIndex(pReverse.count() - 1);
            pReverse.removeControlPointAtIndex(pReverse.count() - 1);

            p = p.Neg();
            pReverse.insertControlPoint(p, 0);

            for (int i = 1; i < pReverse.count(); ++i)
            {
                CCPoint current = pReverse.getControlPointAtIndex(i);
                current = current.Neg();
                CCPoint abs = current.Add(p);
                pReverse.replaceControlPoint(abs, i);

                p = abs;
            }

            return CCCardinalSplineBy.create(m_fDuration, pReverse, m_fTension);
        }
        public virtual void updatePosition(CCPoint newPos)
        {
            m_pTarget.position = newPos.Add(m_startPosition);
        }

        protected CCPoint m_startPosition;
    }
}
