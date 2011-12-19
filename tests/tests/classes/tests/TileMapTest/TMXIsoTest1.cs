using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXIsoTest1 : TileDemo
    {
        public TMXIsoTest1()
        {
            CCLayerColor color = CCLayerColor.layerWithColor(new ccColor4B(64, 64, 64, 255));
            addChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test11");
            addChild(map, 0, 1);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            //map.setAnchorPoint(ccp(0.5f, 0.5f));
        }
        public override string title()
        {
            return "TMX Isometric test + anchorPoint";
        }
    }
}
