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

        // CCTouch m_pTouch;
        internal GraphicsDeviceManager graphics;
        protected Rectangle m_rcViewPort;
        protected IEGLTouchDelegate m_pDelegate;
        // List<CCTouch> m_pSet;

        protected bool m_bCaptured;
        protected float m_fScreenScaleFactor;

        private readonly LinkedList<CCTouch> m_pTouches;
        private readonly Dictionary<int, LinkedListNode<CCTouch>> m_pTouchMap;

        public CCApplication(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            this.game = game;
            this.graphics = graphics;
            this.content = game.Content;

            // MonoGame 3D
#if MONO3D
            if (graphics.GraphicsDevice == null)
            {
                graphics.CreateDevice();
            }
#endif
#if WINDOWS || XBOX || XBOX360
            graphics.DeviceCreated += new EventHandler<EventArgs>(graphics_DeviceCreated);
#endif
            game.Window.OrientationChanged += Window_OrientationChanged;

            TouchPanel.EnabledGestures = GestureType.Tap;

            //m_pTouch = new CCTouch();
            //m_pSet = new List<CCTouch>();
            m_pTouches = new LinkedList<CCTouch>();
            m_pTouchMap = new Dictionary<int, LinkedListNode<CCTouch>>();

            m_fScreenScaleFactor = 1.0f;

#warning "set height and width as Graphics.Device.Viewport"

#if WP7 || WINPHONE || WINDOWS_PHONE
            m_rcViewPort = new Rectangle(0, 0, 800, 480); //graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            _size = new CCSize(800,480);
#elif !WINDOWS && !XBOX && !XBOX360
            m_rcViewPort = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            _size = new CCSize(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
#endif
        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {
            m_rcViewPort = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
            _size = new CCSize(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
        }

        #endregion

        // http://www.cocos2d-x.org/boards/17/topics/10777
        public void ClearTouches()
        {
            m_pTouches.Clear();
            m_pTouchMap.Clear();
            // m_pSet.Clear();
        }

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
            if (m_pDelegate != null)
            {
                TouchCollection touchCollection = TouchPanel.GetState();

                List<CCTouch> newTouches = new List<CCTouch>();
                List<CCTouch> movedTouches = new List<CCTouch>();
                List<CCTouch> endedTouches = new List<CCTouch>();

                foreach (TouchLocation touch in touchCollection)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Pressed:
                            if (m_rcViewPort.Contains((int)touch.Position.X, (int)touch.Position.Y))
                            {
                                m_pTouches.AddLast(new CCTouch(touch.Id, touch.Position.X - m_rcViewPort.Left / m_fScreenScaleFactor, touch.Position.Y - m_rcViewPort.Top / m_fScreenScaleFactor));
                                m_pTouchMap.Add(touch.Id, m_pTouches.Last);
                                newTouches.Add(m_pTouches.Last.Value);
                            }
                            break;

                        case TouchLocationState.Moved:

                            if (m_pTouchMap.ContainsKey(touch.Id))
                            {
                                movedTouches.Add(m_pTouchMap[touch.Id].Value);
                                m_pTouchMap[touch.Id].Value.SetTouchInfo(touch.Id,
                                    touch.Position.X - m_rcViewPort.Left / m_fScreenScaleFactor,
                                                        touch.Position.Y - m_rcViewPort.Top / m_fScreenScaleFactor);
                            }
                            break;


                        case TouchLocationState.Released:

                            if (m_pTouchMap.ContainsKey(touch.Id))
                            {
                                endedTouches.Add(m_pTouchMap[touch.Id].Value);
                                m_pTouches.Remove(m_pTouchMap[touch.Id]);
                                m_pTouchMap.Remove(touch.Id);
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                }
                if (newTouches.Count > 0)
                {
                    m_pDelegate.touchesBegan(newTouches, null);
                }

                if (movedTouches.Count > 0)
                {
                    m_pDelegate.touchesMoved(movedTouches, null);
                }

                if (endedTouches.Count > 0)
                {
                    m_pDelegate.touchesEnded(endedTouches, null);
                }
            }
        }

        private CCTouch getTouchBasedOnID(int nID)
        {
            if (m_pTouchMap.ContainsKey(nID))
            {
                LinkedListNode<CCTouch> curTouch = m_pTouchMap[nID];
                //If ID's match...
                if (curTouch.Value.view() == nID)
                {
                    //return the corresponding touch
                    return curTouch.Value;
                }
            }
            //If we reached here, we found no touches
            //matching the specified id.
            return null;
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
#if WP7 || WINPHONE || WINDOWS_PHONE
            // Windows Phone always has a 480 x 800 configuration for now.
            int w = 480;
            int h = 800;
#else
            int w = graphics.GraphicsDevice.Viewport.Width;
            int h = graphics.GraphicsDevice.Viewport.Height;
            if (w > h)
            {
                // Swap to be in portrate orientation
                // where width < height
                int z = h;
                h = w;
                w = z;
            }
#endif
            switch (orientation)
            {
                case Orientation.kOrientationLandscapeLeft:
                    graphics.PreferredBackBufferWidth = h;
                    graphics.PreferredBackBufferHeight = w;
                    _size = new CCSize(h, w);
                    m_rcViewPort = new Rectangle(0, 0, h, w);
                    graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
                    graphics.ApplyChanges();
                    return Orientation.kOrientationLandscapeLeft;

                case Orientation.kOrientationLandscapeRight:
                    graphics.PreferredBackBufferWidth = h;
                    graphics.PreferredBackBufferHeight = w;
                    _size = new CCSize(h, w);
                    m_rcViewPort = new Rectangle(0, 0, h, w);
                    graphics.SupportedOrientations = DisplayOrientation.LandscapeRight;
                    graphics.ApplyChanges();
                    return Orientation.kOrientationLandscapeRight;

                default:
                    graphics.PreferredBackBufferWidth = w;
                    graphics.PreferredBackBufferHeight = h;
                    _size = new CCSize(w, h);
                    m_rcViewPort = new Rectangle(0, 0, w, h);
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
