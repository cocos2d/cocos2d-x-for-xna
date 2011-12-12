/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// CCLayerColor is a subclass of CCLayer that implements the CCRGBAProtocol protocol
    /// All features from CCLayer are valid, plus the following new features:
    /// - opacity
    /// - RGB colors
    /// </summary>
    public class CCLayerColor : CCLayer, ICCRGBAProtocol, ICCBlendProtocol
    {
        protected ccVertex2F[] m_pSquareVertices = new ccVertex2F[4];
        protected ccColor4B[] m_pSquareColors = new ccColor4B[4];

        public CCLayerColor()
        {
            m_cOpacity = 0;
            m_tColor = new ccColor3B(0, 0, 0);
            // default blend function
            m_tBlendFunc.src = 1;
            m_tBlendFunc.dst = 0x0303;
        }

        public override void draw()
        {
            base.draw();

            // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Needed states: GL_VERTEX_ARRAY, GL_COLOR_ARRAY
            // Unneeded states: GL_TEXTURE_2D, GL_TEXTURE_COORD_ARRAY
            //glDisableClientState(GL_TEXTURE_COORD_ARRAY);
            //glDisable(GL_TEXTURE_2D);

            //glVertexPointer(2, GL_FLOAT, 0, m_pSquareVertices);
            //glColorPointer(4, GL_UNSIGNED_BYTE, 0, m_pSquareColors);


            //@to change to draw an rect color aera,maybe use 3D 
            CCApplication.sharedApplication().GraphicsDevice.Clear(
                new Microsoft.Xna.Framework.Color(m_tColor.r, m_tColor.g, m_tColor.b, m_cOpacity));

            bool newBlend = false;
            if (m_tBlendFunc.src != 1 || m_tBlendFunc.dst != 0x0303)
            {
                newBlend = true;
                //glBlendFunc(m_tBlendFunc.src, m_tBlendFunc.dst);
            }
            else if (m_cOpacity != 255)
            {
                newBlend = true;
                //glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
            }

            //glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

            if (newBlend)
            {
                //glBlendFunc(CC_BLEND_SRC, CC_BLEND_DST);
            }
            // restore default GL state
            //glEnableClientState(GL_TEXTURE_COORD_ARRAY);
            //glEnable(GL_TEXTURE_2D);
        }

        /// <summary>
        /// override contentSize
        /// </summary>
        public override CCSize contentSize
        {
            get { return base.contentSize; }
            set
            {
                float factor = CCDirector.sharedDirector().ContentScaleFactor;

                m_pSquareVertices[1].x = value.width * factor;
                m_pSquareVertices[2].y = value.height * factor;
                m_pSquareVertices[3].x = value.width * factor;
                m_pSquareVertices[3].y = value.height * factor;

                base.contentSize = value;
            }
        }

        #region create and init

        /// <summary>
        /// creates a CCLayer with color, width and height in Points
        /// </summary>
        public static CCLayerColor layerWithColorWidthHeight(ccColor4B color, float width, float height)
        {
            CCLayerColor pLayer = new CCLayerColor();
            if (pLayer != null && pLayer.initWithColorWidthHeight(color, width, height))
            {
                return pLayer;
            }
            pLayer = null;
            return null;
        }

        /// <summary>
        /// creates a CCLayer with color. Width and height are the window size. 
        /// </summary>
        public static CCLayerColor layerWithColor(ccColor4B color)
        {
            CCLayerColor pLayer = new CCLayerColor();
            if (pLayer != null && pLayer.initWithColor(color))
            {
                return pLayer;
            }
            pLayer = null;
            return null;
        }

        /// <summary>
        ///  initializes a CCLayer with color, width and height in Points
        /// </summary>
        public virtual bool initWithColorWidthHeight(ccColor4B color, float width, float height)
        {
            // default blend function
            m_tBlendFunc.src = 1;
            m_tBlendFunc.dst = 0x0303;

            m_tColor.r = color.r;
            m_tColor.g = color.g;
            m_tColor.b = color.b;
            m_cOpacity = color.a;
            for (int i = 0; i < m_pSquareVertices.Length; i++)
            {
                m_pSquareVertices[i].x = 0.0f;
                m_pSquareVertices[i].y = 0.0f;
            }

            this.updateColor();
            this.contentSize = new CCSize(width, height);
            return true;
        }

        /// <summary>
        /// initializes a CCLayer with color. Width and height are the window size.
        /// </summary>
        public virtual bool initWithColor(ccColor4B color)
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            this.initWithColorWidthHeight(color, s.width, s.height);
            return true;
        }

        #endregion

        #region changesize

        /// <summary>
        /// change width in Points
        /// </summary>
        /// <param name="w"></param>
        public void changeWidth(float w)
        {
            this.contentSize = new CCSize(w, base.m_tContentSize.height);
        }

        /// <summary>
        /// change height in Points
        /// </summary>
        /// <param name="h"></param>
        public void changeHeight(float h)
        {
            this.contentSize = new CCSize(base.m_tContentSize.width, h);
        }

        /// <summary>
        ///  change width and height in Points
        ///  @since v0.8
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void changeWidthAndHeight(float w, float h)
        {
            this.contentSize = new CCSize(w, h);
        }

        #endregion

        #region ICCRGBAProtocol

        protected byte m_cOpacity;
        /// <summary>
        /// Opacity: conforms to CCRGBAProtocol protocol
        /// </summary>
        public virtual byte Opacity
        {
            get { return m_cOpacity; }
            set { m_cOpacity = value; }
        }

        protected ccColor3B m_tColor;
        /// <summary>
        /// Color: conforms to CCRGBAProtocol protocol 
        /// </summary>
        public virtual ccColor3B Color
        {
            get { return m_tColor; }
            set { m_tColor = value; }
        }

        public bool IsOpacityModifyRGB
        {
            get { return false; }
            set { }
        }

        #endregion

        protected ccBlendFunc m_tBlendFunc;
        /// <summary>
        /// BlendFunction. Conforms to CCBlendProtocol protocol 
        /// </summary>
        public virtual ccBlendFunc BlendFunc
        {
            get { return m_tBlendFunc; }
            set { m_tBlendFunc = value; }
        }

        public virtual ICCRGBAProtocol convertToRGBAProtocol()
        {
            return (ICCRGBAProtocol)this;
        }
        public static CCLayerColor node()
        {
            CCLayerColor pRet = new CCLayerColor();
            if (pRet != null && pRet.init())
            {
                return pRet;
            }
            else
            {
                pRet = null;
                return pRet;
            }
        }

        protected virtual void updateColor()
        {
            for (int i = 0; i < 4; i++)
            {
                m_pSquareColors[i].r = m_tColor.r;
                m_pSquareColors[i].g = m_tColor.g;
                m_pSquareColors[i].b = m_tColor.b;
                m_pSquareColors[i].a = m_cOpacity;
            }
        }
    }
}
