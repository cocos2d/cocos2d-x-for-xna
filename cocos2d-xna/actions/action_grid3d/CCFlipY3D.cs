/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com
 
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
    public class CCFlipY3D : CCFlipX3D
    {
        public override void update(float time)
        {
            float angle = (float)Math.PI * time; // 180 degrees
            float mz = (float)Math.Sin(angle);
            angle = angle / 2.0f;     // x calculates degrees from 0 to 90
            float my = (float)Math.Cos(angle);

            ccVertex3F v0, v1, v;
            ccVertex3F diff = new ccVertex3F();

            v0 = originalVertex(new ccGridSize(1, 1));
            v1 = originalVertex(new ccGridSize(0, 0));

            float y0 = v0.y;
            float y1 = v1.y;
            float y;
            ccGridSize a, b, c, d;

            if (y0 > y1)
            {
                // Normal Grid
                a = new ccGridSize(0, 0);
                b = new ccGridSize(0, 1);
                c = new ccGridSize(1, 0);
                d = new ccGridSize(1, 1);
                y = y0;
            }
            else
            {
                // Reversed Grid
                b = new ccGridSize(0, 0);
                a = new ccGridSize(0, 1);
                d = new ccGridSize(1, 0);
                c = new ccGridSize(1, 1);
                y = y1;
            }

            diff.y = y - y * my;
            diff.z = Math.Abs((float)Math.Floor((y * mz) / 4.0f));

            // bottom-left
            v = originalVertex(a);
            v.y = diff.y;
            v.z += diff.z;
            setVertex(a, v);

            // upper-left
            v = originalVertex(b);
            v.y -= diff.y;
            v.z -= diff.z;
            setVertex(b, v);

            // bottom-right
            v = originalVertex(c);
            v.y = diff.y;
            v.z += diff.z;
            setVertex(c, v);

            // upper-right
            v = originalVertex(d);
            v.y -= diff.y;
            v.z -= diff.z;
            setVertex(d, v);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFlipY3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFlipY3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFlipY3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            // CCFlipX3D::copyWithZone(pZone);
            copyWithZone(pZone);
            pCopy.initWithSize(m_sGridSize, m_fDuration);

            // CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        /** creates the action with duration */
        public static CCFlipY3D actionWithDuration(float duration)
        {
            CCFlipY3D pAction = new CCFlipY3D();

            if (pAction != null)
            {
                if (pAction.initWithSize(new ccGridSize(1, 1), duration))
                {
                    // pAction->autorelease();
                }
                else
                {
                    // CC_SAFE_RELEASE_NULL(pAction);
                }
            }

            return pAction;
        }
    }
}
