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
    public class CCLens3D : CCGrid3DAction
    {
        /// <summary>
        /// Get lens center position
        /// </summary>
        public float getLensEffect()
        {
            return m_fLensEffect;
        }

        /// <summary>
        /// Set lens center position
        /// </summary>
        public void setLensEffect(float fLensEffect)
        {
            m_fLensEffect = fLensEffect;
        }

        public CCPoint getPosition()
        {
            return m_position;
        }

        public void setPosition(CCPoint pos)
        {
            if (!CCPoint.CCPointEqualToPoint(pos, m_position))
            {
                m_position = pos;
                m_positionInPixels.x = pos.x * CCDirector.sharedDirector().ContentScaleFactor;
                m_positionInPixels.y = pos.y * CCDirector.sharedDirector().ContentScaleFactor;
                m_bDirty = true;
            }
        }

        /// <summary>
        ///  initializes the action with center position, radius, a grid size and duration
        /// </summary>
        public bool initWithPosition(CCPoint pos, float r, ccGridSize gridSize, float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_position = new CCPoint(-1, -1);
                m_positionInPixels = new CCPoint();

                setPosition(pos);
                m_fRadius = r;
                m_fLensEffect = 0.7f;
                m_bDirty = true;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCLens3D pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                // in case of being called at sub class
                pCopy = (CCLens3D)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCLens3D();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithPosition(m_position, m_fRadius, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void update(float time)
        {
            if (m_bDirty)
            {
                int i, j;

                for (i = 0; i < m_sGridSize.x + 1; ++i)
                {
                    for (j = 0; j < m_sGridSize.y + 1; ++j)
                    {
                        ccVertex3F v = originalVertex(new ccGridSize(i, j));
                        CCPoint vect = new CCPoint(m_positionInPixels.x - new CCPoint(v.x, v.y).x, m_positionInPixels.y - new CCPoint(v.x, v.y).y);
                        float r = CCPointExtension.ccpLength(vect);

                        if (r < m_fRadius)
                        {
                            r = m_fRadius - r;
                            float pre_log = r / m_fRadius;
                            if (pre_log == 0)
                            {
                                pre_log = 0.001f;
                            }

                            float l = (float)Math.Log(pre_log) * m_fLensEffect;
                            float new_r = (float)Math.Exp(l) * m_fRadius;

                            if (Math.Sqrt((vect.x * vect.x + vect.y * vect.y)) > 0)
                            {
                                vect = CCPointExtension.ccpNormalize(vect);

                                CCPoint new_vect = CCPointExtension.ccpMult(vect, new_r); ;
                                v.z += CCPointExtension.ccpLength(new_vect) * m_fLensEffect;
                            }
                        }

                        setVertex(new ccGridSize(i, j), v);
                    }
                }

                m_bDirty = false;
            }
        }

        /// <summary>
        /// creates the action with center position, radius, a grid size and duration
        /// </summary>
        public static CCLens3D actionWithPosition(CCPoint pos, float r, ccGridSize gridSize, float duration)
        {
            CCLens3D pAction = new CCLens3D();

            if (pAction.initWithPosition(pos, r, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        /* lens center position */
        protected CCPoint m_position;
        protected float m_fRadius;
        /** lens effect. Defaults to 0.7 - 0 means no effect, 1 is very strong effect */
        protected float m_fLensEffect;

        /* @since v0.99.5 */
        // CCPoint m_lastPosition;
        protected CCPoint m_positionInPixels;
        protected bool m_bDirty;
    }
}
