using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public abstract class CCVertex
    {
        public static void LineToPolygon(CCPoint[] points, float stroke, ccVertex2F[] vertices, int offset, int nuPoints)
        {
            nuPoints += offset;
            if (nuPoints <= 1) return;

            stroke *= 0.5f;

            int idx;
            int nuPointsMinus = nuPoints - 1;

            for (int i = offset; i < nuPoints; i++)
            {
                idx = i * 2;
                CCPoint p1 = points[i];
                CCPoint perpVector;

                if (i == 0)
                    perpVector = CCPointExtension.ccpPerp(CCPointExtension.ccpNormalize(CCPointExtension.ccpSub(p1, points[i + 1])));
                else if (i == nuPointsMinus)
                    perpVector = CCPointExtension.ccpPerp(CCPointExtension.ccpNormalize(CCPointExtension.ccpSub(points[i - 1], p1)));
                else
                {
                    CCPoint p2 = points[i + 1];
                    CCPoint p0 = points[i - 1];

                    CCPoint p2p1 = CCPointExtension.ccpNormalize(CCPointExtension.ccpSub(p2, p1));
                    CCPoint p0p1 = CCPointExtension.ccpNormalize(CCPointExtension.ccpSub(p0, p1));

                    // Calculate angle between vectors
                    float angle = (float)Math.Acos(CCPointExtension.ccpDot(p2p1, p0p1));

                    if (angle < ccMacros.CC_DEGREES_TO_RADIANS(70))
                        perpVector = CCPointExtension.ccpPerp(CCPointExtension.ccpNormalize(CCPointExtension.ccpMidpoint(p2p1, p0p1)));
                    else if (angle < ccMacros.CC_DEGREES_TO_RADIANS(170))
                        perpVector = CCPointExtension.ccpNormalize(CCPointExtension.ccpMidpoint(p2p1, p0p1));
                    else
                        perpVector = CCPointExtension.ccpPerp(CCPointExtension.ccpNormalize(CCPointExtension.ccpSub(p2, p0)));
                }
                perpVector = CCPointExtension.ccpMult(perpVector, stroke);

                vertices[idx] = new ccVertex2F(p1.x + perpVector.x, p1.y + perpVector.y);
                vertices[idx + 1] = new ccVertex2F(p1.x - perpVector.x, p1.y - perpVector.y);

            }

            // Validate vertexes
            offset = (offset == 0) ? 0 : offset - 1;
            for (int i = offset; i < nuPointsMinus; i++)
            {
                idx = i * 2;
                int idx1 = idx + 2;

                ccVertex2F p1 = vertices[idx];
                ccVertex2F p2 = vertices[idx + 1];
                ccVertex2F p3 = vertices[idx1];
                ccVertex2F p4 = vertices[idx1 + 1];

                float s = 0f;
                //BOOL fixVertex = !ccpLineIntersect(ccp(p1.x, p1.y), ccp(p4.x, p4.y), ccp(p2.x, p2.y), ccp(p3.x, p3.y), &s, &t);
                bool fixVertex = !LineIntersect(p1.x, p1.y, p4.x, p4.y, p2.x, p2.y, p3.x, p3.y, out s);
                if (!fixVertex)
                    if (s < 0.0f || s > 1.0f)
                        fixVertex = true;

                if (fixVertex)
                {
                    vertices[idx1] = p4;
                    vertices[idx1 + 1] = p3;
                }
            }
        }

        public static bool LineIntersect(float Ax, float Ay,
                                       float Bx, float By,
                                       float Cx, float Cy,
                                       float Dx, float Dy, out float T)
        {
            float distAB, theCos, theSin, newX;
            T = 0f;

            // FAIL: Line undefined
            if ((Ax == Bx && Ay == By) || (Cx == Dx && Cy == Dy)) return false;

            //  Translate system to make A the origin
            Bx -= Ax; By -= Ay;
            Cx -= Ax; Cy -= Ay;
            Dx -= Ax; Dy -= Ay;

            // Length of segment AB
            distAB = (float)Math.Sqrt(Bx * Bx + By * By);

            // Rotate the system so that point B is on the positive X axis.
            theCos = Bx / distAB;
            theSin = By / distAB;
            newX = Cx * theCos + Cy * theSin;
            Cy = Cy * theCos - Cx * theSin; Cx = newX;
            newX = Dx * theCos + Dy * theSin;
            Dy = Dy * theCos - Dx * theSin; Dx = newX;

            // FAIL: Lines are parallel.
            if (Cy == Dy) return false;

            // Discover the relative position of the intersection in the line AB
            T = (Dx + (Cx - Dx) * Dy / (Dy - Cy)) / distAB;

            // Success.
            return true;
        }
    }
}
