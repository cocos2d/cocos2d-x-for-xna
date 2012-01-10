using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXTilesetTest : TileDemo
    {
        public TMXTilesetTest()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test5");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            CCTMXLayer layer = map.layerNamed("Layer 0");
            layer.Texture.setAntiAliasTexParameters();

            layer = map.layerNamed("Layer 1");
            layer.Texture.setAntiAliasTexParameters();

            layer = map.layerNamed("Layer 2");
            layer.Texture.setAntiAliasTexParameters();
        }

        public override string title()
        {
            return "TMX Tileset test";
        }
    }
}
