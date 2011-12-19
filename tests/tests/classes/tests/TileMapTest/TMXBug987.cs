using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class TMXBug987 : TileDemo
    {
        public TMXBug987()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test6");
            addChild(map, 0, 1);

            CCSize s1 = map.contentSize;
            Debug.WriteLine("ContentSize: %f, %f", s1.width, s1.height);

            List<CCNode> childs = map.children;
            CCTMXLayer pNode;
            foreach (var item in childs)
            {
                pNode = (CCTMXLayer)item;
                if (pNode == null)
                {
                    break;
                }
                pNode.Texture.setAntiAliasTexParameters();
            }

            map.anchorPoint = new CCPoint(0, 0);
            CCTMXLayer layer = map.layerNamed("Tile Layer 1");
            layer.setTileGID(3, new CCPoint(2, 2));
        }

        public virtual string title()
        {
            return "TMX Bug 987";
        }

        public virtual string subtitle()
        {
            return "You should see an square";
        }
    }
}
