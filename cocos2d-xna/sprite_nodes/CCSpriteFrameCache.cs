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
using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// Singleton that handles the loading of the sprite frames.
    /// It saves in a cache the sprite frames.
    /// @since v0.9
    /// </summary>
    public class CCSpriteFrameCache
    {
        protected Dictionary<string, CCSpriteFrame> m_pSpriteFrames;
        protected Dictionary<string, string> m_pSpriteFramesAliases;
        public static CCSpriteFrameCache pSharedSpriteFrameCache = null;
        private string valueForKey(string key, Dictionary<string, Object> dict)
        {
            if (dict != null)
            {
                if (dict.Keys.Contains(key))
                {
                    string pString = (string)dict[key];
                    return pString != null ? pString : "";
                }
            }
            return "";
        }

        public bool init()
        {
            m_pSpriteFrames = new Dictionary<string, CCSpriteFrame>();
            m_pSpriteFramesAliases = new Dictionary<string, string>();

            return true;
        }

        /// <summary>
        /// Adds multiple Sprite Frames with a dictionary. The texture will be associated with the created sprite frames.
        /// </summary>
        public void addSpriteFramesWithDictionary(Dictionary<string, Object> pobDictionary, CCTexture2D pobTexture)
        {
            /*
Supported Zwoptex Formats:

ZWTCoordinatesFormatOptionXMLLegacy = 0, // Flash Version
ZWTCoordinatesFormatOptionXML1_0 = 1, // Desktop Version 0.0 - 0.4b
ZWTCoordinatesFormatOptionXML1_1 = 2, // Desktop Version 1.0.0 - 1.0.1
ZWTCoordinatesFormatOptionXML1_2 = 3, // Desktop Version 1.0.2+
*/
            Dictionary<string, Object> metadataDict = null;
            if (pobDictionary.Keys.Contains("metadata"))
            {
                metadataDict = (Dictionary<string, Object>)pobDictionary["metadata"];
            }

            Dictionary<string, Object> framesDict = null;
            if (pobDictionary.Keys.Contains("frames"))
            {
                framesDict = (Dictionary<string, Object>)pobDictionary["frames"];
            }

            int format = 0;

            // get the format
            if (metadataDict != null)
            {
                format = int.Parse(metadataDict["format"].ToString());
            }

            // check the format
            Debug.Assert(format >= 0 && format <= 3);

            foreach (var key in framesDict.Keys)
            {
                Dictionary<string, Object> frameDict = framesDict[key] as Dictionary<string, Object>;
                CCSpriteFrame spriteFrame = new CCSpriteFrame();

                if (format == 0)
                {
                    float x = float.Parse(frameDict["x"].ToString());
                    float y = float.Parse(frameDict["y"].ToString());
                    float w = float.Parse(frameDict["width"].ToString());
                    float h = float.Parse(frameDict["height"].ToString());
                    float ox = float.Parse(frameDict["offsetX"].ToString());
                    float oy = float.Parse(frameDict["offsetY"].ToString());
                    int ow = int.Parse(frameDict["originalWidth"].ToString());
                    int oh = int.Parse(frameDict["originalHeight"].ToString());
                    // check ow/oh
                    if (ow == 0 || oh == 0)
                    {
                        Debug.WriteLine("cocos2d: WARNING: originalWidth/Height not found on the CCSpriteFrame. AnchorPoint won't work as expected. Regenrate the .plist");
                    }
                    // abs ow/oh
                    ow = Math.Abs(ow);
                    oh = Math.Abs(oh);
                    // create frame
                    spriteFrame = new CCSpriteFrame();
                    spriteFrame.initWithTexture(pobTexture,
                                                new CCRect(x, y, w, h),
                                                false,
                                                new CCPoint(ox, oy),
                                                new CCSize((float)ow, (float)oh)
                                                );
                }
                else if (format == 1 || format == 2)
                {
                    CCRect frame = CCNS.CCRectFromString(frameDict["frame"].ToString());
                    bool rotated = false;

                    // rotation
                    if (format == 2)
                    {
                        if (frameDict.Keys.Contains("rotated"))
                        {
                            rotated = int.Parse(valueForKey("rotated", frameDict)) == 0 ? false : true;
                        }
                    }

                    CCPoint offset = CCNS.CCPointFromString(valueForKey("offset", frameDict));
                    CCSize sourceSize = CCNS.CCSizeFromString(valueForKey("sourceSize", frameDict));

                    // create frame
                    spriteFrame = new CCSpriteFrame();
                    spriteFrame.initWithTexture(pobTexture,
                        frame,
                        rotated,
                        offset,
                        sourceSize
                        );
                }
                else
                    if (format == 3)
                    {
                        // get values
                        CCSize spriteSize = CCNS.CCSizeFromString(valueForKey("spriteSize", frameDict));
                        CCPoint spriteOffset = CCNS.CCPointFromString(valueForKey("spriteOffset", frameDict));
                        CCSize spriteSourceSize = CCNS.CCSizeFromString(valueForKey("spriteSourceSize", frameDict));
                        CCRect textureRect = CCNS.CCRectFromString(valueForKey("textureRect", frameDict));
                        bool textureRotated = false;
                        if (frameDict.Keys.Contains("textureRotated"))
                        {
                            textureRotated = int.Parse(valueForKey("textureRotated", frameDict)) == 0 ? false : true;
                        }

                        // get aliases
                        var list = frameDict["aliases"];
                        List<object> aliases = (frameDict["aliases"] as List<object>);
                        string frameKey = key;
                        foreach (var item2 in aliases)
                        {
                            string oneAlias = item2.ToString();
                            if (m_pSpriteFramesAliases.Keys.Contains(oneAlias))
                            {
                                if (m_pSpriteFramesAliases[oneAlias] != null)
                                {
                                    Debug.WriteLine("cocos2d: WARNING: an alias with name {0} already exists", oneAlias);
                                }
                            }
                            if (!m_pSpriteFramesAliases.Keys.Contains(frameKey))
                            {
                                m_pSpriteFramesAliases.Add(frameKey, oneAlias);
                            }
                        }

                        // create frame
                        spriteFrame = new CCSpriteFrame();
                        spriteFrame.initWithTexture(pobTexture,
                                        new CCRect(textureRect.origin.x, textureRect.origin.y, spriteSize.width, spriteSize.height),
                                        textureRotated,
                                        spriteOffset,
                                        spriteSourceSize);
                    }

                // add sprite frame
                if (!m_pSpriteFrames.Keys.Contains(key))
                {
                    m_pSpriteFrames.Add(key, spriteFrame);
                }
            }
        }

        /// <summary>
        /// Adds multiple Sprite Frames from a plist file.
        /// A texture will be loaded automatically. The texture name will composed by replacing the .plist suffix with .png
        /// If you want to use another texture, you should use the addSpriteFramesWithFile:texture method.
        /// </summary>
        public void addSpriteFramesWithFile(string pszPlist)
        {
            string pszPath = CCFileUtils.fullPathFromRelativePath(pszPlist);
            Dictionary<string, Object> dict = CCFileUtils.dictionaryWithContentsOfFile(pszPath);

            string texturePath = "";
            Dictionary<string, Object> metadataDict = dict.Keys.Contains("metadata") ?
                (Dictionary<string, Object>)dict["metadata"] : null;

            if (metadataDict != null)
            {
                // try to read  texture file name from meta data
                if (dict.Keys.Contains("textureFileName"))
                {
                    texturePath = (valueForKey("textureFileName", metadataDict));
                }
            }

            if (!string.IsNullOrEmpty(texturePath))
            {
                // build texture path relative to plist file
                texturePath = CCFileUtils.fullPathFromRelativeFile(texturePath, pszPath);
            }
            else
            {
                // build texture path by replacing file path,case xna resource has no extension,so we move the image to images folder
                texturePath = pszPath;
                int index = pszPath.IndexOf("/");
                if (index < 0)
                {
                    index = pszPath.IndexOf(@"\");
                }
                if (index > 0)
                {
                    texturePath = pszPath.Substring(0, index) + "/images" + pszPath.Substring(index);
                }

                Debug.WriteLine("cocos2d: CCSpriteFrameCache: Trying to use file {0} as texture", texturePath);
            }

            CCTexture2D pTexture = CCTextureCache.sharedTextureCache().addImage(texturePath);

            if (pTexture != null)
            {
                addSpriteFramesWithDictionary(dict, pTexture);
            }
            else
            {
                Debug.WriteLine("cocos2d: CCSpriteFrameCache: Couldn't load texture");
            }
        }

        /// <summary>
        /// Adds multiple Sprite Frames from a plist file. The texture will be associated with the created sprite frames.
        /// @since v0.99.5
        /// </summary>
        public void addSpriteFramesWithFile(string plist, string textureFileName)
        {
            Debug.Assert(textureFileName != null);
            CCTexture2D texture = CCTextureCache.sharedTextureCache().addImage(textureFileName);

            if (texture != null)
            {
                addSpriteFramesWithFile(plist, texture);
            }
            else
            {
                Debug.WriteLine("cocos2d: CCSpriteFrameCache: couldn't load texture file. File not found {0}", textureFileName);
            }
        }

        /// <summary>
        /// Adds multiple Sprite Frames from a plist file. The texture will be associated with the created sprite frames.
        /// </summary>
        public void addSpriteFramesWithFile(string pszPlist, CCTexture2D pobTexture)
        {
            string pszPath = CCFileUtils.fullPathFromRelativePath(pszPlist);
            Dictionary<string, Object> dict = CCFileUtils.dictionaryWithContentsOfFile(pszPath);

            addSpriteFramesWithDictionary(dict, pobTexture);
        }

        /// <summary>
        /// Adds an sprite frame with a given name.
        /// If the name already exists, then the contents of the old name will be replaced with the new one.
        /// </summary>
        public void addSpriteFrame(CCSpriteFrame pobFrame, string pszFrameName)
        {
            m_pSpriteFrames.Add(pszFrameName, pobFrame);
        }

        /// <summary>
        /// Purges the dictionary of loaded sprite frames.
        /// Call this method if you receive the "Memory Warning".
        /// In the short term: it will free some resources preventing your app from being killed.
        /// In the medium term: it will allocate more resources.
        /// In the long term: it will be the same.
        /// </summary>
        public void removeSpriteFrames()
        {
            this.m_pSpriteFrames.Clear();
            this.m_pSpriteFramesAliases.Clear();
        }

        /// <summary>
        /// Removes unused sprite frames.
        /// Sprite Frames that have a retain count of 1 will be deleted.
        /// It is convenient to call this method after when starting a new Scene.
        /// </summary>
        public void removeUnusedSpriteFrames()
        {
            //            m_pSpriteFrames->begin();
            //std::string key = "";
            //CCSpriteFrame *spriteFrame = NULL;
            //while( (spriteFrame = m_pSpriteFrames->next(&key)) )
            //{
            //    if( spriteFrame->retainCount() == 1 ) 
            //    {
            //        CCLOG("cocos2d: CCSpriteFrameCache: removing unused frame: %s", key.c_str());
            //        m_pSpriteFrames->removeObjectForKey(key);
            //    }
            //}
            //m_pSpriteFrames->end();
        }

        /// <summary>
        /// Deletes an sprite frame from the sprite frame cache.
        /// </summary>
        public void removeSpriteFrameByName(string pszName)
        {
            // explicit nil handling
            if (string.IsNullOrEmpty(pszName))
            {
                return;
            }

            // Is this an alias ?
            string key = m_pSpriteFramesAliases[pszName];

            if (!string.IsNullOrEmpty(key))
            {
                m_pSpriteFrames.Remove(key);
                m_pSpriteFramesAliases.Remove(key);
            }
            else
            {
                m_pSpriteFrames.Remove(pszName);
            }
        }

        /// <summary>
        /// Removes multiple Sprite Frames from a plist file.
        /// Sprite Frames stored in this file will be removed.
        /// It is convinient to call this method when a specific texture needs to be removed.
        /// @since v0.99.5
        /// </summary>
        public void removeSpriteFramesFromFile(string plist)
        {
            string path = CCFileUtils.fullPathFromRelativePath(plist);
            Dictionary<string, object> dict = CCFileUtils.dictionaryWithContentsOfFile(path);

            removeSpriteFramesFromDictionary(dict);
        }

        /// <summary>
        /// Removes multiple Sprite Frames from CCDictionary.
        /// @since v0.99.5
        /// </summary>
        /// <param name="dictionary"></param>
        public void removeSpriteFramesFromDictionary(Dictionary<string, object> dictionary)
        {
            Dictionary<string, Object> framesDict = (Dictionary<string, Object>)dictionary["frames"];
            List<string> keysToRemove = new List<string>();

            foreach (var key in framesDict.Keys)
            {
                if (m_pSpriteFrames.ContainsKey(key))
                {
                    keysToRemove.Remove(key);
                }
            }

            foreach (var key in keysToRemove)
            {
                m_pSpriteFrames.Remove(key);
            }
        }

        /// <summary>
        /// Removes all Sprite Frames associated with the specified textures.
        /// It is convinient to call this method when a specific texture needs to be removed.
        /// @since v0.995.
        /// </summary>
        public void removeSpriteFramesFromTexture(CCTexture2D texture)
        {
            List<string> keysToRemove = new List<string>();

            foreach (var key in m_pSpriteFrames.Keys)
            {
                CCSpriteFrame frame = m_pSpriteFrames[key];
                if (frame != null && (frame.Texture.Name == texture.Name))
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (var key in keysToRemove)
            {
                m_pSpriteFrames.Remove(key);
            }
        }

        /// <summary>
        /// Returns an Sprite Frame that was previously added.
        /// If the name is not found it will return nil.
        /// You should retain the returned copy if you are going to use it.
        /// </summary>
        public CCSpriteFrame spriteFrameByName(string pszName)
        {
            CCSpriteFrame
                frame = m_pSpriteFrames[pszName];
            if (frame == null)
            {
                // try alias dictionary
                string key = (string)m_pSpriteFramesAliases[pszName];
                if (key != null)
                {
                    frame = m_pSpriteFrames[key];
                    if (frame == null)
                    {
                        Debug.WriteLine("cocos2d: CCSpriteFrameCahce: Frame '{0}' not found", pszName);
                    }
                }
            }
            return frame;
        }

        /// <summary>
        /// Returns the shared instance of the Sprite Frame cache
        /// </summary>
        public static CCSpriteFrameCache sharedSpriteFrameCache()
        {
            if (pSharedSpriteFrameCache == null)
            {
                pSharedSpriteFrameCache = new CCSpriteFrameCache();
                pSharedSpriteFrameCache.init();
            }

            return pSharedSpriteFrameCache;
        }

        /// <summary>
        /// Purges the cache. It releases all the Sprite Frames and the retained instance. 
        /// </summary>
        public static void purgeSharedSpriteFrameCache()
        {
            pSharedSpriteFrameCache = null;
        }
    }
}
