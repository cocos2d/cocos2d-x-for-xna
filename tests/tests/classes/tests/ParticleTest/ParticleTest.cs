using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;
using cocos2d.menu_nodes;

namespace tests
{
    
    public class ParticleTestScene : TestScene
    {
        public static int kTagLabelAtlas = 1;

        public enum eIDClick
        {
	        IDC_NEXT = 100,
	        IDC_BACK,
	        IDC_RESTART,
	        IDC_TOGGLE
        };

        public static int sceneIdx = -1; 

        public static int MAX_LAYER = 29;

        public static CCLayer createParticleLayer(int nIndex)
        {
	        switch(nIndex)
	        {
                case 0: return new DemoFlower();
                case 1: return new DemoGalaxy();
                case 2: return new DemoFirework();
                case 3: return new DemoSpiral();
                case 4: return new DemoSun();
                case 5: return new DemoMeteor();
                case 6: return new DemoFire();
                case 7: return new DemoSmoke();
                case 8: return new DemoExplosion();
                case 9: return new DemoSnow();
                case 10: return new DemoRain();
                case 11: return new DemoBigFlower();
                case 12: return new DemoRotFlower();

                // this test depends on opengl, can't be realized in xna
                // case 13: return new DemoModernArt();
                
                case 13: return new DemoRing();
                case 14: return new ParallaxParticle();
                case 15: return new DemoParticleFromFile("BoilingFoam");
                case 16: return new DemoParticleFromFile("BurstPipe");
                case 17: return new DemoParticleFromFile("CometPlist");
                case 18: return new DemoParticleFromFile("debian");
                case 19: return new DemoParticleFromFile("ExplodingRing");
                case 20: return new DemoParticleFromFile("LavaFlow");
                case 21: return new DemoParticleFromFile("SpinningPeasPlist");
                case 22: return new DemoParticleFromFile("SpookyPeasPlist");
                case 23: return new DemoParticleFromFile("Upsidedown");

                // !the following three tests are using texturedata
                // case 24: return new DemoParticleFromFile("FlowerPlist");
                // case 25: return new DemoParticleFromFile("Spiral");
                // case 26: return new DemoParticleFromFile("GalaxyPlist");

                case 24: return new RadiusMode1();
                case 25: return new RadiusMode2();
                case 26: return new Issue704();
                case 27: return new Issue870();
                case 28: return new DemoParticleFromFile("Phoenix");
                
                //case 0: return new DemoFlower();
                //case 1: return new DemoGalaxy();
                //case 2: return new DemoFirework();
                //case 3: return new DemoSpiral();
                //case 4: return new DemoSun();
                //case 5: return new DemoMeteor();
                //case 6: return new DemoFire();
                //case 7: return new DemoSmoke();
                //case 8: return new DemoExplosion();
                //case 9: return new DemoSnow();
                //case 10: return new DemoRain();
                //case 11: return new DemoBigFlower();
                //case 12: return new DemoRotFlower();
                //case 13: return new DemoModernArt();
                //case 14: return new DemoRing();
                //case 15: return new ParallaxParticle();
                //case 16: return new DemoParticleFromFile("BoilingFoam");
                //case 17: return new DemoParticleFromFile("BurstPipe");
                //case 18: return new DemoParticleFromFile("Comet");
                //case 19: return new DemoParticleFromFile("debian");
                //case 20: return new DemoParticleFromFile("ExplodingRing");
                //case 21: return new DemoParticleFromFile("LavaFlow");
                //case 22: return new DemoParticleFromFile("SpinningPeas");
                //case 23: return new DemoParticleFromFile("SpookyPeas");
                //case 24: return new DemoParticleFromFile("Upsidedown");
                //case 25: return new DemoParticleFromFile("Flower");
                //case 26: return new DemoParticleFromFile("Spiral");
                //case 27: return new DemoParticleFromFile("Galaxy");
                //case 28: return new RadiusMode1();
                //case 29: return new RadiusMode2();
                //case 30: return new Issue704();
                //case 31: return new Issue870();
                //case 32: return new DemoParticleFromFile("Phoenix");
	        }

	        return null;
        }


        public static CCLayer nextParticleAction()
        {
	        sceneIdx++;
	        sceneIdx = sceneIdx % MAX_LAYER;

	        CCLayer pLayer = createParticleLayer(sceneIdx);

	        return pLayer;
        }

