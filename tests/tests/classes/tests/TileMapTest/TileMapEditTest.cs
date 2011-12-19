using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileMapEditTest : TileDemo
    {
        string s_TilesPng = "TileMaps/tiles.png";
        string s_LevelMapTga = "TileMaps/levelmap.tga";
        public TileMapEditTest()
        {
            //        CCTileMapAtlas map = CCTileMapAtlas.tileMapAtlasWithTileFile(s_TilesPng, s_LevelMapTga, 16, 16);
            //// Create an Aliased Atlas
            //map.Texture.setAliasTexParameters();

            //CCSize s = map.contentSize;
            //////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            //// If you are not going to use the Map, you can free it now
            //// [tilemap releaseMap);
            //// And if you are going to use, it you can access the data with:
            //schedule(TileMapEditTest.updateMap), 0.2f);//:@selector(updateMap:) interval:0.2f);

            //addChild(map, 0, kTagTileMap);

            //map->setAnchorPoint( ccp(0, 0) );
            //map->setPosition( ccp(-20,-200) );
            //    }
            //    public virtual string title()
            //    {

            //    }
            //    public void updateMap(float dt)
            //    {

            //    }
        }
    }
}
