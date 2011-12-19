using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    class TMXBug787 : TileDemo
    {
        public TMXBug787()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test-bug787");
            addChild(map, 0, 1);

            map.scale = 0.25f;
        }
        public override string title()
        {
            return "TMX Bug 787";
        }

        public override string subtitle()
        {
            return "You should see a map";
        }
    }
}
