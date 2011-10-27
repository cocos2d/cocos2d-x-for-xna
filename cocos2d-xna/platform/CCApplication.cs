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

namespace cocos2d.platform
{
    public abstract class CCApplication : Microsoft.Xna.Framework.DrawableGameComponent
    {
        /// <summary>
        /// This function change the PVRFrame show/hide setting in register.
        /// </summary>
        /// <param name="bEnable"> If true show the PVRFrame window, otherwise hide.</param>
        static void PVRFrameEnableControlWindow(bool bEnable)
        { }

        // sharedApplication pointer
        static CCApplication sm_pSharedApplication;

        /// <summary>
        /// Get current applicaiton instance.
        /// </summary>
        /// <returns>Current application instance pointer.</returns>
        public static CCApplication sharedApplication()
        {
            return sm_pSharedApplication;
        }

        #region virtual Method

        /// <summary>
        /// Implement for initialize OpenGL instance, set source path, etc...
        /// </summary>
        public virtual bool initInstance()
        {
            return false;
        }

        /// <summary>
        /// Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///     return true    Initialize success, app continue.
        ///     return false   Initialize failed, app terminate.
        /// </returns>
        public virtual bool applicationDidFinishLaunching()
        {
            return false;
        }

        /// <summary>
        ///  The function be called when the application enter background
        /// </summary>
        public virtual void applicationDidEnterBackground() { }

        /// <summary>
        /// The function be called when the application enter foreground
        /// </summary>
        public virtual void applicationWillEnterForeground() { }

        #endregion

        /// <summary>
        /// Callback by CCDirector for limit FPS
        /// </summary>
        /// <param name="interval">The time, which expressed in second in second, between current frame and next. </param>
        public double animationInterval
        {
            set
            {
                game.TargetElapsedTime = TimeSpan.FromSeconds(value);
            }
        }

        /// <summary>
        /// Callback by CCDirector for change device orientation. 
        /// </summary>
        /// <param name="orientation">  The defination of orientation which CCDirector want change to.</param>
        /// <returns>The actual orientation of the application.</returns>
        public Orientation orientation
        {
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get status bar rectangle in EGLView window.
        /// </summary>
        /// <param name="rect"></param>
        public void statusBarFrame(CCRect rect)
        {
            throw new NotImplementedException();
        }

        //static ccLanguageType CCApplication::getCurrentLanguage()
        //{

        //}

        #region GameComponent

        Game game;
        public CCApplication(Game game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            sm_pSharedApplication = this;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            CCDirector.sharedDirector().mainLoop();
            base.Draw(gameTime);
        }

        #endregion
    }

    public enum Orientation
    {
        /// Device oriented vertically, home button on the bottom
        kOrientationPortrait = 0,
        /// Device oriented vertically, home button on the top
        kOrientationPortraitUpsideDown = 1,
        /// Device oriented horizontally, home button on the right
        kOrientationLandscapeLeft = 2,
        /// Device oriented horizontally, home button on the left
        kOrientationLandscapeRight = 3,
    } ;
}
