using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace cocos2d
{
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

        /*Adds multiple Sprite Frames with a dictionary. The texture will be associated with the created sprite frames.
         */
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

            int i = 0;
            string key = "";
            Dictionary<string, Object> frameDict = null;

            foreach (var item in framesDict.Keys)
            {
                frameDict = framesDict[item] as Dictionary<string, Object>;
                CCSpriteFrame spriteFrame = new CCSpriteFrame();
                key = item;
                //if (spriteFrame != null)
                //{
                //    continue;
                //}
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

                #region
                //while (framesDict != null)
                //{
                //    CCSpriteFrame spriteFrame = m_pSpriteFrames[key];
                //    if (spriteFrame != null)
                //    {
                //        continue;
                //    }

                //    if (format == 0)
                //    {
                //        float x = float.Parse(frameDict["x"].ToString());
                //        float y = float.Parse(frameDict["y"].ToString());
                //        float w = float.Parse(frameDict["width"].ToString());
                //        float h = float.Parse(frameDict["height"].ToString());
                //        float ox = float.Parse(frameDict["offsetX"].ToString());
                //        float oy = float.Parse(frameDict["offsetY"].ToString());
                //        int ow = int.Parse(frameDict["originalWidth"].ToString());
                //        int oh = int.Parse(frameDict["originalHeight"].ToString());
                //        // check ow/oh
                //        if (ow == 0 || oh == 0)
                //        {
                //            Debug.WriteLine("cocos2d: WARNING: originalWidth/Height not found on the CCSpriteFrame. AnchorPoint won't work as expected. Regenrate the .plist");
                //        }
                //        // abs ow/oh
                //        ow = Math.Abs(ow);
                //        oh = Math.Abs(oh);
                //        // create frame
                //        spriteFrame = new CCSpriteFrame();
                //        spriteFrame.initWithTexture(pobTexture,
                //                                    new CCRect(x, y, w, h),
                //                                    false,
                //                                    new CCPoint(ox, oy),
                //                                    new CCSize((float)ow, (float)oh)
                //                                    );
                //    }
                //    else if (format == 1 || format == 2)
                //    {
                //        CCRect frame = CCNS.CCRectFromString(frameDict["frame"].ToString());
                //        bool rotated = false;

                //        // rotation
                //        if (format == 2)
                //        {
                //            rotated = int.Parse(valueForKey("rotated", frameDict)) == 0 ? false : true;
                //        }

                //        CCPoint offset = CCNS.CCPointFromString(valueForKey("offset", frameDict));
                //        CCSize sourceSize = CCNS.CCSizeFromString(valueForKey("sourceSize", frameDict));

                //        // create frame
                //        spriteFrame = new CCSpriteFrame();
                //        spriteFrame.initWithTexture(pobTexture,
                //            frame,
                //            rotated,
                //            offset,
                //            sourceSize
                //            );
                //    }
                //    else
                //        if (format == 3)
                //        {
                //            // get values
                //            CCSize spriteSize = CCNS.CCSizeFromString(valueForKey("spriteSize", frameDict));
                //            CCPoint spriteOffset = CCNS.CCPointFromString(valueForKey("spriteOffset", frameDict));
                //            CCSize spriteSourceSize = CCNS.CCSizeFromString(valueForKey("spriteSourceSize", frameDict));
                //            CCRect textureRect = CCNS.CCRectFromString(valueForKey("textureRect", frameDict));
                //            bool textureRotated = int.Parse(valueForKey("textureRotated", frameDict)) == 0 ? false : true;

                //            // get aliases
                //            List<string> aliases = (frameDict["aliases"] as List<string>);
                //            string frameKey = key;
                //            foreach (string item in aliases)
                //            {
                //                string oneAlias = item;
                //                if (m_pSpriteFramesAliases[oneAlias] != null)
                //                {
                //                    Debug.WriteLine("cocos2d: WARNING: an alias with name {0} already exists", oneAlias);
                //                }

                //                m_pSpriteFramesAliases.Add(frameKey, oneAlias);
                //            }

                //            // create frame
                //            spriteFrame = new CCSpriteFrame();
                //            spriteFrame.initWithTexture(pobTexture,
                //                            new CCRect(textureRect.origin.x, textureRect.origin.y, spriteSize.width, spriteSize.height),
                //                            textureRotated,
                //                            spriteOffset,
                //                            spriteSourceSize);
                //        } 
                #endregion

                // add sprite frame
                if (!m_pSpriteFrames.Keys.Contains(key))
                {
                    m_pSpriteFrames.Add(key, spriteFrame);
                }
            }
        }

        /** Adds multiple Sprite Frames from a plist file.
         * A texture will be loaded automatically. The texture name will composed by replacing the .plist suffix with .png
         * If you want to use another texture, you should use the addSpriteFramesWithFile:texture method.
         */
        public void addSpriteFramesWithFile(string pszPlist)
        {
            string pszPath = CCFileUtils.fullPathFromRelativePath(pszPlist);
            Dictionary<string, Object> dict = CCFileUtils.dictionaryWithContentsOfFile(pszPath);

            string texturePath = "";
            Dictionary<string, Object> metadataDict;
            if (dict.Keys.Contains("metadata"))
            {
                metadataDict = (Dictionary<string, Object>)dict["metadata"];
            }
            else
            {
                metadataDict = null;
            }
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
                // build texture path by replacing file extension
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
                // remove .xxx
                //int startPos = texturePath.LastIndexOf(".");
                //texturePath = texturePath.Remove(startPos);

                //// append .png
                //texturePath = texturePath + (".png");

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

        /** Adds multiple Sprite Frames from a plist file. The texture will be associated with the created sprite frames.
        @since v0.99.5
        */
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

        /** Adds multiple Sprite Frames from a plist file. The texture will be associated with the created sprite frames. */
        public void addSpriteFramesWithFile(string pszPlist, CCTexture2D pobTexture)
        {
            string pszPath = CCFileUtils.fullPathFromRelativePath(pszPlist);
            Dictionary<string, Object> dict = CCFileUtils.dictionaryWithContentsOfFile(pszPath);

            addSpriteFramesWithDictionary(dict, pobTexture);
        }

        /** Adds an sprite frame with a given name.
         If the name already exists, then the contents of the old name will be replaced with the new one.
         */
        public void addSpriteFrame(CCSpriteFrame pobFrame, string pszFrameName)
        {
            m_pSpriteFrames.Add(pszFrameName, pobFrame);
        }

        /** Purges the dictionary of loaded sprite frames.
         * Call this method if you receive the "Memory Warning".
         * In the short term: it will free some resources preventing your app from being killed.
         * In the medium term: it will allocate more resources.
         * In the long term: it will be the same.
         */
        public void removeSpriteFrames()
        {

        }

        /** Removes unused sprite frames.
         * Sprite Frames that have a retain count of 1 will be deleted.
         * It is convenient to call this method after when starting a new Scene.
         */
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

        /** Deletes an sprite frame from the sprite frame cache. */
        public void removeSpriteFrameByName(string pszName)
        {

        }

        /** Removes multiple Sprite Frames from a plist file.
        * Sprite Frames stored in this file will be removed.
        * It is convinient to call this method when a specific texture needs to be removed.
        * @since v0.99.5
        */
        public void removeSpriteFramesFromFile(string plist)
        {

        }

        /** Removes multiple Sprite Frames from CCDictionary.
        * @since v0.99.5
        */
        public void removeSpriteFramesFromDictionary(Dictionary<string, CCSpriteFrame> dictionary)
        {
            //        Dictionary<string, Object> framesDict = (Dictionary<string, Object>)dictionary["frames"]);
            //vector<string> keysToRemove;

            //framesDict->begin();
            //std::string key = "";
            //CCDictionary<std::string, CCObject*> *frameDict = NULL;
            //while( (frameDict = (CCDictionary<std::string, CCObject*>*)framesDict->next(&key)) )
            //{
            //    if (m_pSpriteFrames->objectForKey(key))
            //    {
            //        keysToRemove.push_back(key);
            //    }
            //}
            //framesDict->end();

            //vector<string>::iterator iter;
            //for (iter = keysToRemove.begin(); iter != keysToRemove.end(); ++iter)
            //{
            //    m_pSpriteFrames->removeObjectForKey(*iter);
            //}
        }

        /** Removes all Sprite Frames associated with the specified textures.
        * It is convinient to call this method when a specific texture needs to be removed.
        * @since v0.995.
        */
        public void removeSpriteFramesFromTexture(CCTexture2D texture)
        {
            //        vector<string> keysToRemove;

            //m_pSpriteFrames->begin();
            //std::string key = "";
            //CCDictionary<std::string, CCObject*> *frameDict = NULL;
            //while( (frameDict = (CCDictionary<std::string, CCObject*>*)m_pSpriteFrames->next(&key)) )
            //{
            //    CCSpriteFrame *frame = m_pSpriteFrames->objectForKey(key);
            //    if (frame && (frame->getTexture() == texture))
            //    {
            //        keysToRemove.push_back(key);
            //    }
            // }
            //m_pSpriteFrames->end();

            //vector<string>::iterator iter;
            //for (iter = keysToRemove.begin(); iter != keysToRemove.end(); ++iter)
            //{
            //    m_pSpriteFrames->removeObjectForKey(*iter);
            //}
        }

        /** Returns an Sprite Frame that was previously added.
         If the name is not found it will return nil.
         You should retain the returned copy if you are going to use it.
         */
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

        /** Returns the shared instance of the Sprite Frame cache */
        public static CCSpriteFrameCache sharedSpriteFrameCache()
        {
            if (pSharedSpriteFrameCache == null)
            {
                pSharedSpriteFrameCache = new CCSpriteFrameCache();
                pSharedSpriteFrameCache.init();
            }
            return pSharedSpriteFrameCache;
        }

        /** Purges the cache. It releases all the Sprite Frames and the retained instance. */
        public static void purgeSharedSpriteFrameCache()
        {
            if (pSharedSpriteFrameCache != null)
            {
                pSharedSpriteFrameCache = null;
            }
        }
    }
}
