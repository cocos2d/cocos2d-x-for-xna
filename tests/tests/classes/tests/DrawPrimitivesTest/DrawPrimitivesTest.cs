using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class DrawPrimitivesTest : CCLayer
    {
        public DrawPrimitivesTest()
        {

        }
        public virtual void draw()
        {
            base.draw();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // draw a simple line
            // The default state is:
            // Line Width: 1
            // color: 255,255,255,255 (white, non-transparent)
            // Anti-Aliased
            //glEnable(GL_LINE_SMOOTH);
            //ccDrawLine( CCPointMake(0, 0), CCPointMake(s.width, s.height) );

            // line: color, width, aliased
            // glLineWidth > 1 and GL_LINE_SMOOTH are not compatible
            // GL_SMOOTH_LINE_WIDTH_RANGE = (1,1) on iPhone
            //glDisable(GL_LINE_SMOOTH);
            //glLineWidth( 5.0f );
            /*glColor4ub(255,0,0,255);*/
            //glColor4f(1.0, 0.0, 0.0, 1.0);
            //ccDrawLine( CCPointMake(0, s.height), CCPointMake(s.width, 0) );

            // TIP:
            // If you are going to use always the same color or width, you don't
            // need to call it before every draw
            //
            // Remember: OpenGL is a state-machine.

            // draw big point in the center
            //glPointSize(64);
            /*glColor4ub(0,0,255,128);*/
            //glColor4f(0.0, 0.0, 1.0, 0.5);
            //ccDrawPoint( CCPointMake(s.width / 2, s.height / 2) );

            // draw 4 small points
            CCPoint[] points = { new CCPoint(60, 60), new CCPoint(70, 70), new CCPoint(60, 70), new CCPoint(70, 60) };
            //glPointSize(4);
            /*glColor4ub(0,255,255,255);*/
            //glColor4f(0.0, 1.0, 1.0, 1.0);
            //ccDrawPoints( points, 4);

            // draw a green circle with 10 segments
            //glLineWidth(16);
            /*glColor4ub(0, 255, 0, 255);*/
            //glColor4f(0.0, 1.0, 0.0, 1.0);
            //ccDrawCircle( CCPointMake(s.width/2,  s.height/2), 100, 0, 10, false);

            // draw a green circle with 50 segments with line to center
            //glLineWidth(2);
            /*glColor4ub(0, 255, 255, 255);*/
            //glColor4f(0.0, 1.0, 1.0, 1.0);
            //ccDrawCircle( CCPointMake(s.width/2, s.height/2), 50, CC_DEGREES_TO_RADIANS(90), 50, true);	

            // open yellow poly
            /*glColor4ub(255, 255, 0, 255);*/
            //glColor4f(1.0, 1.0, 0.0, 1.0);
            //glLineWidth(10);
            CCPoint[] vertices = { new CCPoint(0, 0), new CCPoint(50, 50), new CCPoint(100, 50), new CCPoint(100, 100), new CCPoint(50, 100) };
            //ccDrawPoly( vertices, 5, false);

            // closed purble poly
            /*glColor4ub(255, 0, 255, 255);*/
            //glColor4f(1.0, 0.0, 1.0, 1.0);
            //glLineWidth(2);
            CCPoint[] vertices2 = { new CCPoint(30, 130), new CCPoint(30, 230), new CCPoint(50, 200) };
            //ccDrawPoly( vertices2, 3, true);

            // draw quad bezier path
            //ccDrawQuadBezier(new CCPoint(0, s.height), new CCPoint(s.width / 2, s.height / 2), new CCPoint(s.width, s.height), 50);

            // draw cubic bezier path
            //ccDrawCubicBezier(new CCPoint(s.width / 2, s.height / 2), new CCPoint(s.width / 2 + 30, s.height / 2 + 50), new CCPoint(s.width / 2 + 60, s.height / 2 - 50), new CCPoint(s.width, s.height / 2), 100);


            // restore original values
            //glLineWidth(1);
            /*glColor4ub(255,255,255,255);*/
            //glColor4f(1.0, 1.0, 1.0, 1.0);
            //glPointSize(1); 
        }
    }
}
