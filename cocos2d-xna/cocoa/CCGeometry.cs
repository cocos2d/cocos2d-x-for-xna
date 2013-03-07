/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org


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
using System.Diagnostics;
namespace cocos2d
{
    public class CCPoint
    {
        private float _x, _y;

        public float x
        {
            get
            {
                return (_x);
            }
            set
            {
                _x = value;
            }
        }

        public float y
        {
            get
            {
                return (_y);
            }
            set
            {
                _y = value;
            }
        }

        public CCPoint()
        {
            _x = 0f;
            _y = 0f;
        }

        public CCPoint(CCPoint copy)
        {
            _x = copy.x;
            _y = copy.y;
        }

        public CCPoint(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        public bool IsZero
        {
            get
            {
                return (_x == 0f && _y == 0f);
            }
        }

        public CCPoint Add(CCPoint pt)
        {
            CCPoint np = new CCPoint(x + pt.x, y + pt.y);
            return (np);
        }
        public static CCPoint Zero
        {
            get { return new CCPoint(0f, 0f); }
        }

        public CCPoint Neg()
        {
            return (ccp(-x, -y));
        }

        public CCPoint ccp(float x, float y)
        {
            return (new CCPoint(x, y));
        }

        public CCPoint Sub(CCPoint v2)
        {
            return ccp(x - v2.x, y - v2.y);
        }
        public CCPoint Mult(float s)
        {
            return ccp(x * s, y * s);
        }
        public CCPoint Midpoint(CCPoint v2)
        {
            return Add(v2).Mult(0.5f);
        }
        public float Dot(CCPoint v2)
        {
            return x * v2.x + y * v2.y;
        }
        public float Cross(CCPoint v2)
        {
            return x * v2.y - y * v2.x;
        }
        public CCPoint Perp()
        {
            return ccp(-y, x);
        }
        public CCPoint RPerp()
        {
            return ccp(y, -x);
        }
        /// <summary>
        /// Returns the projection of this over v2.
        /// </summary>
        /// <param name="v2"></param>
        /// <returns></returns>
        public CCPoint Project(CCPoint v2)
        {
            return v2.Mult(Dot(v2) / v2.Dot(v2));
        }
        public CCPoint Rotate(CCPoint v2)
        {
            return ccp(x * v2.x - y * v2.y, x * v2.y + y * v2.x);
        }
        public CCPoint Unrotate(CCPoint v2)
        {
            return ccp(x * v2.x + y * v2.y, y * v2.x - x * v2.y);
        }
        public float LengthSQ()
        {
            return Dot(this);
        }
        [Obsolete("Use DistanceSQ instead. This method was a typograhpic error.")]
        public float DistanceCQ(CCPoint v2)
        {
            return (DistanceSQ(v2));
        }
        public float DistanceSQ(CCPoint v2)
        {
            return Sub(v2).LengthSQ();
        }

        public static float Length(CCPoint v)
        {
            return (float)Math.Sqrt(v.LengthSQ());
        }

        public static float Distance(CCPoint v1, CCPoint v2)
        {
            return Length(v1.Sub(v2));
        }

        public static CCPoint Normalize(CCPoint v)
        {
            return v.Mult(1f / Length(v));
        }

        public static CCPoint ForAngle(float a)
        {
            return new CCPoint((float)Math.Cos(a), (float)Math.Sin(a));
        }

        public static float ToAngle(CCPoint v)
        {
            return (float)Math.Atan2(v.y, v.x);
        }

        public static CCPoint Lerp(CCPoint a, CCPoint b, float alpha)
        {
            return a.Mult(1f - alpha).Add(b.Mult(alpha));
        }

        public static float clampf(float value, float min_inclusive, float max_inclusive)
        {
            if (min_inclusive > max_inclusive) {
                ccMacros.CC_SWAP(ref min_inclusive, ref max_inclusive);
            }
            return value < min_inclusive ? min_inclusive : value < max_inclusive? value : max_inclusive;
        }

        public CCPoint Clamp(CCPoint p, CCPoint min_inclusive, CCPoint max_inclusive)
        {
            return new CCPoint(clampf(p.x, min_inclusive.x, max_inclusive.x), clampf(p.y, min_inclusive.y, max_inclusive.y));
        }

        public static CCPoint FromSize(CCSize s)
        {
            return new CCPoint(s.width, s.height);
        }


        public static bool FuzzyEqual(CCPoint a, CCPoint b, float var)
        {
            if (a.x - var <= b.x && b.x <= a.x + var)
                if (a.y - var <= b.y && b.y <= a.y + var)
                    return true;
            return false;
        }

        public static CCPoint CompMult(CCPoint a, CCPoint b)
        {
            return new CCPoint(a.x * b.x, a.y * b.y);
        }

        public static float AngleSigned(CCPoint a, CCPoint b)
        {
            CCPoint a2 = Normalize(a);
            CCPoint b2 = Normalize(b);
            float angle = (float)Math.Atan2(a2.x * b2.y - a2.y * b2.x, a2.Dot(b2));
            if (Math.Abs(angle) < ccMacros.FLT_EPSILON) return 0f;
            return angle;
        }

        /** Rotates a point counter clockwise by the angle around a pivot
         @param v is the point to rotate
         @param pivot is the pivot, naturally
         @param angle is the angle of rotation cw in radians
         @returns the rotated point
         @since v0.99.1
         */
        public static CCPoint RotateByAngle(CCPoint v, CCPoint pivot, float angle)
        {
            CCPoint r = v.Sub(pivot);
            float cosa = (float)Math.Cos(angle), sina = (float)Math.Sin(angle);
            float t = r.x;
            r.x = t * cosa - r.y * sina + pivot.x;
            r.y = t * sina + r.y * cosa + pivot.y;
            return r;
        }


        public static bool SegmentIntersect(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S, T;

            if (LineIntersect(A, B, C, D, out S, out T)
                && (S >= 0.0f && S <= 1.0f && T >= 0.0f && T <= 1.0f))
                return true;

            return false;
        }

        public static CCPoint IntersectPoint(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S, T;

            if (LineIntersect(A, B, C, D, out S, out T))
            {
                // Point of intersection
                CCPoint P = new CCPoint();
                P.x = A.x + S * (B.x - A.x);
                P.y = A.y + S * (B.y - A.y);
                return P;
            }

            return CCPointZero;
        }

        public static readonly CCPoint CCPointZero = new CCPoint(0, 0);

        /** A general line-line intersection test
         @param p1 
            is the startpoint for the first line P1 = (p1 - p2)
         @param p2 
            is the endpoint for the first line P1 = (p1 - p2)
         @param p3 
            is the startpoint for the second line P2 = (p3 - p4)
         @param p4 
            is the endpoint for the second line P2 = (p3 - p4)
         @param s 
            is the range for a hitpoint in P1 (pa = p1 + s*(p2 - p1))
         @param t
            is the range for a hitpoint in P3 (pa = p2 + t*(p4 - p3))
         @return bool 
            indicating successful intersection of a line
            note that to truly test intersection for segments we have to make 
            sure that s & t lie within [0..1] and for rays, make sure s & t > 0
            the hit point is        p3 + t * (p4 - p3);
            the hit point also is    p1 + s * (p2 - p1);
         @since v0.99.1
         */
        public static bool LineIntersect(CCPoint A, CCPoint B,
                              CCPoint C, CCPoint D,
                              out float S, out float T)
        {
            S = 0f;
            T = 0f;

            // FAIL: Line undefined
            if ((A.x == B.x && A.y == B.y) || (C.x == D.x && C.y == D.y))
            {
                return false;
            }
            float BAx = B.x - A.x;
            float BAy = B.y - A.y;
            float DCx = D.x - C.x;
            float DCy = D.y - C.y;
            float ACx = A.x - C.x;
            float ACy = A.y - C.y;

            float denom = DCy * BAx - DCx * BAy;

            S = DCx * ACy - DCy * ACx;
            T = BAx * ACy - BAy * ACx;

            if (denom == 0)
            {
                if (S == 0 || T == 0)
                {
                    // Lines incident
                    return true;
                }
                // Lines parallel and not incident
                return false;
            }

            S = S / denom;
            T = T / denom;

            // Point of intersection
            // CGPoint P;
            // P.x = A.x + *S * (B.x - A.x);
            // P.y = A.y + *S * (B.y - A.y);

            return true;
        }

        public float Angle(CCPoint b)
        {
            float angle = (float)Math.Acos(Normalize(this).Dot(Normalize(b)));
            if (Math.Abs(angle) < ccMacros.FLT_EPSILON) return 0f;
            return angle;
        }


        public static bool CCPointEqualToPoint(CCPoint point1, CCPoint point2)
        {
            // Debug.Assert((point1 != null) && (point2 != null));

            if (false/*(point1 == null) || (point2 == null)*/)
            {
                return false;
            }
            else
            {
                return ((point1.x == point2.x) && (point1.y == point2.y));
            }
        }
    }

