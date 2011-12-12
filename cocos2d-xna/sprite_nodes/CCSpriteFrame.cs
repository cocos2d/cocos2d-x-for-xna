/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
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
    public class CCSpriteFrame : CCObject
    {

        public CCRect getRectInPixels()
        {
            return m_obRectInPixels;
        }

        public void setRectInPixels(CCRect rectInPixels)
        {
            m_obRectInPixels = rectInPixels;
            m_obRect = new CCRect(rectInPixels.origin.x * CCDirector.sharedDirector().ContentScaleFactor,
                   rectInPixels.origin.y * CCDirector.sharedDirector().ContentScaleFactor, rectInPixels.size.width * CCDirector.sharedDirector().ContentScaleFactor, rectInPixels.size.height * CCDirector.sharedDirector().ContentScaleFactor);
        }

        public bool isRotated()
        {
            return m_bRotated;
        }

        public void setRotated(bool bRotated)
        {
            m_bRotated = bRotated;
        }

        /** get rect of the frame */
        public CCRect getRect()
        {
            return m_obRect;
        }

        /** set rect of the frame */
        public void setRect(CCRect rect)
        {
            m_obRect = rect;
            m_obRectInPixels = new CCRect(m_obRect.origin.x * CCDirector.sharedDirector().ContentScaleFactor,
                   m_obRect.origin.y * CCDirector.sharedDirector().ContentScaleFactor, m_obRect.size.width * CCDirector.sharedDirector().ContentScaleFactor, m_obRect.size.height * CCDirector.sharedDirector().ContentScaleFactor);
        }

        /** get offset of the frame */
        public CCPoint getOffsetInPixels()
        {
            return m_obOffsetInPixels;
        }

        /** set offset of the frame */
        public void setOffsetInPixels(CCPoint offsetInPixels)
        {
            m_obOffsetInPixels = offsetInPixels;
        }

        /** get original size of the trimmed image */
        public CCSize getOriginalSizeInPixels()
        {
            return m_obOriginalSizeInPixels;
        }

        /** set original size of the trimmed image */
        public void setOriginalSizeInPixels(CCSize sizeInPixels)
        {
            m_obOriginalSizeInPixels = sizeInPixels;
        }

        /** get texture of the frame */
        public CCTexture2D getTexture()
        {
            return m_pobTexture;
        }

        /** set texture of the frame, the texture is retained */
        public void setTexture(CCTexture2D pobTexture)
        {
            //  CC_SAFE_RETAIN(pobTexture);
            // CC_SAFE_RELEASE(m_pobTexture);
            m_pobTexture = pobTexture;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            // CC_UNUSED_PARAM(pZone);
            CCSpriteFrame pCopy = new CCSpriteFrame();

            pCopy.initWithTexture(m_pobTexture, m_obRectInPixels, m_bRotated, m_obOffsetInPixels, m_obOriginalSizeInPixels);

            return pCopy;
        }

        /** Create a CCSpriteFrame with a texture, rect in points.
         It is assumed that the frame was not trimmed.
         */
        public static CCSpriteFrame frameWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCSpriteFrame pSpriteFrame = new CCSpriteFrame(); ;
            pSpriteFrame.initWithTexture(pobTexture, rect);
            // pSpriteFrame->autorelease();

            return pSpriteFrame;
        }

        /** Create a CCSpriteFrame with a texture, rect, rotated, offset and originalSize in pixels.
         The originalSize is the size in points of the frame before being trimmed.
         */
        public static CCSpriteFrame frameWithTexture(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            CCSpriteFrame pSpriteFrame = new CCSpriteFrame();
            pSpriteFrame.initWithTexture(pobTexture, rect, rotated, offset, originalSize);
            // pSpriteFrame->autorelease();

            return pSpriteFrame;
        }

        /** Initializes a CCSpriteFrame with a texture, rect in points.
         It is assumed that the frame was not trimmed.
         */
        public bool initWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCRect rectInPixels = new CCRect(rect.origin.x * CCDirector.sharedDirector().ContentScaleFactor,
                   rect.origin.y * CCDirector.sharedDirector().ContentScaleFactor, rect.size.width * CCDirector.sharedDirector().ContentScaleFactor, rect.size.height * CCDirector.sharedDirector().ContentScaleFactor);
            return initWithTexture(pobTexture, rectInPixels, false, new CCPoint(0, 0), rectInPixels.size);
        }

        /** Initializes a CCSpriteFrame with a texture, rect, rotated, offset and originalSize in pixels.
        The originalSize is the size in points of the frame before being trimmed.
        */
        public bool initWithTexture(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            m_pobTexture = pobTexture;

            if (pobTexture != null)
            {
                // pobTexture->retain();
            }

            m_obRectInPixels = rect;
            m_obRect = new CCRect(rect.origin.x / CCDirector.sharedDirector().ContentScaleFactor,
                rect.origin.y / CCDirector.sharedDirector().ContentScaleFactor, rect.size.width / CCDirector.sharedDirector().ContentScaleFactor, rect.size.height / CCDirector.sharedDirector().ContentScaleFactor);
            m_bRotated = rotated;
            m_obOffsetInPixels = offset;
            m_obOriginalSizeInPixels = originalSize;
            
            return true;
        }

        public CCSpriteFrame()
        {
            // CCLOGINFO("cocos2d: deallocing %p", this);
            // CC_SAFE_RELEASE(m_pobTexture);
        }

        protected CCRect m_obRectInPixels;
        protected bool m_bRotated;
        protected CCRect m_obRect;
        protected CCPoint m_obOffsetInPixels;
        protected CCSize m_obOriginalSizeInPixels;
        protected CCTexture2D m_pobTexture;
    }
}
