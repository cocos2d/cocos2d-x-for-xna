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
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// CCOrbitCamera action
    /// Orbits the camera around the center of the screen using spherical coordinates
    /// </summary>
    public class CCOrbitCamera : CCActionCamera
    {
        public CCOrbitCamera()
        {
            m_fRadius = 0.0f;
            m_fDeltaRadius = 0.0f;
            m_fAngleZ = 0.0f;
            m_fDeltaAngleZ = 0.0f;
            m_fAngleX = 0.0f;
            m_fDeltaAngleX = 0.0f;
            m_fRadZ = 0.0f;
            m_fRadDeltaZ = 0.0f;
            m_fRadX = 0.0f;
            m_fRadDeltaX = 0.0f;
        }

        /// <summary>
        ///  creates a CCOrbitCamera action with radius, delta-radius,  z, deltaZ, x, deltaX
        /// </summary>
        public static CCOrbitCamera actionWithDuration(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX)
        {
            CCOrbitCamera pRet = new CCOrbitCamera();
            if (pRet.initWithDuration(t, radius, deltaRadius, angleZ, deltaAngleZ, angleX, deltaAngleX))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes a CCOrbitCamera action with radius, delta-radius,  z, deltaZ, x, deltaX 
        /// </summary>
        public bool initWithDuration(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX)
        {
            if (initWithDuration(t))
            {
                m_fRadius = radius;
                m_fDeltaRadius = deltaRadius;
                m_fAngleZ = angleZ;
                m_fDeltaAngleZ = deltaAngleZ;
                m_fAngleX = angleX;
                m_fDeltaAngleX = deltaAngleX;

                m_fRadDeltaZ = ccMacros.CC_DEGREES_TO_RADIANS(deltaAngleZ);
                m_fRadDeltaX = ccMacros.CC_DEGREES_TO_RADIANS(deltaAngleX);
                return true;
            }

            return false;
        }

        /// <summary>
        /// positions the camera according to spherical coordinates 
        /// </summary>
        public void sphericalRadius(out float newRadius, out float zenith, out float azimuth)
        {
            float ex, ey, ez, cx, cy, cz, x, y, z;
            float r; // radius
            float s;

            CCCamera pCamera = m_pTarget.Camera;
            pCamera.getEyeXYZ(out ex, out  ey, out ez);
            pCamera.getCenterXYZ(out cx, out  cy, out  cz);

            x = ex - cx;
            y = ey - cy;
            z = ez - cz;

            r = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2) + (float)Math.Pow(z, 2));
            s = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2));
            if (s == 0.0f)
                s = ccMacros.FLT_EPSILON;
            if (r == 0.0f)
                r = ccMacros.FLT_EPSILON;

            zenith = (float)Math.Acos(z / r);
            if (x < 0)
                azimuth = (float)Math.PI - (float)Math.Sin(y / s);
            else
                azimuth = (float)Math.Sin(y / s);

            newRadius = r / CCCamera.getZEye();
        }

        // super methods
        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCOrbitCamera pRet = null;
            if (pZone != null && pZone.m_pCopyObject != null) //in case of being called at sub class
                pRet = (CCOrbitCamera)(pZone.m_pCopyObject);
            else
            {
                pRet = new CCOrbitCamera();
                pZone = pNewZone = new CCZone(pRet);
            }

            copyWithZone(pZone);

            pRet.initWithDuration(m_fDuration, m_fRadius, m_fDeltaRadius, m_fAngleZ, m_fDeltaAngleZ, m_fAngleX, m_fDeltaAngleX);

            return pRet;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            startWithTargetUsedByCCOrbitCamera(pTarget);
            //CCActionInterval::startWithTarget(pTarget);

            float r, zenith, azimuth;
            this.sphericalRadius(out r, out  zenith, out azimuth);

            if (float.IsNaN(m_fRadius))
                m_fRadius = r;
            if (float.IsNaN(m_fAngleZ))
                m_fAngleZ = ccMacros.CC_RADIANS_TO_DEGREES(zenith);
            if (float.IsNaN(m_fAngleX))
                m_fAngleX = ccMacros.CC_RADIANS_TO_DEGREES(azimuth);

            m_fRadZ = ccMacros.CC_DEGREES_TO_RADIANS(m_fAngleZ);
            m_fRadX = ccMacros.CC_DEGREES_TO_RADIANS(m_fAngleX);
        }

        public override void update(float dt)
        {
            float r = (m_fRadius + m_fDeltaRadius * dt) * CCCamera.getZEye();
            float za = m_fRadZ + m_fRadDeltaZ * dt;
            float xa = m_fRadX + m_fRadDeltaX * dt;

            float i = (float)Math.Sin(za) * (float)Math.Cos(xa) * r + m_fCenterXOrig;
            float j = (float)Math.Sin(za) * (float)Math.Sin(xa) * r + m_fCenterYOrig;
            float k = (float)Math.Cos(za) * r + m_fCenterZOrig;

            m_pTarget.Camera.setEyeXYZ(i, j, k);
        }

        protected float m_fRadius;
        protected float m_fDeltaRadius;
        protected float m_fAngleZ;
        protected float m_fDeltaAngleZ;
        protected float m_fAngleX;
        protected float m_fDeltaAngleX;

        protected float m_fRadZ;
        protected float m_fRadDeltaZ;
        protected float m_fRadX;
        protected float m_fRadDeltaX;
    }
}
