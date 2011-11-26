/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
    public class CCCamera : CCObject
    {

        protected float m_fEyeX;
        protected float m_fEyeY;
        protected float m_fEyeZ;

        protected float m_fCenterX;
        protected float m_fCenterY;
        protected float m_fCenterZ;

        protected float m_fUpX;
        protected float m_fUpY;
        protected float m_fUpZ;

        protected bool m_bDirty;
        /// <summary>
        ///  sets \ get the dirty value
        /// </summary>
        public bool Dirty
        {
            get { return m_bDirty; }
            set { m_bDirty = value; }
        }

        public CCCamera()
        {
            throw new NotImplementedException();
        }

        public void init()
        {
            throw new NotImplementedException();
        }

        public string description()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets the camera in the default position
        /// </summary>
        public void restore()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///  Sets the camera using gluLookAt using its eye, center and up_vector
        /// </summary>
        public void locate()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// sets the eye values in points
        /// </summary>
        /// <param name="fEyeX"></param>
        /// <param name="fEyeY"></param>
        /// <param name="fEyeZ"></param>
        public void setEyeXYZ(float fEyeX, float fEyeY, float fEyeZ)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// sets the center values in points
        /// </summary>
        /// <param name="fCenterX"></param>
        /// <param name="fCenterY"></param>
        /// <param name="fCenterZ"></param>
        public void setCenterXYZ(float fCenterX, float fCenterY, float fCenterZ)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  sets the up values
        /// </summary>
        /// <param name="fUpX"></param>
        /// <param name="fUpY"></param>
        /// <param name="fUpZ"></param>
        public void setUpXYZ(float fUpX, float fUpY, float fUpZ)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  get the eye vector values in points
        /// </summary>
        /// <param name="pEyeX"></param>
        /// <param name="pEyeY"></param>
        /// <param name="pEyeZ"></param>
        public void getEyeXYZ(float pEyeX, float pEyeY, float pEyeZ)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  get the center vector values int points 
        /// </summary>
        /// <param name="pCenterX"></param>
        /// <param name="pCenterY"></param>
        /// <param name="pCenterZ"></param>
        public void getCenterXYZ(float pCenterX, float pCenterY, float pCenterZ)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///  get the up vector values
        /// </summary>
        /// <param name="pUpX"></param>
        /// <param name="pUpY"></param>
        /// <param name="pUpZ"></param>
        public void getUpXYZ(float pUpX, float pUpY, float pUpZ)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// returns the Z eye
        /// </summary>
        /// <returns></returns>
        public static float getZEye()
        {
            throw new NotImplementedException();
        }

        //private:


        //    DISALLOW_COPY_AND_ASSIGN(CCCamera);
    }
}
