using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class ParticleMainScene : CCScene
    {
        public virtual void initWithSubTest(int asubtest, int particles)
        {
            //srandom(0);

            subtestNumber = asubtest;
            CCSize s = CCDirector.sharedDirector().getWinSize();

            lastRenderedCount = 0;
            quantityParticles = particles;

            CCMenuItemFont.FontSize = 65;
            CCMenuItemFont decrease = CCMenuItemFont.itemFromString(" - ", this, onDecrease);
            decrease.Color = new ccColor3B(0, 200, 20);
            CCMenuItemFont increase = CCMenuItemFont.itemFromString(" + ", this, onIncrease);
            increase.Color = new ccColor3B(0, 200, 20);

            CCMenu menu = CCMenu.menuWithItems(decrease, increase);
            menu.alignItemsHorizontally();
            menu.position = new CCPoint(s.width / 2, s.height / 2 + 15);
            addChild(menu, 1);

            CCLabelTTF infoLabel = CCLabelTTF.labelWithString("0 nodes", "Marker Felt", 30);
            infoLabel.Color = new ccColor3B(0, 200, 20);
            infoLabel.position = new CCPoint(s.width / 2, s.height - 90);
            addChild(infoLabel, 1, PerformanceParticleTest.kTagInfoLayer);

            // particles on stage
            CCLabelAtlas labelAtlas = CCLabelAtlas.labelWithString("0000", "Images/fps_images", 16, 24, '.');
            addChild(labelAtlas, 0, PerformanceParticleTest.kTagLabelAtlas);
            labelAtlas.position = new CCPoint(s.width - 66, 50);

            // Next Prev Test
            ParticleMenuLayer pMenu = new ParticleMenuLayer(true, PerformanceParticleTest.TEST_COUNT, PerformanceParticleTest.s_nParCurIdx);
            addChild(pMenu, 1, PerformanceParticleTest.kTagMenuLayer);

            // Sub Tests
            CCMenuItemFont.FontSize = 40;
            CCMenu pSubMenu = CCMenu.menuWithItems(null);
            for (int i = 1; i <= 6; ++i)
            {
                //char str[10] = {0};
                string str;
                //sprintf(str, "%d ", i);
                str = string.Format("{0:G}", i);
                CCMenuItemFont itemFont = CCMenuItemFont.itemFromString(str, this, testNCallback);
                itemFont.tag = i;
                pSubMenu.addChild(itemFont, 10);

                if (i <= 3)
                {
                    itemFont.Color = new ccColor3B(200, 20, 20);
                }
                else
                {
                    itemFont.Color = new ccColor3B(0, 200, 20);
                }
            }
            pSubMenu.alignItemsHorizontally();
            pSubMenu.position = new CCPoint(s.width / 2, 80);
            addChild(pSubMenu, 2);

            CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 40);
            addChild(label, 1);
            label.position = new CCPoint(s.width / 2, s.height - 32);
            label.Color = new ccColor3B(255, 255, 40);

            updateQuantityLabel();
            createParticleSystem();

            schedule(step);
        }

        public virtual string title()
        {
            return "No title";
        }

        public void step(float dt)
        {
            CCLabelAtlas atlas = (CCLabelAtlas)getChildByTag(PerformanceParticleTest.kTagLabelAtlas);
            CCParticleSystem emitter = (CCParticleSystem)getChildByTag(PerformanceParticleTest.kTagParticleSystem);

            //char str[10] = {0};
            string str;
            //sprintf(str, "%4d", emitter->getParticleCount());
            str = string.Format("{0:D4}", emitter.ParticleCount);
            atlas.setString(str);
        }

        public void createParticleSystem()
        {
            CCParticleSystem particleSystem = null;

            /*
            * Tests:
            * 1: Point Particle System using 32-bit textures (PNG)
            * 2: Point Particle System using 16-bit textures (PNG)
            * 3: Point Particle System using 8-bit textures (PNG)
            * 4: Point Particle System using 4-bit textures (PVRTC)

            * 5: Quad Particle System using 32-bit textures (PNG)
            * 6: Quad Particle System using 16-bit textures (PNG)
            * 7: Quad Particle System using 8-bit textures (PNG)
            * 8: Quad Particle System using 4-bit textures (PVRTC)
            */
            removeChildByTag(PerformanceParticleTest.kTagParticleSystem, true);

            // remove the "fire.png" from the TextureCache cache. 
            CCTexture2D texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
            CCTextureCache.sharedTextureCache().removeTexture(texture);

            if (subtestNumber <= 3)
            {
                particleSystem = new CCParticleSystemPoint();
            }
            else
            {
                particleSystem = new CCParticleSystemQuad();
            }

            switch (subtestNumber)
            {
                case 1:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);
                    particleSystem.initWithTotalParticles((uint)quantityParticles);
                    particleSystem.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
                    break;
                case 2:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444);
                    particleSystem.initWithTotalParticles((uint)quantityParticles);
                    particleSystem.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
                    break;
                case 3:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_A8);
                    particleSystem.initWithTotalParticles((uint)quantityParticles);
                    particleSystem.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
                    break;
                //     case 4:
                //         particleSystem->initWithTotalParticles(quantityParticles);
                //         ////---- particleSystem.texture = [[CCTextureCache sharedTextureCache] addImage:@"fire.pvr"];
                //         particleSystem->setTexture(CCTextureCache::sharedTextureCache()->addImage("Images/fire.png"));
                //         break;
                case 4:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);
                    particleSystem.initWithTotalParticles((uint)quantityParticles);
                    particleSystem.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
                    break;
                case 5:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA4444);
                    particleSystem.initWithTotalParticles((uint)quantityParticles);
                    particleSystem.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
                    break;
                case 6:
                    CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_A8);
                    particleSystem.initWithTotalParticles((uint)quantityParticles);
                    particleSystem.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");
                    break;
                //     case 8:
                //         particleSystem->initWithTotalParticles(quantityParticles);
                //         ////---- particleSystem.texture = [[CCTextureCache sharedTextureCache] addImage:@"fire.pvr"];
                //         particleSystem->setTexture(CCTextureCache::sharedTextureCache()->addImage("Images/fire.png"));
                //         break;
                default:
                    particleSystem = null;
                    Debug.WriteLine("Shall not happen!");
                    break;
            }
            addChild(particleSystem, 0, PerformanceParticleTest.kTagParticleSystem);

            doTest();

            // restore the default pixel format
            CCTexture2D.setDefaultAlphaPixelFormat(CCTexture2DPixelFormat.kCCTexture2DPixelFormat_RGBA8888);
        }

        public void onDecrease(CCObject pSender)
        {
            quantityParticles -= PerformanceParticleTest.kNodesIncrease;
            if (quantityParticles < 0)
                quantityParticles = 0;

            updateQuantityLabel();
            createParticleSystem();
        }

        public void onIncrease(CCObject pSender)
        {
            quantityParticles += PerformanceParticleTest.kNodesIncrease;
            if (quantityParticles > PerformanceParticleTest.kMaxParticles)
                quantityParticles = PerformanceParticleTest.kMaxParticles;

            updateQuantityLabel();
            createParticleSystem();
        }

        public void testNCallback(CCObject pSender)
        {
            subtestNumber = ((CCNode)pSender).tag;

            ParticleMenuLayer pMenu = (ParticleMenuLayer)getChildByTag(PerformanceParticleTest.kTagMenuLayer);
            pMenu.restartCallback(pSender);
        }

        public void updateQuantityLabel()
        {
            if (quantityParticles != lastRenderedCount)
            {
                CCLabelTTF infoLabel = (CCLabelTTF)getChildByTag(PerformanceParticleTest.kTagInfoLayer);
                //char str[20] = {0};
                string str;
                //sprintf(str, "%u particles", quantityParticles);
                str = string.Format("{0:u}particles", quantityParticles);
                infoLabel.setString(str);

                lastRenderedCount = quantityParticles;
            }
        }

        public int getSubTestNum()
        { 
            return subtestNumber;
        }

        public int getParticlesNum()
        { 
            return quantityParticles;
        }

        public virtual void doTest()
        {
            throw new NotFiniteNumberException();
        }


        protected int lastRenderedCount;
        protected int quantityParticles;
        protected int subtestNumber;
    }
}
