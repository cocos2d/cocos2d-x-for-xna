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

        public static bool CCPointEqualToPoint(CCPoint point1, CCPoint point2)
        {
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

        public static bool CCSizeEqualSize(CCSize size1, CCSize size2)
        {
            ///@todo
            throw new NotImplementedException();
        }
    }

    public class CCRect
    {
        public CCPoint origin;
        public CCSize size;

        public CCRect()
        {
            throw new NotImplementedException();
            ///@todo initialization
        }

        public CCRect(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
            ///@todo initialization
        }

        // return the leftmost x-value of 'rect'
        public static float CCRectGetMinX(CCRect rect)
        {
            ///@todo
            throw new NotImplementedException();
        }

        // return the rightmost x-value of 'rect'
        public static float CCRectGetMaxX(CCRect rect)
        {
            ///@todo
            throw new NotImplementedException();
        }

        // return the midpoint x-value of 'rect'
        public static float CCRectGetMidX(CCRect rect)
        {
            ///@todo
            throw new NotImplementedException();
        }

        // Return the bottommost y-value of 'rect'
        public static float CCRectGetMinY(CCRect rect)
        {
            ///@todo
            throw new NotImplementedException();
        }

        // Return the topmost y-value of 'rect'
        public static float CCRectGetMaxY(CCRect rect)
        {
            ///@todo
            throw new NotImplementedException();
        }

        // Return the midpoint y-value of 'rect'
        public static float CCRectGetMidY(CCRect rect)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static bool CCRectEqualToRect(CCRect rect1, CCRect rect2)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static bool CCRectContainsPoint(CCRect rect1, CCRect rect2)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static bool CCRectIntersetsRect(CCRect rect1, CCRect rect2)
        {
            ///@todo
            throw new NotImplementedException();
        }
    }
}
