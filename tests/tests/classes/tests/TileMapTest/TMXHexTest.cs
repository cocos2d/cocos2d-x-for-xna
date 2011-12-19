using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXHexTest : TileDemo
    {
        public TMXHexTest()
        {
            CCLayerColor color = CCLayerColor.layerWithColor(new ccColor4B(64, 64, 64, 255));
            addChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/hexa-test1");
            addChild(map, 0, 1);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);
        }
        public override string title()
        {
            return "TMX Hex tes";
        }
    }
}
