/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
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
using System.Diagnostics;
using System.IO;
using cocos2d;
using WP7Contrib.Communications.Compression;
using ICSharpCode.SharpZipLib.GZip;
using ComponentAce.Compression.Libs.zlib;


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

        protected byte[] m_sCurrentString;
        /// <summary>
        /// ! current string
        /// </summary>
        public byte[] CurrentString
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
            m_sCurrentString = null;
            m_bStoringCharacters = false;
            m_nLayerAttribs = (int)TMXLayerAttrib.TMXLayerAttribNone;
            m_nParentElement = (int)TMXProperty.TMXPropertyNone;

            return parseXMLFile(m_sTMXFileName);
        }

        /// <summary>
        /// initalises parsing of an XML file, either a tmx (Map) file or tsx (Tileset) file
        /// </summary>
        public bool parseXMLFile(string xmlFilename)
        {
            CCSAXParser parser = new CCSAXParser();

            if (false == parser.init("UTF-8"))
            {
                return false;
            }

            parser.setDelegator(this);

            return parser.parse(xmlFilename); ;
        }

        // the XML parser calls here with all the elements
        public void startElement(object ctx, string name, string[] atts)
        {
            CCTMXMapInfo pTMXMapInfo = this;
            string elementName = name;
            Dictionary<string, string> attributeDict = new Dictionary<string, string>();
            if (atts != null && atts[0] != null)
            {
                for (int i = 0; i + 1 < atts.Length; i += 2)
                {
                    string key = atts[i];
                    string value = atts[i + 1];
                    attributeDict.Add(key, value);
                }
            }
            if (elementName == "map")
            {
                string version = attributeDict["version"];
                if (version != "1.0")
                {
                    Debug.WriteLine("cocos2d: TMXFormat: Unsupported TMX version: {0}", version);
                }
                string orientationStr = attributeDict["orientation"];
                if (orientationStr == "orthogonal")
                    pTMXMapInfo.Orientation = (int)(CCTMXOrientatio.CCTMXOrientationOrtho);
                else if (orientationStr == "isometric")
                    pTMXMapInfo.Orientation = (int)(CCTMXOrientatio.CCTMXOrientationIso);
                else if (orientationStr == "hexagonal")
                    pTMXMapInfo.Orientation = (int)(CCTMXOrientatio.CCTMXOrientationHex);
                else
                    Debug.WriteLine("cocos2d: TMXFomat: Unsupported orientation: {0}", pTMXMapInfo.Orientation);

                CCSize s = new CCSize();
                s.width = float.Parse(attributeDict["width"]);
                s.height = float.Parse(attributeDict["height"]);
                pTMXMapInfo.MapSize = s;

                s.width = float.Parse(attributeDict["tilewidth"]);
                s.height = float.Parse(attributeDict["tileheight"]);
                pTMXMapInfo.TileSize = s;

                // The parent element is now "map"
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyMap;
            }
            else if (elementName == "tileset")
            {
                // If this is an external tileset then start parsing that

                if (attributeDict.Keys.Contains("source"))
                {
                    string externalTilesetFilename = attributeDict["source"];

                    externalTilesetFilename = CCFileUtils.fullPathFromRelativeFile(externalTilesetFilename, pTMXMapInfo.TMXFileName);
                    pTMXMapInfo.parseXMLFile(externalTilesetFilename);
                }
                else
                {
                    CCTMXTilesetInfo tileset = new CCTMXTilesetInfo();
                    tileset.m_sName = attributeDict["name"];
                    tileset.m_uFirstGid = int.Parse(attributeDict["firstgid"]);

                    if (attributeDict.Keys.Contains("spacing"))
                        tileset.m_uSpacing = int.Parse(attributeDict["spacing"]);

                    if (attributeDict.Keys.Contains("margin"))
                        tileset.m_uMargin = int.Parse(attributeDict["margin"]);
                    CCSize s = new CCSize();
                    s.width = float.Parse(attributeDict["tilewidth"]);
                    s.height = float.Parse(attributeDict["tileheight"]);
                    tileset.m_tTileSize = s;

                    pTMXMapInfo.Tilesets.Add(tileset);
                }
            }
            else if (elementName == "tile")
            {
                CCTMXTilesetInfo info = pTMXMapInfo.Tilesets.LastOrDefault();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                pTMXMapInfo.ParentGID = (info.m_uFirstGid + int.Parse(attributeDict["id"]));
                pTMXMapInfo.TileProperties.Add(pTMXMapInfo.ParentGID, dict);

                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyTile;

            }
            else if (elementName == "layer")
            {
                CCTMXLayerInfo layer = new CCTMXLayerInfo();
                layer.m_sName = attributeDict["name"];

                CCSize s = new CCSize();
                s.width = float.Parse(attributeDict["width"]);
                s.height = float.Parse(attributeDict["height"]);
                layer.m_tLayerSize = s;

                layer.m_pTiles = new int[(int)s.width * (int)s.height];

                if (attributeDict.Keys.Contains("visible"))
                {
                    string visible = attributeDict["visible"];
                    layer.m_bVisible = !(visible == "0");
                }
                else
                {
                    layer.m_bVisible = true;
                }

                if (attributeDict.Keys.Contains("opacity"))
                {
                    string opacity = attributeDict["opacity"];
                    layer.m_cOpacity = (byte)(255 * float.Parse(opacity));
                }
                else
                {
                    layer.m_cOpacity = 255;
                }

                float x = attributeDict.Keys.Contains("x") ? float.Parse(attributeDict["x"]) : 0;
                float y = attributeDict.Keys.Contains("y") ? float.Parse(attributeDict["y"]) : 0;
                layer.m_tOffset = new CCPoint(x, y);

                pTMXMapInfo.Layers.Add(layer);

                // The parent element is now "layer"
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyLayer;

            }
            else if (elementName == "objectgroup")
            {
                CCTMXObjectGroup objectGroup = new CCTMXObjectGroup();
                objectGroup.GroupName = attributeDict["name"];
                CCPoint positionOffset = new CCPoint();
                if (attributeDict.ContainsKey("x"))
                    positionOffset.x = float.Parse(attributeDict["x"]) * pTMXMapInfo.TileSize.width;
                if (attributeDict.ContainsKey("y"))
                    positionOffset.y = float.Parse(attributeDict["y"]) * pTMXMapInfo.TileSize.height;
                objectGroup.PositionOffset = positionOffset;

                pTMXMapInfo.ObjectGroups.Add(objectGroup);

                // The parent element is now "objectgroup"
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyObjectGroup;

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
                string encoding = attributeDict.ContainsKey("encoding") ? attributeDict["encoding"] : "";
                string compression = attributeDict.ContainsKey("compression") ? attributeDict["compression"] : "";

                if (encoding == "base64")
                {
                    int layerAttribs = pTMXMapInfo.LayerAttribs;
                    pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribBase64;
                    pTMXMapInfo.StoringCharacters = true;

                    if (compression == "gzip")
                    {
                        layerAttribs = pTMXMapInfo.LayerAttribs;
                        pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribGzip;
                    }
                    else
                        if (compression == "zlib")
                        {
                            layerAttribs = pTMXMapInfo.LayerAttribs;
                            pTMXMapInfo.LayerAttribs = layerAttribs | (int)TMXLayerAttrib.TMXLayerAttribZlib;
                        }
                    Debug.Assert(compression == "" || compression == "gzip" || compression == "zlib", "TMX: unsupported compression method");
                }
                Debug.Assert(pTMXMapInfo.LayerAttribs != (int)TMXLayerAttrib.TMXLayerAttribNone, "TMX tile map: Only base64 and/or gzip/zlib maps are supported");

            }
            else if (elementName == "object")
            {
                char[] buffer = new char[32];
                CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();

                // The value for "type" was blank or not a valid class name
                // Create an instance of TMXObjectInfo to store the object and its properties
                Dictionary<string, string> dict = new Dictionary<string, string>();

                // Set the name of the object to the value for "name"
                string key = "name";
                string value = attributeDict.ContainsKey("name") ? attributeDict["name"] : "";
                dict.Add(key, value);

                // Assign all the attributes as key/name pairs in the properties dictionary
                key = "type";
                value = attributeDict.ContainsKey("type") ? attributeDict["type"] : "";
                dict.Add(key, value);

                int x = int.Parse(attributeDict["x"]) + (int)objectGroup.PositionOffset.x;
                key = "x";
                value = x.ToString();
                dict.Add(key, value);

                int y = int.Parse(attributeDict["y"]) + (int)objectGroup.PositionOffset.y;
                // Correct y position. (Tiled uses Flipped, cocos2d uses Standard)
                y = (int)(pTMXMapInfo.MapSize.height * pTMXMapInfo.TileSize.height) - y - (attributeDict.ContainsKey("height") ? int.Parse(attributeDict["height"]) : 0);
                key = "y";
                value = y.ToString();
                dict.Add(key, value);

                key = "width";
                value = attributeDict.ContainsKey("width") ? attributeDict["width"] : "";
                dict.Add(key, value);

                key = "height";
                value = attributeDict.ContainsKey("height") ? attributeDict["height"] : "";
                dict.Add(key, value);

                // Add the object to the objectGroup
                objectGroup.Objects.Add(dict);

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

                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyLayer)
                {
                    // The parent element is the last layer
                    CCTMXLayerInfo layer = pTMXMapInfo.Layers.LastOrDefault();
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    // Add the property to the layer
                    layer.Properties.Add(key, value);
                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyObjectGroup)
                {
                    // The parent element is the last object group
                    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
                    string value = attributeDict["value"];
                    string key = attributeDict["name"];
                    objectGroup.Properties.Add(key, value);
                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyObject)
                {
                    // The parent element is the last object
                    CCTMXObjectGroup objectGroup = pTMXMapInfo.ObjectGroups.LastOrDefault();
                    Dictionary<string, string> dict = objectGroup.Objects.LastOrDefault();

                    string propertyName = attributeDict["name"];
                    string propertyValue = attributeDict["value"];
                    dict.Add(propertyName, propertyValue);
                }
                else if (pTMXMapInfo.ParentElement == (int)TMXProperty.TMXPropertyTile)
                {
                    Dictionary<string, string> dict = pTMXMapInfo.TileProperties[pTMXMapInfo.ParentGID];

                    string propertyName = attributeDict["name"];
                    string propertyValue = attributeDict["value"];
                    dict.Add(propertyName, propertyValue);
                }
            }
            if (attributeDict != null)
            {
                attributeDict = null;
            }
        }

        public void endElement(object ctx, string elementName)
        {
            CCTMXMapInfo pTMXMapInfo = this;
            byte[] buffer = pTMXMapInfo.CurrentString;

            if (elementName == "data" && (pTMXMapInfo.LayerAttribs & (int)TMXLayerAttrib.TMXLayerAttribBase64) != 0)
            {
                pTMXMapInfo.StoringCharacters = false;
                CCTMXLayerInfo layer = pTMXMapInfo.Layers.LastOrDefault();
                if ((pTMXMapInfo.LayerAttribs & ((int)(TMXLayerAttrib.TMXLayerAttribGzip) | (int)TMXLayerAttrib.TMXLayerAttribZlib)) != 0)
                {
                    using (MemoryStream ms = new MemoryStream(buffer, false))
                    {
                        //gzip compress
                        if ((pTMXMapInfo.LayerAttribs & (int)TMXLayerAttrib.TMXLayerAttribGzip) != 0)
                        {
                            using (GZipInputStream inStream = new GZipInputStream(ms))
                            {
                                using (var br = new BinaryReader(inStream))
                                {
                                    for (int i = 0; i < layer.m_pTiles.Length; i++)
                                        layer.m_pTiles[i] = br.ReadInt32();
                                }
                            }
                        }

                        //zlib
                        if ((pTMXMapInfo.LayerAttribs & (int)TMXLayerAttrib.TMXLayerAttribZlib) != 0)
                        {
                            using (ZInputStream inZStream = new ZInputStream(ms))
                            {
                                for (int i = 0; i < layer.m_pTiles.Length; i++)
                                {
                                    layer.m_pTiles[i] = inZStream.Read();
                                    inZStream.Read();
                                    inZStream.Read();
                                    inZStream.Read();
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < layer.m_pTiles.Length; i++)
                        layer.m_pTiles[i] = buffer[i * 4];
                }

                pTMXMapInfo.CurrentString = null;
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
                pTMXMapInfo.ParentElement = (int)TMXProperty.TMXPropertyNone;
            }
        }

        public void textHandler(object ctx, byte[] ch, int len)
        {
            //CC_UNUSED_PARAM(ctx);
            CCTMXMapInfo pTMXMapInfo = this;
            //string pText = System.Text.UTF8Encoding.UTF8.GetString(ch, 0, len);

            if (pTMXMapInfo.StoringCharacters)
            {
                //string currentString = pTMXMapInfo.CurrentString;
                //currentString += pText;
                pTMXMapInfo.CurrentString = ch;
            }
        }
    }
}
