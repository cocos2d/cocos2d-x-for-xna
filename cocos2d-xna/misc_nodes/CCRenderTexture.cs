using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCRenderTexture : CCNode
    {
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
            throw new NotImplementedException();
        }

        /// <summary>
        ///  creates a RenderTexture object with width and height in Points, pixel format is RGBA8888
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static CCRenderTexture renderTextureWithWidthAndHeight(int w, int h)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// starts grabbing
        /// </summary>
        public void begin()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /** saves the texture into a file */
        // para szFilePath      the absolute path to save
        // para x,y         the lower left corner coordinates of the buffer to save
        // pare nWidth,nHeight    the size of the buffer to save
        //                        when nWidth = 0 and nHeight = 0, the image size to save equals to buffer texture size
        public bool saveBuffer(string szFilePath, int x, int y, int nWidth, int nHeight)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        protected uint m_uFBO;
        protected int m_nOldFBO;
        protected CCTexture2D m_pTexture;
        protected CCTexture2D m_pUITextureImage;
        protected uint m_ePixelFormat;
    }
}
