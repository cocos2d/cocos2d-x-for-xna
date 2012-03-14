/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
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
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        protected VertexPositionColor[] vertices = new VertexPositionColor[4];
        short[] indexes = new short[6];

        public CCLayerColor()
        {
            initWithColor(ccTypes.ccc4(0, 0, 0, 0));
        }

        public static CCLayerColor node()
        {
            CCLayerColor pRet = new CCLayerColor();
            if (pRet.init())
            {
                return pRet;
            }

            return null;
        }

        public override void draw()
        {
            base.draw();

            CCApplication app = CCApplication.sharedApplication();
            CCSize size = CCDirector.sharedDirector().getWinSize();

            app.basicEffect.VertexColorEnabled = true;
            app.basicEffect.TextureEnabled = false;
            app.basicEffect.Alpha = (float)this.m_cOpacity / 255.0f;
            VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.Color, 0)
                });

            foreach (var pass in app.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                app.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip,
                    vertices, 0, 2);
            }

            app.basicEffect.Alpha = 1;
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

                vertices[0].Position = new Microsoft.Xna.Framework.Vector3(0, value.height * factor, 0);
                vertices[1].Position = new Microsoft.Xna.Framework.Vector3(value.width * factor, value.height * factor, 0);
                vertices[2].Position = new Microsoft.Xna.Framework.Vector3(0, 0, 0);
                vertices[3].Position = new Microsoft.Xna.Framework.Vector3(value.width * factor, 0, 0);

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
            if (pLayer.initWithColorWidthHeight(color, width, height))
            {
                return pLayer;
            }

            return null;
        }

        /// <summary>
        /// creates a CCLayer with color. Width and height are the window size. 
        /// </summary>
        public static CCLayerColor layerWithColor(ccColor4B color)
        {
            CCLayerColor pLayer = new CCLayerColor();
            if (pLayer.initWithColor(color))
            {
                return pLayer;
            }

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
                m_pSquareVertices[i] = new ccVertex2F();
                m_pSquareVertices[i].x = 0.0f;
                m_pSquareVertices[i].y = 0.0f;

                vertices[i] = new VertexPositionColor();
            }

            indexes[0] = 0;
            indexes[0] = 1;
            indexes[0] = 2;
            indexes[0] = 2;
            indexes[0] = 1;
            indexes[0] = 3;

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
            set
            {
                m_cOpacity = value;
                updateColor();
            }
        }

        protected ccColor3B m_tColor = new ccColor3B(0, 0, 0);
        /// <summary>
        /// Color: conforms to CCRGBAProtocol protocol 
        /// </summary>
        public virtual ccColor3B Color
        {
            get { return m_tColor; }
            set
            {
                m_tColor = value;
                updateColor();
            }
        }

        public bool IsOpacityModifyRGB
        {
            get { return false; }
            set { }
        }

        protected ccBlendFunc m_tBlendFunc = new ccBlendFunc();
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

        #endregion

        protected virtual void updateColor()
        {
            for (int i = 0; i < 4; i++)
            {
                m_pSquareColors[i] = new ccColor4B();
                m_pSquareColors[i].r = m_tColor.r;
                m_pSquareColors[i].g = m_tColor.g;
                m_pSquareColors[i].b = m_tColor.b;
                m_pSquareColors[i].a = m_cOpacity;

                vertices[i].Color = new Microsoft.Xna.Framework.Color(m_tColor.r, m_tColor.g, m_tColor.b, m_cOpacity);
            }
        }
    }
}
