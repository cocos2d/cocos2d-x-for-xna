/*
 * Copyright (c) 2010-2012 cocos2d-x.org
 * cocos2d for iPhone: http://www.cocos2d-iphone.org
 *
 * Copyright (c) 2008 Radu Gruian
 *
 * Copyright (c) 2011 Vit Valentin
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 *
 * Orignal code by Radu Gruian: http://www.codeproject.com/Articles/30838/Overhauser-Catmull-Rom-Splines-for-Camera-Animatio.So
 *
 * Adapted to cocos2d-x by Vit Valentin
 *
 * Adapted from cocos2d-x to cocos2d-iphone by Ricardo Quesada
 */
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
