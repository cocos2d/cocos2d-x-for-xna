using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXOrthoZorder : TileDemo
    {
        readonly int kTagTileMap = 1;
        CCSprite m_tamara;

        public TMXOrthoZorder()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test-zorder");
            base.addChild(map, 0, kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            m_tamara = CCSprite.spriteWithFile(TestResource.s_pPathSister1);
            map.addChild(m_tamara, map.children.Count);
            m_tamara.anchorPoint = new CCPoint(0.5f, 0);

            CCActionInterval move = CCMoveBy.actionWithDuration(10, CCPointExtension.ccpMult(new CCPoint(400, 450), 1 / ccMacros.CC_CONTENT_SCALE_FACTOR()));
            CCFiniteTimeAction back = move.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(move, back);
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));

            
            schedule((this.repositionSprite));
        }

        public override string title()
        {
            return "TMX Ortho Zorder";
        }
        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
        }

        //¸´Î»
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
}