        public static CCLayer backParticleAction()
        {
	        sceneIdx--;
	        int total = MAX_LAYER;
	        if( sceneIdx < 0 )
		        sceneIdx += total;	
	
	        CCLayer pLayer = createParticleLayer(sceneIdx);

	        return pLayer;
        }

        public static CCLayer restartParticleAction()
        {
	        CCLayer pLayer = createParticleLayer(sceneIdx);

	        return pLayer;
        } 


        public override void runThisTest()
        {
            addChild(nextParticleAction());

            CCDirector.sharedDirector().replaceScene(this);
        }
    };

    public class ParticleDemo : CCLayerColor
    {
	    public CCParticleSystem m_emitter;
	    public CCSprite m_background;

        public ParticleDemo()
        {
        	initWithColor( ccTypes.ccc4(127,127,127,255) );

	        m_emitter = null;

	        isTouchEnabled = true;
	
	        CCSize s = CCDirector.sharedDirector().getWinSize();
	        CCLabelTTF label = CCLabelTTF.labelWithString(title(), "Arial", 28);
	        addChild(label, 100, 1000);
	        label.position = new CCPoint(s.width/2, s.height-50);
	
	        CCLabelTTF tapScreen = CCLabelTTF.labelWithString("(Tap the Screen)", "Arial", 20);
	        tapScreen.position = new CCPoint(s.width/2, s.height-80);
	        addChild(tapScreen, 100);
	
	        CCMenuItemImage item1 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathB1, TestResource.s_pPathB2, this, new SEL_MenuHandler(backCallback) );
	        CCMenuItemImage item2 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathR1, TestResource.s_pPathR2, this, new SEL_MenuHandler(restartCallback) );
            CCMenuItemImage item3 = CCMenuItemImage.itemFromNormalImage(TestResource.s_pPathF1, TestResource.s_pPathF2, this, new SEL_MenuHandler(nextCallback));
	
	        CCMenuItemToggle item4 = CCMenuItemToggle.itemWithTarget(	this, 
																        new SEL_MenuHandler(toggleCallback), 
																        CCMenuItemFont.itemFromString( "Free Movement" ),
                                                                        CCMenuItemFont.itemFromString( "Relative Movement" ),
																        CCMenuItemFont.itemFromString( "Grouped Movement" ));
	
	        CCMenu menu = CCMenu.menuWithItems(item1, item2, item3, item4);
		
	        menu.position = new CCPoint(0,0);
	        item1.position = new CCPoint(s.width/2 - 100,30);
	        item2.position = new CCPoint(s.width/2, 30);
	        item3.position = new CCPoint(s.width/2 + 100,30);
	        item4.position = new CCPoint( 0, 100);
	        item4.anchorPoint = new CCPoint(0,0);

	        addChild( menu, 100 );	
	
            CCLabelAtlas labelAtlas = CCLabelAtlas.labelWithString("0000", "fonts/fnt/images/fps_images", 16, 24, '.');
            addChild(labelAtlas, 100, ParticleTestScene.kTagLabelAtlas);
	        labelAtlas.position = new CCPoint(s.width-66,50);
	
	        // moving background
	        m_background = CCSprite.spriteWithFile(TestResource.s_back3);
	        addChild(m_background, 5);
	        m_background.position = new CCPoint(s.width/2 - 120, s.height - 240);

