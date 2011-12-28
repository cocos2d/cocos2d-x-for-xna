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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            //ccVertex2F p = { point.x * CC_CONTENT_SCALE_FACTOR(), point.y * CC_CONTENT_SCALE_FACTOR() };
            //// Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            //// Needed states: GL_VERTEX_ARRAY, 
            //// Unneeded states: GL_TEXTURE_2D, GL_TEXTURE_COORD_ARRAY, GL_COLOR_ARRAY	
            //glDisable(GL_TEXTURE_2D);
            //glDisableClientState(GL_TEXTURE_COORD_ARRAY);
            //glDisableClientState(GL_COLOR_ARRAY);

            //glVertexPointer(2, GL_FLOAT, 0, &p);
            //glDrawArrays(GL_POINTS, 0, 1);

            //// restore default state
            //glEnableClientState(GL_COLOR_ARRAY);
            //glEnableClientState(GL_TEXTURE_COORD_ARRAY);
            //glEnable(GL_TEXTURE_2D);
        }

        /// <summary>
        /// draws an array of points.
        ///@since v0.7.2
        /// </summary>
        public static void ccDrawPoints(CCPoint points, int numberOfPoints)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// draws a line given the origin and destination point measured in points
        /// </summary>
        public static void ccDrawLine(CCPoint origin, CCPoint destination, ccColor4F color)
        {
            float factor = CCDirector.sharedDirector().ContentScaleFactor;

            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0] = new VertexPositionColor(new Vector3(origin.x * factor, origin.y * factor, 0), new Color(color.r, color.g, color.b, color.a));
            vertices[1] = new VertexPositionColor(new Vector3(destination.x * factor, destination.y * factor, 0), new Color(color.r, color.g, color.b, color.a));

            CCApplication app = CCApplication.sharedApplication();
            app.basicEffect.TextureEnabled = false;
            app.basicEffect.VertexColorEnabled = true;
            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);
            }
        }

        /// <summary>
        /// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
        /// The polygon can be closed or open
        /// </summary>
        public static void ccDrawPoly(CCPoint[] vertices, int numOfVertices, bool closePolygon, ccColor4F color)
        {
            ccDrawPoly(vertices, numOfVertices, closePolygon, false, color);
        }

        /// <summary>
        /// draws a poligon given a pointer to CCPoint coordiantes and the number of vertices measured in points.
        /// The polygon can be closed or open and optionally filled with current GL color
        /// </summary>
        public static void ccDrawPoly(CCPoint[] vertices, int numOfVertices, bool closePolygon, bool fill, ccColor4F color)
        {
            VertexPositionColor[] newPoint = new VertexPositionColor[numOfVertices + 1];
            for (int i = 0; i < numOfVertices; i++)
            {
                newPoint[i] = new VertexPositionColor();
                newPoint[i].Position = new Vector3(vertices[i].x, vertices[i].y, 0);
                newPoint[i].Color = new Color(color.r, color.g, color.b, color.a);
            }

            CCApplication app = CCApplication.sharedApplication();
            app.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            app.basicEffect.TextureEnabled = false;
            app.basicEffect.VertexColorEnabled = true;

            short[] indexes = new short[(numOfVertices - 2) * 3];
            if (fill)
            {
                for (int i = 0; i < numOfVertices - 2; i++)
                {
                    indexes[i * 3 + 0] = 0;
                    indexes[i * 3 + 1] = (short)(i + 2);
                    indexes[i * 3 + 2] = (short)(i + 1);
                }

                foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    app.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip,
                        newPoint, 0, numOfVertices,
                        indexes, 0, numOfVertices - 2);
                }
            }
            else
            {
                if (closePolygon)
                {
                    newPoint[numOfVertices] = newPoint[0];
                    numOfVertices += 1;
                }

                foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    app.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip,
                          newPoint, 0, numOfVertices - 1);
                }
            }




            //if (closePolygon)
            //{
            //    glDrawArrays(fill ? GL_TRIANGLE_FAN : GL_LINE_LOOP, 0, numberOfPoints);
            //}
            //else
            //{
            //    glDrawArrays(fill ? GL_TRIANGLE_FAN : GL_LINE_STRIP, 0, numberOfPoints);
            //}
        }

        /// <summary>
        /// draws a circle given the center, radius and number of segments.
        /// </summary>
        public static void ccDrawCircle(CCPoint center, float radius, float angle, int segments, bool drawLineToCenter, ccColor4B color)
        {
            int additionalSegment = 1;
            if (drawLineToCenter)
            {
                ++additionalSegment;
            }

            CCApplication app = CCApplication.sharedApplication();
            float factor = CCDirector.sharedDirector().ContentScaleFactor;
            float coef = 2.0f * (float)(Math.PI) / segments;

            VertexPositionColor[] vertices = new VertexPositionColor[2 * (segments + 2)]; //	float *vertices = (float *)malloc( sizeof(float)*2*(segs+2));

            //memset(vertices, 0, sizeof(float) * 2 * (segs + 2));

            for (int i = 0; i <= segments; i++)
            {
                float rads = i * coef;
                float j = radius * (float)Math.Cos(rads + angle) + center.x;
                float k = radius * (float)Math.Sin(rads + angle) + center.y;

                vertices[i] = new VertexPositionColor();
                vertices[i].Position = new Vector3(j * factor, k * factor, 0);
                vertices[i].Color = new Color(color.r, color.g, color.b, color.a);
            }

            //vertices[(segments + 1) * 2] = new VertexPositionColor();
            //vertices[(segments + 1) * 2].Position = new Vector3(center.x * factor, center.y * factor, 0);

            app.basicEffect.TextureEnabled = false;
            app.basicEffect.VertexColorEnabled = true;
            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, segments);
            }
        }

        /// <summary>
        /// draws a quad bezier path
        /// @since v0.8
        /// </summary>
        public static void ccDrawQuadBezier(CCPoint origin, CCPoint control, CCPoint destination, int segments, ccColor4F color)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[segments + 1];
            float factor = CCDirector.sharedDirector().ContentScaleFactor;
            CCApplication app = CCApplication.sharedApplication();

            float t = 0.0f;
            for (int i = 0; i < segments; i++)
            {
                float x = (float)Math.Pow(1 - t, 2) * origin.x + 2.0f * (1 - t) * t * control.x + t * t * destination.x;
                float y = (float)Math.Pow(1 - t, 2) * origin.y + 2.0f * (1 - t) * t * control.y + t * t * destination.y;
                vertices[i] = new VertexPositionColor();
                vertices[i].Position = new Vector3(x * factor, y * factor, 0);
                vertices[i].Color = new Color(color.r, color.g, color.b, color.a);
                t += 1.0f / segments;
            }
            vertices[segments] = new VertexPositionColor()
            {
                Position = new Vector3(destination.x * factor, destination.y * factor, 0),
                Color = new Color(color.r, color.g, color.b, color.a),
            };

            app.basicEffect.TextureEnabled = false;
            app.basicEffect.VertexColorEnabled = true;
            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                app.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, segments);
            }
        }

        /// <summary>
        /// draws a cubic bezier path
        /// @since v0.8
        /// </summary>
        public static void ccDrawCubicBezier(CCPoint origin, CCPoint control1, CCPoint control2, CCPoint destination, int segments, ccColor4F color)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[segments + 1];
            float factor = CCDirector.sharedDirector().ContentScaleFactor;
            CCApplication app = CCApplication.sharedApplication();

            float t = 0;
            for (int i = 0; i < segments; ++i)
            {
                float x = (float)Math.Pow(1 - t, 3) * origin.x + 3.0f * (float)Math.Pow(1 - t, 2) * t * control1.x + 3.0f * (1 - t) * t * t * control2.x + t * t * t * destination.x;
                float y = (float)Math.Pow(1 - t, 3) * origin.y + 3.0f * (float)Math.Pow(1 - t, 2) * t * control1.y + 3.0f * (1 - t) * t * t * control2.y + t * t * t * destination.y;
                vertices[i] = new VertexPositionColor();
                vertices[i].Position = new Vector3(x * factor, y * factor, 0);
                vertices[i].Color = new Color(color.r, color.g, color.b, color.a);
                t += 1.0f / segments;
            }
            vertices[segments] = new VertexPositionColor()
            {
                Color = new Color(color.r, color.g, color.b, color.a),
                Position = new Vector3(destination.x * factor, destination.y * factor, 0)
            };

            app.basicEffect.TextureEnabled = false;
            app.basicEffect.VertexColorEnabled = true;
            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, segments);
            }

            // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Needed states: GL_VERTEX_ARRAY, 
            // Unneeded states: GL_TEXTURE_2D, GL_TEXTURE_COORD_ARRAY, GL_COLOR_ARRAY	
            //glDisable(GL_TEXTURE_2D);
            //glDisableClientState(GL_TEXTURE_COORD_ARRAY);
            //glDisableClientState(GL_COLOR_ARRAY);

            //glVertexPointer(2, GL_FLOAT, 0, vertices);
            //glDrawArrays(GL_LINE_STRIP, 0, (GLsizei)segments + 1);
            //delete[] vertices;

            //// restore default state
            //glEnableClientState(GL_COLOR_ARRAY);
            //glEnableClientState(GL_TEXTURE_COORD_ARRAY);
            //glEnable(GL_TEXTURE_2D);
        }
    }
}
