using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXIsoVertexZ : TileDemo
    {
        CCSprite m_tamara;
        public TMXIsoVertexZ()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/iso-test-vertexz");
            addChild(map, 0, 1);

            CCSize s = map.contentSize;
            map.position = new CCPoint(-s.width / 2, 0);
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            // because I'm lazy, I'm reusing a tile as an sprite, but since this method uses vertexZ, you
            // can use any CCSprite and it will work OK.
            CCTMXLayer layer = map.layerNamed("Trees");
            m_tamara = layer.tileAt(new CCPoint(29, 29));

            CCActionInterval move = CCMoveBy.actionWithDuration(10, new CCPoint(300 * 1 / CCDirector.sharedDirector().ContentScaleFactor, 250 * 1 / CCDirector.sharedDirector().ContentScaleFactor));
            CCActionInterval back = (CCActionInterval)move.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(move, back);
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));

            schedule(repositionSprite);
        }

        public override string title()
        {
            return "TMX Iso VertexZ";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
        }

        public void repositionSprite(float dt)
        {
            CCPoint p = m_tamara.positionInPixels;
            m_tamara.vertexZ = (-((p.y + 32) / 16));
        }

        public virtual void onEnter()
        {
            base.onEnter();

            // TIP: 2d projection should be used
            CCDirector.sharedDirector().Projection = 0;
        }

        public override void onExit()
        {
            base.onExit();
        }
    }
}