	        CCActionInterval move = CCMoveBy.actionWithDuration(4, new CCPoint(300,0) );
	        CCFiniteTimeAction move_back = move.reverse();
	        CCFiniteTimeAction seq = CCSequence.actions(move, move_back);
	        m_background.runAction( CCRepeatForever.actionWithAction((CCActionInterval)seq) );
	
	
	        schedule( new SEL_SCHEDULE(step) );
        }
	    
        ~ParticleDemo()
        {
        
        }

	    public override void onEnter()
        {
           	base.onEnter();

	        CCLabelTTF pLabel = (CCLabelTTF)(this.getChildByTag(1000));
	        pLabel.setString(title());
        }

	    public virtual string title()
        {
            return "No title";
        }

	    public void restartCallback(CCObject pSender)
        {
            m_emitter.resetSystem(); 
        }

	    public void nextCallback(CCObject pSender)
        {
            CCScene s = new ParticleTestScene();
	        s.addChild( ParticleTestScene.nextParticleAction() );
	        CCDirector.sharedDirector().replaceScene(s);
        }

	    public void backCallback(CCObject pSender)
        {
        	CCScene s = new ParticleTestScene();
	        s.addChild( ParticleTestScene.backParticleAction() );
	        CCDirector.sharedDirector().replaceScene(s);
        }

	    public void toggleCallback(CCObject pSender)
        {
        	if( m_emitter.PositionType == eParticlePositionType.kCCPositionTypeGrouped )
		        m_emitter.PositionType = eParticlePositionType.kCCPositionTypeFree;
            else if (m_emitter.PositionType == eParticlePositionType.kCCPositionTypeFree)
                m_emitter.PositionType = eParticlePositionType.kCCPositionTypeRelative;
	        else if (m_emitter.PositionType == eParticlePositionType.kCCPositionTypeRelative)
		        m_emitter.PositionType = eParticlePositionType.kCCPositionTypeGrouped;
        }

	    public override void registerWithTouchDispatcher()
        {
            CCTouchDispatcher.sharedDispatcher().addTargetedDelegate(this, 0, false);
        }

	    public override bool ccTouchBegan(CCTouch touch, CCEvent eve)
        {
            return true;
        }

	    public override void ccTouchMoved(CCTouch touch, CCEvent eve)
        {
            ccTouchEnded(touch, eve);
        }

	    public override void ccTouchEnded(CCTouch touch, CCEvent eve)
        {
            CCPoint location = touch.locationInView( touch.view() );
	        CCPoint convertedLocation = CCDirector.sharedDirector().convertToGL(location);

            CCPoint pos = new CCPoint(0,0);
            if (m_background != null)
            {
	            pos = m_background.convertToWorldSpace(new CCPoint(0,0));
            }
	        m_emitter.position = CCPointExtension.ccpSub(convertedLocation, pos);	
        }

	    public void step(float dt)
        {
            if (m_emitter != null)
            {
	            CCLabelAtlas atlas = (CCLabelAtlas)getChildByTag(ParticleTestScene.kTagLabelAtlas);
                
                //char str[5] = {0};
                //sprintf(str, "%04d", m_emitter.getParticleCount());
                //atlas.setString(str);
                string str = string.Format("{0}", m_emitter.ParticleCount);
	            atlas.setString(str);

            }
        }

	    public void setEmitterPosition()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

            m_emitter.position = new CCPoint(s.width / 2, s.height / 2 - 30);
        }
    };

    //------------------------------------------------------------------
    //
    // DemoFirework
    //
    //------------------------------------------------------------------
    public class DemoFirework : ParticleDemo
    {
        public override void onEnter()
        { 
            	base.onEnter();

	            m_emitter = CCParticleFireworks.node();
	            m_background.addChild(m_emitter, 10);
	
                m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_stars1);
	
	            setEmitterPosition();
        }

        public override string title()
        {
            return "ParticleFireworks";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoFire
    //
    //------------------------------------------------------------------
    public class DemoFire : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleFire.node();
	        m_background.addChild(m_emitter, 10);
	
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);//.pvr"];
	        CCPoint p = m_emitter.position;
	        m_emitter.position = new CCPoint(p.x, 100);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleFire";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoSun
    //
    //------------------------------------------------------------------
    public class DemoSun : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleSun.node();
	        m_background.addChild(m_emitter, 10);

	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        setEmitterPosition();
        }
        public override string title()
        {
            return "ParticleSun";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoGalaxy
    //
    //------------------------------------------------------------------
    public class DemoGalaxy : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleGalaxy.node();
	        m_background.addChild(m_emitter, 10);
	
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        setEmitterPosition(); 
        }

        public override string title()
        {
	        return "ParticleGalaxy";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoFlower
    //
    //------------------------------------------------------------------
    public class DemoFlower : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleFlower.node();
	        m_background.addChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_stars1);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleFlower";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoBigFlower
    //
    //------------------------------------------------------------------
    public class DemoBigFlower : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = new CCParticleSystemQuad();
	        m_emitter.initWithTotalParticles(50);
	        //m_emitter.autorelease();

	        m_background.addChild(m_emitter, 10);
	        ////m_emitter.release();	// win32 :  use this line or remove this line and use autorelease()
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_stars1);

	        m_emitter.Duration = -1;
	
	        // gravity
	        m_emitter.setGravity(new CCPoint(0,0));
	
	        // angle
	        m_emitter.Angle = 90;
	        m_emitter.AngleVar = 360;
	
	        // speed of particles
	        m_emitter.setSpeed(160);
	        m_emitter.setSpeedVar(20);
	
	        // radial
	        m_emitter.setRadialAccel(-120);
	        m_emitter.setRadialAccelVar(0);
	
	        // tagential
	        m_emitter.setTangentialAccel(30);
	        m_emitter.setTangentialAccelVar(0);
	
	        // emitter position
	        m_emitter.position = new CCPoint(160,240);
	        m_emitter.PosVar = new CCPoint(0,0);
	
	        // life of particles
	        m_emitter.Life = 4;
	        m_emitter.LifeVar = 1;
	
	        // spin of particles
	        m_emitter.StartSpin = 0;
	        m_emitter.StartSizeVar = 0;
	        m_emitter.EndSpin = 0;
	        m_emitter.EndSpinVar = 0;
	
	        // color of particles
	        ccColor4F startColor = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
	        m_emitter.StartColor = startColor;
	
	        ccColor4F startColorVar = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
	        m_emitter.StartColorVar = startColorVar;
	
	        ccColor4F endColor = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
	        m_emitter.EndColor = endColor;
	
	        ccColor4F endColorVar = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);	
	        m_emitter.EndColorVar = endColorVar;
	
	        // size, in pixels
	        m_emitter.StartSize = 80.0f;
	        m_emitter.StartSizeVar = 40.0f;
	        m_emitter.EndSize = (float)eParticleShowingProperty.kParticleStartSizeEqualToEndSize;
	
	        // emits per second
	        m_emitter.EmissionRate = m_emitter.TotalParticles/m_emitter.Life;
	
	        // additive
	        m_emitter.IsBlendAdditive = true;

	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleBigFlower";
        }
    };

    //------------------------------------------------------------------
    //
    // DemoRotFlower
    //
    //------------------------------------------------------------------
    public class DemoRotFlower : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = new CCParticleSystemQuad();
	        m_emitter.initWithTotalParticles(300);
	        //m_emitter.autorelease();

	        m_background.addChild(m_emitter, 10);
	        ////m_emitter.release();	// win32 : Remove this line
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_stars2);
	
	        // duration
	        m_emitter.Duration = -1;
	
	        // gravity
	        m_emitter.setGravity( new CCPoint(0,0));
	
	        // angle
	        m_emitter.Angle = 90;
	        m_emitter.AngleVar = 360;
	
	        // speed of particles
	        m_emitter.setSpeed(160);
	        m_emitter.setSpeedVar(20);
	
	        // radial
	        m_emitter.setRadialAccel(-120);
	        m_emitter.setRadialAccelVar(0);
	
	        // tagential
	        m_emitter.setTangentialAccel(30);
	        m_emitter.setTangentialAccelVar(0);
	
	        // emitter position
	        m_emitter.position =  new CCPoint(160,240);
	        m_emitter.PosVar = new CCPoint(0,0);
	
	        // life of particles
	        m_emitter.Life = 3;
	        m_emitter.LifeVar = 1;

	        // spin of particles
	        m_emitter.StartSpin = 0;
	        m_emitter.StartSpinVar = 0;
	        m_emitter.EndSpin = 0;
	        m_emitter.EndSpinVar = 2000;
	
	        // color of particles
	        ccColor4F startColor = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
	        m_emitter.StartColor = startColor;
	
	        ccColor4F startColorVar = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
	        m_emitter.StartColorVar = startColorVar;
	
	        ccColor4F endColor = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
	        m_emitter.EndColor = endColor;
	
	        ccColor4F endColorVar = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);	
	        m_emitter.EndColorVar = endColorVar;

	        // size, in pixels
	        m_emitter.StartSize = 30.0f;
	        m_emitter.StartSizeVar = 00.0f;
	        m_emitter.EndSize = (float)eParticleShowingProperty.kParticleStartSizeEqualToEndSize;
	
	        // emits per second
	        m_emitter.EmissionRate = m_emitter.TotalParticles/m_emitter.Life;

	        // additive
	        m_emitter.IsBlendAdditive = false;
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleRotFlower";
        }
    };

    public class DemoMeteor : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleMeteor.node();
            
	        m_background.addChild(m_emitter, 10);
	
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleMeteor";
        }
    };

    public class DemoSpiral : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleSpiral.node();
            
	        m_background.addChild(m_emitter, 10);
	
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleSpiral";
        }
    };

    public class DemoExplosion : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleExplosion.node();
            
	        m_background.addChild(m_emitter, 10);
	
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_stars1);
	
	        m_emitter.IsAutoRemoveOnFinish = true;
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleExplosion";
        }
    };

    public class DemoSmoke : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleSmoke.node();
            
	        m_background.addChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        CCPoint p = m_emitter.position;
	        m_emitter.position =  new CCPoint( p.x, 100);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleSmoke";
        }
    };

    public class DemoSnow : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleSnow.node();
            
	        m_background.addChild(m_emitter, 10);
	
	        CCPoint p = m_emitter.position;
	        m_emitter.position =  new CCPoint( p.x, p.y-110);
	        m_emitter.Life = 3;
	        m_emitter.LifeVar = 1;
	
	        // gravity
	        m_emitter.setGravity(new CCPoint(0,-10));
		
	        // speed of particles
	        m_emitter.setSpeed(130);
	        m_emitter.setSpeedVar(30);
	
	        m_emitter.StartColor.r = 0.9f;
	        m_emitter.StartColor.g = 0.9f;
	        m_emitter.StartColor.b = 0.9f;

	        m_emitter.StartColorVar.b = 0.1f;
	
	        m_emitter.EmissionRate = m_emitter.TotalParticles/m_emitter.Life;
	
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_snow);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleSnow";
        }

    };

    public class DemoRain : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleRain.node();
            
	        m_background.addChild(m_emitter, 10);
	
	        CCPoint p = m_emitter.position;
	        m_emitter.position =  new CCPoint( p.x, p.y-100);
	        m_emitter.Life = 4;
	
	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "ParticleRain";
        }
    };

    // todo: CCParticleSystemPoint::draw() hasn't been implemented.
    public class DemoModernArt : ParticleDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            m_emitter = new CCParticleSystemPoint();
            m_emitter.initWithTotalParticles(1000);
            //m_emitter.autorelease();

            m_background.addChild(m_emitter, 10);
            ////m_emitter.release();

            CCSize s = CCDirector.sharedDirector().getWinSize();

            // duration
            m_emitter.Duration = -1;

            // gravity
            m_emitter.setGravity(new CCPoint(0, 0));

            // angle
            m_emitter.Angle = 0;
            m_emitter.AngleVar = 360;

            // radial
            m_emitter.setRadialAccel(70);
            m_emitter.setRadialAccelVar(10);

            // tagential
            m_emitter.setTangentialAccel(80);
            m_emitter.setTangentialAccelVar(0);

            // speed of particles
            m_emitter.setSpeed(50);
            m_emitter.setSpeedVar(10);

            // emitter position
            m_emitter.position = new CCPoint(s.width / 2, s.height / 2);
            m_emitter.PosVar = new CCPoint(0, 0);

            // life of particles
            m_emitter.Life = 2.0f;
            m_emitter.LifeVar = 0.3f;

            // emits per frame
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // color of particles
            ccColor4F startColor = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            ccColor4F startColorVar = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            ccColor4F endColor = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            ccColor4F endColorVar = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 1.0f;
            m_emitter.StartSizeVar = 1.0f;
            m_emitter.EndSize = 32.0f;
            m_emitter.EndSizeVar = 8.0f;

            // texture
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);

            // additive
            m_emitter.IsBlendAdditive = false;

            setEmitterPosition();
        }

        public override string title()
        {
            return "Varying size";
        }
    };

    public class DemoRing : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();

	        m_emitter = CCParticleFlower.node();
            

	        m_background.addChild(m_emitter, 10);

	        m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_stars1);
	        m_emitter.LifeVar = 0;
	        m_emitter.Life = 10;
	        m_emitter.setSpeed(100);
	        m_emitter.setSpeedVar(0);
	        m_emitter.EmissionRate = 10000;
	
	        setEmitterPosition();
        }

        public override string title()
        {
	        return "Ring Demo";
        }

    };

    public class ParallaxParticle : ParticleDemo
    {
        public override void onEnter()
        {
	        base.onEnter();
	
	        m_background.parent.removeChild(m_background, true);
            m_background = null;

	        CCParallaxNode p = CCParallaxNode.node(); 
	        addChild(p, 5);

	        CCSprite p1 = CCSprite.spriteWithFile(TestResource.s_back3);
	        CCSprite p2 = CCSprite.spriteWithFile(TestResource.s_back3);
	
	        p.addChild( p1, 1, new CCPoint(0.5f,1), new CCPoint(0,0) );
	        p.addChild(p2, 2, new CCPoint(1.5f,1), new CCPoint(0,0) );

	        m_emitter = CCParticleFlower.node();
            
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);

	        p1.addChild(m_emitter, 10);
	        m_emitter.position =  new CCPoint(250,200);
	
	        CCParticleSun par = CCParticleSun.node();
	        p2.addChild(par, 10);
            par.Texture = CCTextureCache.sharedTextureCache().addImage(TestResource.s_fire);
	
	        CCActionInterval move = CCMoveBy.actionWithDuration(4, new CCPoint(300,0));
	        CCFiniteTimeAction move_back = move.reverse();
	        CCFiniteTimeAction seq = CCSequence.actions(move, move_back);
	        p.runAction(CCRepeatForever.actionWithAction((CCActionInterval)seq));	
        }

        public override string title()
        {
	        return "Parallax + Particles";
        }

    };

    public class DemoParticleFromFile : ParticleDemo
    {
        string m_title;

        public DemoParticleFromFile()
        {
        }

        public DemoParticleFromFile(string file)
        {
            m_title = file;
        }
        public override void onEnter()
        {
            base.onEnter();

            Color = new ccColor3B(0, 0, 0);
            removeChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            // string filename = "Images/" + m_title + ".plist";
            string filename = "Images/" + m_title;
            m_emitter.initWithFile(filename);
            addChild(m_emitter, 10);

            setEmitterPosition();
        }
        public override string title()
        {
            if (null != m_title)
            {
                return m_title;
            }
            else
            {
                return "ParticleFromFile";
            }
        }
    };

    public class RadiusMode1 : ParticleDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            Color = new ccColor3B(0,0,0);
            removeChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            m_emitter.initWithTotalParticles(200);
            addChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage("Images/stars-grayscale");

            // duration
            m_emitter.Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

            // radius mode
            m_emitter.EmitterMode = (int)eParticleMode.kCCParticleModeRadius;

            // radius mode: start and end radius in pixels
            m_emitter.setStartRadius(0);
            m_emitter.setStartRadiusVar(0);
            m_emitter.setEndRadius(160);
            m_emitter.setEndRadiusVar(0);

            // radius mode: degrees per second
            m_emitter.setRotatePerSecond(180);
            m_emitter.setRotatePerSecondVar(0);


            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 0;

            // emitter position
            CCSize size = CCDirector.sharedDirector().getWinSize();
            m_emitter.position = new CCPoint(size.width/2, size.height/2);
            m_emitter.PosVar = new CCPoint(0,0);

            // life of particles
            m_emitter.Life = 5;
            m_emitter.LifeVar = 0;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            ccColor4F startColor = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            ccColor4F startColorVar = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

            ccColor4F endColor = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            ccColor4F endColorVar = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);	
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 32;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.IsBlendAdditive = false;
        }

        public override string title()
        {
            return "Radius Mode: Spiral";
        }

    };

    public class RadiusMode2 : ParticleDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            this.Color = new ccColor3B(0,0,0);
            removeChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            m_emitter.initWithTotalParticles(200);
            addChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage("Images/stars-grayscale");

            // duration
            m_emitter.Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

            // radius mode
            m_emitter.EmitterMode = (int)eParticleMode.kCCParticleModeRadius;

            // radius mode: start and end radius in pixels
            m_emitter.setStartRadius(100);
            m_emitter.setStartRadiusVar(0);
            m_emitter.setEndRadius((float)eParticleShowingProperty.kCCParticleStartRadiusEqualToEndRadius);
            m_emitter.setEndRadiusVar(0);

            // radius mode: degrees per second
            m_emitter.setRotatePerSecond(45);
            m_emitter.setRotatePerSecondVar(0);


            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 0;

            // emitter position
            CCSize size = CCDirector.sharedDirector().getWinSize();
            m_emitter.position = new CCPoint(size.width/2, size.height/2);
            m_emitter.PosVar = new CCPoint(0,0);

            // life of particles
            m_emitter.Life = 4;
            m_emitter.LifeVar = 0;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            ccColor4F startColor = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            ccColor4F startColorVar = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

                ccColor4F endColor = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            ccColor4F endColorVar = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);	
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 32;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.IsBlendAdditive = false;
        }

        public override string title()
        {
            return "Radius Mode: Semi Circle";
        }
    };

    public class Issue704 : ParticleDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            Color = new ccColor3B(0,0,0);
            removeChild(m_background, true);
            m_background = null;

            m_emitter = new CCParticleSystemQuad();
            m_emitter.initWithTotalParticles(100);
            addChild(m_emitter, 10);
            m_emitter.Texture = CCTextureCache.sharedTextureCache().addImage("Images/fire");

            // duration
            m_emitter.Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

            // radius mode
            m_emitter.EmitterMode = (int)eParticleMode.kCCParticleModeRadius;

            // radius mode: start and end radius in pixels
            m_emitter.setStartRadius(50);
            m_emitter.setStartRadiusVar(0);
            m_emitter.setEndRadius((float)eParticleShowingProperty.kCCParticleStartRadiusEqualToEndRadius);
            m_emitter.setEndRadiusVar(0);

            // radius mode: degrees per second
            m_emitter.setRotatePerSecond(0);
            m_emitter.setRotatePerSecondVar(0);


            // angle
            m_emitter.Angle = 90;
            m_emitter.AngleVar = 0;

            // emitter position
            CCSize size = CCDirector.sharedDirector().getWinSize();
            m_emitter.position = new CCPoint(size.width/2, size.height/2);
            m_emitter.PosVar = new CCPoint(0,0);

            // life of particles
            m_emitter.Life = 5;
            m_emitter.LifeVar = 0;

            // spin of particles
            m_emitter.StartSpin = 0;
            m_emitter.StartSpinVar = 0;
            m_emitter.EndSpin = 0;
            m_emitter.EndSpinVar = 0;

            // color of particles
            ccColor4F startColor = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColor = startColor;

            ccColor4F startColorVar = new ccColor4F(0.5f, 0.5f, 0.5f, 1.0f);
            m_emitter.StartColorVar = startColorVar;

                ccColor4F endColor = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);
            m_emitter.EndColor = endColor;

            ccColor4F endColorVar = new ccColor4F(0.1f, 0.1f, 0.1f, 0.2f);	
            m_emitter.EndColorVar = endColorVar;

            // size, in pixels
            m_emitter.StartSize = 16;
            m_emitter.StartSizeVar = 0;
            m_emitter.EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

            // emits per second
            m_emitter.EmissionRate = m_emitter.TotalParticles / m_emitter.Life;

            // additive
            m_emitter.IsBlendAdditive = false;

            CCRotateBy rot = CCRotateBy.actionWithDuration(16, 360);
            m_emitter.runAction(CCRepeatForever.actionWithAction(rot));
        }

        public override string title()
        {
            return "Issue 704. Free + Rot";
        }

        public string subtitle()
        {
            return "Emitted particles should not rotate";
        }
    };

    public class Issue870 : ParticleDemo
    {
        public override void onEnter()
        {
            base.onEnter();

            Color = new ccColor3B(0,0,0);
            removeChild(m_background, true);
            m_background = null;

            CCParticleSystemQuad system = new CCParticleSystemQuad();
            system.initWithFile("Images/SpinningPeasPlist");
            system.setTextureWithRect(CCTextureCache.sharedTextureCache().addImage("Images/particles"), new CCRect(0,0,32,32));
            addChild(system, 10);
            m_emitter = system;

            m_nIndex = 0;
            schedule(updateQuads, 2.0f);
        }

        public void updateQuads(float dt)
        {
            m_nIndex = (m_nIndex + 1) % 4;
            CCRect rect = new CCRect(m_nIndex * 32, 0, 32, 32);
            CCParticleSystemQuad system = (CCParticleSystemQuad)m_emitter;
            system.setTextureWithRect(m_emitter.Texture, rect);
        }

        public override string title()
        {
            return "Issue 870. SubRect";
        }

        public string subtitle()
        {
            return "Every 2 seconds the particle should change";
        }

        int m_nIndex;
    };
}