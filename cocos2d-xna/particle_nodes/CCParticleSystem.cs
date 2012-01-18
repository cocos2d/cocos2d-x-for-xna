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
using System.Diagnostics;

namespace cocos2d
{
    //* @enum
    public enum eParticleShowingProperty
    {
	    /** The Particle emitter lives forever */
	    kCCParticleDurationInfinity = -1,

	    /** The starting size of the particle is equal to the ending size */
	    kCCParticleStartSizeEqualToEndSize = -1,

	    /** The starting radius of the particle is equal to the ending radius */
	    kCCParticleStartRadiusEqualToEndRadius = -1,

	    // backward compatible
	    kParticleStartSizeEqualToEndSize = kCCParticleStartSizeEqualToEndSize,
	    kParticleDurationInfinity = kCCParticleDurationInfinity,
    };

    //* @enum
    public enum eParticleMode 
    {
	    /** Gravity mode (A mode) */
	    kCCParticleModeGravity,

	    /** Radius mode (B mode) */
	    kCCParticleModeRadius,	
    };


    /** @typedef tCCPositionType
    possible types of particle positions
    */
    public enum eParticlePositionType
    {
        /** Living particles are attached to the world and are unaffected by emitter repositioning. */
	    kCCPositionTypeFree,

        /** Living particles are attached to the world but will follow the emitter repositioning.
        Use case: Attach an emitter to an sprite, and you want that the emitter follows the sprite.
        */
        kCCPositionTypeRelative,

        /** Living particles are attached to the emitter and are translated along with it. */
	    kCCPositionTypeGrouped,
    };

    // backward compatible
    public enum eParticlePositionTypeBackward 
    {
	    kPositionTypeFree = eParticlePositionType.kCCPositionTypeFree,
	    kPositionTypeGrouped = eParticlePositionType.kCCPositionTypeGrouped,
    }; 

    /**
    Structure that contains the values of each particle
    */
    public class CCParticle 
    {
        public CCParticle()
        {
            pos = new CCPoint();
            startPos = new CCPoint();
            color = new ccColor4F();
            deltaColor = new ccColor4F();
            modeA = new sModeA();
            modeB = new sModeB();
        }

        public void copy(CCParticle particleCopied)
        { 
            pos.x = particleCopied.pos.x;
            pos.y = particleCopied.pos.y;
            
            startPos.x = particleCopied.startPos.x;
            startPos.y = particleCopied.startPos.y;

            color.r = particleCopied.color.r;
            color.g = particleCopied.color.g;
            color.b = particleCopied.color.b;
            color.a = particleCopied.color.a;

            deltaColor.r = particleCopied.deltaColor.r;
            deltaColor.g = particleCopied.deltaColor.g;
            deltaColor.b = particleCopied.deltaColor.b;
            deltaColor.a = particleCopied.deltaColor.a;

            size = particleCopied.size;
            deltaSize = particleCopied.deltaSize;

            rotation = particleCopied.rotation;
            deltaRotation = particleCopied.deltaRotation;

            timeToLive = particleCopied.timeToLive;

            // modeA
            modeA.dir.x = particleCopied.modeA.dir.x;
            modeA.dir.y = particleCopied.modeA.dir.y;
            modeA.radialAccel = particleCopied.modeA.radialAccel;
            modeA.tangentialAccel = particleCopied.modeA.tangentialAccel;

            // mocdB
            modeB.angle = particleCopied.modeB.angle;
            modeB.degreesPerSecond = particleCopied.modeB.degreesPerSecond;
            modeB.radius = particleCopied.modeB.radius;
            modeB.deltaRadius = particleCopied.modeB.deltaRadius;
        }

	    public CCPoint     pos;
        public CCPoint startPos;

        public ccColor4F color;
        public ccColor4F deltaColor;

        public float size;
        public float deltaSize;

        public float rotation;
        public float deltaRotation;

        public float timeToLive;

	    //! Mode A: gravity, direction, radial accel, tangential accel
	    public class sModeA
        {
            public sModeA()
            {
                dir = new CCPoint();
            }

            public CCPoint dir;
            public float radialAccel;
            public float tangentialAccel;
	    };
        public sModeA modeA;

	    //! Mode B: radius mode
	    public class sModeB
        {
            public sModeB()
            { 
            
            }

            public float angle;
            public float degreesPerSecond;
            public float radius;
            public float deltaRadius;
	    };
        public sModeB modeB;
    };

    //typedef void (*CC_UPDATE_PARTICLE_IMP)(id, SEL, tCCParticle*, CCPoint);

    /** @brief Particle System base class.
    Attributes of a Particle System:
    - emmision rate of the particles
    - Gravity Mode (Mode A):
    - gravity
    - direction
    - speed +-  variance
    - tangential acceleration +- variance
    - radial acceleration +- variance
    - Radius Mode (Mode B):
    - startRadius +- variance
    - endRadius +- variance
    - rotate +- variance
    - Properties common to all modes:
    - life +- life variance
    - start spin +- variance
    - end spin +- variance
    - start size +- variance
    - end size +- variance
    - start color +- variance
    - end color +- variance
    - life +- variance
    - blending function
    - texture

    cocos2d also supports particles generated by Particle Designer (http://particledesigner.71squared.com/).
    'Radius Mode' in Particle Designer uses a fixed emit rate of 30 hz. Since that can't be guarateed in cocos2d,
    cocos2d uses a another approach, but the results are almost identical. 

    cocos2d supports all the variables used by Particle Designer plus a bit more:
    - spinning particles (supported when using CCParticleSystemQuad)
    - tangential acceleration (Gravity mode)
    - radial acceleration (Gravity mode)
    - radius direction (Radius mode) (Particle Designer supports outwards to inwards direction only)

    It is possible to customize any of the above mentioned properties in runtime. Example:

    @code
    emitter.radialAccel = 15;
    emitter.startSpin = 0;
    @endcode

    */
    public class CCParticleSystem : CCNode, ICCTextureProtocol
    {
        public string m_sPlistFile;
	    //! time elapsed since the start of the system (in seconds)
        public float m_fElapsed;

