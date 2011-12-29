using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RenderTextureTest : RenderTextureTestDemo
    {
        public RenderTextureTest()
        {
            //if (CCConfiguration.sharedConfiguration().getGlesVersion() <= GLES_VER_1_0)
            //{
            //    CCMessageBox("The Opengl ES version is lower than 1.1, and the test may not run correctly.", "Cocos2d-x Hint");
            //    return;
            //}

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // create a render texture, this is what we're going to draw into
            m_target = CCRenderTexture.renderTextureWithWidthAndHeight((int)s.width, (int)s.height);

            if (null == m_target)
            {
                return;
            }

            m_target.position = new CCPoint(s.width / 2, s.height / 2);

            // note that the render texture is a cocosnode, and contains a sprite of it's texture for convience,
            // so we can just parent it to the scene like any other cocos node
            addChild(m_target, 1);

            // create a brush image to draw into the texture with
            m_brush = CCSprite.spriteWithFile("Images/stars.png");
            //m_brush.retain();

            ccBlendFunc bf = new ccBlendFunc { src = 1, dst = 0x0303 };
            m_brush.BlendFunc = bf;
            m_brush.Opacity = 20;
            isTouchEnabled = true;
        }

        public override void ccTouchesMoved(List<CCTouch> touches, CCEvent events)
        {
            foreach (var it in touches)
            {
                CCTouch touch = it;
                CCPoint start = touch.locationInView(touch.view());
                start = CCDirector.sharedDirector().convertToGL(start);
                CCPoint end = touch.previousLocationInView(touch.view());
                end = CCDirector.sharedDirector().convertToGL(end);

                // begin drawing to the render texture
                //m_target->begin();

                // for extra points, we'll draw this smoothly from the last position and vary the sprite's
                // scale/rotation/offset
                float distance = CCPointExtension.ccpDistance(start, end);
                if (distance > 1)
                {
                    int d = (int)distance;
                    Random rand = new Random();
                    ;
                    for (int i = 0; i < d; i++)
                    {
                        float difx = end.x - start.x;
                        float dify = end.y - start.y;
                        float delta = (float)i / distance;
                        m_brush.position = new CCPoint(start.x + (difx * delta), start.y + (dify * delta));
                        m_brush.rotation = rand.Next() % 360;
                        float r = ((float)(rand.Next() % 50) / 50.0f) + 0.25f;
                        m_brush.scale = r;
                        // Call visit to draw the brush, don't call draw..
                        m_brush.visit();
                    }
                }
                // finish drawing and return context back to the screen
                //m_target->end(false);
            }
        }


        public override void ccTouchesEnded(List<CCTouch> touches, CCEvent events)
        {
#if CC_ENABLE_CACHE_TEXTTURE_DATA

	CCSetIterator it;
	CCTouch* touch;

	for( it = touches->begin(); it != touches->end(); it++) 
	{
		touch = (CCTouch*)(*it);

		if(!touch)
			break;

		CCPoint location = touch->locationInView(touch->view());

		location = CCDirector::sharedDirector()->convertToGL(location);

		m_brush->setPosition(location);
		m_brush->setRotation( rand()%360 );
	}

	m_target->begin();
	m_brush->visit();
	m_target->end(true);
#endif
        }

        private CCRenderTexture m_target;
        private CCSprite m_brush;
    }
}
