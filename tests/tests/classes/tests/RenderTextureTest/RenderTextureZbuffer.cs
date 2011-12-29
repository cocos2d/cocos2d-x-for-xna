using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RenderTextureZbuffer : RenderTextureTestDemo
    {
        public RenderTextureZbuffer()
        {
            //this->setIsTouchEnabled(true);
            isTouchEnabled = true;
            CCSize size = CCDirector.sharedDirector().getWinSize();
            CCLabelTTF label = CCLabelTTF.labelWithString("vertexZ = 50", "Marker Felt", 64);
            label.position = new CCPoint(size.width / 2, size.height * 0.25f);
            this.addChild(label);

            CCLabelTTF label2 = CCLabelTTF.labelWithString("vertexZ = 0", "Marker Felt", 64);
            label2.position = new CCPoint(size.width / 2, size.height * 0.5f);
            this.addChild(label2);

            CCLabelTTF label3 = CCLabelTTF.labelWithString("vertexZ = -50", "Marker Felt", 64);
            label3.position = new CCPoint(size.width / 2, size.height * 0.75f);
            this.addChild(label3);

            label.vertexZ = 50;
            label2.vertexZ = 0;
            label3.vertexZ = -50;

#warning "CCSpriteFrameCache is not exist! "
            //CCSpriteFrameCache.sharedSpriteFrameCache().addSpriteFramesWithFile("Images/bugs/circle.plist");
            mgr = CCSpriteBatchNode.batchNodeWithFile("Images/bugs/circle.png", 9);
            this.addChild(mgr);
            sp1 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp2 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp3 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp4 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp5 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp6 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp7 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp8 = CCSprite.spriteWithSpriteFrameName("circle.png");
            sp9 = CCSprite.spriteWithSpriteFrameName("circle.png");

            mgr.addChild(sp1, 9);
            mgr.addChild(sp2, 8);
            mgr.addChild(sp3, 7);
            mgr.addChild(sp4, 6);
            mgr.addChild(sp5, 5);
            mgr.addChild(sp6, 4);
            mgr.addChild(sp7, 3);
            mgr.addChild(sp8, 2);
            mgr.addChild(sp9, 1);

            sp1.vertexZ = 400;
            sp2.vertexZ = 300;
            sp3.vertexZ = 200;
            sp4.vertexZ = 100;
            sp5.vertexZ = 0;
            sp6.vertexZ = -100;
            sp7.vertexZ = -200;
            sp8.vertexZ = -300;
            sp9.vertexZ = -400;

            sp9.scale = 2;
            sp9.Color = new ccColor3B { r = 255, g = 255, b = 0 };
        }

        public override void ccTouchesMoved(List<CCTouch> touches, CCEvent events)
        {
            //CCSetIterator iter;
            //CCTouch *touch;
            //for (iter = touches->begin(); iter != touches->end(); ++iter)
            //{
            foreach (var touch in touches)
            {
                CCPoint location = touch.locationInView(touch.view());

                location = CCDirector.sharedDirector().convertToGL(location);
                sp1.position = location;
                sp2.position = location;
                sp3.position = location;
                sp4.position = location;
                sp5.position = location;
                sp6.position = location;
                sp7.position = location;
                sp8.position = location;
                sp9.position = location;
            }
            //touch = (CCTouch *)(*iter);
            //}
        }

        public override void ccTouchesBegan(List<CCTouch> touches, CCEvent events)
        {
            //CCSetIterator iter;
            //CCTouch touch;
            //for (iter = touches.begin(); iter != touches.end(); ++iter)
            //{
            foreach (var touch in touches)
            {
                CCPoint location = touch.locationInView(touch.view());

                location = CCDirector.sharedDirector().convertToGL(location);
                sp1.position = location;
                sp2.position = location;
                sp3.position = location;
                sp4.position = location;
                sp5.position = location;
                sp6.position = location;
                sp7.position = location;
                sp8.position = location;
                sp9.position = location;
            }
            //}
        }

        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent events)
        {
            this.renderScreenShot();
        }

        public override string title()
        {
            return "Testing Z Buffer in Render Texture";
        }

        public override string subtitle()
        {
            return "Touch screen. It should be green";
        }

        public void renderScreenShot()
        {
        }

        private cocos2d.CCSpriteBatchNode mgr;

        private cocos2d.CCSprite sp1;
        private cocos2d.CCSprite sp2;
        private cocos2d.CCSprite sp3;
        private cocos2d.CCSprite sp4;
        private cocos2d.CCSprite sp5;
        private cocos2d.CCSprite sp6;
        private cocos2d.CCSprite sp7;
        private cocos2d.CCSprite sp8;
        private cocos2d.CCSprite sp9;
    }
}
