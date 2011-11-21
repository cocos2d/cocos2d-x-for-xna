/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org

http://www.cocos2d-x.org

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
        public float x;
        public float y;

        public CCPoint()
        {
            x = 0.0f;
            y = 0.0f;
        }

        public CCPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static CCPoint Zero
        {
            get { return new CCPoint(0, 0); }
        }

        public static bool CCPointEqualToPoint(CCPoint point1, CCPoint point2)
        {
            Debug.Assert((point1 != null) && (point2 != null));

            if ((point1 == null) || (point2 == null))
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
            Debug.Assert((rect != null) && (point != null));

            bool bRet = false;

            if (rect != null && point != null)
            {
                float minx = CCRectGetMinX(rect);
                float maxx = CCRectGetMaxX(rect);
                float miny = CCRectGetMinY(rect);
                float maxy= CCRectGetMaxY(rect);

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
