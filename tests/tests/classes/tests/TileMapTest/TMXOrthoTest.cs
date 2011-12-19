using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TMXOrthoTest : TileDemo
    {
        public TMXOrthoTest()
        {
            //
            // Test orthogonal with 3d camera and anti-alias textures
            //
            // it should not flicker. No artifacts should appear
            //
            //CCLayerColor* color = CCLayerColor::layerWithColor( ccc4(64,64,64,255) );
            //addChild(color, -1);

            CCTMXTiledMap map = CCTMXTiledMap.tiledMapWithTMXFile("TileMaps/orthogonal-test2");
            addChild(map, 0, 1);

            CCSize s = map.contentSize;
            ////----UXLOG("ContentSize: %f, %f", s.width,s.height);

            List<CCNode> pChildrenArray = map.children;
            CCSpriteBatchNode child = null;

            foreach (var item in pChildrenArray)
            {
                child = (CCSpriteBatchNode)item;

                if (child == null)
                    break;

                //child.Texture.setAntiAliasTexParameters();
            }

            float x = 0, y = 0, z = 0;
            map.Camera.getEyeXYZ(x, y, z);
            map.Camera.setEyeXYZ(x - 200, y, z + 300);
        }

        public virtual string title()
        {
            return "TMX Orthogonal test";
        }

        public virtual void onEnter()
        {
            base.onEnter();
            CCDirector.sharedDirector().Projection = ccDirectorProjection.CCDirectorProjection3D;
        }

        public virtual void onExit()
        {
            CCDirector.sharedDirector().Projection = ccDirectorProjection.CCDirectorProjection2D;
            base.onExit();
        }
    }
}
