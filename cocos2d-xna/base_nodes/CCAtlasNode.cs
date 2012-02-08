/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
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
using System.Text;
using System.Diagnostics;

namespace cocos2d
{
    ///<summary>
    /// CCAtlasNode is a subclass of CCNode that implements the CCRGBAProtocol and CCTextureProtocol protocol
    /// It knows how to render a TextureAtlas object.
    /// If you are going to render a TextureAtlas consider subclassing CCAtlasNode (or a subclass of CCAtlasNode)
    /// All features from CCNode are valid, plus the following features:
    ///- opacity and RGB colors
    ///</summary>
    public class CCAtlasNode : CCNode, ICCRGBAProtocol, ICCTextureProtocol
    {
        //chars per row
        protected int m_uItemsPerRow;
        //chars per column
        protected int m_uItemsPerColumn;

        //width of each char
        protected int m_uItemWidth;
        //height of each char
        protected int m_uItemHeight;

        protected ccColor3B m_tColorUnmodified;

        protected CCTextureAtlas m_pTextureAtlas;
        public CCTextureAtlas TextureAtlas
        {
            get
            {
                return m_pTextureAtlas;
            }
            set
            {
                m_pTextureAtlas = value;
            }
        }

        // protocol variables
        protected bool m_bIsOpacityModifyRGB;
        public bool IsOpacityModifyRGB
        {
            get
            {
                return m_bIsOpacityModifyRGB;
            }
            set
            {
                ccColor3B oldColor = this.m_tColor;
                m_bIsOpacityModifyRGB = value;
                this.m_tColor = oldColor;
            }
        }

        protected ccBlendFunc m_tBlendFunc;
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

        protected byte m_cOpacity;
        public byte Opacity
        {
            get
            {
                return m_cOpacity;
            }
            set
            {
                m_cOpacity = value;

                // special opacity for premultiplied textures
                if (m_bIsOpacityModifyRGB)
                    this.Color = m_tColorUnmodified;
            }
        }

        protected ccColor3B m_tColor;
        public ccColor3B Color
        {
            get
            {
                if (m_bIsOpacityModifyRGB)
                {
                    return m_tColorUnmodified;
                }
                return m_tColor;
            }
            set
            {
                m_tColor = new ccColor3B(value.r, value.g, value.b);
                m_tColorUnmodified = new ccColor3B(value.r, value.g, value.b);

                if (m_bIsOpacityModifyRGB)
                {
                    m_tColor.r = (byte)(value.r * m_cOpacity / 255);
                    m_tColor.g = (byte)(value.g * m_cOpacity / 255);
                    m_tColor.b = (byte)(value.b * m_cOpacity / 255);
                }

                this.updateAtlasValues();
            }
        }

        // quads to draw
        protected int m_uQuadsToDraw;
        public int QuadsToDraw
        {
            get
            {
                return m_uQuadsToDraw;
            }
            set
            {
                m_uQuadsToDraw = value;
            }
        }

        public CCAtlasNode()
        {
            m_tBlendFunc = new ccBlendFunc();
        }

