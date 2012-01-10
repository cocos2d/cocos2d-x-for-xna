using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXOrthoTest3 : TileDemo
    {
        public TMXOrthoTest3()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test3");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s = map.contentSize;

            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);
            foreach (var pObject in map.children)
            {
                CCSpriteBatchNode child = (CCSpriteBatchNode)pObject;

                if (child == null)
                    break;

                child.Texture.setAntiAliasTexParameters();
            }

            map.scale = 0.2f;
            map.anchorPoint = new CCPoint(0.5f, 0.5f);
        }

        public override string title()
        {
            return "TMX anchorPoint test";
        }
    }
}
