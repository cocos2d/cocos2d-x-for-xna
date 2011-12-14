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

namespace cocos2d
{
    /** @brief CCTMXTiledMap knows how to parse and render a TMX map.

   It adds support for the TMX tiled map format used by http://www.mapeditor.org
   It supports isometric, hexagonal and orthogonal tiles.
   It also supports object groups, objects, and properties.

Features:
- Each tile will be treated as an CCSprite
- The sprites are created on demand. They will be created only when you call "layer->tileAt(position)"
- Each tile can be rotated / moved / scaled / tinted / "opacitied", since each tile is a CCSprite
- Tiles can be added/removed in runtime
- The z-order of the tiles can be modified in runtime
- Each tile has an anchorPoint of (0,0)
- The anchorPoint of the TMXTileMap is (0,0)
- The TMX layers will be added as a child
- The TMX layers will be aliased by default
- The tileset image will be loaded using the CCTextureCache
- Each tile will have a unique tag
- Each tile will have a unique z value. top-left: z=1, bottom-right: z=max z
- Each object group will be treated as an CCMutableArray
- Object class which will contain all the properties in a dictionary
- Properties can be assigned to the Map, Layer, Object Group, and Object

Limitations:
- It only supports one tileset per layer.
- Embeded images are not supported
- It only supports the XML format (the JSON format is not supported)

Technical description:
Each layer is created using an CCTMXLayer (subclass of CCSpriteBatchNode). If you have 5 layers, then 5 CCTMXLayer will be created,
unless the layer visibility is off. In that case, the layer won't be created at all.
You can obtain the layers (CCTMXLayer objects) at runtime by:
- map->getChildByTag(tag_number);  // 0=1st layer, 1=2nd layer, 2=3rd layer, etc...
- map->layerNamed(name_of_the_layer);

Each object group is created using a CCTMXObjectGroup which is a subclass of CCMutableArray.
You can obtain the object groups at runtime by:
- map->objectGroupNamed(name_of_the_object_group);

Each object is a CCTMXObject.

Each property is stored as a key-value pair in an CCMutableDictionary.
You can obtain the properties at runtime by:

map->propertyNamed(name_of_the_property);
layer->propertyNamed(name_of_the_property);
objectGroup->propertyNamed(name_of_the_property);
object->propertyNamed(name_of_the_property);

@since v0.8.1
*/
    public class CCTMXTiledMap : CCNode
    {
        #region properties

        protected CCSize m_tMapSize;
        /// <summary>
        /// the map's size property measured in tiles
        /// </summary>
        public CCSize MapSize
        {
            get { return m_tMapSize; }
            set { m_tMapSize = value; }
        }

        protected CCSize m_tTileSize;
        /// <summary>
        /// the tiles's size property measured in pixels
        /// </summary>
        public CCSize TileSize
        {
            get { return m_tTileSize; }
            set { m_tTileSize = value; }
        }

        protected int m_nMapOrientation;
        /// <summary>
        /// map orientation
        /// </summary>
        public int MapOrientation
        {
            get { return m_nMapOrientation; }
            set { m_nMapOrientation = value; }
        }

