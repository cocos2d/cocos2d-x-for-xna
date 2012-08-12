using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d.actions
{
    /** An action that moves the target with a CatmullRom curve to a destination point.
     A Catmull Rom is a Cardinal Spline with a tension of 0.5.
     http://en.wikipedia.org/wiki/Cubic_Hermite_spline#Catmull.E2.80.93Rom_spline
     @ingroup Actions
     */
    public class CCCatmullRomTo : CCCardinalSplineTo
    {
        /** creates an action with a Cardinal Spline array of points and tension 
        @deprecated: This interface will be deprecated sooner or later.
        */
        public static CCCatmullRomTo actionWithDuration(float dt, CCPointArray points)
        {
            return CCCatmullRomTo.create(dt, points);
        }

        /** creates an action with a Cardinal Spline array of points and tension */
        public static CCCatmullRomTo create(float dt, CCPointArray points)
        {
            CCCatmullRomTo ret = new CCCatmullRomTo();
            ret.initWithDuration(dt, points);
            return ret;
        }

        /** initializes the action with a duration and an array of points */
        public virtual bool initWithDuration(float dt, CCPointArray points)
        {
            if (base.initWithDuration(dt, points, 0.5f))
            {
                return true;
            }
    
            return false;
        }
    }
}
