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
    /// <summary>
    /// Base class for CCCamera actions
    /// </summary>
    public class CCActionCamera : CCActionInterval
    {
        public CCActionCamera()
        {
            this.m_fCenterXOrig = 0;
            this.m_fCenterYOrig = 0;
            this.m_fCenterZOrig = 0;

            this.m_fEyeXOrig = 0;
            this.m_fEyeYOrig = 0;
            this.m_fEyeZOrig = 0;

            this.m_fUpXOrig = 0;
            this.m_fUpYOrig = 0;
            this.m_fUpZOrig = 0;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);

            CCCamera camera = pTarget.Camera;

            camera.getCenterXYZ(out m_fCenterXOrig, out m_fCenterYOrig, out m_fCenterZOrig);
            camera.getEyeXYZ(out m_fEyeXOrig, out m_fEyeYOrig, out m_fEyeZOrig);
            camera.getUpXYZ(out m_fUpXOrig, out m_fUpYOrig, out m_fUpZOrig);
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCReverseTime.actionWithAction(this);
        }

        protected float m_fCenterXOrig;
        protected float m_fCenterYOrig;
        protected float m_fCenterZOrig;

        protected float m_fEyeXOrig;
        protected float m_fEyeYOrig;
        protected float m_fEyeZOrig;

        protected float m_fUpXOrig;
        protected float m_fUpYOrig;
        protected float m_fUpZOrig;
    }
}
