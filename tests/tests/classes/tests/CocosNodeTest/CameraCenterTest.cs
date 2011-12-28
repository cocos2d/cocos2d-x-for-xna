using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;

namespace tests
{
    public class CameraCenterTest : TestCocosNodeDemo
    {
        public CameraCenterTest()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            CCSprite sprite;
            CCOrbitCamera orbit;

            // LEFT-TOP
            sprite = new CCSprite();//::node();
            sprite.init();
            addChild(sprite, 0);
            sprite.position = (new CCPoint(s.width / 5 * 1, s.height / 5 * 1));
            sprite.Color = (new ccColor3B(Color.Red));
            sprite.setTextureRect(new CCRect(0, 0, 120, 50));
            orbit = CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 0);
            //sprite.runAction(CCRepeatForever.actionWithAction( orbit));
            //		[sprite setAnchorPoint: CCPointMake(0,1));



            // LEFT-BOTTOM
            sprite = new CCSprite();//::node();
            sprite.init();
            addChild(sprite, 0, 40);
            sprite.position = (new CCPoint(s.width / 5 * 1, s.height / 5 * 4));
            sprite.Color = new ccColor3B(Color.Blue);
            sprite.setTextureRect( new CCRect(0, 0, 120, 50));
            orbit = CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 0);
            //sprite.runAction(CCRepeatForever.actionWithAction( orbit ));
            //		[sprite setAnchorPoint: CCPointMake(0,0));


            // RIGHT-TOP
            sprite = new CCSprite();//::node();
            sprite.init();
            addChild(sprite, 0);
            sprite.position = (new CCPoint(s.width / 5 * 4, s.height / 5 * 1));
            sprite.Color = new ccColor3B(Color.Yellow);
            sprite.setTextureRect (new CCRect(0, 0, 120, 50));
            orbit = CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 0);
            //sprite.runAction(CCRepeatForever.actionWithAction(orbit));
            //		[sprite setAnchorPoint: CCPointMake(1,1));


            // RIGHT-BOTTOM
            sprite = new CCSprite();//::node();
            sprite.init();
            addChild(sprite, 0, 40);
            sprite.position = (new CCPoint(s.width / 5 * 4, s.height / 5 * 4));
            sprite.Color = new ccColor3B(Color.Green);
            sprite.setTextureRect(new CCRect(0, 0, 120, 50));
            orbit = CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 0);
            // sprite.runAction(CCRepeatForever.actionWithAction(orbit));
            //		[sprite setAnchorPoint: CCPointMake(1,0));

            // CENTER
            sprite = new CCSprite();
            sprite.init();
            addChild(sprite, 0, 40);
            sprite.position = (new CCPoint(s.width / 2, s.height / 2));
            sprite.Color = new ccColor3B(Color.White);
            sprite.setTextureRect(new CCRect(0, 0, 120, 50));
            orbit = CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 0);
            // sprite.runAction(CCRepeatForever.actionWithAction(orbit));
            //		[sprite setAnchorPoint: CCPointMake(0.5f, 0.5f));
        }

        public override string title()
        {
            return "Camera Center test";
        }

        public override string subtitle()
        {
            return "Sprites should rotate at the same speed";
        }
    }
}
