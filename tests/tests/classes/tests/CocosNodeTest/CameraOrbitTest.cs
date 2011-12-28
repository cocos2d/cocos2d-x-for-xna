using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class CameraOrbitTest : TestCocosNodeDemo
    {
        public CameraOrbitTest()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite p = CCSprite.spriteWithFile(TestResource.s_back3);
            addChild(p, 0);
            p.position = (new CCPoint(s.width / 2, s.height / 2));
            p.Opacity = 128;

            CCSprite sprite;
            CCOrbitCamera orbit;
            CCCamera cam;
            CCSize ss;

            // LEFT
            s = p.contentSize;
            sprite = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            sprite.scale = (0.5f);
            p.addChild(sprite, 0);
            sprite.position = (new CCPoint(s.width / 4 * 1, s.height / 2));
            cam = sprite.Camera;
            orbit = CCOrbitCamera.actionWithDuration(2, 1, 0, 0, 360, 0, 0);
            //sprite.runAction(CCRepeatForever.actionWithAction(orbit));

            // CENTER
            sprite = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            sprite.scale = 1.0f;
            p.addChild(sprite, 0);
            sprite.position = new CCPoint(s.width / 4 * 2, s.height / 2);
            orbit = CCOrbitCamera.actionWithDuration(2, 1, 0, 0, 360, 45, 0);
            //sprite.runAction(CCRepeatForever.actionWithAction(orbit));


            // RIGHT
            sprite = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            sprite.scale = 2.0f;
            p.addChild(sprite, 0);
            sprite.position = new CCPoint(s.width / 4 * 3, s.height / 2);
            ss = sprite.contentSize;
            orbit = CCOrbitCamera.actionWithDuration(2, 1, 0, 0, 360, 90, -45);
            //sprite.runAction(CCRepeatForever.actionWithAction(orbit));


            // PARENT
            orbit = CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 90);
            //p.runAction(CCRepeatForever.actionWithAction(orbit));
            scale = 1;
        }

        public override void onEnter()
        {
            base.onEnter();
            CCDirector.sharedDirector().Projection = (ccDirectorProjection.kCCDirectorProjection3D);
        }

        public override void onExit()
        {
            CCDirector.sharedDirector().Projection = ccDirectorProjection.kCCDirectorProjection2D;
            base.onExit();
        }

        public override string title()
        {
            return "Camera Orbit test";
        }
    }
}
