using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXUncompressedTest : TileDemo
    {
        public TMXUncompressedTest()
        {
            CCLayerColor color = CCLayerColor.layerWithColor(new ccColor4B(64, 64, 64, 255));
            addChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test2-uncompressed");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.runAction(CCMoveTo.actionWithDuration(1.0f, new CCPoint(-ms.width * ts.width / 2, -ms.height * ts.height / 2)));

            // testing release map
            CCTMXLayer layer;
            foreach (var pObject in map.children)
            {
                layer = (CCTMXLayer)pObject;

                if (layer == null)
                    break;

                layer.releaseMap();
            }
        }

        public override string title()
        {
            return "TMX Uncompressed test";
        }
    }
}
