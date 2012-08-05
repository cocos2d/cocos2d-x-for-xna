using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class TMXGIDObjectsTest : TileDemo
    {

        public TMXGIDObjectsTest()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/test-object-layer");
            addChild(map, -1, 1);

            CCSize s = map.contentSize;
            CCLog.Log("Contentsize: %f, %f", s.width, s.height);
            CCLog.Log("----> Iterating over all the group objets");
        }

        public override string title()
        {
            return "TMX GID objects";
        }

        public virtual string subtitle()
        {
            return "Tiles are created from an object group";
        }

        public virtual void draw()
        {
            CCTMXTiledMap map = (CCTMXTiledMap)getChildByTag(1);
            CCTMXObjectGroup group = map.objectGroupNamed("Object Layer 1");

            List<Dictionary<string, string>> array = group.Objects;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < array.Count; i++)
            {
                dict = array[i];
                if (dict == null)
                {
                    break;
                }

                string key = "x";
                int x = ccUtils.ccParseInt(dict[key]);
                key = "y";
                int y = ccUtils.ccParseInt(dict[key]);
                key = "width";
                int width = ccUtils.ccParseInt(dict[key]);
                key = "height";
                int height = ccUtils.ccParseInt(dict[key]);

                //glLineWidth(3);

                //ccDrawLine(ccp(x, y), ccp(x + width, y));
                //ccDrawLine(ccp(x + width, y), ccp(x + width, y + height));
                //ccDrawLine(ccp(x + width,y + height), ccp(x,y + height));
                //ccDrawLine(ccp(x,y + height), ccp(x,y));

                //glLineWidth(1);
            }
        }
    }
}
