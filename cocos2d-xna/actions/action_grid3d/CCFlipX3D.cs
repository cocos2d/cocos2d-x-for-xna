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
using System.Diagnostics;

namespace cocos2d
{
    public class CCFlipX3D : CCGrid3DAction
    {
        /// <summary>
        /// initializes the action with duration
        /// </summary>
        public bool initWithDuration(float duration)
        {
            return initWithSize(new ccGridSize(1, 1), duration);
        }

        public virtual bool initWithSize(ccGridSize gridSize, float duration)
        {
            if (gridSize.x != 1 || gridSize.y != 1)
            {
                // Grid size must be (1,1)
                Debug.Assert(false);

                return false;
            }

            return base.initWithSize(gridSize, duration);
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCFlipX3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCFlipX3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCFlipX3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithSize(m_sGridSize, m_fDuration);

            // CC_SAFE_DELETE(pNewZone);
            return pCopy;
        }

        public override void update(float time)
        {
            float angle = (float)Math.PI * time; // 180 degrees
            float mz = (float)Math.Sin(angle);
            angle = angle / 2.0f; // x calculates degrees from 0 to 90
            float mx = (float)Math.Cos(angle);

            ccVertex3F v0, v1, v;
            ccVertex3F diff = new ccVertex3F();

            v0 = originalVertex(new ccGridSize(1, 1));
            v1 = originalVertex(new ccGridSize(0, 0));

            float x0 = v0.x;
            float x1 = v1.x;
            float x;
            ccGridSize a, b, c, d;

            if (x0 > x1)
            {
                // Normal Grid
                a = new ccGridSize(0, 0);
                b = new ccGridSize(0, 1);
                c = new ccGridSize(1, 0);
                d = new ccGridSize(1, 1);
                x = x0;
            }
            else
            {
                // Reversed Grid
                c = new ccGridSize(0, 0);
                d = new ccGridSize(0, 1);
                a = new ccGridSize(1, 0);
                b = new ccGridSize(1, 1);
                x = x1;
            }

            diff.x = (x - x * mx);
            diff.z = Math.Abs((float)Math.Floor((x * mz) / 4.0f));

            // bottom-left
            v = originalVertex(a);
            v.x = diff.x;
            v.z += diff.z;
            setVertex(a, v);

            // upper-left
            v = originalVertex(b);
            v.x = diff.x;
            v.z += diff.z;
            setVertex(b, v);

            // bottom-right
            v = originalVertex(c);
            v.x -= diff.x;
            v.z -= diff.z;
            setVertex(c, v);

            // upper-right
            v = originalVertex(d);
            v.x -= diff.x;
            v.z -= diff.z;
            setVertex(d, v);
        }

        /// <summary>
        /// creates the action with duration
        /// </summary>
        public new static CCFlipX3D actionWithDuration(float duration)
        {
            CCFlipX3D pAction = new CCFlipX3D();

            if (pAction.initWithSize(new ccGridSize(1, 1), duration))
            {
                return pAction;
            }

            return null;
        }
    }
}
