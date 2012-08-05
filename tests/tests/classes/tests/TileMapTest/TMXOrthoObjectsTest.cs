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

        public override string title()
        {
            return "TMX Ortho object test";
        }

        public override void draw()
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
                int x = ccUtils.ccParseInt(dict[key]);
                key = "y";
                int y = ccUtils.ccParseInt(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("y"))->getNumber();
                key = "width";
                int width = ccUtils.ccParseInt(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("width"))->getNumber();
                key = "height";
                int height = ccUtils.ccParseInt(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("height"))->getNumber();

                //glLineWidth(3);

                ccColor4F color = new ccColor4F(255, 255, 0, 255);

                CCDrawingPrimitives.ccDrawLine(new CCPoint((float)x, (float)y), new CCPoint((float)(x + width), (float)y),color);
                CCDrawingPrimitives.ccDrawLine(new CCPoint((float)(x + width), (float)y), new CCPoint((float)(x + width), (float)(y + height)),color);
                CCDrawingPrimitives.ccDrawLine(new CCPoint((float)(x + width), (float)(y + height)), new CCPoint((float)x, (float)(y + height)),color);
                CCDrawingPrimitives.ccDrawLine(new CCPoint((float)x, (float)(y + height)), new CCPoint((float)x, (float)y),color);

                //glLineWidth(1);
            }
        }
        public override string subtitle()
        {
            return "You should see a white box around the 3 platforms";
        }
    }
}
