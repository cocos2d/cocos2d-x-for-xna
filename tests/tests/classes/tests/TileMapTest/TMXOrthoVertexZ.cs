using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class TMXOrthoVertexZ : TileDemo
    {
        CCSprite m_tamara;

        public TMXOrthoVertexZ()
        {
            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test-vertexz");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            // because I'm lazy, I'm reusing a tile as an sprite, but since this method uses vertexZ, you
            // can use any CCSprite and it will work OK.
            CCTMXLayer layer = map.layerNamed("trees");
            m_tamara = layer.tileAt(new CCPoint(0, 11));
            CCLog.Log("{0} vertexZ: {1}", m_tamara, m_tamara.vertexZ);

            CCActionInterval move = CCMoveBy.actionWithDuration(10, CCPointExtension.ccpMult(new CCPoint(400, 450), 1 / CCDirector.sharedDirector().ContentScaleFactor));
            CCFiniteTimeAction back = move.reverse();
            CCFiniteTimeAction seq = CCSequence.actions(move, back);
            m_tamara.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));

            schedule(repositionSprite);
        }

        void repositionSprite(float dt)
        {
            // tile height is 101x81
            // map size: 12x12
            CCPoint p = m_tamara.positionInPixels;
            m_tamara.vertexZ = -((p.y + 81) / 81);
        }

        public override void onEnter()
        {
            base.onEnter();

            // TIP: 2d projection should be used
            //CCDirector.sharedDirector().Projection = ccDirectorProjection.kCCDirectorProjection2D;
        }

        public override void onExit()
        {
            // At exit use any other projection. 
            //	CCDirector::sharedDirector()->setProjection:kCCDirectorProjection3D);
            base.onExit();
        }

        public override string title()
        {
            return "TMX Ortho vertexZ";
        }

        public override string subtitle()
        {
            return "Sprite should hide behind the trees";
        }
    }
}
