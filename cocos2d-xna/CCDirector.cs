/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

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

        public virtual bool init()
        {
            //scene
            m_dOldAnimationInterval = m_dAnimationInterval = 1.0 / kDefaultFPS;

            // Set default projection (3D)
            m_eProjection = ccDirectorProjection.kCCDirectorProjectionDefault;

            // projection delegate if "Custom" projection is used
            //m_pProjectionDelegate = NULL;

            //FPS
            m_bDisplayFPS = false;
            m_uTotalFrames = m_uFrames = 0;
            m_pszFPS = "";

            m_bPaused = false;

            //paused?
            m_bPaused = false;

            //purge?
            m_bPurgeDirecotorInNextLoop = false;

            m_obWinSizeInPixels = m_obWinSizeInPoints = new CCSize(0, 0);

            // portrait mode default
            m_eDeviceOrientation = ccDeviceOrientation.CCDeviceOrientationPortrait;

            m_bRetinaDisplay = false;
            m_fContentScaleFactor = 1;
            m_bIsContentScaleSupported = false;

            return true;
        }

        public abstract void mainLoop(GameTime gameTime);

        #region sceneManagement

        /// <summary>
        /// Draw the scene.
        /// This method is called every frame. Don't call it manually.
        /// </summary>
        protected void drawScene(GameTime gameTime)
        {
            //tick before glClear: issue #533
            if (!m_bPaused)
            {
                CCScheduler.sharedScheduler().tick((float)gameTime.ElapsedGameTime.TotalSeconds);
                m_fDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        protected void setNextScene()
        {
            // If it is not a transition, call onExit/cleanup
            /*if (! newIsTransition)*/
            if (!(m_pNextScene is CCTransitionScene))
            {
                if (m_pRunningScene != null)
                {
                    m_pRunningScene.onExit();

                    //CLEAR TOUCHES BEFORE LEAVING
                    CCApplication.sharedApplication().ClearTouches();
                }

                // issue #709. the root node (scene) should receive the cleanup message too
                // otherwise it might be leaked.
                if (m_bSendCleanupToScene && m_pRunningScene != null)
                {
                    m_pRunningScene.cleanup();
                }
            }

            m_pRunningScene = m_pNextScene;
            // m_pNextScene.retain();
            m_pNextScene = null;

            if (m_pRunningScene != null)
            {
                m_pRunningScene.onEnter();
                if (m_pRunningScene is CCTransitionScene)
                {
                m_pRunningScene.onEnterTransitionDidFinish();
            }
        }
        }

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
            Debug.Assert(m_pRunningScene != null, "m_pRunningScene cannot be null");

            if (m_pobScenesStack.Count > 0)
            {
                m_pobScenesStack.RemoveAt(m_pobScenesStack.Count - 1);
            }
            int c = m_pobScenesStack.Count;

            if (c == 0)
            {
                CCApplication.sharedApplication().Game.Exit();
                end();
            }
            else
            {
                m_bSendCleanupToScene = true;
                m_pNextScene = m_pobScenesStack[c - 1];
            }
        }

        public CCScene getLastScene()
        {
            if (m_pobScenesStack.Count > 1)
                return m_pobScenesStack[m_pobScenesStack.Count - 2];
            else
                return null;
        
        }

        /// <summary>
        /// Replaces the running scene with a new one. The running scene is terminated.
        /// ONLY call it if there is a running scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void replaceScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "pScene cannot be null");

            int index = m_pobScenesStack.Count;

            m_bSendCleanupToScene = true;
            m_pobScenesStack[index - 1] = pScene;

            m_pNextScene = pScene;
        }

        ///<summary>
        /// Whether or not the replaced scene will receive the cleanup message.
        /// If the new scene is pushed, then the old scene won't receive the "cleanup" message.
        /// If the new scene replaces the old one, the it will receive the "cleanup" message.
        /// @since v0.99.0
        /// </summary>
        public bool isSendCleanupToScene()
        {
            return m_bSendCleanupToScene;
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
            // [openGLView responseToSelector:@selector(setContentScaleFactor)]
            if (CCApplication.sharedApplication().canSetContentScaleFactor)
            {
                CCApplication.sharedApplication().setContentScaleFactor(m_fContentScaleFactor);
                m_bIsContentScaleSupported = true;
            }
            else
            {
                //CCLOG("cocos2d: setContentScaleFactor:'is not supported on this device");
            }
        }