    public class CCSize
    {
        public float width;
        public float height;

        public CCSize(CCSize copy)
        {
            width = copy.width;
            height = copy.height;
        }
        public CCSize()
        {
            width = 0.0f;
            height = 0.0f;
        }

        public CCSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public static bool CCSizeEqualToSize(CCSize size1, CCSize size2)
        {
            Debug.Assert((size1 != null) && (size2 != null));

            if ((size1 == null) || (size2 == null))
            {
                return false;
            }
            else
            {
                return ((size1.width == size2.width) && (size1.height == size2.height));
            }
        }
    }

    public class CCRect
    {
        public CCPoint origin;
        public CCSize size;

        public CCRect()
            : this(0, 0, 0, 0)
        {

        }

        public CCRect(CCRect copy)
        {
            origin = new CCPoint(copy.origin.x, copy.origin.y);
            size = new CCSize(copy.size.width, copy.size.height);
        }

        public CCRect(float x, float y, float width, float height)
        {
            origin = new CCPoint();
            size = new CCSize();

            // Only support that, the width and height > 0
            Debug.Assert(width >= 0 && height >= 0);

            origin.x = x;
            origin.y = y;

            size.width = width;
            size.height = height;
        }

        // return the leftmost x-value of 'rect'
        public static float CCRectGetMinX(CCRect rect)
        {
            Debug.Assert(rect != null);

            // If rect is null, throw the exception
            return rect.origin.x;
        }

