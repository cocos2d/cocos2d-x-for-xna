/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
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
    /// A CCSpriteFrame has:
    ///- texture: A CCTexture2D that will be used by the CCSprite
    ///- rectangle: A rectangle of the texture
    ///
    /// You can modify the frame of a CCSprite by doing:
    ///	CCSpriteFrame *frame = CCSpriteFrame::frameWithTexture(texture, rect, offset);
    /// sprite->setDisplayFrame(frame);
    /// </summary>
    public class CCSpriteFrame : CCObject
    {
        #region properties

        protected CCRect m_obRectInPixels;
        protected bool m_bRotated;
        protected CCRect m_obRect;
        protected CCPoint m_obOffsetInPixels;
        protected CCSize m_obOriginalSizeInPixels;
        protected CCTexture2D m_pobTexture;

        public CCRect RectInPixels
        {
            get { return m_obRectInPixels; }
            set
            {
                m_obRectInPixels = value;
                m_obRect = ccMacros.CC_RECT_PIXELS_TO_POINTS(m_obRectInPixels);
            }
        }

        public bool IsRotated
        {
            get { return m_bRotated; }
            set { m_bRotated = value; }
        }

        /// <summary>
        /// get or set rect of the frame
        /// </summary>
        public CCRect Rect
        {
            get { return m_obRect; }
            set
            {
                m_obRect = value;
                m_obRectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(m_obRect);
            }
        }

        /// <summary>
        /// get or set offset of the frame
        /// </summary>
        public CCPoint OffsetInPixels
        {
            get { return m_obOffsetInPixels; }
            set { m_obOffsetInPixels = value; }
        }

        /// <summary>
        /// get or set original size of the trimmed image
        /// </summary>
        public CCSize OriginalSizeInPixels
        {
            get { return m_obOriginalSizeInPixels; }
            set { m_obOriginalSizeInPixels = value; }
        }

        /// <summary>
        /// get or set texture of the frame
        /// </summary>
        public CCTexture2D Texture
        {
            get { return m_pobTexture; }
            set { m_pobTexture = value; }
        }

        #endregion

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCSpriteFrame pCopy = new CCSpriteFrame();
            pCopy.initWithTexture(m_pobTexture, m_obRectInPixels, m_bRotated, m_obOffsetInPixels, m_obOriginalSizeInPixels);
            return pCopy;
        }

        /// <summary>
        /// Create a CCSpriteFrame with a texture, rect in points.
        /// It is assumed that the frame was not trimmed.
        /// </summary>
        public static CCSpriteFrame frameWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCSpriteFrame pSpriteFrame = new CCSpriteFrame(); ;
            pSpriteFrame.initWithTexture(pobTexture, rect);

            return pSpriteFrame;
        }

        /// <summary>
        /// Create a CCSpriteFrame with a texture, rect, rotated, offset and originalSize in pixels.
        /// The originalSize is the size in points of the frame before being trimmed.
        /// </summary>
        public static CCSpriteFrame frameWithTexture(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            CCSpriteFrame pSpriteFrame = new CCSpriteFrame();
            pSpriteFrame.initWithTexture(pobTexture, rect, rotated, offset, originalSize);

            return pSpriteFrame;
        }

        /// <summary>
        /// Initializes a CCSpriteFrame with a texture, rect in points.
        /// It is assumed that the frame was not trimmed.
        /// </summary>
        public bool initWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCRect rectInPixels = ccMacros.CC_RECT_POINTS_TO_PIXELS(rect);
            return initWithTexture(pobTexture, rectInPixels, false, new CCPoint(0, 0), rectInPixels.size);
        }

        /// <summary>
        /// Initializes a CCSpriteFrame with a texture, rect, rotated, offset and originalSize in pixels.
        /// The originalSize is the size in points of the frame before being trimmed.
        /// </summary>
        public bool initWithTexture(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            m_pobTexture = pobTexture;

            m_obRectInPixels = rect;
            m_obRect = ccMacros.CC_RECT_PIXELS_TO_POINTS(rect);
            m_bRotated = rotated;
            m_obOffsetInPixels = offset;

            m_obOriginalSizeInPixels = originalSize;

            return true;
        }
    }
}