	    // Different modes
	    //! Mode A:Gravity + Tangential Accel + Radial Accel
	    public class sModeA
        {
		    /** Gravity value. Only available in 'Gravity' mode. */
		    public CCPoint gravity;
		    /** speed of each particle. Only available in 'Gravity' mode.  */
		    public float speed;
		    /** speed variance of each particle. Only available in 'Gravity' mode. */
		    public float speedVar;
		    /** tangential acceleration of each particle. Only available in 'Gravity' mode. */
		    public float tangentialAccel;
		    /** tangential acceleration variance of each particle. Only available in 'Gravity' mode. */
		    public float tangentialAccelVar;
		    /** radial acceleration of each particle. Only available in 'Gravity' mode. */
		    public float radialAccel;
		    /** radial acceleration variance of each particle. Only available in 'Gravity' mode. */
		    public float radialAccelVar;
	    };
        public sModeA modeA;

	    //! Mode B: circular movement (gravity, radial accel and tangential accel don't are not used in this mode)
	    public class sModeB
        {
		    /** The starting radius of the particles. Only available in 'Radius' mode. */
		    public float startRadius;
		    /** The starting radius variance of the particles. Only available in 'Radius' mode. */
		    public float startRadiusVar;
		    /** The ending radius of the particles. Only available in 'Radius' mode. */
		    public float endRadius;
		    /** The ending radius variance of the particles. Only available in 'Radius' mode. */
		    public float endRadiusVar;			
		    /** Number of degress to rotate a particle around the source pos per second. Only available in 'Radius' mode. */
		    public float rotatePerSecond;
		    /** Variance in degrees for rotatePerSecond. Only available in 'Radius' mode. */
		    public float rotatePerSecondVar;
	    };
        public sModeB modeB;

	    //! Array of particles
        public CCParticle[] m_pParticles;

	    // color modulate
	    //	BOOL colorModulate;

	    //! How many particles can be emitted per second
        public float m_fEmitCounter;

	    //!  particle idx
	    public uint m_uParticleIdx;

	    // Optimization
	    //CC_UPDATE_PARTICLE_IMP	updateParticleImp;
	    //SEL						updateParticleSel;

	    // profiling
    #if CC_ENABLE_PROFILERS
	    CCProfilingTimer* m_pProfilingTimer;
    #endif

        // Is the emitter active 
        private bool m_bIsActive;
        public bool IsActive
        {
            get
            {
                return m_bIsActive;
            }
        }

        // Quantity of particles that are being simulated at the moment 
        private uint m_uParticleCount;
        public uint ParticleCount
        {
            get
            {
                return m_uParticleCount;
            }
        }

        // How many seconds the emitter wil run. -1 means 'forever' 
        private float m_fDuration;
        public float Duration
        {
            get
            {
                return m_fDuration;
            }
            set
            {
                m_fDuration = value;
            }
        }

        // sourcePosition of the emitter 
        private CCPoint m_tSourcePosition;
        public virtual CCPoint SourcePosition
        {
            get
            {
                return m_tSourcePosition;
            }
            set
            {
                m_tSourcePosition = value;
            }
        }

        // Position variance of the emitter 
        private CCPoint m_tPosVar;
        public virtual CCPoint PosVar
        {
            get
            {
                return m_tPosVar;
            }
            set
            {
                m_tPosVar = value;
            }
        }

        // life, and life variation of each particle 
        private float m_fLife;
        public float Life
        {
            get
            {
                return m_fLife;
            }
            set
            {
                m_fLife = value;
            }
        }

        // life variance of each particle 
        private float m_fLifeVar;
        public float LifeVar
        {
            get
            {
                return m_fLifeVar;
            }
            set
            {
                m_fLifeVar = value;
            }
        }

        // angle and angle variation of each particle 
        private float m_fAngle;
        public float Angle
        {
            get
            {
                return m_fAngle;
            }
            set
            {
                m_fAngle = value;
            }
        }

        // angle variance of each particle 
        private float m_fAngleVar;
        public float AngleVar
        {
            get
            {
                return m_fAngleVar;
            }
            set
            {
                m_fAngleVar = value;
            }
        }


        //////////////////////////////////////////////////////////////////////////
        
