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

        public static int MAX_LAYER = 3;

        public static CCLayer createParticleLayer(int nIndex)
        {
	        switch(nIndex)
	        {
                case 0: return new DemoFirework();
                case 1: return new DemoFire();
                case 2: return new DemoSun();

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
	        m_background.position = new CCPoint(s.width/2, s.height/2 - 180);

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
                //sprintf(str, "%04d", m_emitter->getParticleCount());
                //atlas->setString(str);
                string str = string.Format("{0}", m_emitter.ParticleCount);
	            atlas.setString(str);

            }
        }

	    public void setEmitterPosition()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();

	        m_emitter.position = new CCPoint(0, s.height / 2);
        }
    };

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

    //class DemoGalaxy : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoFlower : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoBigFlower : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoRotFlower : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoMeteor : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoSpiral : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoExplosion : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoSmoke : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoSnow : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoRain : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoModernArt : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoRing : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class ParallaxParticle : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class DemoParticleFromFile : public ParticleDemo
    //{
    //public:
    //    std::string m_title;
    //    DemoParticleFromFile(const char *file)
    //    {	
    //        m_title = file;
    //    }
    //    virtual void onEnter();
    //    virtual std::string title()
    //    {
    //        return m_title;
    //    }
    //};

    //class RadiusMode1 : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class RadiusMode2 : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //};

    //class Issue704 : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //    virtual std::string subtitle();
    //};

    //class Issue870 : public ParticleDemo
    //{
    //public:
    //    virtual void onEnter();
    //    virtual std::string title();
    //    virtual std::string subtitle();
    //    void updateQuads(ccTime dt);

    //private:
    //    int m_nIndex;
    //};
}