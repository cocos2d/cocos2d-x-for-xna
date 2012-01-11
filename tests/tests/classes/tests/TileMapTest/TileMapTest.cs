using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileMapTest : TileDemo
    {
        public TileMapTest()
        {
            //CCTileMapAtlas map = CCTileMapAtlas.tileMapAtlasWithTileFile(s_TilesPng,  s_LevelMapTga, 16, 16);
            //// Convert it to "alias" (GL_LINEAR filtering)
            //map->getTexture()->setAntiAliasTexParameters();

            //CCSize s = map->getContentSize();
            //////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            //// If you are not going to use the Map, you can free it now
            //// NEW since v0.7
            //map->releaseMap();

            //addChild(map, 0, kTagTileMap);

            //map->setAnchorPoint( ccp(0, 0.5f) );

            //CCScaleBy *scale = CCScaleBy::actionWithDuration(4, 0.8f);
            //CCActionInterval *scaleBack = scale->reverse();

            //CCFiniteTimeAction* seq = CCSequence::actions(scale, scaleBack, NULL);

            //map->runAction(CCRepeatForever::actionWithAction((CCActionInterval *)seq));
        }

        public override string title()
        {
            return "TileMapAtlas";
        }
    }
}
