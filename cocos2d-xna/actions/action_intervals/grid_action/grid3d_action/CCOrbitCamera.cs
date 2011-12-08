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
    public class CCOrbitCamera : CCActionCamera
    {
        public CCOrbitCamera()
        {

        }

        // creates a CCOrbitCamera action with radius, delta-radius,  z, deltaZ, x, deltaX 
        public static CCOrbitCamera actionWithDuration(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX)
        {
            CCOrbitCamera pRet = new CCOrbitCamera();
            if (pRet.initWithDuration(t, radius, deltaRadius, angleZ, deltaAngleZ, angleX, deltaAngleX))
            {
                return pRet;
            }
            return null;
        }

        // initializes a CCOrbitCamera action with radius, delta-radius,  z, deltaZ, x, deltaX 
        public bool initWithDuration(float t, float radius, float deltaRadius, float angleZ, float deltaAngleZ, float angleX, float deltaAngleX)
        {
            //if (initWithDuration(t))
            //{
            //    m_fRadius = radius;
            //    m_fDeltaRadius = deltaRadius;
            //    m_fAngleZ = angleZ;
            //    m_fDeltaAngleZ = deltaAngleZ;
            //    m_fAngleX = angleX;
            //    m_fDeltaAngleX = deltaAngleX;

            //    m_fRadDeltaZ = (CGFloat)CC_DEGREES_TO_RADIANS(deltaAngleZ);
            //    m_fRadDeltaX = (CGFloat)CC_DEGREES_TO_RADIANS(deltaAngleX);
            //    return true;
            //}
            //return false;
            throw new NotImplementedException();
        }

        // positions the camera according to spherical coordinates 
        public void sphericalRadius(float newRadius, float zenith, float azimuth)
        {
            //float ex, ey, ez, cx, cy, cz, x, y, z;
            //float r; // radius
            //float s;

            //CCCamera pCamera = m_pTarget.Camera;
            // pCamera.getEyeXYZ(ex, ey, ez);
            // pCamera.getCenterXYZ(cx, cy, cz);

            // x = ex - cx;
            // y = ey - cy;
            // z = ez - cz;

            //r = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2) + (float)Math.Pow(z, 2));
            //s = (float)Math.Sqrt((float)Math.Pow(x, 2) + (float)Math.Pow(y, 2));
            ////if (s == 0.0f)
            ////    s = FLT_EPSILON;
            ////if (r == 0.0f)
            ////    r = FLT_EPSILON;

            //zenith = (float)Math.Acos(z / r);
            //if (x < 0)
            //    //azimuth = (CGFloat)M_PI - asinf(y / s);
            //    azimuth = (float)Math.PI - (float)Math.Sin(y / s);
            //else
            //    //azimuth = asinf(y/s);
            //    azimuth = (float)Math.Sin(y / s);

            //newRadius = r / CCCamera.getZEye();
            throw new NotImplementedException();
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

            //CC_SAFE_DELETE(pNewZone);
            return pRet;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            startWithTarget(pTarget);
            float r, zenith, azimuth;
            //this.sphericalRadius(r, zenith, azimuth);
            //if (isnan(m_fRadius))
            //    m_fRadius = r;
            //if (isnan(m_fAngleZ))
            //    m_fAngleZ = (CGFloat)CC_RADIANS_TO_DEGREES(zenith);
            //if (isnan(m_fAngleX))
            //    m_fAngleX = (CGFloat)CC_RADIANS_TO_DEGREES(azimuth);

            //m_fRadZ = (CGFloat)CC_DEGREES_TO_RADIANS(m_fAngleZ);
            //m_fRadX = (CGFloat)CC_DEGREES_TO_RADIANS(m_fAngleX);
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
