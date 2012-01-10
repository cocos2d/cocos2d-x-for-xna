using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXOrthoTest4 : TileDemo
    {
        public TMXOrthoTest4()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test4");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s1 = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s1.width,s1.height);

            foreach (var pObject in map.children)
            {
                CCSpriteBatchNode child = (CCSpriteBatchNode)pObject;

                if (child == null)
                    break;

                child.Texture.setAntiAliasTexParameters();
            }

            map.anchorPoint = new CCPoint(0, 0);

            CCTMXLayer layer = map.layerNamed("Layer 0");
            CCSize s = layer.LayerSize;

            CCSprite sprite = layer.tileAt(new CCPoint(0, 0));
            sprite.scale = 2;
            sprite = layer.tileAt(new CCPoint(s.width - 1, 0));
            sprite.scale = 2;
            sprite = layer.tileAt(new CCPoint(0, s.height - 1));
            sprite.scale = 2;
            sprite = layer.tileAt(new CCPoint(s.width - 1, s.height - 1));
            sprite.scale = 2;

            schedule(removeSprite, 2);

        }

        public void removeSprite(float dt)
        {
            unschedule(removeSprite);

            CCTMXTiledMap map = (CCTMXTiledMap)getChildByTag(TileMapTestScene.kTagTileMap);
            CCTMXLayer layer = map.layerNamed("Layer 0");
            CCSize s = layer.LayerSize;

            CCSprite sprite = layer.tileAt(new CCPoint(s.width - 1, 0));
            layer.removeChild(sprite, true);
        }

        public override string title()
        {
            return "TMX width/height test";
        }
    }
}
