using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXOrthoObjectsTest : TileDemo
    {
        public TMXOrthoObjectsTest()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/ortho-objects");
            addChild(map, -1, 1);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            ////----UXLOG("----> Iterating over all the group objets");
            CCTMXObjectGroup group = map.objectGroupNamed("Object Group 1");
            List<Dictionary<string, string>> objects = group.Objects;

            Dictionary<string, string> dict;
            for (int i = 0; i < objects.Count; i++)
            {
                dict = objects[i];//dynamic_cast<CCStringToStringDictionary*>(*it);

                if (dict == null)
                    break;

                ////----UXLOG("object: %x", dict);
            }

            ////----UXLOG("----> Fetching 1 object by name");
            // CCStringToStringDictionary* platform = group->objectNamed("platform");
            ////----UXLOG("platform: %x", platform);
        }

        public virtual string title()
        {
            return "TMX Ortho object test";
        }

        public virtual void draw()
        {
            CCTMXTiledMap map = (CCTMXTiledMap)getChildByTag(1);
            CCTMXObjectGroup group = map.objectGroupNamed("Object Group 1");

            List<Dictionary<string, string>> objects = group.Objects;
            Dictionary<string, string> dict;

            for (int i = 0; i < objects.Count; i++)
            {


                dict = objects[i];//dynamic_cast<CCStringToStringDictionary*>(*it);

                if (dict == null)
                    break;
                string key = "x";
                int x = int.Parse(dict[key]);
                key = "y";
                int y = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("y"))->getNumber();
                key = "width";
                int width = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("width"))->getNumber();
                key = "height";
                int height = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("height"))->getNumber();

                //glLineWidth(3);

                //ccDrawLine(ccp((float)x, (float)y), ccp((float)(x + width), (float)y));
                //ccDrawLine(ccp((float)(x + width), (float)y), ccp((float)(x + width), (float)(y + height)));
                //ccDrawLine(ccp((float)(x + width), (float)(y + height)), ccp((float)x, (float)(y + height)));
                //ccDrawLine(ccp((float)x, (float)(y + height)), ccp((float)x, (float)y));

                //glLineWidth(1);
            }
        }
        public virtual string subtitle()
        {
            return "You should see a white box around the 3 platforms";
        }
    }
}
