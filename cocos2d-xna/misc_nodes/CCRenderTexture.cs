using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace cocos2d
{
    public class CCRenderTexture : CCNode
    {
        public enum tImageFormat
        {
            kCCImageFormatJPG = 0,
            kCCImageFormatPNG = 1,
            kCCImageFormatRawData = 2
        }

        /// <summary>
        /// The CCSprite being used.
        ///The sprite, by default, will use the following blending function: GL_ONE, GL_ONE_MINUS_SRC_ALPHA.
        ///The blending function can be changed in runtime by calling:
        ///- [[renderTexture sprite] setBlendFunc:(ccBlendFunc){GL_ONE, GL_ONE_MINUS_SRC_ALPHA}];
        /// </summary>
        protected CCSprite m_pSprite;

        public CCSprite Sprite
        {
            get { return m_pSprite; }
            set { m_pSprite = value; }
        }

        public CCRenderTexture()
        {
            removeAllChildrenWithCleanup(true);
            //ccglDeleteFramebuffers(1, m_uFBO);
            //CC_SAFE_DELETE(m_pUITextureImage);
        }

        /// <summary>
        /// creates a RenderTexture object with width and height in Points and a pixel format, only RGB and RGBA formats are valid
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="eFormat"></param>
        /// <returns></returns>
        public static CCRenderTexture renderTextureWithWidthAndHeight(int w, int h, CCTexture2DPixelFormat eFormat)
        {
            CCRenderTexture pRet = new CCRenderTexture();

            if (pRet != null && pRet.initWithWidthAndHeight(w, h, eFormat))
            {
                //pRet->autorelease();
                return pRet;
            }
            //CC_SAFE_DELETE(pRet);
            return null;
        }

        /// <summary>
        ///  creates a RenderTexture object with width and height in Points, pixel format is RGBA8888
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static CCRenderTexture renderTextureWithWidthAndHeight(int w, int h)
        {
            CCRenderTexture pRet = new CCRenderTexture();

            if (pRet != null && pRet.initWithWidthAndHeight(w, h, CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888))
            {
                //pRet->autorelease();
                return pRet;
            }
            //CC_SAFE_DELETE(pRet)
            return null;
        }

        /// <summary>
        ///  initializes a RenderTexture object with width and height in Points and a pixel format, only RGB and RGBA formats are valid 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="eFormat"></param>
        /// <returns></returns>
        public bool initWithWidthAndHeight(int w, int h, CCTexture2DPixelFormat eFormat)
        {
            // If the gles version is lower than GLES_VER_1_0, 
            // some extended gles functions can't be implemented, so return false directly.
            if (CCConfiguration.sharedConfiguration().getGlesVersion() <= CCGlesVersion.GLES_VER_1_0)
            {
                return false;
            }

            bool bRet = false;
            do
            {
                w = (int)CCDirector.sharedDirector().ContentScaleFactor;
                h = (int)CCDirector.sharedDirector().ContentScaleFactor;

                //glGetIntegerv(0x8CA6, m_nOldFBO);

                // textures must be power of two squared
                uint powW = (uint)ccUtils.ccNextPOT(w);
                uint powH = (uint)ccUtils.ccNextPOT(h);

                var data = (int)(powW * powH * 4);
                //CC_BREAK_IF(! data);

                //memset(data, 0, (int)(powW * powH * 4));
                //m_ePixelFormat = eFormat;

                m_pTexture = new CCTexture2D();
                //CC_BREAK_IF(! m_pTexture);

                m_pTexture.initWithData(data, (CCTexture2DPixelFormat)m_ePixelFormat, powW, powH, new CCSize((float)w, (float)h));
                //free( data );

                // generate FBO
                //ccglGenFramebuffers(1, &m_uFBO);
                //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_uFBO);

                // associate texture with FBO
                //ccglFramebufferTexture2D(CC_GL_FRAMEBUFFER, CC_GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, m_pTexture->getName(), 0);

                // check if it worked (probably worth doing :) )
                //GLuint status = ccglCheckFramebufferStatus(CC_GL_FRAMEBUFFER);
                //if (status != CC_GL_FRAMEBUFFER_COMPLETE)
                //{
                //    CCAssert(0, "Render Texture : Could not attach texture to framebuffer");
                //    CC_SAFE_DELETE(m_pTexture);
                //    break;
                //}

                m_pTexture.setAliasTexParameters();

                m_pSprite = CCSprite.spriteWithTexture(m_pTexture);

                //m_pTexture->release();
                m_pSprite.scaleY = -1;
                this.addChild(m_pSprite);

                ccBlendFunc tBlendFunc = new ccBlendFunc { src = 1, dst = 0x0303 };
                m_pSprite.BlendFunc = tBlendFunc;

                //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_nOldFBO);
                bRet = true;
            } while (true);
            return bRet;
        }

        /// <summary>
        /// starts grabbing
        /// </summary>
        public void begin()
        {
            // Save the current matrix
            //glPushMatrix();

            CCSize texSize = m_pTexture.ContentSizeInPixels;

            // Calculate the adjustment ratios based on the old and new projections
            CCSize size = CCDirector.sharedDirector().displaySizeInPixels;
            float widthRatio = size.width / texSize.width;
            float heightRatio = size.height / texSize.height;

            // Adjust the orthographic propjection and viewport
            //ccglOrtho((float)-1.0 / widthRatio,  (float)1.0 / widthRatio, (float)-1.0 / heightRatio, (float)1.0 / heightRatio, -1,1);
            //glViewport(0, 0, (GLsizei)texSize.width, (GLsizei)texSize.height);
            //     CCDirector::sharedDirector()->getOpenGLView()->setViewPortInPoints(0, 0, texSize.width, texSize.height);

            //glGetIntegerv(CC_GL_FRAMEBUFFER_BINDING, &m_nOldFBO);
            //ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_uFBO);//Will direct drawing to the frame buffer created above

            // Issue #1145
            // There is no need to enable the default GL states here
            // but since CCRenderTexture is mostly used outside the "render" loop
            // these states needs to be enabled.
            // Since this bug was discovered in API-freeze (very close of 1.0 release)
            // This bug won't be fixed to prevent incompatibilities with code.
            // 
            // If you understand the above mentioned message, then you can comment the following line
            // and enable the gl states manually, in case you need them.

            //CC_ENABLE_DEFAULT_GL_STATES();	
        }

        /// <summary>
        /// starts rendering to the texture while clearing the texture first.
        ///    This is more efficient then calling -clear first and then -begin
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public void beginWithClear(float r, float g, float b, float a)
        {
            this.begin();

            // save clear color
            //GLfloat	clearColor[4];
            //glGetFloatv(GL_COLOR_CLEAR_VALUE,clearColor); 

            //glClearColor(r, g, b, a);
            //glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            //// restore clear color
            //glClearColor(clearColor[0], clearColor[1], clearColor[2], clearColor[3]); 
        }


        /// <summary>
        /// end is key word of lua, use other name to export to lua.
        /// </summary>
        public void endToLua()
        {
            end();
        }

        /** ends grabbing*/
        // para bIsTOCacheTexture       the parameter is only used for android to cache the texture
        public void end(bool bIsTOCacheTexture)
        {
            //    ccglBindFramebuffer(CC_GL_FRAMEBUFFER, m_nOldFBO);
            //    // Restore the original matrix and viewport
            //    glPopMatrix();
            //    CCSize size = CCDirector::sharedDirector()->getDisplaySizeInPixels();
            //    //	glViewport(0, 0, (GLsizei)size.width, (GLsizei)size.height);
            //    CCDirector::sharedDirector()->getOpenGLView()->setViewPortInPoints(0, 0, size.width, size.height);

            //#if CC_ENABLE_CACHE_TEXTTURE_DATA
            //    if (bIsTOCacheTexture)
            //    {
            //        CC_SAFE_DELETE(m_pUITextureImage);

            //        // to get the rendered texture data
            //        const CCSize& s = m_pTexture->getContentSizeInPixels();
            //        int tx = (int)s.width;
            //        int ty = (int)s.height;
            //        m_pUITextureImage = new CCImage;
            //        if (true == getUIImageFromBuffer(m_pUITextureImage, 0, 0, tx, ty))
            //        {
            //            VolatileTexture::addDataTexture(m_pTexture, m_pUITextureImage->getData(), kTexture2DPixelFormat_RGBA8888, s);
            //        } 
            //        else
            //        {
            //            CCLOG("Cache rendertexture failed!");
            //        }
            //    }
            //#endif
        }

        public void end()
        {

        }

        /// <summary>
        /// clears the texture with a color
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public void clear(float r, float g, float b, float a)
        {
            this.beginWithClear(r, g, b, a);
            this.end();
        }

        /** saves the texture into a file */
        // para szFilePath      the absolute path to save
        // para x,y         the lower left corner coordinates of the buffer to save
        // pare nWidth,nHeight    the size of the buffer to save
        //                        when nWidth = 0 and nHeight = 0, the image size to save equals to buffer texture size
        public bool saveBuffer(string szFilePath, int x, int y, int nWidth, int nHeight)
        {
            //bool bRet = false;

            //CCImage pImage = new CCImage();
            //if (pImage != null && getUIImageFromBuffer(pImage, x, y, nWidth, nHeight))
            //{
            //    bRet = pImage->saveToFile(szFilePath);
            //}

            //CC_SAFE_DELETE(pImage);
            //return bRet;
            throw new NotFiniteNumberException();
        }

        /** saves the texture into a file. put format at the first argument, or ti will be overloaded with
         * saveBuffer(const char *szFilePath, int x = 0, int y = 0, int nWidth = 0, int nHeight = 0) */
        // para name        the file name to save
        // para format      the image format to save, here it supports kCCImageFormatPNG and kCCImageFormatJPG */
        // para x,y         the lower left corner coordinates of the buffer to save
        // pare nWidth,nHeight    the size of the buffer to save
        //                        when nWidth = 0 and nHeight = 0, the image size to save equals to buffer texture size
        public bool saveBuffer(int format, string name, int x, int y, int nWidth, int nHeight)
        {
            //bool bRet = false;
            //Debug.Assert(format == (int)tImageFormat.kCCImageFormatJPG || format == (int)tImageFormat.kCCImageFormatPNG,
            //         "the image can only be saved as JPG or PNG format");

            //CCImage pImage = new CCImage();
            //if (pImage != null && getUIImageFromBuffer(pImage, x, y, nWidth, nHeight))
            //{
            //    string fullpath = CCFileUtils.getWriteablePath() + name;

            //    bRet = pImage->saveToFile(fullpath);
            //}

            ////CC_SAFE_DELETE(pImage);

            //return bRet;
            throw new NotFiniteNumberException();
        }

        /* get buffer as UIImage, can only save a render buffer which has a RGBA8888 pixel format */
        public CCData getUIImageAsDataFromBuffer(int format)
        {
            CCData pData = null;
            //@ todo CCRenderTexture::getUIImageAsDataFromBuffer

            // #include "Availability.h"
            // #include "UIKit.h"

            //     GLubyte * pBuffer   = NULL;
            //     GLubyte * pPixels   = NULL;
            //     do 
            //     {
            //         CC_BREAK_IF(! m_pTexture);
            // 
            //         CCAssert(m_ePixelFormat == kCCTexture2DPixelFormat_RGBA8888, "only RGBA8888 can be saved as image");
            // 
            //         const CCSize& s = m_pTexture->getContentSizeInPixels();
            //         int tx = s.width;
            //         int ty = s.height;
            // 
            //         int bitsPerComponent = 8;
            //         int bitsPerPixel = 32;
            // 
            //         int bytesPerRow = (bitsPerPixel / 8) * tx;
            //         int myDataLength = bytesPerRow * ty;
            // 
            //         CC_BREAK_IF(! (pBuffer = new GLubyte[tx * ty * 4]));
            //         CC_BREAK_IF(! (pPixels = new GLubyte[tx * ty * 4]));
            // 
            //         this->begin();
            //         glReadPixels(0,0,tx,ty,GL_RGBA,GL_UNSIGNED_BYTE, pBuffer);
            //         this->end();
            // 
            //         int x,y;
            // 
            //         for(y = 0; y <ty; y++) {
            //             for(x = 0; x <tx * 4; x++) {
            //                 pPixels[((ty - 1 - y) * tx * 4 + x)] = pBuffer[(y * 4 * tx + x)];
            //             }
            //         }
            // 
            //         if (format == kCCImageFormatRawData)
            //         {
            //             pData = CCData::dataWithBytesNoCopy(pPixels, myDataLength);
            //             break;
            //         }

            //@ todo impliment save to jpg or png
            /*
            CGImageCreate(size_t width, size_t height,
            size_t bitsPerComponent, size_t bitsPerPixel, size_t bytesPerRow,
            CGColorSpaceRef space, CGBitmapInfo bitmapInfo, CGDataProviderRef provider,
            const CGFloat decode[], bool shouldInterpolate,
            CGColorRenderingIntent intent)
            */
            // make data provider with data.
            //         CGBitmapInfo bitmapInfo = kCGImageAlphaPremultipliedLast | kCGBitmapByteOrderDefault;
            //         CGDataProviderRef provider		= CGDataProviderCreateWithData(NULL, pixels, myDataLength, NULL);
            //         CGColorSpaceRef colorSpaceRef	= CGColorSpaceCreateDeviceRGB();
            //         CGImageRef iref					= CGImageCreate(tx, ty,
            //             bitsPerComponent, bitsPerPixel, bytesPerRow,
            //             colorSpaceRef, bitmapInfo, provider,
            //             NULL, false,
            //             kCGRenderingIntentDefault);
            // 
            //         UIImage* image					= [[UIImage alloc] initWithCGImage:iref];
            // 
            //         CGImageRelease(iref);	
            //         CGColorSpaceRelease(colorSpaceRef);
            //         CGDataProviderRelease(provider);
            // 
            // 
            // 
            //         if (format == kCCImageFormatPNG)
            //             data = UIImagePNGRepresentation(image);
            //         else
            //             data = UIImageJPEGRepresentation(image, 1.0f);
            // 
            //         [image release];
            //     } while (0);
            //     
            //     CC_SAFE_DELETE_ARRAY(pBuffer);
            //     CC_SAFE_DELETE_ARRAY(pPixels);
            return pData;
        }

        /** save the buffer data to a CCImage */
        // para pImage      the CCImage to save
        // para x,y         the lower left corner coordinates of the buffer to save
        // pare nWidth,nHeight    the size of the buffer to save
        //                        when nWidth = 0 and nHeight = 0, the image size to save equals to buffer texture size
        public bool getUIImageFromBuffer(CCTexture2D pImage, int x, int y, int nWidth, int nHeight)
        {
            if (null == pImage || null == m_pTexture)
            {
                return false;
            }

            CCSize s = m_pTexture.ContentSizeInPixels;
            int tx = (int)s.width;
            int ty = (int)s.height;

            if (x < 0 || x >= tx || y < 0 || y >= ty)
            {
                return false;
            }

            if (nWidth < 0
                || nHeight < 0
                || (0 == nWidth && 0 != nHeight)
                || (0 == nHeight && 0 != nWidth))
            {
                return false;
            }

            // to get the image size to save
            //		if the saving image domain exeeds the buffer texture domain,
            //		it should be cut
            int nSavedBufferWidth = nWidth;
            int nSavedBufferHeight = nHeight;
            if (0 == nWidth)
            {
                nSavedBufferWidth = tx;
            }
            if (0 == nHeight)
            {
                nSavedBufferHeight = ty;
            }
            nSavedBufferWidth = x + nSavedBufferWidth > tx ? (tx - x) : nSavedBufferWidth;
            nSavedBufferHeight = y + nSavedBufferHeight > ty ? (ty - y) : nSavedBufferHeight;

            byte[] pBuffer = null;
            byte[] pTempData = null;
            bool bRet = false;

            do
            {
                Debug.Assert(m_ePixelFormat == (uint)CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888, "only RGBA8888 can be saved as image");

                if ((pBuffer = new byte[nSavedBufferWidth * nSavedBufferHeight * 4]) == null)
                    break;

                // On some machines, like Samsung i9000, Motorola Defy,
                // the dimension need to be a power of 2
                int nReadBufferWidth = 0;
                int nReadBufferHeight = 0;
                int nMaxTextureSize = 0;
                //glGetIntegerv(0x0D33, nMaxTextureSize);

                nReadBufferWidth = (int)ccUtils.ccNextPOT(tx);
                nReadBufferHeight = (int)ccUtils.ccNextPOT(ty);

                if (0 == nReadBufferWidth || 0 == nReadBufferHeight)
                    break;
                if (nReadBufferWidth > nMaxTextureSize || nReadBufferHeight > nMaxTextureSize)
                    break;

                if ((pTempData = new byte[nReadBufferWidth * nReadBufferHeight * 4]) == null)
                    break;

                this.begin();
                //glPixelStorei(0x0D05, 1);
                //glReadPixels(0, 0, nReadBufferWidth, nReadBufferHeight, 0x1908, 0x1401, pTempData);
                this.end(false);

                // to get the actual texture data 
                // #640 the image read from rendertexture is upseted
                //for (int i = 0; i < nSavedBufferHeight; ++i)
                //{
                //    memcpy(pBuffer[i * nSavedBufferWidth * 4], 
                //        pTempData[(y + nSavedBufferHeight - i - 1) * nReadBufferWidth * 4 + x * 4], 
                //        nSavedBufferWidth * 4);
                //}

                //bRet = pImage.initWithImageData(pBuffer, nSavedBufferWidth * nSavedBufferHeight * 4, CCImage.kFmtRawData, nSavedBufferWidth, nSavedBufferHeight, 8);
            } while (true);

            //CC_SAFE_DELETE_ARRAY(pBuffer);
            //CC_SAFE_DELETE_ARRAY(pTempData);

            return bRet;
        }

        protected uint m_uFBO;
        protected int m_nOldFBO;
        protected CCTexture2D m_pTexture;
        protected CCTexture2D m_pUITextureImage;
        protected uint m_ePixelFormat;
    }
}