        // ParticleSystem - Properties of Gravity Mode 
        public void setTangentialAccel(float t)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.tangentialAccel = t;
        }
        public float getTangentialAccel()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.tangentialAccel;
        }
        public void setTangentialAccelVar(float t)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.tangentialAccelVar = t;
        }
        public float getTangentialAccelVar()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.tangentialAccelVar;
        }	
        public void setRadialAccel(float t)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.radialAccel = t;
        }
        public float getRadialAccel()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.radialAccel;
        }
        public void setRadialAccelVar(float t)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.radialAccelVar = t;
        }
        public float getRadialAccelVar()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.radialAccelVar;
        }
        public  void setGravity(CCPoint g)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.gravity = g;
        }
        public CCPoint getGravity()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.gravity;
        }
        public void setSpeed(float speed)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.speed = speed;
        }
        public float getSpeed()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.speed;
        }
        public void setSpeedVar(float speedVar)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        modeA.speedVar = speedVar;
        }
        public float getSpeedVar()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity, "Particle Mode should be Gravity");
	        return modeA.speedVar;
        }

        // ParticleSystem - Properties of Radius Mode
        public void setStartRadius(float startRadius)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        modeB.startRadius = startRadius;
        }
        public float getStartRadius()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        return modeB.startRadius;
        }
        public void setStartRadiusVar(float startRadiusVar)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        modeB.startRadiusVar = startRadiusVar;
        }
        public float getStartRadiusVar()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        return modeB.startRadiusVar;
        }
        public void setEndRadius(float endRadius)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        modeB.endRadius = endRadius;
        }
        public float getEndRadius()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        return modeB.endRadius;
        }
        public void setEndRadiusVar(float endRadiusVar)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        modeB.endRadiusVar = endRadiusVar;
        }
        public float getEndRadiusVar()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        return modeB.endRadiusVar;
        }
        public void setRotatePerSecond(float degrees)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        modeB.rotatePerSecond = degrees;
        }
        public float getRotatePerSecond()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        return modeB.rotatePerSecond;
        }
        public void setRotatePerSecondVar(float degrees)
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        modeB.rotatePerSecondVar = degrees;
        }
        public float getRotatePerSecondVar()
        {
	        Debug.Assert( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius, "Particle Mode should be Radius");
	        return modeB.rotatePerSecondVar;
        }
        //////////////////////////////////////////////////////////////////////////
	
	    /** start size in pixels of each particle */
	    float m_fStartSize;
        public float StartSize
        {
            get
            {
                return m_fStartSize;
            }
            set
            {
                m_fStartSize = value;
            }
        }

        /** size variance in pixels of each particle */
	    float m_fStartSizeVar;
        public float StartSizeVar
        {
            get
            {
                return m_fStartSizeVar;
            }
            set
            {
                m_fStartSizeVar = value;
            }
        }

            
        /** end size in pixels of each particle */
	    float m_fEndSize;
        public float EndSize
        {
            get
            {
                return m_fEndSize;
            }
            set
            {
                m_fEndSize = value;
            }
        }
	    /** end size variance in pixels of each particle */
	    float m_fEndSizeVar;
        public float EndSizeVar
        {
            get
            {
                return m_fEndSizeVar;
            }
            set
            {
                m_fEndSizeVar = value;
            }
        }
	    /** start color of each particle */
	    ccColor4F m_tStartColor = new ccColor4F();
        public ccColor4F StartColor
        {
            get
            {
                return m_tStartColor;
            }
            set
            {
                m_tStartColor = value;
            }
        }
	    /** start color variance of each particle */
	    ccColor4F m_tStartColorVar = new ccColor4F();
        public ccColor4F StartColorVar
        {
            get
            {
                return m_tStartColorVar;
            }
            set
            {
                m_tStartColorVar = value;
            }
        }
	    /** end color and end color variation of each particle */
	    ccColor4F m_tEndColor = new ccColor4F();
        public ccColor4F EndColor
        {
            get
            {
                return m_tEndColor;
            }
            set
            {
                m_tEndColor = value;
            }
        }
	    /** end color variance of each particle */
	    ccColor4F m_tEndColorVar = new ccColor4F();
        public ccColor4F EndColorVar
        {
            get
            {
                return m_tEndColorVar;
            }
            set
            {
                m_tEndColorVar = value;
            }
        }
	    //* initial angle of each particle
	    float m_fStartSpin;
        public float StartSpin
        {
            get
            {
                return m_fStartSpin;
            }
            set
            {
                m_fStartSpin = value;
            }
        }
	    //* initial angle of each particle
	    float m_fStartSpinVar;
        public float StartSpinVar
        {
            get
            {
                return m_fStartSpinVar;
            }
            set
            {
                m_fStartSpinVar = value;
            }
        }
	    //* initial angle of each particle
	    float m_fEndSpin;
        public float EndSpin
        {
            get
            {
                return m_fEndSpin;
            }
            set
            {
                m_fEndSpin = value;
            }
        }
	    //* initial angle of each particle
	    float m_fEndSpinVar;
        public float EndSpinVar
        {
            get
            {
                return m_fEndSpinVar;
            }
            set
            {
                m_fEndSpinVar = value;
            }
        }
	    /** emission rate of the particles */
	    float m_fEmissionRate;
        public float EmissionRate
        {
            get
            {
                return m_fEmissionRate;
            }
            set
            {
                m_fEmissionRate = value;
            }
        }
	    /** maximum particles of the system */
	    uint m_uTotalParticles;
        public uint TotalParticles
        {
            get
            {
                return m_uTotalParticles;
            }
            set
            {
                m_uTotalParticles = value;
            }
        }
	    /** conforms to CocosNodeTexture protocol */
	    CCTexture2D m_pTexture;
        public virtual CCTexture2D Texture
        {
            get
            {
                return m_pTexture;
            }
            set
            {
                m_pTexture = value;
            }
        }

	    /** conforms to CocosNodeTexture protocol */
	    ccBlendFunc m_tBlendFunc = new ccBlendFunc();
        public ccBlendFunc BlendFunc
        {
            get
            {
                return m_tBlendFunc;
            }
            set
            {
                m_tBlendFunc = value;
            }
        }
	    /** whether or not the particles are using blend additive.
	    If enabled, the following blending function will be used.
	    @code
	    source blend function = GL_SRC_ALPHA;
	    dest blend function = GL_ONE;
	    @endcode
	    */
	    bool m_bIsBlendAdditive;
        public bool IsBlendAdditive
        {
            get
            {
                return m_bIsBlendAdditive;
            }
            set
            {
                m_bIsBlendAdditive = value;
            }
        }
	    /** particles movement type: Free or Grouped
	    @since v0.8
	    */
	    eParticlePositionType m_ePositionType;
        public eParticlePositionType PositionType
        {
            get
            {
                return m_ePositionType;
            }
            set
            {
                m_ePositionType = value;
            }
        }
        /** whether or not the node will be auto-removed when it has no particles left.
	    By default it is false.
	    @since v0.8
	    */
	    bool m_bIsAutoRemoveOnFinish;
        public bool IsAutoRemoveOnFinish
        {
            get
            {
                return m_bIsAutoRemoveOnFinish;
            }
            set
            {
                m_bIsAutoRemoveOnFinish = value;
            }
        }
	    /** Switch between different kind of emitter modes:
	    - kCCParticleModeGravity: uses gravity, speed, radial and tangential acceleration
	    - kCCParticleModeRadius: uses radius movement + rotation
	    */
	    int m_nEmitterMode;
        public int EmitterMode
        {
            get
            {
                return m_nEmitterMode;
            }
            set
            {
                m_nEmitterMode = value;
            }
        }

        public CCParticleSystem()
        {
            m_sPlistFile = "";
	        m_fElapsed = 0;
	        m_pParticles = null;
	        m_fEmitCounter = 0;
	        m_uParticleIdx = 0;
        #if CC_ENABLE_PROFILERS
	        m_pProfilingTimer = NULL;
        #endif
	        m_bIsActive = true;
	        m_uParticleCount = 0;
	        m_fDuration = 0;
	        m_tSourcePosition = new CCPoint(0,0);
	        m_tPosVar = new CCPoint(0,0);
	        m_fLife = 0;
	        m_fLifeVar = 0;
	        m_fAngle = 0;
	        m_fAngleVar = 0;
	        m_fStartSize = 0;
	        m_fStartSizeVar = 0;
	        m_fEndSize = 0;
	        m_fEndSizeVar = 0;
	        m_fStartSpin = 0;
	        m_fStartSpinVar = 0;
	        m_fEndSpin = 0;
	        m_fEndSpinVar = 0;
	        m_fEmissionRate = 0;
	        m_uTotalParticles = 0;
	        m_pTexture = null;
	        m_bIsBlendAdditive = false;
	        m_ePositionType = eParticlePositionType.kCCPositionTypeFree;
	        m_bIsAutoRemoveOnFinish = false;
	        m_nEmitterMode = (int)eParticleMode.kCCParticleModeGravity;
            modeA = new sModeA();
	        modeA.gravity = new CCPoint(0,0);
	        modeA.speed = 0;
	        modeA.speedVar = 0;
	        modeA.tangentialAccel = 0;
	        modeA.tangentialAccelVar = 0;
	        modeA.radialAccel = 0;
	        modeA.radialAccelVar = 0;
            modeB = new sModeB();
	        modeB.startRadius = 0;
	        modeB.startRadiusVar = 0;
	        modeB.endRadius = 0;
	        modeB.endRadiusVar = 0;			
	        modeB.rotatePerSecond = 0;
	        modeB.rotatePerSecondVar = 0;
            m_tBlendFunc = new ccBlendFunc();
	        m_tBlendFunc.src = 0;// CC_BLEND_SRC;
            m_tBlendFunc.dst = 0x0303;// CC_BLEND_DST;
        }


	    ~CCParticleSystem()
        {
        }

	    /** creates an initializes a CCParticleSystem from a plist file.
	    This plist files can be creted manually or with Particle Designer:
	    http://particledesigner.71squared.com/
	    @since v0.99.3
	    */
	    public static CCParticleSystem particleWithFile(string plistFile)
        {
            CCParticleSystem pRet = new CCParticleSystem();
	        if (null != pRet && pRet.initWithFile(plistFile))
	        {
		        return pRet;
	        }
	        return pRet;
        }

	    /** initializes a CCParticleSystem from a plist file.
	    This plist files can be creted manually or with Particle Designer:
	    http://particledesigner.71squared.com/
	    @since v0.99.3
	    */
	    public bool initWithFile(string plistFile)
        {
          	m_sPlistFile = CCFileUtils.fullPathFromRelativePath(plistFile);
	        Dictionary<string, object> dict = CCFileUtils.dictionaryWithContentsOfFile(m_sPlistFile);

	        Debug.Assert( dict != null, "Particles: file not found");
	        return initWithDictionary(dict);
        }

	    /** initializes a CCQuadParticleSystem from a CCDictionary.
	    @since v0.99.3
	    */
        bool initWithDictionary(Dictionary<string, object> dictionary)
        {
	        bool bRet = false;
	        string buffer;
	        string deflated;
	        CCTexture2D image;
	        do 
	        {
		        int maxParticles = int.Parse(ChangeToZeroIfNull(valueForKey("maxParticles", dictionary)));
		        // self, not super
		        if(initWithTotalParticles((uint)maxParticles))
		        {
			        // angle
			        m_fAngle = float.Parse(ChangeToZeroIfNull(valueForKey("angle", dictionary)));
			        m_fAngleVar = float.Parse(ChangeToZeroIfNull(valueForKey("angleVariance", dictionary)));

			        // duration
			        m_fDuration = float.Parse(ChangeToZeroIfNull(valueForKey("duration", dictionary)));

			        // blend function 
			        m_tBlendFunc.src = (uint)int.Parse(ChangeToZeroIfNull(valueForKey("blendFuncSource", dictionary)));
			        m_tBlendFunc.dst = (uint)int.Parse(ChangeToZeroIfNull(valueForKey("blendFuncDestination", dictionary)));

			        // color
			        m_tStartColor.r = float.Parse(ChangeToZeroIfNull(valueForKey("startColorRed", dictionary)));
			        m_tStartColor.g = float.Parse(ChangeToZeroIfNull(valueForKey("startColorGreen", dictionary)));
			        m_tStartColor.b = float.Parse(ChangeToZeroIfNull(valueForKey("startColorBlue", dictionary)));
			        m_tStartColor.a = float.Parse(ChangeToZeroIfNull(valueForKey("startColorAlpha", dictionary)));

			        m_tStartColorVar.r = float.Parse(ChangeToZeroIfNull(valueForKey("startColorVarianceRed", dictionary)));
			        m_tStartColorVar.g = float.Parse(ChangeToZeroIfNull(valueForKey("startColorVarianceGreen", dictionary)));
			        m_tStartColorVar.b = float.Parse(ChangeToZeroIfNull(valueForKey("startColorVarianceBlue", dictionary)));
			        m_tStartColorVar.a = float.Parse(ChangeToZeroIfNull(valueForKey("startColorVarianceAlpha", dictionary)));

			        m_tEndColor.r = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorRed", dictionary)));
			        m_tEndColor.g = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorGreen", dictionary)));
			        m_tEndColor.b = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorBlue", dictionary)));
			        m_tEndColor.a = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorAlpha", dictionary)));

			        m_tEndColorVar.r = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorVarianceRed", dictionary)));
			        m_tEndColorVar.g = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorVarianceGreen", dictionary)));
			        m_tEndColorVar.b = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorVarianceBlue", dictionary)));
			        m_tEndColorVar.a = float.Parse(ChangeToZeroIfNull(valueForKey("finishColorVarianceAlpha", dictionary)));

			        // particle size
			        m_fStartSize = float.Parse(ChangeToZeroIfNull(valueForKey("startParticleSize", dictionary)));
			        m_fStartSizeVar = float.Parse(ChangeToZeroIfNull(valueForKey("startParticleSizeVariance", dictionary)));
			        m_fEndSize = float.Parse(ChangeToZeroIfNull(valueForKey("finishParticleSize", dictionary)));
			        m_fEndSizeVar = float.Parse(ChangeToZeroIfNull(valueForKey("finishParticleSizeVariance", dictionary)));

			        // position
                    float x = float.Parse(ChangeToZeroIfNull(valueForKey("sourcePositionx", dictionary)));
                    float y = float.Parse(ChangeToZeroIfNull(valueForKey("sourcePositiony", dictionary)));
                    this.position = new CCPoint(x,y);			
                    m_tPosVar.x = float.Parse(ChangeToZeroIfNull(valueForKey("sourcePositionVariancex", dictionary)));
			        m_tPosVar.y = float.Parse(ChangeToZeroIfNull(valueForKey("sourcePositionVariancey", dictionary)));

			        // Spinning
			        m_fStartSpin = float.Parse(ChangeToZeroIfNull(valueForKey("rotationStart", dictionary)));
			        m_fStartSpinVar = float.Parse(ChangeToZeroIfNull(valueForKey("rotationStartVariance", dictionary)));
			        m_fEndSpin= float.Parse(ChangeToZeroIfNull(valueForKey("rotationEnd", dictionary)));
			        m_fEndSpinVar= float.Parse(ChangeToZeroIfNull(valueForKey("rotationEndVariance", dictionary)));

			        m_nEmitterMode = int.Parse(ChangeToZeroIfNull(valueForKey("emitterType", dictionary)));

			        // Mode A: Gravity + tangential accel + radial accel
			        if( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity ) 
			        {
				        // gravity
				        modeA.gravity.x = float.Parse(ChangeToZeroIfNull(valueForKey("gravityx", dictionary)));
				        modeA.gravity.y = float.Parse(ChangeToZeroIfNull(valueForKey("gravityy", dictionary)));

				        // speed
				        modeA.speed = float.Parse(ChangeToZeroIfNull(valueForKey("speed", dictionary)));
				        modeA.speedVar = float.Parse(ChangeToZeroIfNull(valueForKey("speedVariance", dictionary)));

                        string pszTmp;
				        // radial acceleration
                        pszTmp = valueForKey("radialAcceleration", dictionary);
                        modeA.radialAccel = (pszTmp != null) ? float.Parse(ChangeToZeroIfNull(pszTmp)) : 0;

                        pszTmp = valueForKey("radialAccelVariance", dictionary);
				        modeA.radialAccelVar = (pszTmp != null) ? float.Parse(ChangeToZeroIfNull(pszTmp)) : 0;

				        // tangential acceleration
                        pszTmp = valueForKey("tangentialAcceleration", dictionary);
				        modeA.tangentialAccel = (pszTmp != null) ? float.Parse(ChangeToZeroIfNull(pszTmp)) : 0;

                        pszTmp = valueForKey("tangentialAccelVariance", dictionary);
				        modeA.tangentialAccelVar = (pszTmp != null) ? float.Parse(ChangeToZeroIfNull(pszTmp)) : 0;
			        }

			        // or Mode B: radius movement
			        else if( m_nEmitterMode == (int)eParticleMode.kCCParticleModeRadius ) 
			        {
				        modeB.startRadius = float.Parse(ChangeToZeroIfNull(valueForKey("maxRadius", dictionary)));
				        modeB.startRadiusVar = float.Parse(ChangeToZeroIfNull(valueForKey("maxRadiusVariance", dictionary)));
				        modeB.endRadius = float.Parse(ChangeToZeroIfNull(valueForKey("minRadius", dictionary)));
				        modeB.endRadiusVar = 0;
				        modeB.rotatePerSecond = float.Parse(ChangeToZeroIfNull(valueForKey("rotatePerSecond", dictionary)));
				        modeB.rotatePerSecondVar = float.Parse(ChangeToZeroIfNull(valueForKey("rotatePerSecondVariance", dictionary)));

			        } else {
				        Debug.Assert(false, "Invalid emitterType in config file");
				        break;
			        }

			        // life span
			        m_fLife = float.Parse(ChangeToZeroIfNull(valueForKey("particleLifespan", dictionary)));
			        m_fLifeVar = float.Parse(ChangeToZeroIfNull(valueForKey("particleLifespanVariance", dictionary)));

			        // emission Rate
			        m_fEmissionRate = m_uTotalParticles / m_fLife;

			        // texture		
			        // Try to get the texture from the cache
			        string textureName = valueForKey("textureFileName", dictionary);
                    string fullpath = CCFileUtils.fullPathFromRelativeFile(textureName, m_sPlistFile);

			        CCTexture2D tex = null;

                    if (textureName.Length > 0)
                    {
                        // set not pop-up message box when load image failed
                        bool bNotify = CCFileUtils.IsPopupNotify;
                        CCFileUtils.IsPopupNotify = false;
                        tex = CCTextureCache.sharedTextureCache().addImage(fullpath);

                        // reset the value of UIImage notify
                        CCFileUtils.IsPopupNotify = bNotify;
                    }

			        if (null != tex)
			        {
				        m_pTexture = tex;
			        }
                    else
                    {
                        throw new NotImplementedException();

                    //    string textureData = valueForKey("textureImageData", dictionary);
                    //    Debug.Assert(textureData != null);

                    //    int dataLen = textureData.Length;
                    //    if (dataLen != 0)
                    //    {
                    //        // if it fails, try to get it from the base64-gzipped data	
                    //        int decodeLen = base64Decode(textureData, (uint)dataLen, buffer);
                    //        Debug.Assert(buffer != null, "CCParticleSystem: error decoding textureImageData");
                    //        if (buffer == null)
                    //            break;

                    //        int deflatedLen = ZipUtils.ccInflateMemory(buffer, decodeLen, &deflated);
                    //        Debug.Assert(deflated != null, "CCParticleSystem: error ungzipping textureImageData");
                    //        if (deflated == null)
                    //            break;

                    //        image = new CCTexture2D();
                    //        bool isOK = image.initWithImageData(deflated, deflatedLen);
                    //        Debug.Assert(isOK, "CCParticleSystem: error init image with Data");
                    //        if (!isOK)
                    //            break;

                    //        m_pTexture = CCTextureCache.sharedTextureCache().addUIImage(image, fullpath);
                    //    }
                    }
			        Debug.Assert(m_pTexture != null, "CCParticleSystem: error loading the texture");
			
			        if(m_pTexture == null)
                        break;
			        bRet = true;
		        }
	        } while (false);
	        return bRet;
        }

        //! Initializes a system with a fixed number of particles
        public virtual bool initWithTotalParticles(uint numberOfParticles)
        {
            m_uTotalParticles = numberOfParticles;

            m_pParticles = new CCParticle[m_uTotalParticles];

            if (null == m_pParticles)
            {
                Debug.WriteLine("Particle system: not enough memory");
                return false;
            }

            for (int i = 0; i < m_uTotalParticles; i++)
            {
                m_pParticles[i] = new CCParticle();
            }

            // default, active
            m_bIsActive = true;

            // default blend function
            m_tBlendFunc.src = 1;// CC_BLEND_SRC;
            m_tBlendFunc.dst = 0x0303;// CC_BLEND_DST;

            // default movement type;
            m_ePositionType = eParticlePositionType.kCCPositionTypeFree;

            // by default be in mode A:
            m_nEmitterMode = (int)eParticleMode.kCCParticleModeGravity;

            // default: modulate
            // XXX: not used
            //	colorModulate = YES;

            m_bIsAutoRemoveOnFinish = false;

            // profiling
#if CC_ENABLE_PROFILERS
	/// @todo _profilingTimer = [[CCProfiler timerWithName:@"particle system" andInstance:self] retain];
#endif

            // Optimization: compile udpateParticle method
            //updateParticleSel = @selector(updateQuadWithParticle:newPosition:);
            //updateParticleImp = (CC_UPDATE_PARTICLE_IMP) [self methodForSelector:updateParticleSel];

            // udpate after action in run!
            this.scheduleUpdateWithPriority(1);

            return true;
        
        }


        //! Add a particle to the emitter
        public bool addParticle()
        {
            if (this.isFull())
            {
                return false;
            }

            CCParticle particle = m_pParticles[m_uParticleCount];
            this.initParticle(particle);
            ++m_uParticleCount;

            return true;        
        }
        //! Initializes a particle
        public void initParticle(CCParticle particle)
        {
            Debug.Assert(null != particle, "particle shouldn't be null.");

        	// timeToLive
	        // no negative life. prevent division by 0
	        particle.timeToLive = m_fLife + m_fLifeVar * ccMacros.CCRANDOM_MINUS1_1();
	        particle.timeToLive = (((0) > (particle.timeToLive)) ? (0) : (particle.timeToLive));

	        // position
	        particle.pos.x = m_tSourcePosition.x + m_tPosVar.x * ccMacros.CCRANDOM_MINUS1_1();
            particle.pos.x *= CCDirector.sharedDirector().ContentScaleFactor;
	        particle.pos.y = m_tSourcePosition.y + m_tPosVar.y * ccMacros.CCRANDOM_MINUS1_1();
            particle.pos.y *= CCDirector.sharedDirector().ContentScaleFactor;

	        // Color
	        ccColor4F start = new ccColor4F();
	        start.r = CCPointExtension.clampf(m_tStartColor.r + m_tStartColorVar.r * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);
            start.g = CCPointExtension.clampf(m_tStartColor.g + m_tStartColorVar.g * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);
            start.b = CCPointExtension.clampf(m_tStartColor.b + m_tStartColorVar.b * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);
            start.a = CCPointExtension.clampf(m_tStartColor.a + m_tStartColorVar.a * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);

	        ccColor4F end = new ccColor4F();
            end.r = CCPointExtension.clampf(m_tEndColor.r + m_tEndColorVar.r * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);
            end.g = CCPointExtension.clampf(m_tEndColor.g + m_tEndColorVar.g * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);
            end.b = CCPointExtension.clampf(m_tEndColor.b + m_tEndColorVar.b * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);
            end.a = CCPointExtension.clampf(m_tEndColor.a + m_tEndColorVar.a * ccMacros.CCRANDOM_MINUS1_1(), 0, 1);

	        particle.color = start;
	        particle.deltaColor.r = (end.r - start.r) / particle.timeToLive;
	        particle.deltaColor.g = (end.g - start.g) / particle.timeToLive;
	        particle.deltaColor.b = (end.b - start.b) / particle.timeToLive;
	        particle.deltaColor.a = (end.a - start.a) / particle.timeToLive;

	        // size
	        float startS = m_fStartSize + m_fStartSizeVar * ccMacros.CCRANDOM_MINUS1_1();
	        startS = (((0) > (startS)) ? (0) : (startS)); // No negative value
            startS *= CCDirector.sharedDirector().ContentScaleFactor;

	        particle.size = startS;

	        if( m_fEndSize == (float)eParticleShowingProperty.kCCParticleStartSizeEqualToEndSize )
	        {
		        particle.deltaSize = 0;
	        }
	        else
	        {
		        float endS = m_fEndSize + m_fEndSizeVar * ccMacros.CCRANDOM_MINUS1_1();
		        endS = (((0) > (endS)) ? (0) : (endS)); // No negative values
                endS *= CCDirector.sharedDirector().ContentScaleFactor;
		        particle.deltaSize = (endS - startS) / particle.timeToLive;
	        }

	        // rotation
	        float startA = m_fStartSpin + m_fStartSpinVar * ccMacros.CCRANDOM_MINUS1_1();
	        float endA = m_fEndSpin + m_fEndSpinVar * ccMacros.CCRANDOM_MINUS1_1();
	        particle.rotation = startA;
	        particle.deltaRotation = (endA - startA) / particle.timeToLive;

	        // position
	        if( m_ePositionType == eParticlePositionType.kCCPositionTypeFree )
	        {
                CCPoint p = this.convertToWorldSpace(new CCPoint(0,0));
		        particle.startPos = CCPointExtension.ccpMult( p, CCDirector.sharedDirector().ContentScaleFactor );
	        }
            else if ( m_ePositionType == eParticlePositionType.kCCPositionTypeRelative )
            {
                particle.startPos = CCPointExtension.ccpMult( m_tPosition, CCDirector.sharedDirector().ContentScaleFactor );
            }

	        // direction
            float a = ccMacros.CC_DEGREES_TO_RADIANS(m_fAngle + m_fAngleVar * ccMacros.CCRANDOM_MINUS1_1());

	        // Mode Gravity: A
	        if( m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity ) 
	        {
		        CCPoint v = new CCPoint( (float)System.Math.Cos( a ), (float)System.Math.Sin( a ));
		        float s = modeA.speed + modeA.speedVar * ccMacros.CCRANDOM_MINUS1_1();
                s *= CCDirector.sharedDirector().ContentScaleFactor;

		        // direction
		        particle.modeA.dir = CCPointExtension.ccpMult( v, s );

		        // radial accel
		        particle.modeA.radialAccel = modeA.radialAccel + modeA.radialAccelVar * ccMacros.CCRANDOM_MINUS1_1();
                particle.modeA.radialAccel *= CCDirector.sharedDirector().ContentScaleFactor;

		        // tangential accel
		        particle.modeA.tangentialAccel = modeA.tangentialAccel + modeA.tangentialAccelVar * ccMacros.CCRANDOM_MINUS1_1();
                particle.modeA.tangentialAccel *= CCDirector.sharedDirector().ContentScaleFactor;
            }

	        // Mode Radius: B
	        else {
		        // Set the default diameter of the particle from the source position
		        float startRadius = modeB.startRadius + modeB.startRadiusVar * ccMacros.CCRANDOM_MINUS1_1();
		        float endRadius = modeB.endRadius + modeB.endRadiusVar * ccMacros.CCRANDOM_MINUS1_1();
                startRadius *= CCDirector.sharedDirector().ContentScaleFactor;
                endRadius *= CCDirector.sharedDirector().ContentScaleFactor;

		        particle.modeB.radius = startRadius;

		        if( modeB.endRadius == (float)eParticleShowingProperty.kCCParticleStartRadiusEqualToEndRadius )
			        particle.modeB.deltaRadius = 0;
		        else
			        particle.modeB.deltaRadius = (endRadius - startRadius) / particle.timeToLive;

		        particle.modeB.angle = a;
                particle.modeB.degreesPerSecond = ccMacros.CC_DEGREES_TO_RADIANS(modeB.rotatePerSecond + modeB.rotatePerSecondVar * ccMacros.CCRANDOM_MINUS1_1());
	        }	
        }

        //! stop emitting particles. Running particles will continue to run until they die
        public void stopSystem()
        {
            m_bIsActive = false;
            m_fElapsed = m_fDuration;
            m_fEmitCounter = 0;
        }

        //! Kill all living particles.
        public void resetSystem()
        {
            m_bIsActive = true;
            m_fElapsed = 0;
            for (m_uParticleIdx = 0; m_uParticleIdx < m_uParticleCount; ++m_uParticleIdx)
            {
                CCParticle p = m_pParticles[m_uParticleIdx];
                p.timeToLive = 0;
            }
        }

        //! whether or not the system is full
        public bool isFull()
        {
            return (m_uParticleCount == m_uTotalParticles);
        }

        //! should be overriden by subclasses
        public virtual void updateQuadWithParticle(CCParticle particle, CCPoint newPosition)
        {
            // should be overriden
        }

        //! should be overriden by subclasses
        public virtual void postStep()
        {
            // should be overriden
        }

        public override void update(float dt)
        {
            if (m_bIsActive && m_fEmissionRate != 0)
            {
                float rate = 1.0f / m_fEmissionRate;
                m_fEmitCounter += dt;
                while (m_uParticleCount < m_uTotalParticles && m_fEmitCounter > rate)
                {
                    this.addParticle();
                    m_fEmitCounter -= rate;
                }

                m_fElapsed += dt;
                if (m_fDuration != -1 && m_fDuration < m_fElapsed)
                {
                    this.stopSystem();
                }
            }

            m_uParticleIdx = 0;


#if CC_ENABLE_PROFILERS
	/// @todo CCProfilingBeginTimingBlock(_profilingTimer);
#endif


            CCPoint currentPosition = new CCPoint(0,0);
            if (m_ePositionType == eParticlePositionType.kCCPositionTypeFree)
            {
                currentPosition = this.convertToWorldSpace(new CCPoint(0,0));
                currentPosition.x *= CCDirector.sharedDirector().ContentScaleFactor;
                currentPosition.y *= CCDirector.sharedDirector().ContentScaleFactor;
            }
            else if (m_ePositionType == eParticlePositionType.kCCPositionTypeRelative)
            {
                currentPosition = m_tPosition;
                currentPosition.x *= CCDirector.sharedDirector().ContentScaleFactor;
                currentPosition.y *= CCDirector.sharedDirector().ContentScaleFactor;
            }

            while (m_uParticleIdx < m_uParticleCount)
            {
                CCParticle p = m_pParticles[m_uParticleIdx];

                // life
                p.timeToLive -= dt;

                if (p.timeToLive > 0)
                {
                    // Mode A: gravity, direction, tangential accel & radial accel
                    if (m_nEmitterMode == (int)eParticleMode.kCCParticleModeGravity)
                    {
                        CCPoint tmp, radial, tangential;

                        radial = new CCPoint(0,0);
                        // radial acceleration
                        if (p.pos.x != 0 || p.pos.y != 0)
                            radial = CCPointExtension.ccpNormalize(p.pos);
                        tangential = radial;
                        radial = CCPointExtension.ccpMult(radial, p.modeA.radialAccel);

                        // tangential acceleration
                        float newy = tangential.x;
                        tangential.x = -tangential.y;
                        tangential.y = newy;
                        tangential = CCPointExtension.ccpMult(tangential, p.modeA.tangentialAccel);

                        // (gravity + radial + tangential) * dt
                        tmp = CCPointExtension.ccpAdd(CCPointExtension.ccpAdd(radial, tangential), modeA.gravity);
                        tmp = CCPointExtension.ccpMult(tmp, dt);
                        p.modeA.dir = CCPointExtension.ccpAdd(p.modeA.dir, tmp);
                        tmp = CCPointExtension.ccpMult(p.modeA.dir, dt);
                        p.pos = CCPointExtension.ccpAdd(p.pos, tmp);
                    }

                    // Mode B: radius movement
                    else
                    {
                        // Update the angle and radius of the particle.
                        p.modeB.angle += p.modeB.degreesPerSecond * dt;
                        p.modeB.radius += p.modeB.deltaRadius * dt;

                        p.pos.x = -(float)System.Math.Cos(p.modeB.angle) * p.modeB.radius;
                        p.pos.y = -(float)System.Math.Sin(p.modeB.angle) * p.modeB.radius;
                    }

                    // color
                    p.color.r += (p.deltaColor.r * dt);
                    p.color.g += (p.deltaColor.g * dt);
                    p.color.b += (p.deltaColor.b * dt);
                    p.color.a += (p.deltaColor.a * dt);

                    // size
                    p.size += (p.deltaSize * dt);
                    p.size = (((0) > (p.size)) ? (0) : (p.size));

                    // angle
                    p.rotation += (p.deltaRotation * dt);

                    //
                    // update values in quad
                    //

                    CCPoint newPos;

                    if (m_ePositionType == eParticlePositionType.kCCPositionTypeFree || m_ePositionType == eParticlePositionType.kCCPositionTypeRelative)
                    {
                        CCPoint diff = CCPointExtension.ccpSub(currentPosition, p.startPos);
                        newPos = CCPointExtension.ccpSub(p.pos, diff);
                    }
                    else
                    {
                        newPos = p.pos;
                    }

                    updateQuadWithParticle(p, newPos);
                    //updateParticleImp(self, updateParticleSel, p, newPos);

                    // update particle counter
                    ++m_uParticleIdx;

                }
                else
                {
                    // life < 0
                    if (m_uParticleIdx != m_uParticleCount - 1)
                    {
                        m_pParticles[m_uParticleIdx].copy(m_pParticles[m_uParticleCount - 1]);
                    }
                    --m_uParticleCount;

                    if (m_uParticleCount == 0 && m_bIsAutoRemoveOnFinish)
                    {
                        this.unscheduleUpdate();
                        m_pParent.removeChild(this, true);
                        return;
                    }
                }
            }

#if CC_ENABLE_PROFILERS
	/// @todo CCProfilingEndTimingBlock(_profilingTimer);
#endif

            //#ifdef CC_USES_VBO
            this.postStep();
            //#endif

        }

        
        /** Private method, return the string found by key in dict.
        @return "" if not found; return the string if found.
        */
        string valueForKey(string key, Dictionary<string, object> dict)
        {
            if (null != dict)
            {
                object val = new object();
                
                return dict.TryGetValue(key, out val) ? (val as string) : "";
            }
            return "";
        }

        string ChangeToZeroIfNull(string str)
        {
            if ("" == str)
            {
                str = "0";
            }
            return str;
        }
    };
}
