using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXIsoMoveLayer : TileDemo
    {
        public TMXIsoMoveLayer()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test-movelayer");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            map.position = new CCPoint(-700, -50);

            CCSize s = map.contentSize;
        }

        public override string title()
        {
            return "TMX Iso Move Layer";
        }

        public override string subtitle()
        {
            return "Trees should be horizontally aligned";
        }
    }
}
