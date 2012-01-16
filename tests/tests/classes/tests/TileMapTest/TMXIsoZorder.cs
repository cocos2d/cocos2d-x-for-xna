using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXIsoZorder : TileDemo
    {
        string s_pPathSister1 = "Images/grossinis_sister1";
        CCSprite m_tamara;

        public TMXIsoZorder()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test-zorder");
            addChild(map, 0, 1);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);
            map.position = (new CCPoint(-s.width / 2, 0));

            m_tamara = CCSprite.spriteWithFile(s_pPathSister1);
            map.addChild(m_tamara, map.children.Count);
            int mapWidth = (int)(map.MapSize.width * map.TileSize.width);
            m_tamara.positionInPixels = new CCPoint(mapWidth / 2, 0);
            m_tamara.anchorPoint = new CCPoint(0.5f, 0);


            CCActionInterval move = CCMoveBy.actionWithDuration(10, new CCPoint(300 * 1 / CCDirector.sharedDirector().ContentScaleFactor, 250 * 1 / CCDirector.sharedDirector().ContentScaleFactor));
            CCActionInterval back = (CCActionInterval)move.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(move, back);
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));

            schedule((this.repositionSprite));
        }

        public override string title()
        {
            return "TMX Iso Zorder";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
        }

        public override void onExit()
        {
            unschedule(this.repositionSprite);
            base.onExit();
        }

        public void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.positionInPixels;
            CCNode map = getChildByTag(1);

            // there are only 4 layers. (grass and 3 trees layers)
            // if tamara < 48, z=4
            // if tamara < 96, z=3
            // if tamara < 144,z=2

            int newZ = 4 - (int)(p.y / 48);
            newZ = Math.Max(newZ, 0);

            map.reorderChild(m_tamara, newZ);
        }
    }
}
