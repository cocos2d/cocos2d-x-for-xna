using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;

namespace tests
{
    public class TMXIsoTest : TileDemo
    {

        public TMXIsoTest()
        {
            CCLayerColor color = CCLayerColor.layerWithColor(new ccColor4B(new Color(64, 64, 64, 255)));
            addChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test01");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            // move map to the center of the screen
            CCSize ms = map.MapSize;
            CCSize ts = map.TileSize;
            map.runAction(CCMoveTo.actionWithDuration(1.0f, new CCPoint(-ms.width * ts.width / 2, -ms.height * ts.height / 2)));
        }
        public override string title()
        {
            return "TMX Isometric test 0";
        }
    }
}
