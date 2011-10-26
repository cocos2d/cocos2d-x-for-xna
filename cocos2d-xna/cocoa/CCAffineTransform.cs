/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org

http://www.cocos2d-x.org

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
namespace cocos2d
{
    public class CCAffineTransform
    {
        private float a, b, c, d;
        private float tx, ty;

        private CCAffineTransform() {}

        public static CCAffineTransform CCAffineTransformMake(float a, float b, float c, float d, float tx, float ty)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCPoint CCPointApplyAffineTransform(CCPoint point, CCAffineTransform t)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCSize CCSizeApplyAffineTransform(CCSize size, CCAffineTransform t)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCRect CCRectApplyAffineTransform(CCRect rect, CCAffineTransform anAffineTransform)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCAffineTransform CCAffineTransformTranslate(CCAffineTransform t, float tx, float ty)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCAffineTransform CCAffineTransformRotate(CCAffineTransform t, float anAngle)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCAffineTransform CCAffineTransformScale(CCAffineTransform t, float sx, float sy)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCAffineTransform CCAffineTransformConcat(CCAffineTransform t1, CCAffineTransform t2)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static bool CCAffineTransformEqualToTransform(CCAffineTransform t1, CCAffineTransform t2)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCAffineTransform CCAffineTransformInvert(CCAffineTransform t)
        {
            ///@todo
            throw new NotImplementedException();
        }
    }
}
