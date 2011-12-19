using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXOrthoMoveLayer : TileDemo
    {
        public TMXOrthoMoveLayer()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test-movelayer");
            addChild(map, 0, 1);

            CCSize s = map.contentSize;
        }

        public virtual string title()
        {
            return "TMX Ortho Move Layer";
        }

        public virtual string subtitle()
        {
            return "Trees should be horizontally aligned";
        }
    }
}
