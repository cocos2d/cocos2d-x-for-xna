using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class CameraZoomTest : TestCocosNodeDemo
    {
        float m_z;
        public CameraZoomTest()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite;
            CCCamera cam;

            // LEFT
            sprite = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            addChild(sprite, 0);
            sprite.position = (new CCPoint(s.width / 4 * 1, s.height / 2));
            cam = sprite.Camera;
            cam.setEyeXYZ(0, 0, 415);

            // CENTER
            sprite = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            addChild(sprite, 0, 40);
            sprite.position = (new CCPoint(s.width / 4 * 2, s.height / 2));
            //		cam = [sprite camera);
            //		[cam setEyeX:0 eyeY:0 eyeZ:415/2);

            // RIGHT
            sprite = CCSprite.spriteWithFile(TestResource.s_pPathGrossini);
            addChild(sprite, 0, 20);
            sprite.position = (new CCPoint(s.width / 4 * 3, s.height / 2));
            //		cam = [sprite camera);
            //		[cam setEyeX:0 eyeY:0 eyeZ:-485);
            //		[cam setCenterX:0 centerY:0 centerZ:0);

            m_z = 0;
            base.sheduleUpdate();
        }

        public void update(float dt)
        {
            CCNode sprite;
            CCCamera cam;

            m_z += dt * 100;

            sprite = getChildByTag(20);
            cam = sprite.Camera;
            cam.setEyeXYZ(0, 0, m_z);

            sprite = getChildByTag(40);
            cam = sprite.Camera;
            cam.setEyeXYZ(0, 0, m_z);
        }

        public override void onEnter()
        {
            base.onEnter();

            CCDirector.sharedDirector().Projection = (ccDirectorProjection.kCCDirectorProjection3D);
        }

        public override void onExit()
        {
            CCDirector.sharedDirector().Projection = (ccDirectorProjection.kCCDirectorProjection2D);
            base.onExit();
        }

        public override string title()
        {
            return "Camera Zoom test";
        }
    }
}
