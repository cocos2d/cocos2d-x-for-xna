using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SubTest
    {

        public void removeByTag(int tag)
        {
            switch (subtestNumber)
            {
                case 1:
                case 4:
                case 7:
                    parent.removeChildByTag(tag + 100, true);
                    break;
                case 2:
                case 3:
                case 5:
                case 6:
                case 8:
                case 9:
                    batchNode.removeChildAtIndex(tag, true);
                    //			[batchNode removeChildByTag:tag+100 cleanup:YES];
                    break;
                default:
                    break;
            }
        }

        public CCSprite createSpriteWithTag(int tag)
        {
            Random random = new Random();
            // create 
            CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);

            CCSprite sprite = null;
            switch (subtestNumber)
            {
                case 1:
                    {
                        sprite = CCSprite.spriteWithFile("Images/grossinis_sister1");
                        parent.addChild(sprite, 0, tag + 100);
                        break;
                    }
                case 2:
                case 3:
                    {
                        sprite = CCSprite.spriteWithBatchNode(batchNode, new CCRect(0, 0, 52, 139));
                        batchNode.addChild(sprite, 0, tag + 100);
                        break;
                    }
                case 4:
                    {
                        int idx = (random.Next() * 1400 / 100) + 1;
                        //char str[32] = {0};
                        string str;
                        //sprintf(str, "Images/grossini_dance_%02d.png", idx);
                        str = string.Format("Images/grossini_dance_{0:2d}.png", idx);
                        sprite = CCSprite.spriteWithFile(str);
                        parent.addChild(sprite, 0, tag + 100);
                        break;
                    }
                case 5:
                case 6:
                    {
                        int y, x;
                        int r = (random.Next() * 1400 / 100);

                        y = r / 5;
                        x = r % 5;

                        x *= 85;
                        y *= 121;
                        sprite = CCSprite.spriteWithBatchNode(batchNode, new CCRect(x, y, 85, 121));
                        batchNode.addChild(sprite, 0, tag + 100);
                        break;
                    }

                case 7:
                    {
                        int y, x;
                        int r = (random.Next() * 6400 / 100);

                        y = r / 8;
                        x = r % 8;

                        //char str[40] = {0};
                        string str;
                        //sprintf(str, "Images/sprites_test/sprite-%d-%d.png", x, y);
                        str = string.Format("Images/sprites_test/sprite-{0:D}-{1:D}.png", x, y);
                        sprite = CCSprite.spriteWithFile(str);
                        parent.addChild(sprite, 0, tag + 100);
                        break;
                    }

                case 8:
                case 9:
                    {
                        int y, x;
                        int r = (random.Next() * 6400 / 100);

                        y = r / 8;
                        x = r % 8;

                        x *= 32;
                        y *= 32;
                        sprite = CCSprite.spriteWithBatchNode(batchNode, new CCRect(x, y, 32, 32));
                        batchNode.addChild(sprite, 0, tag + 100);
                        break;
                    }

                default:
                    break;
            }

            CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_Default);

            return sprite;
        }
        public void initWithSubTest(int nSubTest, CCNode p)
        {
            subtestNumber = nSubTest;
            parent = p;
            batchNode = null;
            /*
            * Tests:
            * 1: 1 (32-bit) PNG sprite of 52 x 139
            * 2: 1 (32-bit) PNG Batch Node using 1 sprite of 52 x 139
            * 3: 1 (16-bit) PNG Batch Node using 1 sprite of 52 x 139
            * 4: 1 (4-bit) PVRTC Batch Node using 1 sprite of 52 x 139

            * 5: 14 (32-bit) PNG sprites of 85 x 121 each
            * 6: 14 (32-bit) PNG Batch Node of 85 x 121 each
            * 7: 14 (16-bit) PNG Batch Node of 85 x 121 each
            * 8: 14 (4-bit) PVRTC Batch Node of 85 x 121 each

            * 9: 64 (32-bit) sprites of 32 x 32 each
            *10: 64 (32-bit) PNG Batch Node of 32 x 32 each
            *11: 64 (16-bit) PNG Batch Node of 32 x 32 each
            *12: 64 (4-bit) PVRTC Batch Node of 32 x 32 each
            */

            // purge textures
            CCTextureCache mgr = CCTextureCache.sharedTextureCache();
            //		[mgr removeAllTextures];
            mgr.removeTexture(mgr.addImage("Images/grossinis_sister1"));
            mgr.removeTexture(mgr.addImage("Images/grossini_dance_atlas"));
            mgr.removeTexture(mgr.addImage("Images/spritesheet1"));

            switch (subtestNumber)
            {
                case 1:
                case 4:
                case 7:
                    break;
                ///
                case 2:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);
                    batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/grossinis_sister1", 100);
                    p.addChild(batchNode, 0);
                    break;
                case 3:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444);
                    batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/grossinis_sister1", 100);
                    p.addChild(batchNode, 0);
                    break;

                ///
                case 5:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);
                    batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 100);
                    p.addChild(batchNode, 0);
                    break;
                case 6:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444);
                    batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/grossini_dance_atlas", 100);
                    p.addChild(batchNode, 0);
                    break;

                ///
                case 8:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);
                    batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/spritesheet1", 100);
                    p.addChild(batchNode, 0);
                    break;
                case 9:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444);
                    batchNode = CCSpriteBatchNode.batchNodeWithFile("Images/spritesheet1", 100);
                    p.addChild(batchNode, 0);
                    break;

                default:
                    break;
            }

            //if (batchNode != null)
            //{
            //    batchNode.retain();
            //}

            CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_Default);
        }


        protected int subtestNumber;
        protected CCSpriteBatchNode batchNode;
        protected CCNode parent;
    }
}
