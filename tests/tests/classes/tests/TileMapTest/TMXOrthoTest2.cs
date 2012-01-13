using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    class TMXOrthoTest2 : TileDemo
    {
        readonly int kTagTileMap = 1;
        public TMXOrthoTest2()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test1");
            addChild(map, 0, kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            List<CCNode> pChildrenArray = map.children;
            CCSpriteBatchNode child = null;
            CCObject pObject = null;

            if (pChildrenArray != null && pChildrenArray.Count > 0)
            {
                for (int i = 0; i < pChildrenArray.Count; i++)
                {
                    pObject = pChildrenArray[i];
                    child = (CCSpriteBatchNode)pObject;

                    if (child == null)
                        break;

                    //child.Texture.setAntiAliasTexParameters();
                }

                map.runAction(CCScaleBy.actionWithDuration(2, 0.5f));
            }
        }
        public override string title()
        {
            return "TMX Ortho test2";
        }
    }
}
