/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 Fulcrum Mobile Network, Inc
 
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
    /// <summary>
    /// tructure which can tell where mimap begins and how long is it
    /// </summary>
    public class CCPVRMipmap
    {
        string address;
        uint len;
    }

    /// <summary>
    /// CCTexturePVR
    /// Object that loads PVR images.
    /// Supported PVR formats
    /// - RGBA8888
    /// - BGRA8888
    /// - RGBA4444
    /// - RGBA5551
    /// - RGB565
    /// - A8
    /// - I8
    /// - AI88
    /// - PVRTC 4BPP
    /// - PVRTC 2BPP
    /// Limitations:
    /// Pre-generated mipmaps, such as PVR textures with mipmap levels embedded in file,
    /// are only supported if all individual sprites are of _square_ size. 
    /// To use mipmaps with non-square textures, instead call CCTexture2D#generateMipmap on the sheet texture itself
    /// (and to save space, save the PVR sprite sheet without mip maps included).
    /// </summary>      
    public class CCTexturePVR
    {
        const int CC_PVRMIPMAP_MAX = 16;

        public CCTexturePVR()
        { }

        #region initializes a CCTexturePVR with a path
        /// <summary>
        /// initializes a CCTexturePVR with a path
        /// </summary>
        #endregion
        public bool initWithContentsOfFile(string path)
        {
            throw new NotImplementedException();
        }

        #region creates and initializes a CCTexturePVR with a path
        /// <summary>
        /// creates and initializes a CCTexturePVR with a path
        /// </summary>
        #endregion
        public static CCTexturePVR pvrTextureWithContentsOfFile(string path)
        {
            throw new NotImplementedException();
        }

        //CC_PROPERTY_READONLY(GLuint, m_uName, Name)
        protected ulong m_uName;
        public ulong Name
        {
            get 
            {
                return m_uName;
            }
        }

        //CC_PROPERTY_READONLY(unsigned int, m_uWidth, Width)
        protected uint m_uWidth;
        public uint Width
        {
            get
            {
                return m_uWidth;
            }
        }

        //CC_PROPERTY_READONLY(unsigned int, m_uHeight, Height)
        protected uint m_uHeight;
        public uint Height
        {
            get
            {
                return m_uHeight;
            }
        }

        //CC_PROPERTY_READONLY(CCTexture2DPixelFormat, m_eFormat, Format)
        protected CCTexture2DPixelFormat m_eFormat;
        public CCTexture2DPixelFormat Format
        {
            get
            {
                return m_eFormat;
            }
        }

        //CC_PROPERTY_READONLY(bool, m_bHasAlpha, HasAlpha)
        protected bool m_bHasAlpha;
        public bool HasAlpha
        {
            get
            {
                return m_bHasAlpha;
            }
        }

        // cocos2d integration
        //CC_PROPERTY(bool, m_bRetainName, RetainName);
        protected bool m_bRetainName;
        public bool RetainName
        {
            get
            {
                return m_bRetainName;
            }
            set
            {
                m_bRetainName = value;
            }
        }

        #region Unpacks data (data of pvr texture file) and determine how many mipmaps it uses (m_uNumberOfMipmaps)
        /// <summary>
        /// Unpacks data (data of pvr texture file) and determine
        /// how many mipmaps it uses (m_uNumberOfMipmaps). Adresses
        /// of mimaps (m_asMipmaps). And basic data like size, format
        /// and alpha presence
        /// </summary>
        /// <param name="of"></param>
        /// <returns></returns>
        #endregion
        protected bool unpackPVRData(string data, uint len)
        {
            throw new NotImplementedException();
        }

        #region Binds all mipmaps to the GL state machine as separate textures
        /// <summary>
        /// Binds all mipmaps to the GL state machine as separate
        /// textures
        /// </summary>
        #endregion
        protected bool createGLTexture()
        {
            throw new NotImplementedException();
        }

        #region Index to the tableFormats array
        /// <summary>
        /// Index to the tableFormats array. Which tells us what exact
        /// format is file which initializes this object. 
        /// </summary>
        #endregion
        uint m_uTableFormatIndex;

        #region How many mipmaps do we have
        /// <summary>
        /// How many mipmaps do we have. It must be at least one
        /// when proper initialization finishes
        /// </summary>
        #endregion
        uint m_uNumberOfMipmaps;

        #region Makrs for mipmaps
        /// <summary>
        /// Makrs for mipmaps. Each entry contains position in file
        /// and lenght of data which represents one mipmap.
        /// </summary>
        #endregion
        List<CCPVRMipmap> m_asMipmaps;
    }
}
