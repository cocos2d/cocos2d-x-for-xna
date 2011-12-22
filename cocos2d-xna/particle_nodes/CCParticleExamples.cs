/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (C) 2008      Apple Inc. All Rights Reserved.

http://www.cocos2d-x.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    //! @brief A fire particle system
    public class CCParticleFire :  CCParticleSystemQuad
    {
	    public CCParticleFire()
        {
        }

	    ~CCParticleFire()
        {
        }

	    public bool init()
        { 
            return initWithTotalParticles(250); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        this.Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        this.EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        this.modeA.gravity = new CCPoint(0,0);

		        // Gravity Mode: radial acceleration
		        this.modeA.radialAccel = 0;
		        this.modeA.radialAccelVar = 0;

		        // Gravity Mode: speed of particles
		        this.modeA.speed = 60;
		        this.modeA.speedVar = 20;		

		        // starting angle
		        Angle = 90;
		        AngleVar = 10;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, 60);
                this.PosVar = new CCPoint(40, 20);

		        // life of particles
		        Life = 3;
		        LifeVar = 0.25f;


		        // size, in pixels
		        StartSize = 54.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per frame
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.76f;
		        StartColor.g = 0.25f;
		        StartColor.b = 0.12f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.0f;
		        StartColorVar.g = 0.0f;
		        StartColorVar.b = 0.0f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.0f;
		        EndColor.g = 0.0f;
		        EndColor.b = 0.0f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = true;
		        return true;
	        }
	        return false;
        }

	    public static CCParticleFire node()
	    {
		    CCParticleFire pRet = new CCParticleFire();
		    if (pRet.init())
		    {
			    return pRet;
		    }
		    return null;
	    }
    };

    //! @brief A fireworks particle system
    public class CCParticleFireworks : CCParticleSystemQuad
    {
        public CCParticleFireworks()
        {
        }

        ~CCParticleFireworks()
        {
        }

        public bool init()
        { 
            return initWithTotalParticles(15); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        Duration= (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        this.EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        this.modeA.gravity = new CCPoint(0,-90);

		        // Gravity Mode:  radial
		        this.modeA.radialAccel = 0;
		        this.modeA.radialAccelVar = 0;

		        //  Gravity Mode: speed of particles
		        this.modeA.speed = 180;
		        this.modeA.speedVar = 50;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);

		        // angle
		        this.Angle= 90;
		        this.AngleVar = 20;

		        // life of particles
		        this.Life = 3.5f;
		        this.LifeVar = 1;

		        // emits per frame
		        this.EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.5f;
		        StartColor.g = 0.5f;
		        StartColor.b = 0.5f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.5f;
		        StartColorVar.g = 0.5f;
		        StartColorVar.b = 0.5f;
		        StartColorVar.a = 0.1f;
		        EndColor.r = 0.1f;
		        EndColor.g = 0.1f;
		        EndColor.b = 0.1f;
		        EndColor.a = 0.2f;
		        EndColorVar.r = 0.1f;
		        EndColorVar.g = 0.1f;
		        EndColorVar.b = 0.1f;
		        EndColorVar.a = 0.2f;

		        // size, in pixels
		        StartSize = 8.0f;
		        StartSizeVar = 2.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // additive
		        this.IsBlendAdditive = false;
		        return true;
	        }
	        return false;
        }

        public static CCParticleFireworks node()
        {
            CCParticleFireworks pRet = new CCParticleFireworks();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief A sun particle system
    public class CCParticleSun : CCParticleSystemQuad
    {
        public CCParticleSun()
        {
        }

        ~CCParticleSun()
        {
        }

        public bool init()
        {
            return initWithTotalParticles(350); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // additive
		        this.IsBlendAdditive = true;

		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,0);

		        // Gravity mode: radial acceleration
		        modeA.radialAccel = 0;
		        modeA.radialAccelVar = 0;

		        // Gravity mode: speed of particles
		        modeA.speed = 20;
		        modeA.speedVar = 5;


		        // angle
		        Angle = 90;
		        AngleVar = 360;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);
		        PosVar = new CCPoint(0,0);

		        // life of particles
		        Life = 1;
		        LifeVar = 0.5f;

		        // size, in pixels
		        StartSize = 30.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per seconds
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.76f;
		        StartColor.g = 0.25f;
		        StartColor.b = 0.12f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.0f;
		        StartColorVar.g = 0.0f;
		        StartColorVar.b = 0.0f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.0f;
		        EndColor.g = 0.0f;
		        EndColor.b = 0.0f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        return true;
	        }
	        return false;
        }

        public static CCParticleSun node()
        {
            CCParticleSun pRet = new CCParticleSun();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief A galaxy particle system
    public class CCParticleGalaxy : CCParticleSystemQuad
    {
        public CCParticleGalaxy(){}
        
        ~CCParticleGalaxy(){}
        
        public bool init()
        { 
            return initWithTotalParticles(200);
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,0);

		        // Gravity Mode: speed of particles
		        modeA.speed = 60;
		        modeA.speedVar = 10;

		        // Gravity Mode: radial
		        modeA.radialAccel = -80;
		        modeA.radialAccelVar = 0;

		        // Gravity Mode: tagential
		        modeA.tangentialAccel = 80;
		        modeA.tangentialAccelVar = 0;

		        // angle
		        Angle = 90;
		        AngleVar = 360;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);
		        PosVar = new CCPoint(0,0);

		        // life of particles
		        Life = 4;
		        LifeVar = 1;

		        // size, in pixels
		        StartSize = 37.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.12f;
		        StartColor.g = 0.25f;
		        StartColor.b = 0.76f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.0f;
		        StartColorVar.g = 0.0f;
		        StartColorVar.b = 0.0f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.0f;
		        EndColor.g = 0.0f;
		        EndColor.b = 0.0f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = true;
		        return true;
	        }
	        return false;
        }

        public static CCParticleGalaxy node()
        {
            CCParticleGalaxy pRet = new CCParticleGalaxy();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief A flower particle system
    public class CCParticleFlower : CCParticleSystemQuad
    {
        public CCParticleFlower(){}
        
        ~CCParticleFlower(){}
        
        public bool init()
        { 
            return initWithTotalParticles(250); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
        	if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,0);

		        // Gravity Mode: speed of particles
		        modeA.speed = 80;
		        modeA.speedVar = 10;

		        // Gravity Mode: radial
		        modeA.radialAccel = -60;
		        modeA.radialAccelVar = 0;

		        // Gravity Mode: tagential
		        modeA.tangentialAccel = 15;
		        modeA.tangentialAccelVar = 0;

		        // angle
		        Angle = 90;
		        AngleVar = 360;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);
		        PosVar = new CCPoint(0,0);

		        // life of particles
		        Life = 4;
		        LifeVar = 1;

		        // size, in pixels
		        StartSize = 30.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.50f;
		        StartColor.g = 0.50f;
		        StartColor.b = 0.50f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.5f;
		        StartColorVar.g = 0.5f;
		        StartColorVar.b = 0.5f;
		        StartColorVar.a = 0.5f;
		        EndColor.r = 0.0f;
		        EndColor.g = 0.0f;
		        EndColor.b = 0.0f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = true;
		        return true;
	        }
	        return false;
        }

        public static CCParticleFlower node()
        {
            CCParticleFlower pRet = new CCParticleFlower();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief A meteor particle system
    public class CCParticleMeteor : CCParticleSystemQuad
    {
        public CCParticleMeteor(){}
        
        ~CCParticleMeteor(){}
        
        public bool init()
        { 
            return initWithTotalParticles(150); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
            if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(-200,200);

		        // Gravity Mode: speed of particles
		        modeA.speed = 15;
		        modeA.speedVar = 5;

		        // Gravity Mode: radial
		        modeA.radialAccel = 0;
		        modeA.radialAccelVar = 0;

		        // Gravity Mode: tagential
		        modeA.tangentialAccel = 0;
		        modeA.tangentialAccelVar = 0;

		        // angle
		        Angle = 90;
		        AngleVar = 360;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);
		        PosVar = new CCPoint(0,0);

		        // life of particles
		        Life = 2;
		        LifeVar = 1;

		        // size, in pixels
		        StartSize = 60.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.2f;
		        StartColor.g = 0.4f;
		        StartColor.b = 0.7f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.0f;
		        StartColorVar.g = 0.0f;
		        StartColorVar.b = 0.2f;
		        StartColorVar.a = 0.1f;
		        EndColor.r = 0.0f;
		        EndColor.g = 0.0f;
		        EndColor.b = 0.0f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = true;
		        return true;
	        }
	        return false;
        }

        public static CCParticleMeteor node()
        {
            CCParticleMeteor pRet = new CCParticleMeteor();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief An spiral particle system
    public class CCParticleSpiral : CCParticleSystemQuad
    {
        public CCParticleSpiral(){}
        
        ~CCParticleSpiral(){}
        
        public bool init()
        { 
            return initWithTotalParticles(500); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
        	if( base.initWithTotalParticles(numberOfParticles) ) 
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Gravity Mode
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,0);

		        // Gravity Mode: speed of particles
		        modeA.speed = 150;
		        modeA.speedVar = 0;

		        // Gravity Mode: radial
		        modeA.radialAccel = -380;
		        modeA.radialAccelVar = 0;

		        // Gravity Mode: tagential
		        modeA.tangentialAccel = 45;
		        modeA.tangentialAccelVar = 0;

		        // angle
		        Angle = 90;
		        AngleVar = 0;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);
		        PosVar = new CCPoint(0,0);

		        // life of particles
		        Life = 12;
		        LifeVar = 0;

		        // size, in pixels
		        StartSize = 20.0f;
		        StartSizeVar = 0.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.5f;
		        StartColor.g = 0.5f;
		        StartColor.b = 0.5f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.5f;
		        StartColorVar.g = 0.5f;
		        StartColorVar.b = 0.5f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.5f;
		        EndColor.g = 0.5f;
		        EndColor.b = 0.5f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.5f;
		        EndColorVar.g = 0.5f;
		        EndColorVar.b = 0.5f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = false;
		        return true;
	        }
	        return false;
        }

        public static CCParticleSpiral node()
        {
            CCParticleSpiral pRet = new CCParticleSpiral();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief An explosion particle system
    public class CCParticleExplosion : CCParticleSystemQuad
    {
        public CCParticleExplosion(){}
        
        ~CCParticleExplosion(){}
        
        public bool init()
        { 
            return initWithTotalParticles(700); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
        	if( base.initWithTotalParticles(numberOfParticles) ) 
	        {
		        // duration
		        Duration = 0.1f;

		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,0);

		        // Gravity Mode: speed of particles
		        modeA.speed = 70;
		        modeA.speedVar = 40;

		        // Gravity Mode: radial
		        modeA.radialAccel = 0;
		        modeA.radialAccelVar = 0;

		        // Gravity Mode: tagential
		        modeA.tangentialAccel = 0;
		        modeA.tangentialAccelVar = 0;

		        // angle
		        Angle = 90;
		        AngleVar = 360;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height/2);
		        PosVar = new CCPoint(0,0);

		        // life of particles
		        Life = 5.0f;
		        LifeVar = 2;

		        // size, in pixels
		        StartSize = 15.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = TotalParticles/Duration;

		        // color of particles
		        StartColor.r = 0.7f;
		        StartColor.g = 0.1f;
		        StartColor.b = 0.2f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.5f;
		        StartColorVar.g = 0.5f;
		        StartColorVar.b = 0.5f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.5f;
		        EndColor.g = 0.5f;
		        EndColor.b = 0.5f;
		        EndColor.a = 0.0f;
		        EndColorVar.r = 0.5f;
		        EndColorVar.g = 0.5f;
		        EndColorVar.b = 0.5f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = false;
		        return true;
	        }
	        return false;
        }

        public static CCParticleExplosion node()
        {
            CCParticleExplosion pRet = new CCParticleExplosion();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief An smoke particle system
    public class CCParticleSmoke : CCParticleSystemQuad
    {
        public CCParticleSmoke(){}
        
        ~CCParticleSmoke(){}
        
        public bool init()
        { 
            return initWithTotalParticles(200); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
        	if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // Emitter mode: Gravity Mode
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,0);

		        // Gravity Mode: radial acceleration
		        modeA.radialAccel = 0;
		        modeA.radialAccelVar = 0;

		        // Gravity Mode: speed of particles
		        modeA.speed = 25;
		        modeA.speedVar = 10;

		        // angle
		        Angle = 90;
		        AngleVar = 5;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, 0);
		        PosVar = new CCPoint(20, 0);

		        // life of particles
		        Life = 4;
		        LifeVar = 1;

		        // size, in pixels
		        StartSize = 60.0f;
		        StartSizeVar = 10.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per frame
		        EmissionRate = TotalParticles/Life;

		        // color of particles
		        StartColor.r = 0.8f;
		        StartColor.g = 0.8f;
		        StartColor.b = 0.8f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.02f;
		        StartColorVar.g = 0.02f;
		        StartColorVar.b = 0.02f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.0f;
		        EndColor.g = 0.0f;
		        EndColor.b = 0.0f;
		        EndColor.a = 1.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = false;
		        return true;
	        }
	        return false;
        }

        public static CCParticleSmoke node()
        {
            CCParticleSmoke pRet = new CCParticleSmoke();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief An snow particle system
    public class CCParticleSnow : CCParticleSystemQuad
    {
        public CCParticleSnow(){}
        
        ~CCParticleSnow(){}
        
        public bool init()
        { 
            return initWithTotalParticles(700); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
        	if( base.initWithTotalParticles(numberOfParticles) ) 
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        // set gravity mode.
		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(0,-1);

		        // Gravity Mode: speed of particles
		        modeA.speed = 5;
		        modeA.speedVar = 1;

		        // Gravity Mode: radial
		        modeA.radialAccel = 0;
		        modeA.radialAccelVar = 1;

		        // Gravity mode: tagential
		        modeA.tangentialAccel = 0;
		        modeA.tangentialAccelVar = 1;

		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height + 10);
		        PosVar = new CCPoint( winSize.width/2, 0 );

		        // angle
		        Angle = -90;
		        AngleVar = 5;

		        // life of particles
		        Life = 45;
		        LifeVar = 15;

		        // size, in pixels
		        StartSize = 10.0f;
		        StartSizeVar = 5.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = 10;

		        // color of particles
		        StartColor.r = 1.0f;
		        StartColor.g = 1.0f;
		        StartColor.b = 1.0f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.0f;
		        StartColorVar.g = 0.0f;
		        StartColorVar.b = 0.0f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 1.0f;
		        EndColor.g = 1.0f;
		        EndColor.b = 1.0f;
		        EndColor.a = 0.0f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = false;
		        return true;
	        }
	        return false;    
        }

        public static CCParticleSnow node()
        {
            CCParticleSnow pRet = new CCParticleSnow();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

    //! @brief A rain particle system
    public class CCParticleRain : CCParticleSystemQuad
    {
        public CCParticleRain(){}
        
        ~CCParticleRain(){}
        
        public bool init()
        {
            return initWithTotalParticles(1000); 
        }

        public override bool initWithTotalParticles(uint numberOfParticles)
        { 
        	if( base.initWithTotalParticles(numberOfParticles) )
	        {
		        // duration
		        Duration = (float)eParticleShowingProperty.kCCParticleDurationInfinity;

		        EmitterMode = (int)eParticleMode.kCCParticleModeGravity;

		        // Gravity Mode: gravity
		        modeA.gravity = new CCPoint(10,-10);

		        // Gravity Mode: radial
		        modeA.radialAccel = 0;
		        modeA.radialAccelVar = 1;

		        // Gravity Mode: tagential
		        modeA.tangentialAccel = 0;
		        modeA.tangentialAccelVar = 1;

		        // Gravity Mode: speed of particles
		        modeA.speed = 130;
		        modeA.speedVar = 30;

		        // angle
		        Angle = -90;
		        AngleVar = 5;


		        // emitter position
		        CCSize winSize = CCDirector.sharedDirector().getWinSize();
		        this.position = new CCPoint(winSize.width/2, winSize.height);
		        PosVar = new CCPoint( winSize.width/2, 0 );

		        // life of particles
		        Life = 4.5f;
		        LifeVar = 0;

		        // size, in pixels
		        StartSize = 4.0f;
		        StartSizeVar = 2.0f;
		        EndSize = (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize;

		        // emits per second
		        EmissionRate = 20;

		        // color of particles
		        StartColor.r = 0.7f;
		        StartColor.g = 0.8f;
		        StartColor.b = 1.0f;
		        StartColor.a = 1.0f;
		        StartColorVar.r = 0.0f;
		        StartColorVar.g = 0.0f;
		        StartColorVar.b = 0.0f;
		        StartColorVar.a = 0.0f;
		        EndColor.r = 0.7f;
		        EndColor.g = 0.8f;
		        EndColor.b = 1.0f;
		        EndColor.a = 0.5f;
		        EndColorVar.r = 0.0f;
		        EndColorVar.g = 0.0f;
		        EndColorVar.b = 0.0f;
		        EndColorVar.a = 0.0f;

		        // additive
		        this.IsBlendAdditive = false;
		        return true;
	        }
	        return false;    
        }

        public static CCParticleRain node()
        {
            CCParticleRain pRet = new CCParticleRain();
            if (pRet.init())
            {
                return pRet;
            }
            return null;
        }
    };

}
