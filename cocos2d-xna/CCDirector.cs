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
    public abstract class CCDirector
    {
        public virtual bool init()
        {
            throw new NotImplementedException();
        }

        // attribute

        /// <summary>
        ///  Get current running Scene. Director can only run one Scene at the time 
        /// </summary>
        CCScene getRunningScene() { throw new NotImplementedException(); }

        /// <summary>
        /// Get the FPS value
        /// </summary>
        public double getAnimationInterval()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set the FPS value.
        /// </summary>
        public abstract void setAnimationInterval(double dValue);

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
            throw new NotImplementedException();
        }

        /** Get the CCEGLView, where everything is rendered */
        //public static CC_GLVIEW* getOpenGLView(void) { return m_pobOpenGLView; }
        //void setOpenGLView(CC_GLVIEW *pobOpenGLView);

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
        public ccDirectorProjection getProjection() { throw new NotImplementedException(); }
        public void setProjection(ccDirectorProjection kProjection) { }

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// returns the size of the OpenGL view in pixels.
        /// It takes into account any possible rotation (device orientation) of the window.
        /// On Mac winSize and winSizeInPixels return the same value.
        /// </summary>
        /// <returns></returns>
        public CCSize getWinSizeInPixels()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// returns the display size of the OpenGL view in pixels.
        /// It doesn't take into account any possible rotation of the window.
        /// </summary>
        /// <returns></returns>
        public CCSize getDisplaySizeInPixels()
        {
            throw new NotImplementedException();
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
        public float getZEye() { throw new NotImplementedException(); }

        // Scene Management

        /// <summary>
        /// Enters the Director's main loop with the given Scene. 
        /// Call it to run only your FIRST scene.
        /// Don't call it if there is already a running scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void runWithScene(CCScene pScene)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
        public void drawScene()
        {
            throw new NotImplementedException();
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

        public abstract void mainLoop();

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
            throw new NotImplementedException();
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
        public static bool setDirectorType(ccDirectorType obDirectorType) { throw new NotImplementedException(); }

        /// <summary>
        /// returns a shared instance of the director
        /// </summary>
        /// <returns></returns>
        public static CCDirector sharedDirector()
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
