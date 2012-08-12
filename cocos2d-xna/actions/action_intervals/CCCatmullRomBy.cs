using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /** An action that moves the target with a CatmullRom curve by a certain distance.
     A Catmull Rom is a Cardinal Spline with a tension of 0.5.
     http://en.wikipedia.org/wiki/Cubic_Hermite_spline#Catmull.E2.80.93Rom_spline
     */
    public class CCCatmullRomBy : CCCardinalSplineBy
    {
        /** creates an action with a Cardinal Spline array of points and tension 
        */
        public static CCCatmullRomBy actionWithDuration(float dt, CCPointArray points)
        {
            return CCCatmullRomBy.create(dt, points);
        }

        /** creates an action with a Cardinal Spline array of points and tension */
        public static CCCatmullRomBy create(float dt, CCPointArray points)
        {
            CCCatmullRomBy by = new CCCatmullRomBy();
            by.initWithDuration(dt, points);
            return (by);
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
