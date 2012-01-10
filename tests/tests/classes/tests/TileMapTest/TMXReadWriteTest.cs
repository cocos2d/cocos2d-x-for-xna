using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    class TMXReadWriteTest : TileDemo
    {
        enum SIDType
        {
            SID_UPDATECOL = 100,
            SID_REPAINTWITHGID,
            SID_REMOVETILES
        }

        int m_gid;
        int m_gid2;

        public TMXReadWriteTest()
        {
            m_gid = 0;

            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test2");
            addChild(map, 0, TileMapTestScene.kTagTileMap);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);


            CCTMXLayer layer = map.layerNamed("Layer 0");
            layer.Texture.setAntiAliasTexParameters();

            map.scale = 1;

            CCSprite tile0 = layer.tileAt(new CCPoint(1, 63));
            CCSprite tile1 = layer.tileAt(new CCPoint(2, 63));
            CCSprite tile2 = layer.tileAt(new CCPoint(3, 62));//ccp(1,62));
            CCSprite tile3 = layer.tileAt(new CCPoint(2, 62));
            tile0.anchorPoint = new CCPoint(0.5f, 0.5f);
            tile1.anchorPoint = new CCPoint(0.5f, 0.5f);
            tile2.anchorPoint = new CCPoint(0.5f, 0.5f);
            tile3.anchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval move = CCMoveBy.actionWithDuration(0.5f, new CCPoint(0, 160));
            CCActionInterval rotate = CCRotateBy.actionWithDuration(2, 360);
            CCActionInterval scale = CCScaleBy.actionWithDuration(2, 5);
            CCActionInterval opacity = CCFadeOut.actionWithDuration(2);
            CCActionInterval fadein = CCFadeIn.actionWithDuration(2);
            CCActionInterval scaleback = CCScaleTo.actionWithDuration(1, 1);
            CCActionInstant finish = CCCallFuncN.actionWithTarget(this, removeSprite);
            CCFiniteTimeAction seq0 = CCSequence.actions(move, rotate, scale, opacity, fadein, scaleback, finish);
            CCActionInterval seq1 = (CCActionInterval)(seq0.copy());
            CCActionInterval seq2 = (CCActionInterval)(seq0.copy());
            CCActionInterval seq3 = (CCActionInterval)(seq0.copy());

            tile0.runAction(seq0);
            tile1.runAction(seq1);
            tile2.runAction(seq2);
            tile3.runAction(seq3);


            m_gid = layer.tileGIDAt(new CCPoint(0, 63));
            ////----UXLOG("Tile GID at:(0,63) is: %d", m_gid);

            schedule(updateCol, 2.0f);
            schedule(repaintWithGID, 2.0f);
            schedule(removeTiles, 1.0f);

            ////----UXLOG("++++atlas quantity: %d", layer->textureAtlas()->getTotalQuads());
            ////----UXLOG("++++children: %d", layer->getChildren()->count() );

            m_gid2 = 0;
        }

        public void removeSprite(CCNode sender)
        {
            ////----UXLOG("removing tile: %x", sender);
            CCNode p = ((CCNode)sender).parent;

            if (p != null)
            {
                p.removeChild((CCNode)sender, true);
            }

            //////----UXLOG("atlas quantity: %d", p->textureAtlas()->totalQuads());
        }

        void updateCol(float dt)
        {
            CCTMXTiledMap map = (CCTMXTiledMap)getChildByTag(TileMapTestScene.kTagTileMap);
            CCTMXLayer layer = (CCTMXLayer)map.getChildByTag(0);

            ////----UXLOG("++++atlas quantity: %d", layer->textureAtlas()->getTotalQuads());
            ////----UXLOG("++++children: %d", layer->getChildren()->count() );


            CCSize s = layer.LayerSize;

            for (int y = 0; y < s.height; y++)
            {
                layer.setTileGID(m_gid2, new CCPoint((float)3, (float)y));
            }

            m_gid2 = (m_gid2 + 1) % 80;
        }

        void repaintWithGID(float dt)
        {
            //	[self unschedule:_cmd);

            CCTMXTiledMap map = (CCTMXTiledMap)getChildByTag(TileMapTestScene.kTagTileMap);
            CCTMXLayer layer = (CCTMXLayer)map.getChildByTag(0);

            CCSize s = layer.LayerSize;
            for (int x = 0; x < s.width; x++)
            {
                int y = (int)s.height - 1;
                int tmpgid = layer.tileGIDAt(new CCPoint((float)x, (float)y));
                layer.setTileGID(tmpgid + 1, new CCPoint((float)x, (float)y));
            }
        }

        void removeTiles(float dt)
        {
            unschedule(removeTiles);

            CCTMXTiledMap map = (CCTMXTiledMap)getChildByTag(TileMapTestScene.kTagTileMap);
            CCTMXLayer layer = (CCTMXLayer)map.getChildByTag(0);
            CCSize s = layer.LayerSize; ;

            for (int y = 0; y < s.height; y++)
            {
                layer.removeTileAt(new CCPoint(5.0f, (float)y));
            }
        }
    }
}
