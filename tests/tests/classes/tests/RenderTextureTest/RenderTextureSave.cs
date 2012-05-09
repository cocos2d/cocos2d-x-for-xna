using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class RenderTextureSave : RenderTextureTestDemo
    {

        public RenderTextureSave()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            // create a render texture, this is what we are going to draw into
            m_pTarget = CCRenderTexture.renderTextureWithWidthAndHeight((int)s.width, (int)s.height);
            //m_pTarget->retain();
            m_pTarget.position = new CCPoint(s.width / 2, s.height / 2);

            // note that the render texture is a CCNode, and contains a sprite of its texture for convience,
            // so we can just parent it to the scene like any other CCNode
            this.addChild(m_pTarget, -1);

            // create a brush image to draw into the texture with
            m_pBrush = CCSprite.spriteWithFile("Images/fire.png");
            //m_pBrush->retain();
            m_pBrush.Opacity = 20;
            //this->setIsTouchEnabled(true);
            isTouchEnabled = true;

            // Save Image menu
            CCMenuItemFont.FontSize = 16;
            CCMenuItem item1 = CCMenuItemFont.itemFromString("Save Image", this, saveImage);
            CCMenuItem item2 = CCMenuItemFont.itemFromString("Clear", this, clearImage);
            CCMenu menu = CCMenu.menuWithItems(item1, item2);
            this.addChild(menu);
            menu.alignItemsVertically();
            menu.position = new CCPoint(s.width - 80, s.height - 30);
        }

        public override string title()
        {
            return "Touch the screen";
        }

        public override string subtitle()
        {
            return "Press 'Save Image' to create an snapshot of the render texture";
        }

        public void ccTouchesMoved(List<CCObject> touches, CCEvent events)
        {
            foreach (var it in touches)
            {
                CCTouch touch = (CCTouch)it;
                CCPoint start = touch.locationInView(touch.view());
                start = CCDirector.sharedDirector().convertToGL(start);
                CCPoint end = touch.previousLocationInView(touch.view());

                // begin drawing to the render texture
                //m_pTarget->begin();

                // for extra points, we'll draw this smoothly from the last position and vary the sprite's
                // scale/rotation/offset
                float distance = CCPointExtension.ccpDistance(start, end);
                if (distance > 1)
                {
                    int d = (int)distance;
                    Random rand = new Random();

                    for (int i = 0; i < d; i++)
                    {
                        float difx = end.x - start.x;
                        float dify = end.y - start.y;
                        float delta = (float)i / distance;
                        m_pBrush.position = new CCPoint(start.x + (difx * delta), start.y + (dify * delta));
                        m_pBrush.rotation = rand.Next() % 360;
                        float r = (float)(rand.Next() % 50 / 50.0f) + 0.25f;
                        m_pBrush.scale = r;
                        /*m_pBrush->setColor(ccc3(CCRANDOM_0_1() * 127 + 128, 255, 255));*/
                        // Use CCRANDOM_0_1() will cause error when loading libtests.so on android, I don't know why.
                        m_pBrush.Color = new ccColor3B{r = (byte)(rand.Next() % 127 + 128), g = 255, b = 255};
                        // Call visit to draw the brush, don't call draw..
                        m_pBrush.visit();
                    }
                }
            }
            // finish drawing and return context back to the screen
            //m_pTarget->end();
        }

        public void clearImage(CCObject pSender)
        {
            m_pTarget.clear(ccMacros.CCRANDOM_0_1(), ccMacros.CCRANDOM_0_1(), ccMacros.CCRANDOM_0_1(), ccMacros.CCRANDOM_0_1());
        }

        public void saveImage(CCObject pSender)
        {
            int counter = 0;
            char[] str = new char[20];
            //sprintf(str, "image-%d.png", counter);
            //m_pTarget.saveBuffer(CCRenderTexture.tImageFormat.kCCImageFormatPNG, str);
            CCLog.Log("Image saved %s", str);

            counter++;
        }


        private CCRenderTexture m_pTarget;
        private CCSprite m_pBrush;
    }
}
