using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TileMapTest
    {
        public static CCLayer nextTileMapAction() 
        {
            CCLayer pLayer = createTileMapLayer(1);
            return pLayer;
        }

        public static CCLayer createTileMapLayer(int index)
        {
            switch (index)
            {
                case 1:
                    return new TMXOrthoZorder();
                default:
                    return new CCLayer();
            }
        }

        public class TMXOrthoZorder : TileDemo
        {
            CCSprite m_tamara;
            public TMXOrthoZorder()
            {
                CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test-zorder");
                base.addChild(map, 0, 1);

                CCSize s = map.contentSize;
                ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

                m_tamara = CCSprite.spriteWithFile("Images/grossinis_sister1");
                map.addChild(m_tamara, map.children.Count);
                m_tamara.anchorPoint = new CCPoint(0.5f, 0);

                //CCActionInterval move = CCMoveBy.actionWithDuration(10, ccpMult(ccp(400,450), 1/CC_CONTENT_SCALE_FACTOR() ));
                //CCActionInterval back = move->reverse();
                //CCFiniteTimeAction* seq = CCSequence::actions(move, back,NULL);
                //m_tamara->runAction( CCRepeatForever::actionWithAction((CCActionInterval*)seq));

                schedule((this.repositionSprite));
            }

            public virtual string title()
            {
                return "";
            }
            public virtual string subtitle()
            {
                return "";
            }

            void repositionSprite(float dt)
            {
                CCPoint p = m_tamara.positionInPixels;
                CCNode map = getChildByTag(1);

                // there are only 4 layers. (grass and 3 trees layers)
                // if tamara < 81, z=4
                // if tamara < 162, z=3
                // if tamara < 243,z=2

                // -10: customization for this particular sample
                int newZ = 4 - (int)((p.y - 10) / 81);
                newZ = Math.Max(newZ, 0);

                map.reorderChild(m_tamara, newZ);
            }


        }
        public class TileDemo : CCLayer
        {
            public TileDemo()
            {
            }

            public virtual string title()
            {
                return "No tile";
            }

            public virtual string subtitle()
            {
                return "drag the screen";
            }

            public virtual void onEnter()
            {
                base.onEnter();
            }
        }
    }
}
