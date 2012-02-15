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

using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace cocos2d
{
    /// <summary>
    /// Singleton that handles the loading of textures
    /// Once the texture is loaded, the next time it will return
    /// a reference of the previously loaded texture reducing GPU & CPU memory
    /// </summary>
    public class CCTextureCache : CCObject
    {
        protected Dictionary<string, CCTexture2D> m_pTextures;
        object m_pDictLock;
        object m_pContextLock;

        #region Singleton

        private static CCTextureCache g_sharedTextureCache;

        private CCTextureCache()
        {
            Debug.Assert(g_sharedTextureCache == null, "Attempted to allocate a second instance of a singleton.");

            m_pTextures = new Dictionary<string, CCTexture2D>();

            m_pDictLock = new object();
            m_pContextLock = new object();

        }

        ~CCTextureCache()
        {
            Debug.WriteLine("cocos2d: deallocing CCTextureCache.");

            m_pTextures.Clear();
        }

        /// <summary>
        /// Retruns ths shared instance of the cache
        /// </summary>
        public static CCTextureCache sharedTextureCache()
        {
            if (g_sharedTextureCache == null)
            {
                g_sharedTextureCache = new CCTextureCache();
            }

            return g_sharedTextureCache;
        }

        #endregion

        /// <summary>
        /// purges the cache. It releases the retained instance.
        /// @since v0.99.0
        /// </summary>
        public static void purgeSharedTextureCache()
        {
            g_sharedTextureCache = null;
        }

        /// <summary>
        /// Returns a Texture2D object given an file image
        /// If the file image was not previously loaded, it will create a new CCTexture2D
        /// object and it will return it. It will use the filename as a key.
        /// Otherwise it will return a reference of a previosly loaded image.
        /// Supported image extensions: .png, .bmp, .tiff, .jpeg, .pvr, .gif
        /// </summary>
        public CCTexture2D addImage(string fileimage)
        {
            Debug.Assert(fileimage != null, "TextureCache: fileimage MUST not be NULL");

            CCTexture2D texture;
            lock (m_pDictLock)
            {
                //remove possible -HD suffix to prevent caching the same image twice (issue #1040)
                string pathKey = fileimage;
                //CCFileUtils.ccRemoveHDSuffixFromFile(pathKey);

                bool isTextureExist = m_pTextures.TryGetValue(pathKey, out texture);
                if (!isTextureExist)
                {
                    //int lastPos = fileimage.LastIndexOf("1");
                    //if (lastPos>0)
                    //{
                    //    fileimage = fileimage.Substring(0, lastPos);
                    //}
                    //if (fileimage=="Images/b")
                    //{
                    //    fileimage = fileimage + "1";
                    //}
                    //if (fileimage=="Images/r")
                    //{
                    //    fileimage = fileimage + "1";
                    //}
                    //if (fileimage=="Images/f")
                    //{
                    //    fileimage = fileimage + "1";
                    //}
                    Texture2D textureXna = CCApplication.sharedApplication().content.Load<Texture2D>(fileimage);
                    texture = new CCTexture2D();
                    bool isInited = texture.initWithTexture(textureXna);

                    if (isInited)
                    {
                        m_pTextures.Add(pathKey, texture);
                    }
                    else
                    {
                        Debug.Assert(false, "cocos2d: Couldn't add image:" + fileimage + " in CCTextureCache");
                        return null;
                    }
                }
            }
            return texture;
        }

        /** Returns a Texture2D object given an UIImage image
	    * If the image was not previously loaded, it will create a new CCTexture2D object and it will return it.
	    * Otherwise it will return a reference of a previously loaded image
	    * The "key" parameter will be used as the "key" for the cache.
	    * If "key" is nil, then a new texture will be created each time.
	    */
        //public CCTexture2D addUIImage(CCImage image, string key)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Returns an already created texture. Returns nil if the texture doesn't exist.
        /// @since v0.99.5
        /// </summary>
        public CCTexture2D textureForKey(string key)
        {
            //@todo
            //std::string strKey = CCFileUtils::fullPathFromRelativePath(key);
            CCTexture2D texture = null;

            try
            {
                m_pTextures.TryGetValue(key, out texture);
            }
            catch (ArgumentNullException)
            {
                Debug.WriteLine("Texture of key {0} is not exist.", key);
            }

            return texture;
        }

        /// <summary>
        /// Purges the dictionary of loaded textures.
        /// Call this method if you receive the "Memory Warning"
        /// In the short term: it will free some resources preventing your app from being killed
        /// In the medium term: it will allocate more resources
        /// In the long term: it will be the same
        /// </summary>
        public void removeAllTextures()
        {
            m_pTextures.Clear();
        }

        /// <summary>
        /// Removes unused textures
        /// Textures that have a retain count of 1 will be deleted
        /// It is convinient to call this method after when starting a new Scene
        /// @since v0.8
        /// </summary>
        public void removeUnusedTextures()
        {
            //GAC to handle
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a texture from the cache given a texture
        /// </summary>
        public void removeTexture(CCTexture2D texture)
        {
            if (texture == null)
            {
                return;
            }

            string key = null;
            foreach (KeyValuePair<string, CCTexture2D> kvp in m_pTextures)
            {
                if (kvp.Value == texture)
                {
                    key = kvp.Key;
                    break;
                }
            }
            if (key != null)
            {
                m_pTextures.Remove(key);
            }
        }

        /// <summary>
        /// Deletes a texture from the cache given a its key name
        /// @since v0.99.4
        /// </summary>
        public void removeTextureForKey(string textureKeyName)
        {
            if (textureKeyName == null)
            {
                return;
            }

            //string fullPath = CCFileUtils::fullPathFromRelativePath(textureKeyName);
            m_pTextures.Remove(textureKeyName);
        }

        /// <summary>
        /// Output to CCLOG the current contents of this CCTextureCache
        /// This will attempt to calculate the size of each texture, and the total texture memory in use
        /// @since v1.0
        /// </summary>
        public void dumpCachedTextureInfo()
        {
            // throw new NotImplementedException();
        }

#if CC_SUPPORT_PVRTC
        /** Returns a Texture2D object given an PVRTC RAW filename
	    * If the file image was not previously loaded, it will create a new CCTexture2D
	    *  object and it will return it. Otherwise it will return a reference of a previosly loaded image
	    *
	    * It can only load square images: width == height, and it must be a power of 2 (128,256,512...)
	    * bpp can only be 2 or 4. 2 means more compression but lower quality.
	    * hasAlpha: whether or not the image contains alpha channel
	    */
        public CCTexture2D addPVRTCImage(string fileimage, int bpp, bool hasAlpha, int width)
        {
            throw new NotImplementedException();
        }
#endif

        /// <summary>
        /// Returns a Texture2D object given an PVR filename
        /// If the file image was not previously loaded, it will create a new CCTexture2D
        /// object and it will return it. Otherwise it will return a reference of a previosly loaded image
        /// </summary>
        public CCTexture2D addPVRImage(string filename)
        {
            throw new NotImplementedException();
        }

        // It is designed for Android. I think it is not needed with win phone
        /** Reload all textures
        It's only useful when the value of CC_ENABLE_CACHE_TEXTTURE_DATA is 1
        */
        // static void reloadAllTextures();
    }
}