        /// <summary>
        /// creates a CCAtlasNode  with an Atlas file the width and height of each item and the quantity of items to render
        /// </summary>
        public static CCAtlasNode atlasWithTileFile(string tile, int tileWidth, int tileHeight, int itemsToRender)
        {
            CCAtlasNode pRet = new CCAtlasNode();
            if (pRet.initWithTileFile(tile, tileWidth, tileHeight, itemsToRender))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes an CCAtlasNode  with an Atlas file the width and height of each item and the quantity of items to render
        /// </summary>
        public bool initWithTileFile(string tile, int tileWidth, int tileHeight, int itemsToRender)
        {
            Debug.Assert(tile != null);
            m_uItemWidth = tileWidth;
            m_uItemHeight = tileHeight;

            m_cOpacity = 255;
            m_tColor = m_tColorUnmodified = ccTypes.ccWHITE;
            m_bIsOpacityModifyRGB = true;

            m_tBlendFunc.src = ccMacros.CC_BLEND_SRC;
            m_tBlendFunc.dst = ccMacros.CC_BLEND_DST;

            // double retain to avoid the autorelease pool
            // also, using: self.textureAtlas supports re-initialization without leaking
            this.m_pTextureAtlas = new CCTextureAtlas();
            this.m_pTextureAtlas.initWithFile(tile, itemsToRender);

            if (m_pTextureAtlas == null)
            {
                Debug.WriteLine("cocos2d: Could not initialize CCAtlasNode. Invalid Texture.");
                return false;
            }

            this.updateBlendFunc();
            this.updateOpacityModifyRGB();

            this.calculateMaxItems();

            m_uQuadsToDraw = itemsToRender;

            return true;
        }

        //CCAtlasNode - Atlas generation

        private void calculateMaxItems()
        {
            CCSize s = m_pTextureAtlas.Texture.ContentSizeInPixels;
            m_uItemsPerColumn = (int)(s.height / m_uItemHeight);
            m_uItemsPerRow = (int)(s.width / m_uItemWidth);
        }

        /// <summary>
        ///  updates the Atlas (indexed vertex array).
        ///  Shall be overriden in subclasses
        /// </summary>
        public virtual void updateAtlasValues()
        {
            Debug.Assert(false, "CCAtlasNode:Abstract updateAtlasValue not overriden");
            //[NSException raise:@"CCAtlasNode:Abstract" format:@"updateAtlasValue not overriden"];
        }

        // CCAtlasNode - draw
        public override void draw()
        {
            base.draw();

            // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Unneeded states: GL_COLOR_ARRAY
            //glDisableClientState(GL_COLOR_ARRAY);

            // glColor4ub isn't implement on some android devices
            // glColor4ub( m_tColor.r, m_tColor.g, m_tColor.b, m_cOpacity); 

            //CCApplication app = CCApplication.sharedApplication();
            //app.basicEffect.VertexColorEnabled = true;


            //glColor4f(((GLfloat)m_tColor.r) / 255, ((GLfloat)m_tColor.g) / 255, ((GLfloat)m_tColor.b) / 255, ((GLfloat)m_cOpacity) / 255);
            //bool newBlend = m_tBlendFunc.src != CC_BLEND_SRC || m_tBlendFunc.dst != CC_BLEND_DST;
            //if (newBlend)
            //{
            //    glBlendFunc(m_tBlendFunc.src, m_tBlendFunc.dst);
            //}

            m_pTextureAtlas.drawNumberOfQuads(m_uQuadsToDraw, 0);

            //if (newBlend)
            //    glBlendFunc(CC_BLEND_SRC, CC_BLEND_DST);

            // is this chepear than saving/restoring color state ?
            // XXX: There is no need to restore the color to (255,255,255,255). Objects should use the color
            // XXX: that they need
            //	glColor4ub( 255, 255, 255, 255);

            // restore default GL state
            //glEnableClientState(GL_COLOR_ARRAY);

        }

        public virtual ICCRGBAProtocol convertToRGBAProtocol()
        {
            return (ICCRGBAProtocol)this;
        }

        // CC Texture protocol
        public virtual CCTexture2D Texture
        {
            get
            {
                return m_pTextureAtlas.Texture;
            }
            set
            {
                m_pTextureAtlas.Texture = value;
                this.updateBlendFunc();
                this.updateOpacityModifyRGB();
            }
        }

        private void updateBlendFunc()
        {
            if (!m_pTextureAtlas.Texture.HasPremultipliedAlpha)
            {
                m_tBlendFunc.src = OGLES.GL_SRC_ALPHA;
                m_tBlendFunc.dst = OGLES.GL_ONE_MINUS_SRC_ALPHA;
            }
        }

        private void updateOpacityModifyRGB()
        {
            m_bIsOpacityModifyRGB = m_pTextureAtlas.Texture.HasPremultipliedAlpha;
        }

        /// <summary>
        /// setIsOpacityModifyRGB getIsOpacityModifyRGB->ccIsOpacityModifyRGB
        /// </summary>
        public bool ccIsOpacityModifyRGB
        {
            get
            {
                return m_bIsOpacityModifyRGB;
            }
            set
            {
                ccColor3B oldColor = this.m_tColor;
                m_bIsOpacityModifyRGB = value;
                this.m_tColor = oldColor;
            }
        }

        public int ccQuadsToDraw
        {
            get
            {
                return m_uQuadsToDraw;
            }
            set
            {
                m_uQuadsToDraw = value;
            }
        }

        public CCTextureAtlas ccTextureAtlas
        {
            get { return m_pTextureAtlas; }
            set { m_pTextureAtlas = value; }
        }
    }
}
