/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
    public enum CCTMXOrientatio : int
    {
        /// <summary>
        /// Orthogonal orientation
        /// </summary>
        CCTMXOrientationOrtho = 0,

        /// <summary>
        /// Hexagonal orientation
        /// </summary>
        CCTMXOrientationHex = 1,

        /// <summary>
        ///  Isometric orientation
        /// </summary>
        CCTMXOrientationIso = 2,
    };

    public enum TMXLayerAttrib
    {
        TMXLayerAttribNone = 1 << 0,
        TMXLayerAttribBase64 = 1 << 1,
        TMXLayerAttribGzip = 1 << 2,
        TMXLayerAttribZlib = 1 << 3,
    };

    public enum TMXProperty
    {
        TMXPropertyNone,
        TMXPropertyMap,
        TMXPropertyLayer,
        TMXPropertyObjectGroup,
        TMXPropertyObject,
        TMXPropertyTile
    };

}
