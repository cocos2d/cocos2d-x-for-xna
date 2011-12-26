/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
using System.IO;
using cocos2d.tileMap_parallax_nodes;


namespace cocos2d
{
    /// <summary>
    /// CCTMXMapInfo contains the information about the map like:
    ///- Map orientation (hexagonal, isometric or orthogonal)
    ///- Tile size
    ///- Map size
    ///
    ///	And it also contains:
    ///- Layers (an array of TMXLayerInfo objects)
    ///- Tilesets (an array of TMXTilesetInfo objects)
    ///- ObjectGroups (an array of TMXObjectGroupInfo objects)
    ///
    ///This information is obtained from the TMX file.
    /// </summary>
    public class CCTMXMapInfo : CCObject, ICCSAXDelegator
    {
        public static byte[] ToByte(string str)
        {
            byte[] bytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                int btvalue = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)btvalue;
            }
            return bytes;
        }

        #region properties

        protected int m_nOrientation;
        /// <summary>
        ///  map orientation
        /// </summary>
        public int Orientation
        {
            get { return m_nOrientation; }
            set { m_nOrientation = value; }
        }

        protected CCSize m_tMapSize;
        /// <summary>
        /// map width & height
        /// </summary>
        public CCSize MapSize
        {
            get { return m_tMapSize; }
            set { m_tMapSize = value; }
        }

        protected CCSize m_tTileSize;
        /// <summary>
        /// tiles width & height
        /// </summary>
        public CCSize TileSize
        {
            get { return m_tTileSize; }
            set { m_tTileSize = value; }
        }

        protected List<CCTMXLayerInfo> m_pLayers;
        /// <summary>
        /// Layers
        /// </summary>
        public virtual List<CCTMXLayerInfo> Layers
        {
            get { return m_pLayers; }
            set { m_pLayers = value; }
        }

        protected List<CCTMXTilesetInfo> m_pTilesets;
        /// <summary>
        /// tilesets
        /// </summary>
        public virtual List<CCTMXTilesetInfo> Tilesets
        {
            get { return m_pTilesets; }
            set { m_pTilesets = value; }
        }

        protected List<CCTMXObjectGroup> m_pObjectGroups;
        /// <summary>
        /// ObjectGroups
        /// </summary>
        public virtual List<CCTMXObjectGroup> ObjectGroups
        {
            get { return m_pObjectGroups; }
            set { m_pObjectGroups = value; }
        }

        protected int m_nParentElement;
        /// <summary>
        /// parent element
        /// </summary>
        public int ParentElement
        {
            get { return m_nParentElement; }
            set { m_nParentElement = value; }
        }

        protected int m_uParentGID;
        /// <summary>
        /// parent GID
        /// </summary>
        public int ParentGID
        {
            get { return m_uParentGID; }
            set { m_uParentGID = value; }
        }

        protected int m_nLayerAttribs;
        /// <summary>
        /// layer attribs
        /// </summary>
        public int LayerAttribs
        {
            get { return m_nLayerAttribs; }
            set { m_nLayerAttribs = value; }
        }

        protected bool m_bStoringCharacters;
        /// <summary>
        /// is stroing characters?
        /// </summary>
        public bool StoringCharacters
        {
            get { return m_bStoringCharacters; }
            set { m_bStoringCharacters = value; }
        }

        protected Dictionary<string, string> m_pProperties;
        /// <summary>
        /// properties
        /// </summary>
        public Dictionary<string, string> Properties
        {
            get { return m_pProperties; }
            set { m_pProperties = value; }
        }

        protected string m_sTMXFileName;
        /// <summary>
        /// ! tmx filename
        /// </summary>
        public string TMXFileName
        {
            get { return m_sTMXFileName; }
            set { m_sTMXFileName = value; }
        }

        protected string m_sCurrentString;
        /// <summary>
        /// ! current string
        /// </summary>
        public string CurrentString
        {
            get { return m_sCurrentString; }
            set { m_sCurrentString = value; }
        }

        protected Dictionary<int, Dictionary<string, string>> m_pTileProperties;
        /// <summary>
        /// ! tile properties
        /// </summary>
        public Dictionary<int, Dictionary<string, string>> TileProperties
        {
            get { return m_pTileProperties; }
            set { m_pTileProperties = value; }
        }

        #endregion

        public CCTMXMapInfo()
        {
        }

        /// <summary>
        /// creates a TMX Format with a tmx file
        /// </summary>
        public static CCTMXMapInfo formatWithTMXFile(string tmxFile)
        {
            CCTMXMapInfo pRet = new CCTMXMapInfo();
            if (pRet.initWithTMXFile(tmxFile))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes a TMX format witha  tmx file
        /// </summary>
        public bool initWithTMXFile(string tmxFile)
        {
            m_pTilesets = new List<CCTMXTilesetInfo>();
            m_pLayers = new List<CCTMXLayerInfo>();
            m_sTMXFileName = CCFileUtils.fullPathFromRelativePath(tmxFile);
            m_pObjectGroups = new List<CCTMXObjectGroup>();
            m_pProperties = new Dictionary<string, string>();
            m_pTileProperties = new Dictionary<int, Dictionary<string, string>>();

            // tmp vars
            m_sCurrentString = "";
            m_bStoringCharacters = false;
            m_nLayerAttribs = (int)TMXLayerAttrib.TMXLayerAttribNone;
            m_nParentElement = (int)TMXProperty.TMXPropertyNone;

            return parseXMLFile(m_sTMXFileName);
        }

        /// <summary>
        /// initalises parsing of an XML file, either a tmx (Map) file or tsx (Tileset) file
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool parseXMLFile(string xmlFilename)
        {
            //g:\cocos2d\cocos2d-1.0.1-x-0.9.2\cocos2d-1.0.1-x-0.9.2\cocos2dx\platform\ccsaxparser.cpp
            CCSAXParser parser = new CCSAXParser();

            if (false == parser.init("UTF-8"))
            {
                return false;
            }

            parser.setDelegator(this);

            return parser.parse(xmlFilename); ;
        }
        public void startElement(object ctx, string name, string[] atts)
        {
            CCTMXMapInfo pTMXMapInfo = this;
            string elementName = name;
            Dictionary<string, string> attributeDict = new Dictionary<string, string>();
            if (atts != null && atts[0] != null)
            {
                for (int i = 0; i < atts.Length; i += 2)
                {
                    attributeDict.Add(atts[i], atts[i + 1]);
                }
            }
            if (elementName == "map")
            {
                string version = attributeDict["version"];
                if (version != "1.0")
                {
                    Debug.WriteLine("cocos2d: TMXFormat: Unsupported TMX version:{0}", version);
                }

                string orientationStr = attributeDict["orientation"];
                //if( orientationStr == "orthogonal")
                //    pTMXMapInfo->setOrientation(CCTMXOrientationOrtho);
                //else if ( orientationStr  == "isometric")
                //    pTMXMapInfo->setOrientation(CCTMXOrientationIso);
                //else if( orientationStr == "hexagonal")
                //    pTMXMapInfo->setOrientation(CCTMXOrientationHex);
                //else
                //    CCLOG("cocos2d: TMXFomat: Unsupported orientation: %d", pTMXMapInfo->getOrientation());

                CCSize s = new CCSize();
                s.width = float.Parse(attributeDict["width"]);
                s.height = float.Parse(attributeDict["height"]);
                pTMXMapInfo.MapSize = s;

                s.width = float.Parse(attributeDict["tilewidth"]);
                s.height = float.Parse(attributeDict["tileheight"]);
                pTMXMapInfo.TileSize = s;

                // The parent element is now "map"
                pTMXMapInfo.ParentElement = 1;
            }

            else if (elementName == "tileset")
            {
                // If this is an external tileset then start parsing that
                if (attributeDict.Keys.Contains("source"))
                {
                    string externalTilesetFilename = attributeDict["source"];
                    if (externalTilesetFilename != "")
                    {
                        externalTilesetFilename = CCFileUtils.fullPathFromRelativeFile(externalTilesetFilename, pTMXMapInfo.TMXFileName);
                        pTMXMapInfo.parseXMLFile(externalTilesetFilename);
                    }
                }
                else
                {
                    CCTMXTilesetInfo tileset = new CCTMXTilesetInfo();
                    tileset.m_sName = attributeDict["name"];
                    tileset.m_uFirstGid = int.Parse(attributeDict["firstgid"]);
                    if (attributeDict.Keys.Contains("spacing"))
                    {
                        tileset.m_uSpacing = int.Parse(attributeDict["spacing"]);
                    }
                    if (attributeDict.Keys.Contains("margin"))
                    {
                        tileset.m_uMargin = int.Parse(attributeDict["margin"]);
                    }
                    CCSize s = new CCSize();
                    if (attributeDict.Keys.Contains("tilewidth"))
                    {
                        s.width = float.Parse(attributeDict["tilewidth"]);
                    }
                    if (attributeDict.Keys.Contains("tileheight"))
                    {
                        s.height = float.Parse(attributeDict["tileheight"]);
                    }
                    tileset.m_tTileSize = s;

                    pTMXMapInfo.Tilesets.Add(tileset);
                }
            }
            else if (elementName == "tile")
            {
                CCTMXTilesetInfo info = pTMXMapInfo.Tilesets.LastOrDefault();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                pTMXMapInfo.ParentGID = info.m_uFirstGid + int.Parse(attributeDict["id"]);
                pTMXMapInfo.TileProperties.Add((int)pTMXMapInfo.ParentGID, dict);
                //CC_SAFE_RELEASE(dict);

                pTMXMapInfo.ParentElement = 5;
            }
            else if (elementName == "layer")
            {
                CCTMXLayerInfo layer = new CCTMXLayerInfo();
                layer.m_sName = attributeDict["name"];

                CCSize s = new CCSize();
                s.width = float.Parse(attributeDict["width"]);
                s.height = float.Parse(attributeDict["height"]);
                layer.m_tLayerSize = s;

                foreach (var item in attributeDict.Keys)
                {
                    if (item == "visible")
                    {
                        string visible = attributeDict["visible"];
                        layer.m_bVisible = !(visible == "0");
                    }
                    if (item == "opacity")
                    {
                        string opacity = attributeDict["opacity"];
                        if (opacity != "")
                        {
                            layer.m_cOpacity = (byte)(255 * float.Parse(opacity));
                        }
                        else
                        {
                            layer.m_cOpacity = 255;
                        }
                    }
                    float x = 0;
                    if (item == "x")
                    {
                        x = float.Parse(attributeDict["x"]);

                    }
                    float y = 0;
                    if (item == "y")
                    {
                        y = float.Parse(attributeDict["y"]);
                        layer.m_tOffset = new CCPoint(x, y);
                    }
                }


                pTMXMapInfo.Layers.Add(layer);

                // The parent element is now "layer"
                pTMXMapInfo.ParentElement = 2;

            }
            else if (elementName == "objectgroup")
            {
                CCTMXObjectGroup objectGroup = new CCTMXObjectGroup();
                objectGroup.GroupName = attributeDict["name"];
                CCPoint positionOffset = new CCPoint();
                if (attributeDict.Keys.Contains("x"))
                    positionOffset.x = float.Parse(attributeDict["x"]) * pTMXMapInfo.TileSize.width;
                if (attributeDict.Keys.Contains("y"))
                    positionOffset.y = float.Parse(attributeDict["y"]) * pTMXMapInfo.TileSize.height;
             
                objectGroup.PositionOffset = positionOffset;

                pTMXMapInfo.ObjectGroups.Add(objectGroup);

                // The parent element is now "objectgroup"
                pTMXMapInfo.ParentElement = 3;

            }

            else if (elementName == "image")
            {
                CCTMXTilesetInfo tileset = pTMXMapInfo.Tilesets.LastOrDefault();

                // build full path
                string imagename = attributeDict["source"];
                tileset.m_sSourceImage = CCFileUtils.fullPathFromRelativeFile(imagename, pTMXMapInfo.TMXFileName);

            }
            else if (elementName == "data")
            {
                string encoding = attributeDict["encoding"];
                string compression = attributeDict["compression"];

                if (encoding == "base64")
                {
                    int layerAttribs = pTMXMapInfo.LayerAttribs;
                    pTMXMapInfo.LayerAttribs = layerAttribs | 1 << 1;
                    pTMXMapInfo.StoringCharacters = true;

                    if (compression == "gzip")
                    {
                        layerAttribs = pTMXMapInfo.LayerAttribs;
                        pTMXMapInfo.LayerAttribs = layerAttribs | 4;
                    }
                    else
                        if (compression == "zlib")
                        {
                            layerAttribs = pTMXMapInfo.LayerAttribs;
                            pTMXMapInfo.LayerAttribs = layerAttribs | 8;
                        }
                    Debug.Assert(compression == "" || compression == "gzip" || compression == "zlib", "TMX: unsupported compression method");
                }
                Debug.Assert(pTMXMapInfo.LayerAttribs != 1, "TMX tile map: Only base64 and/or gzip/zlib maps are supported");

            }
            //else if (elementName == "object")
            //{
            //    byte[] buffer = new byte[32];
            //    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();

            //    // The value for "type" was blank or not a valid class name
            //    // Create an instance of TMXObjectInfo to store the object and its properties
            //    Dictionary<string, string> dict = new Dictionary<string, string>();

            //    // Set the name of the object to the value for "name"
            //    string key;
            //     string value;
            //    if (attributeDict.Keys.Contains("name"))
            //    {
            //        key = "name";
            //        value = attributeDict["name"];
            //        dict.Add(key, value);
            //    }
            

            //    // Assign all the attributes as key/name pairs in the properties dictionary
            //    if (attributeDict.Keys.Contains("type"))
            //    {
            //        key = "type";
            //        value = attributeDict["type"];
            //        dict.Add(key, value);
            //    }
            //    if (attributeDict.Keys.Contains("x"))
            //    {
            //        int x = int.Parse(attributeDict["x"]) + (int)objectGroup.PositionOffset.x;
            //    }
            //    //key = "x";
            //    //sprintf(buffer, "%d", x);
            //    //value = new CCString(buffer);
            //    //dict->setObject(value, key);
            //    //value->release();

            //    //int y = atoi(valueForKey("y", attributeDict)) + (int)objectGroup->getPositionOffset().y;
            //    //// Correct y position. (Tiled uses Flipped, cocos2d uses Standard)
            //    //y = (int)(pTMXMapInfo->getMapSize().height * pTMXMapInfo->getTileSize().height) - y - atoi(valueForKey("height", attributeDict));
            //    //key = "y";
            //    //sprintf(buffer, "%d", y);
            //    //value = new CCString(buffer);
            //    //dict->setObject(value, key);
            //    //value->release();

            //    //key = "width";
            //    //value = new CCString(valueForKey("width", attributeDict));
            //    //dict->setObject(value, key);
            //    //value->release();

            //    //key = "height";
            //    //value = new CCString(valueForKey("height", attributeDict));
            //    //dict->setObject(value, key);
            //    //value->release();

            //    //// Add the object to the objectGroup
            //    //objectGroup->getObjects()->addObject(dict);
            //    //dict->release();

            //    //// The parent element is now "object"
            //    //pTMXMapInfo->setParentElement(TMXPropertyObject);

            //}
            #region
            // implement pure virtual methods of CCSAXDelegator
            //public void startElement(object ctx, string name, string[] atts)
            //{
            //    //CC_UNUSED_PARAM(ctx);
            //    CCTMXMapInfo pTMXMapInfo = this;
            //    string elementName = name;
            //    Dictionary<string, string> attributeDict = new Dictionary<string, string>();
            //    if (atts != null && atts[0] != null)
            //    {
            //        for (int i = 0; i < atts.Length; i += 2)
            //        {
            //            string key = (string)atts[i];
            //            string value = (string)atts[i + 1];
            //            attributeDict.Add(key, value);
            //        }
            //    }
            //    if (elementName == "map")
            //    {
            //        string version = attributeDict["version"];
            //        if (version != "1.0")
            //        {
            //            Debug.WriteLine("cocos2d: TMXFormat: Unsupported TMX version: {0}", version);
            //        }
            //        string orientationStr = attributeDict["orientation"];
            //        if (orientationStr == "orthogonal")
            //            pTMXMapInfo.Orientation = (int)CCTMXOrientatio.CCTMXOrientationOrtho;
            //        else if (orientationStr == "isometric")
            //            pTMXMapInfo.Orientation = (int)CCTMXOrientatio.CCTMXOrientationIso;
            //        else if (orientationStr == "hexagonal")
            //            pTMXMapInfo.Orientation = (int)CCTMXOrientatio.CCTMXOrientationHex;
            //        else
            //            Debug.WriteLine("cocos2d: TMXFomat: Unsupported orientation: {0}", pTMXMapInfo.Orientation);

            //        CCSize s = new CCSize();
            //        s.width = float.Parse(attributeDict["width"]);
            //        s.height = (float.Parse(attributeDict["height"]));
            //        pTMXMapInfo.MapSize = s;

            //        s.width = float.Parse(attributeDict["tilewidth"]);
            //        s.height = float.Parse(attributeDict["tileheight"]);
            //        pTMXMapInfo.TileSize = s;

            //        // The parent element is now "map"
            //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyMap;
            //    }
            //    else if (elementName == "tileset")
            //    {
            //        // If this is an external tileset then start parsing that
            //        string externalTilesetFilename = attributeDict["source"];
            //        if (externalTilesetFilename != "")
            //        {
            //            externalTilesetFilename = CCFileUtils.fullPathFromRelativeFile(externalTilesetFilename, pTMXMapInfo.TMXFileName);
            //            pTMXMapInfo.parseXMLFile(externalTilesetFilename);
            //        }
            //        else
            //        {
            //            CCTMXTilesetInfo tileset = new CCTMXTilesetInfo();
            //            tileset.m_sName = attributeDict["name"];
            //            tileset.m_uFirstGid = uint.Parse(attributeDict["firstgid"]);
            //            tileset.m_uSpacing = uint.Parse(attributeDict["spacing"]);
            //            tileset.m_uMargin = uint.Parse(attributeDict["margin"]);
            //            CCSize s = new CCSize();
            //            s.width = float.Parse(attributeDict["tilewidth"]);
            //            s.height = float.Parse(attributeDict["tileheight"]);
            //            tileset.m_tTileSize = s;

            //            pTMXMapInfo.Tilesets.Add(tileset);
            //            //tileset->release();
            //        }
            //    }
            //    else if (elementName == "tile")
            //    {
            //        CCTMXTilesetInfo info = pTMXMapInfo.Tilesets.LastOrDefault();
            //        Dictionary<string, string> dict = new Dictionary<string, string>();
            //        pTMXMapInfo.ParentGID = info.m_uFirstGid + uint.Parse(attributeDict["id"]);
            //        pTMXMapInfo.TileProperties.Add((int)pTMXMapInfo.ParentGID, dict);
            //        //CC_SAFE_RELEASE(dict);

            //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyTile;
            //    }
            //    else if (elementName == "layer")
            //    {
            //        CCTMXLayerInfo layer = new CCTMXLayerInfo();
            //        layer.m_sName = attributeDict["name"];

            //        CCSize s = new CCSize();
            //        s.width = float.Parse(attributeDict["width"]);
            //        s.height = float.Parse(attributeDict["height"]);
            //        layer.m_tLayerSize = s;

            //        string visible = attributeDict["visible"];
            //        layer.m_bVisible = !(visible == "0");

            //        string opacity = attributeDict["opacity"];
            //        if (opacity != "")
            //        {
            //            layer.m_cOpacity = (byte)(255 * int.Parse(opacity));
            //        }
            //        else
            //        {
            //            layer.m_cOpacity = 255;
            //        }

            //        float x = float.Parse(attributeDict["x"]);
            //        float y = float.Parse(attributeDict["y"]);
            //        layer.m_tOffset = new CCPoint(x, y);

            //        pTMXMapInfo.Layers.Add(layer);
            //        //layer->release();

            //        // The parent element is now "layer"
            //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyLayer;
            //    }
            //    else if (elementName == "objectgroup")
            //    {
            //        CCTMXObjectGroup objectGroup = new CCTMXObjectGroup();
            //        objectGroup.GroupName = attributeDict["name"];
            //        CCPoint positionOffset = new CCPoint();
            //        positionOffset.x = float.Parse(attributeDict["x"]) * pTMXMapInfo.TileSize.width;
            //        positionOffset.y = float.Parse(attributeDict["y"]) * pTMXMapInfo.TileSize.height;
            //        objectGroup.PositionOffset = positionOffset;

            //        pTMXMapInfo.ObjectGroups.Add(objectGroup);
            //        //objectGroup->release();

            //        // The parent element is now "objectgroup"
            //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyObjectGroup;

            //    }
            //    else if (elementName == "image")
            //    {
            //        CCTMXTilesetInfo tileset = pTMXMapInfo.Tilesets.LastOrDefault();

            //        // build full path
            //        string imagename = attributeDict["source"];
            //        tileset.m_sSourceImage = CCFileUtils.fullPathFromRelativeFile(imagename, pTMXMapInfo.TMXFileName);
            //    }
            //    else if (elementName == "data")
            //    {
            //        string encoding = attributeDict["encoding"];
            //        string compression = attributeDict["compression"];

            //        if (encoding == "base64")
            //        {
            //            int layerAttribs = pTMXMapInfo.LayerAttribs;
            //            pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribBase64;
            //            pTMXMapInfo.StoringCharacters = true;

            //            if (compression == "gzip")
            //            {
            //                layerAttribs = pTMXMapInfo.LayerAttribs;
            //                pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribGzip;
            //            }
            //            else
            //                if (compression == "zlib")
            //                {
            //                    layerAttribs = pTMXMapInfo.LayerAttribs;
            //                    pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribZlib;
            //                }
            //            Debug.Assert(compression == "" || compression == "gzip" || compression == "zlib", "TMX: unsupported compression method");
            //        }
            //        Debug.Assert(pTMXMapInfo.LayerAttribs != (int)TMXLayerAttrib.TMXLayerAttribNone, "TMX tile map: Only base64 and/or gzip/zlib maps are supported");
            //    }
            else if (elementName == "object")
            {
                char[] buffer = new char[32];
                CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();

                // The value for "type" was blank or not a valid class name
                // Create an instance of TMXObjectInfo to store the object and its properties
                Dictionary<string, string> dict = new Dictionary<string, string>();

                // Set the name of the object to the value for "name"
                string key = "name";
                string value = attributeDict["name"];
                dict.Add(value, key);
                //value->release();

                // Assign all the attributes as key/name pairs in the properties dictionary
                if (attributeDict.Keys.Contains("type"))
                {
                    key = "type";
                    value = attributeDict["type"];
                    dict.Add(value, key);
                    //value->release();

                }
            

                int x = int.Parse(attributeDict["x"]) + (int)objectGroup.PositionOffset.x;
                key = "x";
                buffer = x.ToString().ToArray();
                //string.Format(buffer, "%d", x);
                value = buffer.ToString();
                dict.Add(key, value);
                //value->release();

                int y = int.Parse(attributeDict["y"]) + (int)objectGroup.PositionOffset.y;
                // Correct y position. (Tiled uses Flipped, cocos2d uses Standard)
                if (attributeDict.Keys.Contains("y") && attributeDict.Keys.Contains("height"))
                {
                    y = (int)(pTMXMapInfo.MapSize.height * pTMXMapInfo.TileSize.height) - y - int.Parse(attributeDict["height"]);
                    key = "y";
                    buffer = x.ToString().ToArray();
                    value = buffer.ToString();
                    //sprintf(buffer, "%d", y);
                    //value = new CCString(buffer);
                    dict.Add(key, value);
                    //value->release();

                }
                if (attributeDict.Keys.Contains("width"))
                {
                    key = "width";
                    value = attributeDict["width"];
                    dict.Add(key, value);
                    //value->release();
                }
                if (attributeDict.Keys.Contains("height"))
                {
                    key = "height";
                    value = attributeDict["height"];
                    dict.Add(key, value);
                    //value->release();
                }
           
                // Add the object to the objectGroup
                objectGroup.Objects.Add(dict);
                //dict->release();

                // The parent element is now "object"
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyObject;

            }
            else if (elementName == "property")
            {
                if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyNone)
                {
                    Debug.WriteLine("TMX tile map: Parent element is unsupported. Cannot add property named '{0}' with value '{1}'",
                        attributeDict["name"], attributeDict["value"]);
                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyMap)
                {
                    // The parent element is the map
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    pTMXMapInfo.Properties.Add(key, value);
                    //value->release();

                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyLayer)
                {
                    // The parent element is the last layer
                    CCTMXLayerInfo layer = pTMXMapInfo.Layers.LastOrDefault();
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    // Add the property to the layer
                    layer.Properties.Add(key, value);
                    //value->release();

                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyLayer)
                {
                    // The parent element is the last object group
                    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    objectGroup.Properties.Add(key, value);
                    //value->release();
                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyObject)
                {
                    // The parent element is the last object
                    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
                    Dictionary<string, string> dict = objectGroup.Objects.LastOrDefault();

                    string propertyName = attributeDict["name"];
                    string propertyValue = attributeDict["value"];
                    dict.Add(propertyName, propertyValue);
                    //propertyValue->release();
                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyTile)
                {
                    Dictionary<string, string> dict;
                    dict = pTMXMapInfo.TileProperties[(int)pTMXMapInfo.ParentGID];

                    string propertyName = attributeDict["name"];
                    string propertyValue = attributeDict["value"];
                    dict.Add(propertyName, propertyValue);
                    //propertyValue->release();
                }
            } 
            #endregion
            if (attributeDict != null)
            {
                attributeDict.Clear();
                //delete attributeDict;
            }
        }

        public void endElement(object ctx, string name)
        {
            //CC_UNUSED_PARAM(ctx);
            CCTMXMapInfo pTMXMapInfo = this;
            string elementName = name;

            if (elementName == "data" && (pTMXMapInfo.LayerAttribs & (int)TMXLayerAttrib.TMXLayerAttribBase64) != 0)
            {
                pTMXMapInfo.StoringCharacters = false;
                for (int i = 0; i < pTMXMapInfo.Layers.Count; i++)
                {
                    CCTMXLayerInfo layer = pTMXMapInfo.Layers[i];
                    byte[] bs = ((cocos2d.Framework.CCContent)ctx).Date[i];
                    int[] bytes = new int[bs.Length];
                    for (int j = 0; j < bytes.Length; j++)
                    {
                        bytes[j] = 0;
                    }
                    for (int k = 0; k < bs.Length; k++)
                    {
                        if (bs[k] > 0)
                        {
                            bytes[k / 4] = bs[k];
                        }
                    }
                    layer.m_pTiles = bytes;
                }
                //int len = 0;
                //buffer = Convert.FromBase64CharArray(currentString.ToCharArray(), 0, currentString.Length);
                //len = buffer.Length;

                //if (buffer == null)
                //{
                //    Debug.WriteLine("cocos2d: TiledMap: decode data error");
                //    return;
                //}

                //if ((pTMXMapInfo.LayerAttribs & ((int)TMXLayerAttrib.TMXLayerAttribGzip | (int)TMXLayerAttrib.TMXLayerAttribZlib)) != 0)
                //{
                //    byte[] deflated = null;
                //    CCSize s = layer.m_tLayerSize;
                //    int sizeHint = (int)(s.width * s.height * sizeof(uint));

                //    int inflatedLen = ZipUtils.ccInflateMemoryWithHint(buffer, len, deflated, sizeHint);
                //    Debug.Assert(inflatedLen == sizeHint);

                //    // this.inflatedLen =inflatedLen; // XXX: to avoid warings in compiler

                //    buffer = null;

                //    if (deflated == null)
                //    {
                //        Debug.WriteLine("cocos2d: TiledMap: inflate data error");
                //        return;
                //    }

                //    //layer.m_pTiles =deflated;
                //}
                //else
                //{
                //    //layer.m_pTiles = (uint[]) buffer;
                //}

                //pTMXMapInfo.CurrentString = "";

            }
            else if (elementName == "map")
            {
                // The map element has ended
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyNone;
            }
            else if (elementName == "layer")
            {
                // The layer element has ended
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyNone;
            }
            else if (elementName == "objectgroup")
            {
                // The objectgroup element has ended
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyNone;
            }
            else if (elementName == "object")
            {
                // The object element has ended
                pTMXMapInfo.ParentElement = (int)(int)TMXProperty.TMXPropertyNone;
            }
        }

        public void textHandler(object ctx, string ch, int len)
        {
            //CC_UNUSED_PARAM(ctx);
            CCTMXMapInfo pTMXMapInfo = this;
            string pText = new string(ch.ToArray(), 0, len);

            if (pTMXMapInfo.StoringCharacters)
            {
                string currentString = pTMXMapInfo.CurrentString;
                currentString += pText;
                pTMXMapInfo.CurrentString = currentString;
            }
        }

        //public void startElement(object ctx, string name, string[] atts)
        //{
        //    //CC_UNUSED_PARAM(ctx);
        //    CCTMXMapInfo pTMXMapInfo = this;
        //    string elementName = name;
        //    Dictionary<string, string> attributeDict = new Dictionary<string, string>();
        //    if (atts != null && atts[0] != null)
        //    {
        //        for (int i = 0; i < atts.Length; i += 2)
        //        {
        //            string key = atts[i].ToString();
        //            string value = atts[i + 1].ToString();
        //            attributeDict.Add(key, value);
        //        }
        //    }
        //    if (elementName == "map")
        //    {
        //        string version = attributeDict["version"];
        //        if (version != "1.0")
        //        {
        //            Debug.WriteLine("cocos2d: TMXFormat: Unsupported TMX version: {0}", version);
        //        }
        //        string orientationStr = attributeDict["orientation"];
        //        if (orientationStr == "orthogonal")
        //            pTMXMapInfo.Orientation = (int)CCTMXOrientatio.CCTMXOrientationOrtho;
        //        else if (orientationStr == "isometric")
        //            pTMXMapInfo.Orientation = (int)CCTMXOrientatio.CCTMXOrientationIso;
        //        else if (orientationStr == "hexagonal")
        //            pTMXMapInfo.Orientation = (int)CCTMXOrientatio.CCTMXOrientationHex;
        //        else
        //            Debug.WriteLine("cocos2d: TMXFomat: Unsupported orientation:{0}", pTMXMapInfo.Orientation);

        //        CCSize s = new CCSize();
        //        s.width = float.Parse(attributeDict["width"]);
        //        s.height = float.Parse(attributeDict["height"]);
        //        pTMXMapInfo.MapSize = s;

        //        s.width = float.Parse(attributeDict["tilewidth"]);
        //        s.height = float.Parse(attributeDict["tileheight"]);
        //        pTMXMapInfo.TileSize = s;

        //        // The parent element is now "map"
        //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyMap;
        //    }
        //    else if (elementName == "tileset")
        //    {
        //        // If this is an external tileset then start parsing that
        //        string externalTilesetFilename = attributeDict["source"];
        //        if (externalTilesetFilename != "")
        //        {
        //            externalTilesetFilename = CCFileUtils.fullPathFromRelativeFile(externalTilesetFilename, pTMXMapInfo.TMXFileName);
        //            pTMXMapInfo.parseXMLFile(externalTilesetFilename);
        //        }
        //        else
        //        {
        //            CCTMXTilesetInfo tileset = new CCTMXTilesetInfo();
        //            tileset.m_sName = attributeDict["name"];
        //            tileset.m_uFirstGid = uint.Parse(attributeDict["firstgid"]);
        //            tileset.m_uSpacing = uint.Parse(attributeDict["spacing"]);
        //            tileset.m_uMargin = uint.Parse(attributeDict["margin"]);
        //            CCSize s = new CCSize();
        //            s.width = float.Parse(attributeDict["tilewidth"]);
        //            s.height = float.Parse(attributeDict["tileheight"]);
        //            tileset.m_tTileSize = s;

        //            pTMXMapInfo.Tilesets.Add(tileset);
        //            //tileset->release();
        //        }
        //    }
        //    else if (elementName == "tile")
        //    {
        //        CCTMXTilesetInfo info = pTMXMapInfo.Tilesets.LastOrDefault();
        //        Dictionary<string, string> dict = new Dictionary<string, string>();
        //        pTMXMapInfo.ParentGID = info.m_uFirstGid + uint.Parse(attributeDict["id"]);
        //        pTMXMapInfo.TileProperties.Add((int)pTMXMapInfo.ParentGID, dict);
        //        //CC_SAFE_RELEASE(dict);

        //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyTile;

        //    }
        //    else if (elementName == "layer")
        //    {
        //        CCTMXLayerInfo layer = new CCTMXLayerInfo();
        //        layer.m_sName = attributeDict["name"];

        //        CCSize s = new CCSize();
        //        s.width = float.Parse(attributeDict["width"]);
        //        s.height = float.Parse(attributeDict["height"]);
        //        layer.m_tLayerSize = s;

        //        string visible = attributeDict["visible"];
        //        layer.m_bVisible = !(visible == "0");

        //        string opacity = attributeDict["opacity"];
        //        if (opacity != "")
        //        {
        //            layer.m_cOpacity = (byte)(255 * float.Parse(opacity));
        //        }
        //        else
        //        {
        //            layer.m_cOpacity = 255;
        //        }

        //        float x = float.Parse(attributeDict["x"]);
        //        float y = float.Parse(attributeDict["y"]);
        //        layer.m_tOffset = new CCPoint(x, y);

        //        pTMXMapInfo.Layers.Add(layer);
        //        //layer->release();

        //        // The parent element is now "layer"
        //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyLayer;

        //    }
        //    else if (elementName == "objectgroup")
        //    {
        //        CCTMXObjectGroup objectGroup = new CCTMXObjectGroup();
        //        objectGroup.GroupName = attributeDict["name"];
        //        CCPoint positionOffset = new CCPoint();
        //        positionOffset.x = float.Parse(attributeDict["x"]) * pTMXMapInfo.TileSize.width;
        //        positionOffset.y = float.Parse(attributeDict["y"]) * pTMXMapInfo.TileSize.height;
        //        objectGroup.PositionOffset = positionOffset;

        //        pTMXMapInfo.ObjectGroups.Add(objectGroup);
        //        //objectGroup->release();

        //        // The parent element is now "objectgroup"
        //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyObjectGroup;

        //    }
        //    else if (elementName == "image")
        //    {
        //        CCTMXTilesetInfo tileset = pTMXMapInfo.Tilesets.LastOrDefault();

        //        // build full path
        //        string imagename = attributeDict["source"];
        //        tileset.m_sSourceImage = CCFileUtils.fullPathFromRelativeFile(imagename, pTMXMapInfo.TMXFileName);

        //    }
        //    else if (elementName == "data")
        //    {
        //        string encoding = attributeDict["encoding"];
        //        string compression = attributeDict["compression"];

        //        if (encoding == "base64")
        //        {
        //            int layerAttribs = pTMXMapInfo.LayerAttribs;
        //            pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribBase64;
        //            pTMXMapInfo.StoringCharacters = true;

        //            if (compression == "gzip")
        //            {
        //                layerAttribs = pTMXMapInfo.LayerAttribs;
        //                pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribGzip;
        //            }
        //            else
        //                if (compression == "zlib")
        //                {
        //                    layerAttribs = pTMXMapInfo.LayerAttribs;
        //                    pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribZlib;
        //                }
        //            Debug.Assert(compression == "" || compression == "gzip" || compression == "zlib", "TMX: unsupported compression method");
        //        }
        //        Debug.Assert(pTMXMapInfo.LayerAttribs != (int)TMXLayerAttrib.TMXLayerAttribNone, "TMX tile map: Only base64 and/or gzip/zlib maps are supported");

        //    }
        //    else if (elementName == "object")
        //    {
        //        char[] buffer = new char[32];
        //        CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();

        //        // The value for "type" was blank or not a valid class name
        //        // Create an instance of TMXObjectInfo to store the object and its properties
        //        Dictionary<string, string> dict = new Dictionary<string, string>();

        //        // Set the name of the object to the value for "name"
        //        string key = "name";
        //        string value = attributeDict["name"];
        //        dict.Add(key, value);
        //        //value->release();

        //        // Assign all the attributes as key/name pairs in the properties dictionary
        //        key = "type";
        //        value = attributeDict["type"];
        //        dict.Add(key, value);
        //        //value->release();

        //        int x = int.Parse(attributeDict["x"]) + (int)objectGroup.PositionOffset.x;
        //        key = "x";
        //        //sprintf(buffer, "%d", x);
        //        value = buffer.ToString();
        //        dict.Add(key, value);
        //        //value->release();

        //        int y = int.Parse(attributeDict["y"]) + (int)objectGroup.PositionOffset.y;
        //        // Correct y position. (Tiled uses Flipped, cocos2d uses Standard)
        //        y = (int)(pTMXMapInfo.MapSize.height * pTMXMapInfo.TileSize.height) - y - int.Parse(attributeDict["height"]);
        //        key = "y";
        //        //sprintf(buffer, "%d", y);
        //        value = buffer.ToString();
        //        dict.Add(key, value);
        //        //value->release();

        //        key = "width";
        //        value = attributeDict["width"];
        //        dict.Add(key, value);
        //        //value->release();

        //        key = "height";
        //        value = attributeDict["height"];
        //        dict.Add(key, value);
        //        //value->release();

        //        // Add the object to the objectGroup
        //        objectGroup.Objects.Add(dict);
        //        //dict->release();

        //        // The parent element is now "object"
        //        pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyObject;

        //    }
        //    else if (elementName == "property")
        //    {
        //        if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyNone)
        //        {
        //            Debug.WriteLine("TMX tile map: Parent element is unsupported. Cannot add property named '{0}' with value '{1}'",
        //                attributeDict["name"], attributeDict["value"]);
        //        }
        //        else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyMap)
        //        {
        //            // The parent element is the map
        //            string value = attributeDict["value"];
        //            string key = attributeDict["name"];
        //            pTMXMapInfo.Properties.Add(key, value);
        //            //value->release();

        //        }
        //        else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyLayer)
        //        {
        //            // The parent element is the last layer
        //            CCTMXLayerInfo layer = pTMXMapInfo.Layers.LastOrDefault();
        //            string value = attributeDict["value"];
        //            string key = attributeDict["name"];
        //            // Add the property to the layer
        //            layer.Properties.Add(key, value);
        //            //value->release();

        //        }
        //        else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyObjectGroup)
        //        {
        //            // The parent element is the last object group
        //            CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
        //            string value = attributeDict["value"];
        //            string key = attributeDict["name"];
        //            objectGroup.Properties.Add(key, value);
        //            //value->release();

        //        }
        //        else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyObject)
        //        {
        //            // The parent element is the last object
        //            CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
        //            Dictionary<string, string> dict = objectGroup.Objects.LastOrDefault();

        //            string propertyName = attributeDict["name"];
        //            string propertyValue = attributeDict["value"];
        //            dict.Add(propertyName, propertyValue);
        //            //propertyValue->release();
        //        }
        //        else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyTile)
        //        {
        //            Dictionary<string, string> dict;
        //            dict = pTMXMapInfo.TileProperties[(int)pTMXMapInfo.ParentGID];

        //            string propertyName = attributeDict["name"];
        //            string propertyValue = attributeDict["value"];
        //            dict.Add(propertyValue, propertyName);
        //            //propertyValue->release();
        //        }
        //    }
        //    if (attributeDict != null)
        //    {
        //        attributeDict.Clear();
        //        attributeDict = null;
        //    }
        //}
    }
}