        protected List<CCTMXObjectGroup> m_pObjectGroups;
        /// <summary>
        /// object groups
        /// </summary>
        public List<CCTMXObjectGroup> ObjectGroups
        {
            get { return m_pObjectGroups; }
            set { m_pObjectGroups = value; }
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

        #endregion

        public CCTMXTiledMap()
        {
        }

        /// <summary>
        /// creates a TMX Tiled Map with a TMX file.
        /// </summary>
        public static CCTMXTiledMap tiledMapWithTMXFile(string tmxFile)
        {
            CCTMXTiledMap pRet = new CCTMXTiledMap();
            if (pRet.initWithTMXFile(tmxFile))
            {
                return pRet;
            }
            return null;
        }

        /// <summary>
        /// initializes a TMX Tiled Map with a TMX file
        /// </summary>
        public bool initWithTMXFile(string tmxFile)
        {
            Debug.Assert(tmxFile != null && tmxFile.Length > 0, "TMXTiledMap: tmx file should not bi nil");

            contentSize = new CCSize(0, 0);

            CCTMXMapInfo mapInfo = CCTMXMapInfo.formatWithTMXFile(tmxFile);

            if (mapInfo == null)
            {
                return false;
            }
            Debug.Assert(mapInfo.Tilesets.Count != 0, "TMXTiledMap: Map not found. Please check the filename.");

            m_tMapSize = mapInfo.MapSize;
            m_tTileSize = mapInfo.TileSize;
            m_nMapOrientation = mapInfo.Orientation;
            ObjectGroups = mapInfo.ObjectGroups;
            Properties = mapInfo.Properties;
            m_pTileProperties = null;
            m_pTileProperties = mapInfo.TileProperties;
            //CC_SAFE_RETAIN(m_pTileProperties);

            int idx = 0;

            List<CCTMXLayerInfo> layers = mapInfo.Layers;
            if (layers != null && layers.Count > 0)
            {
                if (null == m_pTMXLayers)
                {
                    m_pTMXLayers = new Dictionary<string, CCTMXLayer>();
                    Debug.Assert(m_pTMXLayers != null, "Allocate memory failed!");
                }

                CCTMXLayerInfo layerInfo = null;
                for (int i = 0; i < layers.Count; i++)
                {
                    layerInfo = layers[i];
                    if (layerInfo != null && layerInfo.m_bVisible)
                    {
                        CCTMXLayer child = parseLayer(layerInfo, mapInfo);
                        addChild((CCNode)child, idx, idx);

                        // record the CCTMXLayer object by it's name
                        string layerName = child.LayerName;
                        m_pTMXLayers.Add(layerName, child);

                        // update content size with the max size
                        CCSize childSize = child.contentSize;
                        CCSize currentSize = this.contentSize;
                        currentSize.width = Math.Max(currentSize.width, childSize.width);
                        currentSize.height = Math.Max(currentSize.height, childSize.height);
                        this.contentSize = currentSize;

                        idx++;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// return the TMXLayer for the specific layer
        /// </summary>
        public CCTMXLayer layerNamed(string layerName)
        {
            string sLayerName = layerName;
            CCTMXLayer pRet = m_pTMXLayers[sLayerName];
            return pRet;
        }

        /// <summary>
        /// return the TMXObjectGroup for the secific group 
        /// </summary>
        public CCTMXObjectGroup objectGroupNamed(string groupName)
        {
            string sGroupName = groupName;
            if (m_pObjectGroups != null && m_pObjectGroups.Count > 0)
            {
                CCTMXObjectGroup objectGroup;

                for (int i = 0; i < m_pObjectGroups.Count; i++)
                {
                    objectGroup = m_pObjectGroups[i] as CCTMXObjectGroup;
                    if (objectGroup != null && objectGroup.GroupName == sGroupName)
                    {
                        return objectGroup;
                    }
                }
            }

            // objectGroup not found
            return null;
        }

        /// <summary>
        ///  return the value for the specific property name
        /// </summary>
        public string propertyNamed(string propertyName)
        {
            return m_pProperties[propertyName];
        }

        /// <summary>
        /// return properties dictionary for tile GID
        /// </summary>
        public Dictionary<string, string> propertiesForGID(int GID)
        {
            return m_pTileProperties[GID];
        }

        //private
        private CCTMXLayer parseLayer(CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            CCTMXTilesetInfo tileset = tilesetForLayer(layerInfo, mapInfo);
            CCTMXLayer layer = CCTMXLayer.layerWithTilesetInfo(tileset, layerInfo, mapInfo);

            // tell the layerinfo to release the ownership of the tiles map.
            layerInfo.m_bOwnTiles = false;
            layer.setupTiles();

            return layer;
        }

        private CCTMXTilesetInfo tilesetForLayer(CCTMXLayerInfo layerInfo, CCTMXMapInfo mapInfo)
        {
            CCSize size = layerInfo.m_tLayerSize;
            List<CCTMXTilesetInfo> tilesets = mapInfo.Tilesets;

            if (tilesets != null && tilesets.Count > 0)
            {
                CCTMXTilesetInfo tileset = null;

                for (int i = 0; i < tilesets.Count; i++)
                {
                    tileset = tilesets[i];
                    if (tileset != null)
                    {
                        for (uint y = 0; y < size.height; y++)
                        {
                            for (uint x = 0; x < size.width; x++)
                            {
                                uint pos = (uint)(x + size.width * y);
                                uint gid = layerInfo.m_pTiles[pos];

                                // gid are stored in little endian.
                                // if host is big endian, then swap
                                //if( o == CFByteOrderBigEndian )
                                //	gid = CFSwapInt32( gid );
                                /* We support little endian.*/

                                // XXX: gid == 0 --> empty tile
                                if (gid != 0)
                                {
                                    // Optimization: quick return
                                    // if the layer is invalid (more than 1 tileset per layer) an assert will be thrown later
                                    if (gid >= tileset.m_uFirstGid)
                                        return tileset;
                                }
                            }
                        }
                    }
                }
            }

            // If all the tiles are 0, return empty tileset
            Debug.WriteLine("cocos2d: Warning: TMX Layer '{0}' has no tiles", layerInfo.m_sName);
            return null;
        }

        //! tile properties
        protected Dictionary<int, Dictionary<string, string>> m_pTileProperties;
        protected Dictionary<string, CCTMXLayer> m_pTMXLayers;
    }
}
