using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeZVertex : SpriteTestDemo
    {
        int m_dir;
        float m_time;
        public override void onEnter()
        {
            base.onEnter();

            // TIP: don't forget to enable Alpha test
            //glEnable(GL_ALPHA_TEST);
            //glAlphaFunc(GL_GREATER, 0.0f);

            CCDirector.sharedDirector().Projection = ccDirectorProjection.kCCDirectorProjection3D;
        }

        public override void onExit()
        {
            //glDisable(GL_ALPHA_TEST);
            CCDirector.sharedDirector().Projection = (ccDirectorProjection.kCCDirectorProjection2D);
            base.onExit();
        }

        public SpriteBatchNodeZVertex()
        {
            //
            // This test tests z-order
            // If you are going to use it is better to use a 3D projection
            //
            // WARNING:
            // The developer is resposible for ordering it's sprites according to it's Z if the sprite has
            // transparent parts.
            //


            CCSize s = CCDirector.sharedDirector().getWinSize();
            float step = s.width / 12;

            // small capacity. Testing resizing.
            // Don't use capacity=1 in your real game. It is expensive to resize the capacity
            CCSpriteBatchNode batch = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 1);
            // camera uses the center of the image as the pivoting point
            batch.contentSize = new CCSize(s.width, s.height);
            batch.anchorPoint = (new CCPoint(0.5f, 0.5f));
            batch.position = (new CCPoint(s.width / 2, s.height / 2));


            addChild(batch, 0, (int)kTags.kTagSpriteBatchNode);

            for (int i = 0; i < 5; i++)
            {
                CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 0, 121 * 1, 85, 121));
                sprite.position = (new CCPoint((i + 1) * step, s.height / 2));
                sprite.vertexZ = (10 + i * 40);
                batch.addChild(sprite, 0);

            }

            for (int i = 5; i < 11; i++)
            {
                CCSprite sprite = CCSprite.spriteWithTexture(batch.Texture, new CCRect(85 * 1, 121 * 0, 85, 121));
                sprite.position = (new CCPoint((i + 1) * step, s.height / 2));
                sprite.vertexZ = 10 + (10 - i) * 40;
                batch.addChild(sprite, 0);
            }

            batch.runAction(CCOrbitCamera.actionWithDuration(10, 1, 0, 0, 360, 0, 0));
        }

        public override string title()
        {
            return "SpriteBatchNode: openGL Z vertex";
        }
    }
}
