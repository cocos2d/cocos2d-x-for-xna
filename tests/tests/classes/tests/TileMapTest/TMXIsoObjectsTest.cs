using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXIsoObjectsTest : TileDemo
    {
        public TMXIsoObjectsTest()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test-objectgroup");
            addChild(map, -1, 1);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            CCTMXObjectGroup group = map.objectGroupNamed("Object Group 1");

            //UxMutableArray* objects = group->objects();
            List<Dictionary<string, string>> objects = group.Objects;
            //UxMutableDictionary<std::string>* dict;
            Dictionary<string, string> dict;
            //CCMutableArray<CCObject*>::CCMutableArrayIterator it;
            for (int i = 0; i < objects.Count; i++)
            {
                dict = objects[i];

                if (dict == null)
                    break;
            }
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
                int x = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("x"))->getNumber();
                key = "y";
                int y = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("y"))->getNumber();
                key = "width";
                int width = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("width"))->getNumber();
                key = "height";
                int height = int.Parse(dict[key]);//dynamic_cast<NSNumber*>(dict->objectForKey("height"))->getNumber();


                //glLineWidth(3);

                //ccDrawLine( ccp(x,y), ccp(x+width,y) );
                //ccDrawLine( ccp(x+width,y), ccp(x+width,y+height) );
                //ccDrawLine( ccp(x+width,y+height), ccp(x,y+height) );
                //ccDrawLine( ccp(x,y+height), ccp(x,y) );

                //glLineWidth(1);
            }
        }

        public virtual string subtitle()
        {
            return "You need to parse them manually. See bug #810";
        }

        public string title()
        {
            return "TMX Iso object test";
        }

    }
}
