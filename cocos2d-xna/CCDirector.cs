using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace cocos2d
{
    public abstract class CCDirector : CCObject
    {
        readonly double kDefaultFPS = 60;

        #region singleton stuff

        /** There are 4 types of Director.
        - kCCDirectorTypeNSTimer (default)
        - kCCDirectorTypeMainLoop
        - kCCDirectorTypeThreadMainLoop
        - kCCDirectorTypeDisplayLink

        Each Director has it's own benefits, limitations.
        Now we only support DisplayLink director, so it has not effect. 
       */

        ///<summary>
        /// This method should be called before any other call to the director.
        ///@since v0.8.2
        /// </summary>
        public static bool setDirectorType(ccDirectorType obDirectorType)
        {
            // we only support CCDisplayLinkDirector
            CCDirector.sharedDirector();

            return true;
        }

        static CCDirector s_sharedDirector = new CCDisplayLinkDirector();
        static bool s_bFirstRun = true;

        /// <summary>
        /// returns a shared instance of the director
        /// </summary>
        /// <returns></returns>
        public static CCDirector sharedDirector()
        {
            if (s_bFirstRun)
            {
                s_sharedDirector.init();
                s_bFirstRun = false;
            }

            return s_sharedDirector;
        }

        #endregion

        #region Protected

        protected void purgeDirector()
        {
            // don't release the event handlers
            // They are needed in case the director is run again
            //CCTouchDispatcher::sharedDispatcher()->removeAllDelegates();

            if (m_pRunningScene != null)
            {
                //m_pRunningScene->onExit();
                //m_pRunningScene->cleanup();
                //m_pRunningScene->release();
            }

            m_pRunningScene = null;
            m_pNextScene = null;

            // remove all objects, but don't release it.
            // runWithScene might be executed after 'end'.
            m_pobScenesStack.Clear();

            stopAnimation();

#if CC_DIRECTOR_FAST_FPS
            //CC_SAFE_RELEASE_NULL(m_pFPSLabel);
#endif

            //CC_SAFE_RELEASE_NULL(m_pProjectionDelegate);

            // purge bitmap cache
            //CCLabelBMFont::purgeCachedData();

            // purge all managers
            //CCAnimationCache::purgeSharedAnimationCache();
            //CCSpriteFrameCache::purgeSharedSpriteFrameCache();
            //CCActionManager::sharedManager()->purgeSharedManager();
            //CCScheduler::purgeSharedScheduler();
            //CCTextureCache::purgeSharedTextureCache();

#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)	
	CCUserDefault::purgeSharedUserDefault();
#endif
            // OpenGL view
            //m_pobOpenGLView->release();
            //m_pobOpenGLView = NULL;
        }
        protected bool m_bPurgeDirecotorInNextLoop; // this flag will be set to true in end()

        protected void updateContentScaleFactor()
        {
            throw new NotImplementedException();
        }

        protected void setNextScene()
        {
            // If it is not a transition, call onExit/cleanup
            /*if (! newIsTransition)*/
            if (!(m_pNextScene is CCTransitionScene))
            {
                if (m_pRunningScene != null)
                {
                    m_pRunningScene.onExit();
                }

                // issue #709. the root node (scene) should receive the cleanup message too
                // otherwise it might be leaked.
                if (m_bSendCleanupToScene && m_pRunningScene != null)
                {
                    m_pRunningScene.cleanup();
                }
            }

            if (m_pRunningScene != null)
            {
                //m_pRunningScene.release();
            }
            m_pRunningScene = m_pNextScene;
            // m_pNextScene.retain();
            m_pNextScene = null;

            if (!(m_pRunningScene is CCTransitionScene) && m_pRunningScene != null)
            {
                m_pRunningScene.onEnter();
                m_pRunningScene.onEnterTransitionDidFinish();
            }
        }

#if CC_DIRECTOR_FAST_FPS
        /** shows the FPS in the screen */
        protected void showFPS()
        {
            throw new NotImplementedException();
        }
#else
        protected void showFPS()
        {
            throw new NotImplementedException();
        }
#endif // CC_DIRECTOR_FAST_FPS

        /** calculates delta time since last time it was called */
        protected void calculateDeltaTime()
        {
            throw new NotImplementedException();
        }

        protected double m_dAnimationInterval;
        protected double m_dOldAnimationInterval;

        /* landscape mode ? */
        bool m_bLandscape;

        bool m_bDisplayFPS;
        float m_fAccumDt;
        float m_fFrameRate;
#if	CC_DIRECTOR_FAST_FPS
        //CCLabelTTF *m_pFPSLabel;
#endif

        /* is the running scene paused */
        bool m_bPaused;

        /* How many frames were called since the director started */
        uint m_uTotalFrames;
        uint m_uFrames;

        /* The running scene */
        CCScene m_pRunningScene;

        /* will be the next 'runningScene' in the next frame
         nextScene is a weak reference. */
        CCScene m_pNextScene;

        /* If YES, then "old" scene will receive the cleanup message */
        bool m_bSendCleanupToScene;

        /* scheduled scenes */
        List<CCScene> m_pobScenesStack;

        /* last time the main loop was updated */
        // cc_timeval m_pLastUpdate;

        /* delta time since last tick to main loop */
        float m_fDeltaTime;

        /* whether or not the next delta time will be zero */
        bool m_bNextDeltaTimeZero;

        /* projection used */
        ccDirectorProjection m_eProjection;

        /* window size in points */
        CCSize m_obWinSizeInPoints;

        /* window size in pixels */
        CCSize m_obWinSizeInPixels;

        /* content scale factor */
        float m_fContentScaleFactor;

        /* store the fps string */
        string m_pszFPS;

        /* This object will be visited after the scene. Useful to hook a notification node */
        CCNode m_pNotificationNode;

        /* Projection protocol delegate */
        //CCProjectionProtocol *m_pProjectionDelegate;

        /* The device orientation */
        ccDeviceOrientation m_eDeviceOrientation;
        /* contentScaleFactor could be simulated */
        bool m_bIsContentScaleSupported;

        bool m_bRetinaDisplay;

        #endregion

        public virtual bool init()
        {
            //scene
            m_pRunningScene = null;
            m_pNextScene = null;

            m_pNotificationNode = null;

            m_dOldAnimationInterval = m_dAnimationInterval = 1.0 / kDefaultFPS;
            m_pobScenesStack = new List<CCScene>();

            // Set default projection (3D)
            m_eProjection = ccDirectorProjection.kCCDirectorProjectionDefault;

            // projection delegate if "Custom" projection is used
            //m_pProjectionDelegate = NULL;

            //FPS
            m_bDisplayFPS = false;
            m_uTotalFrames = m_uFrames = 0;
            m_pszFPS = "";
            //m_pLastUpdate = new struct cc_timeval();

            m_bPaused = false;

            //paused?
            m_bPaused = false;

            //purge?
            m_bPurgeDirecotorInNextLoop = false;

            m_obWinSizeInPixels = m_obWinSizeInPoints = new CCSize(0, 0);

            // portrait mode default
            m_eDeviceOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;

            //m_pobOpenGLView = NULL;

            m_bRetinaDisplay = false;
            m_fContentScaleFactor = 1;
            m_bIsContentScaleSupported = false;

            return true;
        }

        // attribute

        /// <summary>
        ///  Get current running Scene. Director can only run one Scene at the time 
        /// </summary>
        CCScene runningScene
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// the FPS value
        /// </summary>
        public virtual double animationInterval
        {
            get
            {
                throw new NotImplementedException();
            }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Whether or not to display the FPS on the bottom-left corner 
        /// </summary>
        /// <returns></returns>
        public bool isDisplayFPS()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Display the FPS on the bottom-left corner
        /// </summary>
        /// <param name="bDisplayFPS"></param>
        public void setDisplayFPS(bool bDisplayFPS)
        {
            //throw new NotImplementedException();
        }

        public bool isNextDeltaTimeZero()
        {
            throw new NotImplementedException();
        }
        public void setNextDeltaTimeZero(bool bNextDeltaTimeZero)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Whether or not the Director is paused
        /// </summary>
        public static bool isPaused()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// How many frames were called since the director started
        /// </summary>
        /// <returns></returns>
        public uint getFrames()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets an OpenGL projection
        ///@since v0.8.2
        /// </summary>
        /// <returns></returns>
        public ccDirectorProjection projection
        {
            get;
            set;
        }

        /** How many frames were called since the director started */


        /** Whether or not the replaced scene will receive the cleanup message.
         If the new scene is pushed, then the old scene won't receive the "cleanup" message.
         If the new scene replaces the old one, the it will receive the "cleanup" message.
         @since v0.99.0
         */
        public bool isSendCleanupToScene() { throw new NotImplementedException(); }


        // window size
        /// <summary>
        /// returns the size of the OpenGL view in points.
        /// It takes into account any possible rotation (device orientation) of the window
        /// </summary>
        /// <returns></returns>
        public CCSize getWinSize()
        {
            CCSize s = new CCSize(800, 480);

            return s;
        }

        /// <summary>
        /// returns the size of the OpenGL view in pixels.
        /// It takes into account any possible rotation (device orientation) of the window.
        /// On Mac winSize and winSizeInPixels return the same value.
        /// </summary>
        /// <returns></returns>
        public CCSize winSizeInPixels
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// returns the display size of the OpenGL view in pixels.
        /// It doesn't take into account any possible rotation of the window.
        /// </summary>
        /// <returns></returns>
        public CCSize displaySizeInPixels
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// changes the projection size
        /// </summary>
        /// <param name="newWindowSize"></param>
        public void reshapeProjection(CCSize newWindowSize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// converts a UIKit coordinate to an OpenGL coordinate
        /// Useful to convert (multi) touches coordinates to the current layout (portrait or landscape)
        /// </summary>
        /// <param name="obPoint"></param>
        /// <returns></returns>
        public CCPoint convertToGL(CCPoint obPoint)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// converts an OpenGL coordinate to a UIKit coordinate
        /// Useful to convert node points to window points for calls such as glScissor
        /// </summary>
        /// <param name="obPoint"></param>
        /// <returns></returns>
        public CCPoint convertToUI(CCPoint obPoint)
        {
            throw new NotImplementedException();
        }

        /// XXX: missing description 
        public float zEye
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // Scene Management

        /// <summary>
        /// Enters the Director's main loop with the given Scene. 
        /// Call it to run only your FIRST scene.
        /// Don't call it if there is already a running scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void runWithScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "pScene cannot be null");
            Debug.Assert(m_pRunningScene == null, "m_pRunningScene cannot be null");

            pushScene(pScene);
            startAnimation();
        }

        /// <summary>
        /// Suspends the execution of the running scene, pushing it on the stack of suspended scenes.
        /// The new scene will be executed.
        /// Try to avoid big stacks of pushed scenes to reduce memory allocation. 
        /// ONLY call it if there is a running scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void pushScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "pScene cannot be null");

            m_bSendCleanupToScene = false;

            m_pobScenesStack.Add(pScene);
            m_pNextScene = pScene;
        }

        /// <summary>
        /// Pops out a scene from the queue.
        /// This scene will replace the running one.
        /// The running scene will be deleted. If there are no more scenes in the stack the execution is terminated.
        /// ONLY call it if there is a running scene.
        /// </summary>
        public void popScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces the running scene with a new one. The running scene is terminated.
        /// ONLY call it if there is a running scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void replaceScene(CCScene pScene)
        {
            throw new NotImplementedException();
        }

        /** Ends the execution, releases the running scene.
         It doesn't remove the OpenGL view from its parent. You have to do it manually.
         */

        /* end is key word of lua, use other name to export to lua. */
        public void endToLua()
        {
            throw new NotImplementedException();
        }

        public void end()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pauses the running scene.
        /// The running scene will be _drawed_ but all scheduled timers will be paused
        /// While paused, the draw rate will be 4 FPS to reduce CPU consumption
        /// </summary>
        public void pause()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resumes the paused scene
        /// The scheduled timers will be activated again.
        /// The "delta time" will be 0 (as if the game wasn't paused)
        /// </summary>
        public void resume()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Stops the animation. Nothing will be drawn. The main loop won't be triggered anymore.
        ///  If you don't want to pause your animation call [pause] instead.
        /// </summary>
        public abstract void stopAnimation();

        /// <summary>
        /// The main loop is triggered again.
        /// Call this function only if [stopAnimation] was called earlier
        /// warning Don't call this function to start the main loop. To run the main loop call runWithScene
        /// </summary>
        public abstract void startAnimation();

        /// <summary>
        /// Draw the scene.
        /// This method is called every frame. Don't call it manually.
        /// </summary>
        protected void drawScene()
        {
            //tick before glClear: issue #533
            if (!m_bPaused)
            {
                //CCScheduler::sharedScheduler()->tick(m_fDeltaTime);
            }

            //glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

            /* to avoid flickr, nextScene MUST be here: after tick and before draw.
             XXX: Which bug is this one. It seems that it can't be reproduced with v0.9 */
            if (m_pNextScene != null)
            {
                setNextScene();
            }

            //glPushMatrix();

            applyOrientation();

            // By default enable VertexArray, ColorArray, TextureCoordArray and Texture2D
            // CC_ENABLE_DEFAULT_GL_STATES();

            // draw the scene
            if (m_pRunningScene != null)
            {
                m_pRunningScene.visit();
            }

            // draw the notifications node
            if (m_pNotificationNode != null)
            {
                m_pNotificationNode.visit();
            }

            if (m_bDisplayFPS)
            {
                showFPS();
            }

#if CC_ENABLE_PROFILERS
	showProfilers();
#endif

            //CC_DISABLE_DEFAULT_GL_STATES();

            //glPopMatrix();

            m_uTotalFrames++;
        }

        // Memory Helper

        /// <summary>
        /// Removes cached all cocos2d cached data.
        /// It will purge the CCTextureCache, CCSpriteFrameCache, CCLabelBMFont cache
        /// @since v0.99.3
        /// </summary>
        public void purgeCachedData()
        {
            throw new NotImplementedException();
        }

        // OpenGL Helper

        /// <summary>
        /// sets the OpenGL default values
        /// </summary>
        public void setGLDefaultValues()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// enables/disables OpenGL alpha blending 
        /// </summary>
        /// <param name="bOn"></param>
        public void setAlphaBlending(bool bOn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// enables/disables OpenGL depth test
        /// </summary>
        /// <param name="bOn"></param>
        public void setDepthTest(bool bOn)
        {
            throw new NotImplementedException();
        }

        public abstract void mainLoop(GameTime gameTime);

        // Profiler
        public void showProfilers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// rotates the screen if an orientation different than Portrait is used 
        /// </summary>
        public void applyOrientation()
        {
            CCSize s = m_obWinSizeInPixels;
            float w = s.width / 2;
            float h = s.height / 2;

            // XXX it's using hardcoded values.
            // What if the the screen size changes in the future?
            switch (m_eDeviceOrientation)
            {
                case ccDeviceOrientation.CCDeviceOrientationPortrait:
                    // nothing
                    break;
                case ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown:
                    // upside down
                    //glTranslatef(w, h, 0);
                    //glRotatef(180, 0, 0, 1);
                    //glTranslatef(-w, -h, 0);
                    break;
                case ccDeviceOrientation.CCDeviceOrientationLandscapeRight:
                    //glTranslatef(w, h, 0);
                    //glRotatef(90, 0, 0, 1);
                    //glTranslatef(-h, -w, 0);
                    break;
                case ccDeviceOrientation.CCDeviceOrientationLandscapeLeft:
                    //glTranslatef(w, h, 0);
                    //glRotatef(-90, 0, 0, 1);
                    //glTranslatef(-h, -w, 0);
                    break;
            }
        }

        public ccDeviceOrientation deviceOrientation { get; set; }

        /// <summary>
        /// The size in pixels of the surface. It could be different than the screen size.
        /// High-res devices might have a higher surface size than the screen size.
        /// Only available when compiled using SDK >= 4.0.
        /// @since v0.99.4
        /// </summary>
        public float contentScaleFactor { get; set; }

        /** Will enable Retina Display on devices that supports it.
        It will enable Retina Display on iPhone4 and iPod Touch 4.
        It will return YES, if it could enabled it, otherwise it will return NO.

        This is the recommened way to enable Retina Display.
        @since v0.99.5
        */
        public bool enableRetinaDisplay(bool enabled)
        {
            throw new NotImplementedException();
        }
        public bool isRetinaDisplay()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///  Possible OpenGL projections used by director
    /// </summary>
    public enum ccDirectorProjection
    {
        /// sets a 2D projection (orthogonal projection)
        kCCDirectorProjection2D,

        /// sets a 3D projection with a fovy=60, znear=0.5f and zfar=1500.
        kCCDirectorProjection3D,

        /// it calls "updateProjection" on the projection delegate.
        kCCDirectorProjectionCustom,

        /// Detault projection is 3D projection
        kCCDirectorProjectionDefault = kCCDirectorProjection3D,

        // backward compatibility stuff
        CCDirectorProjection2D = kCCDirectorProjection2D,
        CCDirectorProjection3D = kCCDirectorProjection3D,
        CCDirectorProjectionCustom = kCCDirectorProjectionCustom
    }

    /// <summary>
    /// Possible Director Types.
    /// since v0.8.2
    /// </summary>
    public enum ccDirectorType
    {
        /** Will use a Director that triggers the main loop from an NSTimer object
         *
         * Features and Limitations:
         * - Integrates OK with UIKit objects
         * - It the slowest director
         * - The interval update is customizable from 1 to 60
         */
        kCCDirectorTypeNSTimer,

        /** will use a Director that triggers the main loop from a custom main loop.
         *
         * Features and Limitations:
         * - Faster than NSTimer Director
         * - It doesn't integrate well with UIKit objects
         * - The interval update can't be customizable
         */
        kCCDirectorTypeMainLoop,

        /** Will use a Director that triggers the main loop from a thread, but the main loop will be executed on the main thread.
         *
         * Features and Limitations:
         * - Faster than NSTimer Director
         * - It doesn't integrate well with UIKit objects
         * - The interval update can't be customizable
         */
        kCCDirectorTypeThreadMainLoop,

        /** Will use a Director that synchronizes timers with the refresh rate of the display.
         *
         * Features and Limitations:
         * - Faster than NSTimer Director
         * - Only available on 3.1+
         * - Scheduled timers & drawing are synchronizes with the refresh rate of the display
         * - Integrates OK with UIKit objects
         * - The interval update can be 1/60, 1/30, 1/15
         */
        kCCDirectorTypeDisplayLink,

        /** Default director is the NSTimer directory */
        kCCDirectorTypeDefault = kCCDirectorTypeNSTimer,

        // backward compatibility stuff
        CCDirectorTypeNSTimer = kCCDirectorTypeNSTimer,
        CCDirectorTypeMainLoop = kCCDirectorTypeMainLoop,
        CCDirectorTypeThreadMainLoop = kCCDirectorTypeThreadMainLoop,
        CCDirectorTypeDisplayLink = kCCDirectorTypeDisplayLink,
        CCDirectorTypeDefault = kCCDirectorTypeDefault
    }

    /// <summary>
    /// Possible device orientations
    /// </summary>
    public enum ccDeviceOrientation
    {
        /// Device oriented vertically, home button on the bottom
        kCCDeviceOrientationPortrait = 0, // UIDeviceOrientationPortrait,	
        /// Device oriented vertically, home button on the top
        kCCDeviceOrientationPortraitUpsideDown = 1, // UIDeviceOrientationPortraitUpsideDown,
        /// Device oriented horizontally, home button on the right
        kCCDeviceOrientationLandscapeLeft = 2, // UIDeviceOrientationLandscapeLeft,
        /// Device oriented horizontally, home button on the left
        kCCDeviceOrientationLandscapeRight = 3, // UIDeviceOrientationLandscapeRight,

        // Backward compatibility stuff
        CCDeviceOrientationPortrait = kCCDeviceOrientationPortrait,
        CCDeviceOrientationPortraitUpsideDown = kCCDeviceOrientationPortraitUpsideDown,
        CCDeviceOrientationLandscapeLeft = kCCDeviceOrientationLandscapeLeft,
        CCDeviceOrientationLandscapeRight = kCCDeviceOrientationLandscapeRight
    }
}