#if CC_DIRECTOR_FAST_FPS
        /** shows the FPS in the screen */
        protected void showFPS()
        {
            m_uFrames++;
            m_fAccumDt += m_fDeltaTime;

            if (m_fAccumDt > ccMacros.CC_DIRECTOR_FPS_INTERVAL)
            {
                m_fFrameRate = m_uFrames / m_fAccumDt;
                m_uFrames = 0;
                m_fAccumDt = 0;

                m_pszFPS = string.Format("{0}", m_fFrameRate);
            }

            SpriteFont font = CCApplication.sharedApplication().content.Load<SpriteFont>(@"fonts/Arial");
            CCApplication.sharedApplication().spriteBatch.Begin();
            CCApplication.sharedApplication().spriteBatch.DrawString(font, 
                m_pszFPS, 
                new Vector2(0, CCApplication.sharedApplication().getSize().height - 50), 
                new Color(0, 255, 255));
            CCApplication.sharedApplication().spriteBatch.End();
        }
#else
        protected void showFPS()
        {
            throw new NotImplementedException();
        }
#endif // CC_DIRECTOR_FAST_FPS

        protected double m_dAnimationInterval;
        protected double m_dOldAnimationInterval;

        /* landscape mode ? */
        bool m_bLandscape;

        bool m_bDisplayFPS;
        float m_fAccumDt;
        float m_fFrameRate;

        /* is the running scene paused */
        bool m_bPaused;

        /* How many frames were called since the director started */
        uint m_uTotalFrames;
        uint m_uFrames;

        float m_fDeltaTime;

        /* The running scene */
        CCScene m_pRunningScene;

        /* will be the next 'runningScene' in the next frame
         nextScene is a weak reference. */
        CCScene m_pNextScene;

        /// <summary>
        /// If YES, then "old" scene will receive the cleanup message
        /// </summary>
        bool m_bSendCleanupToScene;

        /// <summary>
        /// scheduled scenes
        /// </summary>
        List<CCScene> m_pobScenesStack = new List<CCScene>();

        /* projection used */
        ccDirectorProjection m_eProjection;

        /* window size in points */
        CCSize m_obWinSizeInPoints;

        /* window size in pixels */
        CCSize m_obWinSizeInPixels;

        /* content scale factor */
        float m_fContentScaleFactor = 1;

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

        #region attribute

        /// <summary>
        ///  Get current running Scene. Director can only run one Scene at the time 
        /// </summary>
        public CCScene runningScene
        {
            get
            {
                return m_pRunningScene;
            }
        }

        /// <summary>
        /// the FPS value
        /// </summary>
        public virtual double animationInterval
        {
            get { return m_dAnimationInterval; }
            set { }
        }

        /// <summary>
        /// Whether or not to display the FPS on the bottom-left corner 
        /// </summary>
        /// <returns></returns>
        public bool DisplayFPS
        {
            get { return m_bDisplayFPS; }
            set { m_bDisplayFPS = value; }
        }

        /// <summary>
        /// Whether or not the Director is paused
        /// </summary>
        public bool isPaused
        {
            get { return m_bPaused; }
        }

        /// <summary>
        /// How many frames were called since the director started
        /// </summary>
        public uint getFrames()
        {
            return m_uFrames;
        }

        /// <summary>
        /// Sets an OpenGL projection
        ///@since v0.8.2
        /// </summary>
        /// <returns></returns>
        public ccDirectorProjection Projection
        {
            set
            {
                CCApplication app = CCApplication.sharedApplication();

                CCSize size = CCApplication.sharedApplication().getSize();
                float zeye = this.zEye;
                switch (value)
                {
                    case ccDirectorProjection.kCCDirectorProjection2D:
                        app.viewMatrix = Matrix.CreateLookAt(new Vector3(size.width / 2.0f, size.height / 2.0f, 5.0f),
                            new Vector3(size.width / 2.0f, size.height / 2.0f, 0), 
                            Vector3.Up);
                        app.projectionMatrix = Matrix.CreateOrthographicOffCenter(-size.width / 2.0f, 
                            size.width / 2.0f, -size.height / 2.0f, size.height / 2.0f, -1024.0f, 1024.0f);
                        app.worldMatrix = Matrix.Identity;

                        break;

                    case ccDirectorProjection.kCCDirectorProjection3D:
                        app.viewMatrix = Matrix.CreateLookAt(new Vector3(size.width / 2.0f, size.height / 2.0f, size.height / 1.1566f),
                            new Vector3(size.width / 2.0f, size.height / 2.0f, 0), Vector3.Up);

                        app.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3.0f, size.width / size.height, 0.5f, 1500.0f);

                        app.worldMatrix = Matrix.Identity;// * Matrix.CreateTranslation(new Vector3(-size.width / 2, -size.height / 2, 0));

                        break;

                    case ccDirectorProjection.kCCDirectorProjectionCustom:
                        //if (m_pProjectionDelegate)
                        //{
                        //    m_pProjectionDelegate->updateProjection();
                        //}
                        break;

                    default:
                        Debug.Assert(true, "cocos2d: Director: unrecognized projecgtion");
                        break;
                }

                m_eProjection = value;
            }
        }

        #endregion

        #region  window size

        /// <summary>
        /// returns the size of the OpenGL view in points.
        /// It takes into account any possible rotation (device orientation) of the window
        /// </summary>
        /// <returns></returns>
        public CCSize getWinSize()
        {
            CCSize s = m_obWinSizeInPoints;

            //it's different from cocos2d-win32. 

            //if (m_eDeviceOrientation == ccDeviceOrientation.CCDeviceOrientationLandscapeLeft
            //    || m_eDeviceOrientation == ccDeviceOrientation.CCDeviceOrientationLandscapeRight)
            //{
            //    // swap x,y in landspace mode
            //    CCSize tmp = s;
            //    s.width = tmp.width;
            //    s.height = tmp.height;
            //}

            return s;
        }

        /// <summary>
        /// returns the size of the OpenGL view in pixels.
        /// It takes into account any possible rotation (device orientation) of the window.
        /// On Mac winSize and winSizeInPixels return the same value.
        /// </summary>
        public CCSize winSizeInPixels
        {
            get
            {
                CCSize s = getWinSize();

                s.width *= CCDirector.sharedDirector().ContentScaleFactor;
                s.height *= CCDirector.sharedDirector().ContentScaleFactor;

                return s;
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
                return m_obWinSizeInPixels;
            }
        }

        #endregion

        /// <summary>
        /// changes the projection size
        /// </summary>
        /// <param name="newWindowSize"></param>
        public void reshapeProjection(CCSize newWindowSize)
        {
            // CC_UNUSED_PARAM(newWindowSize);
            m_obWinSizeInPoints = CCApplication.sharedApplication().getSize();
            m_obWinSizeInPixels = new CCSize(m_obWinSizeInPoints.width * m_fContentScaleFactor,
                                             m_obWinSizeInPoints.height * m_fContentScaleFactor);

            Projection = m_eProjection;
        }

        /// <summary>
        /// converts a UIKit coordinate to an OpenGL coordinate
        /// Useful to convert (multi) touches coordinates to the current layout (portrait or landscape)
        /// </summary>
        public CCPoint convertToGL(CCPoint obPoint)
        {
            CCSize s = m_obWinSizeInPoints;

            //this is different from cocos2d-win32
            return new CCPoint(obPoint.x, s.height - obPoint.y);

            //CCSize s = m_obWinSizeInPoints;
            //float newY = s.height - obPoint.y;
            //float newX = s.width - obPoint.x;

            //CCPoint ret = new CCPoint(0, 0);
            //switch (m_eDeviceOrientation)
            //{
            //    case ccDeviceOrientation.CCDeviceOrientationPortrait:
            //        ret = new CCPoint(obPoint.x, newY);
            //        break;
            //    case ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown:
            //        ret = new CCPoint(newX, obPoint.y);
            //        break;
            //    case ccDeviceOrientation.CCDeviceOrientationLandscapeLeft:
            //        ret.x = obPoint.y;
            //        ret.y = obPoint.x;
            //        break;
            //    case ccDeviceOrientation.CCDeviceOrientationLandscapeRight:
            //        ret.x = newY;
            //        ret.y = newX;
            //        break;
            //}

            //return ret;
        }

        /// <summary>
        /// converts an OpenGL coordinate to a UIKit coordinate
        /// Useful to convert node points to window points for calls such as glScissor
        /// </summary>
        /// <param name="obPoint"></param>
        /// <returns></returns>
        public CCPoint convertToUI(CCPoint obPoint)
        {
            CCSize winSize = m_obWinSizeInPoints;

            //this is different from cocos2d-win32
            return new CCPoint(obPoint.x, winSize.height - obPoint.y);

            //float oppositeX = winSize.width - obPoint.x;
            //float oppositeY = winSize.height - obPoint.y;
            //CCPoint uiPoint = new CCPoint();

            //switch (m_eDeviceOrientation)
            //{
            //    case ccDeviceOrientation.CCDeviceOrientationPortrait:
            //        uiPoint = new CCPoint(obPoint.x, oppositeY);
            //        break;
            //    case ccDeviceOrientation.CCDeviceOrientationPortraitUpsideDown:
            //        uiPoint = new CCPoint(oppositeX, obPoint.y);
            //        break;
            //    case ccDeviceOrientation.CCDeviceOrientationLandscapeLeft:
            //        uiPoint = new CCPoint(obPoint.y, obPoint.x);
            //        break;
            //    case ccDeviceOrientation.CCDeviceOrientationLandscapeRight:
            //        // Can't use oppositeX/Y because x/y are flipped
            //        uiPoint = new CCPoint(winSize.width - obPoint.y, winSize.height - obPoint.x);
            //        break;
            //}

            //return uiPoint;
        }

        /// XXX: missing description 
        public float zEye
        {
            get
            {
                return (m_obWinSizeInPixels.height / 1.1566f);
            }
        }

        /** Ends the execution, releases the running scene.
         It doesn't remove the OpenGL view from its parent. You have to do it manually.
         */

        /* end is key word of lua, use other name to export to lua. */
        public void endToLua()
        {
            end();
        }

        public void end()
        {
            m_bPurgeDirecotorInNextLoop = true;
        }

        /// <summary>
        /// Pauses the running scene.
        /// The running scene will be _drawed_ but all scheduled timers will be paused
        /// While paused, the draw rate will be 4 FPS to reduce CPU consumption
        /// </summary>
        public void pause()
        {
            if (m_bPaused)
            {
                return;
            }

            m_dOldAnimationInterval = m_dAnimationInterval;

            // when paused, don't consume CPU
            animationInterval = 1 / 4.0;
            m_bPaused = true;
        }

        /// <summary>
        /// Resumes the paused scene
        /// The scheduled timers will be activated again.
        /// The "delta time" will be 0 (as if the game wasn't paused)
        /// </summary>
        public void resume()
        {
            if (!m_bPaused)
            {
                return;
            }

            animationInterval = m_dOldAnimationInterval;

            //if (CCTime.gettimeofdayCocos2d(m_pLastUpdate, NULL) != 0)
            //{
            //    //CCLOG("cocos2d: Director: Error in gettimeofday");
            //}

            m_bPaused = false;
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

        // Memory Helper

        /// <summary>
        /// Removes cached all cocos2d cached data.
        /// It will purge the CCTextureCache, CCSpriteFrameCache, CCLabelBMFont cache
        /// @since v0.99.3
        /// </summary>
        public void purgeCachedData()
        {
            //CCLabelBMFont::purgeCachedData();
            //CCTextureCache::sharedTextureCache()->removeUnusedTextures();
        }

        #region OpenGL Helper

        /// <summary>
        /// sets the OpenGL default values
        /// </summary>
        public void setGLDefaultValues()
        {
            Projection = m_eProjection;
        }
        /// <summary>
        /// enables/disables OpenGL alpha blending 
        /// </summary>
        /// <param name="bOn"></param>
        public void setAlphaBlending(bool bOn)
        {
            //if (bOn)
            //{
            //    glEnable(GL_BLEND);
            //    glBlendFunc(CC_BLEND_SRC, CC_BLEND_DST);
            //}
            //else
            //{
            //    glDisable(GL_BLEND);
            //}
        }

        /// <summary>
        /// enables/disables OpenGL depth test
        /// </summary>
        /// <param name="bOn"></param>
        public void setDepthTest(bool bOn)
        {
            //if (bOn)
            //{
            //    ccglClearDepth(1.0f);
            //    glEnable(GL_DEPTH_TEST);
            //    glDepthFunc(GL_LEQUAL);
            //    //		glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);
            //}
            //else
            //{
            //    glDisable(GL_DEPTH_TEST);
            //}
        }

        public void setOpenGLView()
        {
            // set size
            m_obWinSizeInPoints = CCApplication.sharedApplication().getSize();
            m_obWinSizeInPixels = new CCSize(m_obWinSizeInPoints.width * m_fContentScaleFactor, m_obWinSizeInPoints.height * m_fContentScaleFactor);
            setGLDefaultValues();

            if (m_fContentScaleFactor != 1)
            {
                updateContentScaleFactor();
            }

            CCTouchDispatcher pTouchDispatcher = CCTouchDispatcher.sharedDispatcher();
            CCApplication.sharedApplication().TouchDelegate = pTouchDispatcher;
            pTouchDispatcher.IsDispatchEvents = true;
        }

        #endregion

        // Profiler
        public void showProfilers()
        {

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

        /// <summary>
        /// rotate the objects by engine.
        /// </summary>
        public ccDeviceOrientation deviceOrientation
        {
            get { return m_eDeviceOrientation; }
            set
            {
                Orientation eNewOrientation = CCApplication.sharedApplication().setOrientation((Orientation)((int)value));
                ccDeviceOrientation eNewDeviceOrientation = (ccDeviceOrientation)((int)eNewOrientation);

                if (m_eDeviceOrientation != eNewDeviceOrientation)
                {
                    m_eDeviceOrientation = eNewDeviceOrientation;

                    //added in cocos2d-xna
                    m_obWinSizeInPoints = CCApplication.sharedApplication().getSize();
                    m_obWinSizeInPixels = new CCSize(m_obWinSizeInPoints.width * m_fContentScaleFactor, m_obWinSizeInPoints.height * m_fContentScaleFactor);
                    Projection = m_eProjection;
                }
                else
                {
                    // this logic is only run on win32 now
                    // On win32,the return value of CCApplication::setDeviceOrientation is always kCCDeviceOrientationPortrait
                    // So,we should calculate the Projection and window size again.
                    m_obWinSizeInPoints = CCApplication.sharedApplication().getSize();
                    m_obWinSizeInPixels = new CCSize(m_obWinSizeInPoints.width * m_fContentScaleFactor, m_obWinSizeInPoints.height * m_fContentScaleFactor);
                    Projection = m_eProjection;
                }
            }
        }

        /// <summary>
        /// The size in pixels of the surface. It could be different than the screen size.
        /// High-res devices might have a higher surface size than the screen size.
        /// Only available when compiled using SDK >= 4.0.
        /// @since v0.99.4
        /// </summary>
        public float ContentScaleFactor
        {
            get { return m_fContentScaleFactor; }
            set
            {
                if (value != m_fContentScaleFactor)
                {
                    m_fContentScaleFactor = value;
                    m_obWinSizeInPixels = new CCSize(m_obWinSizeInPoints.width * value, m_obWinSizeInPoints.height * value);

                    updateContentScaleFactor();

                    // update projection
                    Projection = m_eProjection;
                }
            }
        }

        /// <summary>
        /// Will enable Retina Display on devices that supports it.
        /// It will enable Retina Display on iPhone4 and iPod Touch 4.
        /// It will return YES, if it could enabled it, otherwise it will return NO.
        /// This is the recommened way to enable Retina Display.
        /// @since v0.99.5
        /// </summary>
        public bool enableRetinaDisplay(bool enabled)
        {
            // Already enabled?
            if (enabled && m_fContentScaleFactor == 2)
            {
                return true;
            }

            // Already diabled?
            if (!enabled && m_fContentScaleFactor == 1)
            {
                return false;
            }

            // setContentScaleFactor is not supported
            if (!CCApplication.sharedApplication().canSetContentScaleFactor)
            {
                return false;
            }

            float newScale = (float)(enabled ? 2 : 1);
            CCApplication.sharedApplication().setContentScaleFactor(newScale);

            // release cached texture
            //CCTextureCache::purgeSharedTextureCache();

#if CC_DIRECTOR_FAST_FPS
            //if (m_pFPSLabel)
            //{
            //    CC_SAFE_RELEASE_NULL(m_pFPSLabel);
            //    m_pFPSLabel = CCLabelTTF::labelWithString("00.0", "Arial", 24);
            //    m_pFPSLabel->retain();
            //}
#endif

            if (m_fContentScaleFactor == 2)
            {
                m_bRetinaDisplay = true;
            }
            else
            {
                m_bRetinaDisplay = false;
            }

            return true;
        }
        public bool isRetinaDisplay()
        {
            return m_bRetinaDisplay;
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
