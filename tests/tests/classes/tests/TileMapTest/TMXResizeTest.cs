using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXResizeTest : TileDemo
    {
        public TMXResizeTest()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test5");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            CCTMXLayer layer = map.layerNamed("Layer 0");

            CCSize ls = layer.LayerSize;
            for (int y = 0; y < ls.height; y++)
            {
                for (int x = 0; x < ls.width; x++)
                {
                    layer.setTileGID(1, new CCPoint(x, y));
                }
            }
        }

        public override string title()
        {
            return "TMX resize test";
        }

        public override string subtitle()
        {
            return "Should not crash. Testing issue #740";
        }
    }
}