        // return the rightmost x-value of 'rect'
        public static float CCRectGetMaxX(CCRect rect)
        {
            Debug.Assert(rect != null);

            // If rect is null, throw the exception
            return rect.origin.x + rect.size.width;
        }

        // return the midpoint x-value of 'rect'
        public static float CCRectGetMidX(CCRect rect)
        {
            Debug.Assert(rect != null);

            // If rect is null, throw the exception
            return (rect.origin.x + rect.size.width / 2.0f);
        }

        // Return the bottommost y-value of 'rect'
        public static float CCRectGetMinY(CCRect rect)
        {
            Debug.Assert(rect != null);

            // If rect is null, throw the exception
            return rect.origin.y;
        }

        // Return the topmost y-value of 'rect'
        public static float CCRectGetMaxY(CCRect rect)
        {
            Debug.Assert(rect != null);

            // If rect is null, throw the exception
            return rect.origin.y + rect.size.height;
        }

        // Return the midpoint y-value of 'rect'
        public static float CCRectGetMidY(CCRect rect)
        {
            Debug.Assert(rect != null);

            // Return the midpoint y-value of 'rect'
            return (rect.origin.y + rect.size.height / 2.0f);
        }

        public static bool CCRectEqualToRect(CCRect rect1, CCRect rect2)
        {
            Debug.Assert((rect1 != null) && (rect2 != null));

            if ((rect1 == null) || (rect2 == null))
            {
                return false;
            }
            else
            {
                return (CCPoint.CCPointEqualToPoint(rect1.origin, rect2.origin)
                && (CCSize.CCSizeEqualToSize(rect1.size, rect2.size)));
            }
        }

        public static bool CCRectContainsPoint(CCRect rect, CCPoint point)
        {
            Debug.Assert((rect != null)/* && (point != null)*/);

            bool bRet = false;

            if (rect != null/* && point != null*/)
            {
                if (float.IsNaN(point.x))
                {
                    point.x = 0;
                }

                if (float.IsNaN(point.y))
                {
                    point.y = 0;
                }

/*
                float minx = CCRectGetMinX(rect);
                float maxx = CCRectGetMaxX(rect);
                float miny = CCRectGetMinY(rect);
                float maxy = CCRectGetMaxY(rect);
*/

                if (point.x >= CCRectGetMinX(rect)
                    && point.x <= CCRectGetMaxX(rect)
                    && point.y >= CCRectGetMinY(rect)
                    && point.y <= CCRectGetMaxY(rect))
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        /// <summary>
        /// Unions the space of the two rects, using rectA as the start of the union, and returns
        /// a new rect that encapsulates both rectangles.
        /// </summary>
        /// <param name="rectA"></param>
        /// <param name="rectB"></param>
        /// <returns></returns>
        public static CCRect CCRectUnion(CCRect rectA, CCRect rectB)
        {
            /* +-----+          +---------+
             * |     |          |         |
             * |  +-------+  ==>|         |
             * |  |       |     |         |
             * +--|       |     |         |
             *    +-------+     +---------+
             */
            // Set the X
            float xLA = rectA.origin.x;
            float xLB = rectB.origin.x;
            float xRA = xLA + rectA.size.width;
            float xRB = xLB + rectB.size.width;
            if (xLB < xLA)
            {
                xLA = xLB;
            }
            if (xRB > xRA)
            {
                xRA = xRB;
            }
            // Set the Y
            float yBA = rectA.origin.y;
            float yBB = rectB.origin.y;
            float yTA = yBA + rectA.size.height;
            float yTB = yBB + rectB.size.height;
            if (yBB < yBA)
            {
                yBA = yBB;
            }
            if (yTB > yTA)
            {
                yTA = yTB;
            }
            // Compute the new rect
            CCRect r = new CCRect(xLA, yBA, xRA - xLA, yTA - yBA);
            return (r);
        }

        public static bool CCRectIntersetsRect(CCRect rectA, CCRect rectB)
        {
            Debug.Assert((rectA != null) && (rectB != null));

            bool bRet = false;

            if ((rectA != null) && (rectB != null))
            {
                bRet = !(CCRectGetMaxX(rectA) < CCRectGetMinX(rectB)
                      || CCRectGetMaxX(rectB) < CCRectGetMinX(rectA)
                      || CCRectGetMaxY(rectA) < CCRectGetMinY(rectB)
                      || CCRectGetMaxY(rectB) < CCRectGetMinY(rectA));
            }

            return bRet;
        }
    }
}
