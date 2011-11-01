/****************************************************************************
Copyright (c) 2010 cocos2d-x.org

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
using System.Collections.Generic;
using System.Linq;
using System.Text;

// #include "CCCommon.h"

namespace cocos2d
{
    public class CCImage
    {
        enum EImageFormat
        {
            kFmtJpg = 0,
            kFmtPng,
		    kFmtRawData,
        };

        enum ETextAlign
        {
            kAlignCenter        = 0x33, ///< Horizontal center and vertical center.
            kAlignTop           = 0x13, ///< Horizontal center and vertical top.
            kAlignTopRight      = 0x12, ///< Horizontal right and vertical top.
            kAlignRight         = 0x32, ///< Horizontal right and vertical center.
            kAlignBottomRight   = 0x22, ///< Horizontal right and vertical bottom.
            kAlignBottom        = 0x23, ///< Horizontal center and vertical bottom.
            kAlignBottomLeft    = 0x21, ///< Horizontal left and vertical bottom.
            kAlignLeft          = 0x31, ///< Horizontal left and vertical center.
            kAlignTopLeft       = 0x11, ///< Horizontal left and vertical top.
        };


        public CCImage()
        {
            throw new NotImplementedException();
        }

        ~CCImage()
        {
            throw new NotImplementedException();
        }

        ///** 
        //@brief  Load the image from the specified path. 
        //@param strPath   the absolute file path
        //@return  true if load correctly
        //*/
        //bool initWithImageFile(char strPath)
        //{
        //    EImageFormat imageType = EImageFormat.kFmtPng;
        //    throw new NotImplementedException();
        //}

        ///** 
        //@brief  Load the image from the specified path. 
        //@param strPath   the absolute file path
        //@param imageType the type of image, now only support two types.
        //@return  true if load correctly
        //*/
        //bool initWithImageFile(char strPath, EImageFormat imageType)
        //{
        //    throw new NotImplementedException();
        //}

        /**
        @brief  Load image from stream buffer.
        @warning kFmtRawData only support RGBA8888
        @param pBuffer  stream buffer that hold the image data
        @param nLength  the length of data(managed in byte)
        @return true if load correctly
        */
        bool initWithImageData(object pData, 
                               int nDataLen)
        {
            EImageFormat eFmt = EImageFormat.kFmtPng;
            int nWidth = 0;
            int nHeight = 0;
            int nBitsPerComponent = 8;
            throw new NotImplementedException();
        }

        /**
        @brief  Load image from stream buffer.
        @warning kFmtRawData only support RGBA8888
        @param pBuffer  stream buffer that hold the image data
        @param nLength  the length of data(managed in byte)
        @param nWidth, nHeight, nBitsPerComponent are used for kFmtRawData
        @return true if load correctly
        */
        bool initWithImageData(object pData, 
                               int nDataLen, 
                               EImageFormat eFmt,
                               int nWidth,
                               int nHeight,
                               int nBitsPerComponent)
        {
             throw new NotImplementedException();
        }

        /**
        @brief	Create image with specified string.
        @param  pText       the text which the image show, nil cause init fail
        */
        bool initWithString(string pText)
        {
            int nWidth = 0;
            int nHeight = 0;
            ETextAlign eAlignMask = ETextAlign.kAlignCenter;
            string pFontName = "";
            int nSize = 0;
            throw new NotImplementedException();
        }

        /**
        @brief	Create image with specified string.
        @param  pText       the text which the image show, nil cause init fail
        @param  nWidth      the image width, if 0, the width match the text's width
        @param  nHeight     the image height, if 0, the height match the text's height
        @param  eAlignMask  the test Alignment
        @param  pFontName   the name of the font which use to draw the text. If nil, use the default system font.
        @param  nSize       the font size, if 0, use the system default size.
        */
        bool initWithString(
                    string          pText, 
                    int             nWidth, 
                    int             nHeight,
                    ETextAlign      eAlignMask,
                    string          pFontName,
                    int             nSize)
        {
             throw new NotImplementedException();
        }

        char[] getData()               
        {   
            throw new NotImplementedException();
        }
    
        int getDataLen()
        { 
             throw new NotImplementedException();
        }

        bool hasAlpha()                     
        { 
            throw new NotImplementedException();
        }

        bool isPremultipliedAlpha()         
        { 
             throw new NotImplementedException();    
        }

        void release()
        {
             throw new NotImplementedException();
        }

        /**
        @brief	Save the CCImage data to specified file with specified format.
        @param	pszFilePath		the file's absolute path, including file subfix
        */
        bool saveToFile(string pszFilePath)
        {
            bool bIsToRGB = true;
            throw new NotImplementedException();
        }

        /**
        @brief	Save the CCImage data to specified file with specified format.
        @param	pszFilePath		the file's absolute path, including file subfix
        @param	bIsToRGB		if the image is saved as RGB format
        */
        bool saveToFile(string pszFilePath, bool bIsToRGB)
        {
             throw new NotImplementedException();
        }

        public short width  { get; set; }
        public short height { get; set; }
        public short bitsPerComponent{ get; set; }

        private bool m_bHasAlpha;
        private bool m_bPreMulti;
    }
}
