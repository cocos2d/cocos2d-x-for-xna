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
    public class CCAtlasNode : CCNode, CCRGBAProtocol, CCTextureProtocol
    {
        //chars per row
        protected uint m_uItemsPerRow;
        //chars per column
        protected uint m_uItemsPerColumn;

        //width of each char
        protected uint m_uItemWidth;
        //height of each char
        protected uint m_uItemHeight;

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
                m_bIsOpacityModifyRGB = value;
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
            }
        }

        protected ccColor3B m_tColor;
        public ccColor3B Color
        {
            get
            {
                return m_tColor;
            }
            set
            {
                m_tColor = value;
            }
        }

        // quads to draw
        protected uint m_uQuadsToDraw;
        public uint QuadsToDraw
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

        }

        #region creates a CCAtlasNode  with an Atlas file the width and height of each item and the quantity of items to render
        /// <summary>
        /// creates a CCAtlasNode  with an Atlas file the width and height of each item and the quantity of items to render
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <param name="itemsToRender"></param>
        /// <returns></returns>
        #endregion
        public static CCAtlasNode atlasWithTileFile(string tile, uint tileWidth, uint tileHeight, uint itemsToRender)
        {
            CCAtlasNode pRet = new CCAtlasNode();
            if (pRet.initWithTileFile(tile, tileWidth, tileHeight, itemsToRender))
            {
                //pRet->autorelease();
                return pRet;
            }
            //CC_SAFE_DELETE(pRet);
            return null;
        }

        /// <summary>
        /// initializes an CCAtlasNode  with an Atlas file the width and height of each item and the quantity of items to render
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tileWidth"></param>
        /// <param name="tileHeight"></param>
        /// <param name="itemsToRender"></param>
        /// <returns></returns>
        public  bool initWithTileFile(string tile, uint tileWidth, uint tileHeight, uint itemsToRender)
        {
            Debug.Assert(tile != null);
            m_uItemWidth = (uint)(tileWidth);
            m_uItemHeight = (uint)(tileHeight);

            m_cOpacity = 255;
            m_tColor = m_tColorUnmodified = new ccColor3B(255, 255, 255); //ccWHITE=static const ccColor3B ccWHITE={255,255,255};
            m_bIsOpacityModifyRGB = true;

            m_tBlendFunc.src = 1; //CC_BLEND_SRC=GL_ONE=1
            m_tBlendFunc.dst = 0x0304; //CC_BLEND_DST=GL_ONE_MINUS_SRC_ALPHA=0x0304

            // double retain to avoid the autorelease pool
            // also, using: self.textureAtlas supports re-initialization without leaking
            this.m_pTextureAtlas = new CCTextureAtlas();
            m_pTextureAtlas.initWithFile(tile, itemsToRender);

            if (m_pTextureAtlas == null)
            {
                //CCLOG("cocos2d: Could not initialize CCAtlasNode. Invalid Texture.");
                //delete this;
                return false;
            }

            this.updateBlendFunc();
            this.updateOpacityModifyRGB();

            this.calculateMaxItems();

            m_uQuadsToDraw = itemsToRender;

            return true;
        }

        #region updates the Atlas (indexed vertex array).
        /// <summary>
        ///  updates the Atlas (indexed vertex array).
        ///  Shall be overriden in subclasses
        /// </summary>
        #endregion
        public virtual void updateAtlasValues()
        {
            Debug.Assert(false, "CCAtlasNode:Abstract updateAtlasValue not overriden");
            //[NSException raise:@"CCAtlasNode:Abstract" format:@"updateAtlasValue not overriden"];
        }

        public override void draw()
        {
            //CCNode.draw();

            // Default GL states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Needed states: GL_TEXTURE_2D, GL_VERTEX_ARRAY, GL_TEXTURE_COORD_ARRAY
            // Unneeded states: GL_COLOR_ARRAY
            //glDisableClientState(GL_COLOR_ARRAY);

            // glColor4ub isn't implement on some android devices
            // glColor4ub( m_tColor.r, m_tColor.g, m_tColor.b, m_cOpacity); 
            // glColor4f(((float)m_tColor.r) / 255, ((float)m_tColor.g) / 255, ((float)m_tColor.b) / 255, ((float)m_cOpacity) / 255);
            //bool newBlend = m_tBlendFunc.src != CC_BLEND_SRC || m_tBlendFunc.dst != CC_BLEND_DST;
            // if (newBlend)
            //  {
            // glBlendFunc(m_tBlendFunc.src, m_tBlendFunc.dst);
            //}

            // m_pTextureAtlas.drawNumberOfQuads(m_uQuadsToDraw, 0);

            //if (newBlend)
            //glBlendFunc(CC_BLEND_SRC, CC_BLEND_DST);

            // is this chepear than saving/restoring color state ?
            // XXX: There is no need to restore the color to (255,255,255,255). Objects should use the color
            // XXX: that they need
            //	glColor4ub( 255, 255, 255, 255);

            // restore default GL state
            //glEnableClientState(GL_COLOR_ARRAY);
        }

        public virtual CCRGBAProtocol convertToRGBAProtocol()
        {
            return (CCRGBAProtocol)this;
        }

        // CC Texture protocol

        #region returns the used texture
        /// <summary>
        /// returns the used texture
        /// </summary>
        /// <returns></returns>
        #endregion
        public virtual CCTexture2D getTexture()
        {
            return m_pTextureAtlas.Texture;
        }

        #region sets a new texture. it will be retained
        /// <summary>
        /// sets a new texture. it will be retained
        /// </summary>
        /// <param name="texture"></param>
        #endregion
        public virtual void setTexture(CCTexture2D texture)
        {
            m_pTextureAtlas.Texture = texture;
            this.updateBlendFunc();
            this.updateOpacityModifyRGB();
        }

        //public virtual CCTexture2D ccTexture
        //{
        //    get
        //    {
        //        return m_pTextureAtlas.Texture;
        //    }
        //    set
        //    {
        //        m_pTextureAtlas.Texture=value;
        //        this.updateBlendFunc();
        //        this.updateOpacityModifyRGB();
        //    }
        //}

        private void calculateMaxItems()
        {
            CCSize s = m_pTextureAtlas.getTexture().getContentSizeInPixels();
            m_uItemsPerColumn = (uint)(s.height / m_uItemHeight);
            m_uItemsPerRow = (uint)(s.width / m_uItemWidth);
        }

        private void updateBlendFunc()
        {
            if (!m_pTextureAtlas.getTexture().getHasPremultipliedAlpha())
            {
                m_tBlendFunc.src = 0x0302;
                m_tBlendFunc.dst = 0x0305;
            }
        }

        private void updateOpacityModifyRGB()
        {
            m_bIsOpacityModifyRGB = m_pTextureAtlas.getTexture().getHasPremultipliedAlpha();
        }

        #region getColor,setColor->ccColor
        /// <summary>
        /// getColor,setColor->ccColor
        /// </summary>
        #endregion
        public ccColor3B ccColor
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
                m_tColor = m_tColorUnmodified = value;

                if (m_bIsOpacityModifyRGB)
                {
                    m_tColor.r = (byte)(value.r * m_cOpacity / 255);
                    m_tColor.g = (byte)(value.g * m_cOpacity / 255);
                    m_tColor.b = (byte)(value.b * m_cOpacity / 255);
                }
            }
        }

        #region etOpacity setOpacity->ccOpacity
        /// <summary>
        /// getOpacity setOpacity->ccOpacity
        /// </summary>
        #endregion 
        public byte ccOpacity
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
                    this.ccColor = m_tColorUnmodified;

            }
        }

        #region setIsOpacityModifyRGB getIsOpacityModifyRGB->ccIsOpacityModifyRGB
        /// <summary>
        /// setIsOpacityModifyRGB getIsOpacityModifyRGB->ccIsOpacityModifyRGB
        /// </summary>
        #endregion
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

        public void setBlendFunc(ccBlendFunc blendFunc)
        {
            m_tBlendFunc = blendFunc;
        }

        public ccBlendFunc getBlendFunc()
        {
            return m_tBlendFunc;
        }

        //public ccBlendFunc BlendFunc
        //{
        //    get 
        //    {
        //        return m_tBlendFunc;
        //    }
        //    set
        //    {
        //        m_tBlendFunc = value;
        //    }
        //}

        public uint ccQuadsToDraw
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
            get
            {
                return m_pTextureAtlas;
            }
            set
            {
                //CC_SAFE_RETAIN(var);
                //CC_SAFE_RELEASE(m_pTextureAtlas);
                m_pTextureAtlas = value;
            }
        }
    }
}
