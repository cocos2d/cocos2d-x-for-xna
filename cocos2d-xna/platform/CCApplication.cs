/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

http://www.cocos2d-x.org
http://www.openxlive.com

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
using Microsoft.Xna.Framework.Input.Touch;

namespace cocos2d
{
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

    public abstract class CCApplication : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Fields and Construct Method

        Game game;

        CCTouch m_pTouch;
        internal GraphicsDeviceManager graphics;
        Rectangle m_rcViewPort;
        IEGLTouchDelegate m_pDelegate;
        List<CCTouch> m_pSet;

        bool m_bCaptured;
        float m_fScreenScaleFactor;

        public CCApplication(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.game = game;
            this.graphics = graphics;
            this.content = game.Content;

            game.Window.OrientationChanged += Window_OrientationChanged;

            TouchPanel.EnabledGestures = GestureType.Tap;

            m_pTouch = new CCTouch();
            m_pSet = new List<CCTouch>();

            m_fScreenScaleFactor = 1.0f;

#warning "set height and width as Graphics.Device.Viewport"
            m_rcViewPort = new Rectangle(0, 0, 800, 480); //graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
        }

        #endregion

        #region GameComponent

        public override void Initialize()
        {
            sm_pSharedApplication = this;

            PVRFrameEnableControlWindow(false);

            initInstance();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Process touch events 
            ProcessTouch();

            base.Update(gameTime);
        }

        VertexDeclaration vertexDeclaration;
        public override void Draw(GameTime gameTime)
        {
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.Projection = projectionMatrix;

            CCDirector.sharedDirector().mainLoop(gameTime);

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            basicEffect = new BasicEffect(GraphicsDevice);

            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            base.LoadContent();

            applicationDidFinishLaunching();
        }

        #endregion

        #region Touch Methods

        public IEGLTouchDelegate TouchDelegate
        {
            set { m_pDelegate = value; }
        }

        private void ProcessTouch()
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (TouchLocation touch in touchCollection)
            {
                if (touch.State == TouchLocationState.Pressed)
                {
                    if (m_pDelegate != null && m_pTouch != null)
                    {
                        if (m_rcViewPort.Contains((int)touch.Position.X, (int)touch.Position.Y))
                        {
                            m_bCaptured = true;

                            m_pTouch.SetTouchInfo(0, touch.Position.X - m_rcViewPort.Left / m_fScreenScaleFactor,
                                                touch.Position.Y - m_rcViewPort.Top / m_fScreenScaleFactor);
                            m_pSet.Add(m_pTouch);
                            m_pDelegate.touchesBegan(m_pSet, null);
                        }
                    }
                }
                else if (touch.State == TouchLocationState.Moved)
                {
                    if (m_bCaptured)
                    {
                        m_pTouch.SetTouchInfo(0, touch.Position.X - m_rcViewPort.Left / m_fScreenScaleFactor,
                                            touch.Position.Y - m_rcViewPort.Top / m_fScreenScaleFactor);
                        m_pDelegate.touchesMoved(m_pSet, null);
                    }
                }
                else if (touch.State == TouchLocationState.Released)
                {
                    if (m_bCaptured)
                    {
                        m_pTouch.SetTouchInfo(0, touch.Position.X - m_rcViewPort.Left / m_fScreenScaleFactor,
                                            touch.Position.Y - m_rcViewPort.Top / m_fScreenScaleFactor);
                        m_pDelegate.touchesEnded(m_pSet, null);
                        m_pSet.Remove(m_pTouch);

                        m_bCaptured = false;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// This function change the PVRFrame show/hide setting in register.
        /// </summary>
        /// <param name="bEnable"> If true show the PVRFrame window, otherwise hide.</param>
        static void PVRFrameEnableControlWindow(bool bEnable)
        { }

        // sharedApplication pointer
        protected static CCApplication sm_pSharedApplication;

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
            return true;
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
        public virtual void applicationDidEnterBackground()
        {

        }

        /// <summary>
        /// The function be called when the application enter foreground
        /// </summary>
        public virtual void applicationWillEnterForeground()
        {

        }

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
        /// Callback by CCDirector for change devic e orientation. 
        /// </summary>
        /// <param name="orientation">The defination of orientation which CCDirector want change to.</param>
        /// <returns>The actual orientation of the application.</returns>
        public Orientation setOrientation(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.kOrientationLandscapeLeft:
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 480;
                    _size = new CCSize(800, 480);
                    m_rcViewPort = new Rectangle(0, 0, 800, 480);
                    graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
                    graphics.ApplyChanges();
                    return Orientation.kOrientationLandscapeLeft;

                case Orientation.kOrientationLandscapeRight:
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 480;
                    _size = new CCSize(800, 480);
                    m_rcViewPort = new Rectangle(0, 0, 800, 480);
                    graphics.SupportedOrientations = DisplayOrientation.LandscapeRight;
                    graphics.ApplyChanges();
                    return Orientation.kOrientationLandscapeRight;

                default:
                    graphics.PreferredBackBufferWidth = 480;
                    graphics.PreferredBackBufferHeight = 800;
                    _size = new CCSize(480, 800);
                    m_rcViewPort = new Rectangle(0, 0, 480, 800);
                    graphics.SupportedOrientations = DisplayOrientation.Portrait;
                    graphics.ApplyChanges();
                    return Orientation.kOrientationPortrait;
            }
        }
        void Window_OrientationChanged(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Get status bar rectangle in EGLView window.
        /// </summary>
        /// <param name="rect"></param>
        public void statusBarFrame(out CCRect rect)
        {
            // Windows doesn't have status bar.
            rect = new CCRect(0, 0, 0, 0);
        }

        //static ccLanguageType CCApplication::getCurrentLanguage()
        //{

        //}

        /// <summary>
        /// Gets the current ContentManager
        /// </summary>
        public ContentManager content
        {
            get;
            private set;
        }
        internal SpriteBatch spriteBatch
        {
            get;
            private set;
        }

        internal Matrix worldMatrix;
        internal Matrix viewMatrix;
        internal Matrix projectionMatrix;
        internal BasicEffect basicEffect
        {
            get;
            private set;
        }

        #region CCEGLView

        bool m_bOrientationReverted;
        Point m_tSizeInPoints;

        public bool canSetContentScaleFactor
        {
            get { return true; }
        }

        private CCSize _size = new CCSize(800, 480);
        public CCSize getSize()
        {
            return _size;
        }

        public void setContentScaleFactor(float contentScaleFactor)
        {
            m_fScreenScaleFactor = contentScaleFactor;
            if (m_bOrientationReverted)
            {
                //resize((int)(m_tSizeInPoints.cy * contentScaleFactor), (int)(m_tSizeInPoints.cx * contentScaleFactor));
            }
            else
            {
                //resize((int)(m_tSizeInPoints.cx * contentScaleFactor), (int)(m_tSizeInPoints.cy * contentScaleFactor));
            }
            centerWindow();
        }

        void resize(int width, int height)
        { }

        void centerWindow()
        { }

        #endregion
    }
}
