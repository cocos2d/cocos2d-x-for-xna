/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

http://www.cocos2d-x.org
http://www.openxlive.com

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

namespace cocos2d
{
    /**
 @file
 Drawing OpenGL ES primitives.
 - ccDrawPoint
 - ccDrawLine
 - ccDrawPoly
 - ccDrawCircle
 - ccDrawQuadBezier
 - ccDrawCubicBezier
 
 You can change the color, width and other property by calling the
 glColor4ub(), glLineWidth(), glPointSize().
 
 @warning These functions draws the Line, Point, Polygon, immediately. They aren't batched. If you are going to make a game that depends on these primitives, I suggest creating a batch.
 */
    public class CCDrawingPrimitives
    {
        /// <summary>
        /// draws a point given x and y coordinate measured in points
        /// </summary>
        public static void ccDrawPoint(CCPoint point)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// draws an array of points.
        ///@since v0.7.2
        /// </summary>
        public void ccDrawPoints(CCPoint points, int numberOfPoints)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// draws a line given the origin and destination point measured in points
        /// </summary>
        public void ccDrawLine(CCPoint origin, CCPoint destination)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
        /// The polygon can be closed or open
        /// </summary>
        public void ccDrawPoly(CCPoint vertices, int numOfVertices, bool closePolygon)
        { }

        /// <summary>
        /// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
        /// The polygon can be closed or open and optionally filled with current GL color
        /// </summary>
        public void ccDrawPoly(CCPoint vertices, int numOfVertices, bool closePolygon, bool fill)
        { throw new NotImplementedException(); }

        /// <summary>
        /// draws a circle given the center, radius and number of segments.
        /// </summary>
        public void ccDrawCircle(CCPoint center, float radius, float angle, int segments, bool drawLineToCenter)
        { throw new NotImplementedException(); }

        /// <summary>
        /// draws a quad bezier path
        /// @since v0.8
        /// </summary>
        public void ccDrawQuadBezier(CCPoint origin, CCPoint control, CCPoint destination, int segments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// draws a cubic bezier path
        /// @since v0.8
        /// </summary>
        public void ccDrawCubicBezier(CCPoint origin, CCPoint control1, CCPoint control2, CCPoint destination, int segments)
        {
            throw new NotImplementedException();
        }
    }
}
