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

    #region It can be as a replacement of CCLabel since it is MUCH faster.
    /// <summary>
    /// It can be as a replacement of CCLabel since it is MUCH faster.
    ///CCLabelAtlas versus CCLabel:
    ///CCLabelAtlas is MUCH faster than CCLabel
    ///CCLabelAtlas "characters" have a fixed height and width
    ///CCLabelAtlas "characters" can be anything you want since they are taken from an image file
    /// A more flexible class is CCLabelBMFont. It supports variable width characters and it also has a nice editor.
    /// </summary>
    #endregion
    public class CCLabelAtlas : CCAtlasNode, CCLabelProtocol
    {
        public CCLabelAtlas()
        {
            m_sString = "";
        }

        #region creates the CCLabelAtlas with a string, a char map file(the atlas), the width and height of each element and the starting char of the atlas
        /// <summary>
        /// creates the CCLabelAtlas with a string, a char map file(the atlas), the width and height of each element and the starting char of the atlas
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        #endregion
        public static CCLabelAtlas labelWithString(string label, string charMapFile, uint itemWidth, uint itemHeight, char startCharMap)
        {
            CCLabelAtlas pRet = new CCLabelAtlas();
            if (pRet != null && pRet.initWithString(label, charMapFile, itemWidth, itemHeight, startCharMap))
            {
                //pRet.autorelease();
                return pRet;
            }
            //CC_SAFE_DELETE(pRet)
            return null;
        }

        #region initializes the CCLabelAtlas with a string, a char map file(the atlas), the width and height of each element and the starting char of the atlas
        /// <summary>
        /// initializes the CCLabelAtlas with a string, a char map file(the atlas), the width and height of each element and the starting char of the atlas
        /// </summary>
        /// <param name="label"></param>
        /// <param name="charMapFile"></param>
        /// <param name="itemWidth"></param>
        /// <param name="itemHeight"></param>
        /// <param name="startCharMap"></param>
        /// <returns></returns>
        #endregion
        public bool initWithString(string label, string charMapFile, uint itemWidth, uint itemHeight, char startCharMap)
        {
            Debug.Assert(label != null);
            CCAtlasNode ccAtlasNode = new CCAtlasNode();
            if (ccAtlasNode.initWithTileFile(charMapFile, itemWidth, itemHeight, (uint)label.Length))
            {
                m_cMapStartChar = startCharMap;
                this.setString(label);
                return true;
            }
            return false;
        }

        // super methods
        public override void updateAtlasValues()
        {
            uint n = (uint)m_sString.Length;
            ccV3F_C4B_T2F_Quad quad;
            string s = (string)m_sString;
            CCTexture2D texture = m_pTextureAtlas.getTexture();
            float textureWide = (float)texture.getTexture2D().Width;
            float textureHigh = (float)texture.getTexture2D().Height;
            for (int i = 0; i < n; i++)
            {
                char a = (char)(s[i] - m_cMapStartChar);
                float row = (float)(a % m_uItemsPerRow);
                float col = (float)(a / m_uItemsPerRow);

#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
            // Issue #938. Don't use texStepX & texStepY
            float left		= (2 * row * m_uItemWidth + 1) / (2 * textureWide);
            float right		= left + (m_uItemWidth * 2 - 2) / (2 * textureWide);
            float top		= (2 * col * m_uItemHeight + 1) / (2 * textureHigh);
            float bottom	= top + (m_uItemHeight * 2 - 2) / (2 * textureHigh);
#else
                float left = row * m_uItemWidth / textureWide;
                float right = left + m_uItemWidth / textureWide;
                float top = col * m_uItemHeight / textureHigh;
                float bottom = top + m_uItemHeight / textureHigh;
#endif // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

                quad.tl.texCoords.u = left;
                quad.tl.texCoords.v = top;
                quad.tr.texCoords.u = right;
                quad.tr.texCoords.v = top;
                quad.bl.texCoords.u = left;
                quad.bl.texCoords.v = bottom;
                quad.br.texCoords.u  = right;
                quad.br.texCoords.v = bottom;

                quad.bl.vertices.x = (float)(i * m_uItemWidth);
                quad.bl.vertices.y = 0;
                quad.bl.vertices.z = 0.0f;
                quad.br.vertices.x = (float)(i * m_uItemWidth + m_uItemWidth);
                quad.br.vertices.y = 0;
                quad.br.vertices.z = 0.0f;
                quad.tl.vertices.x = (float)(i * m_uItemWidth);
                quad.tl.vertices.y = (float)(m_uItemHeight);
                quad.tl.vertices.z = 0.0f;
                quad.tr.vertices.x = (float)(i * m_uItemWidth + m_uItemWidth);
                quad.tr.vertices.y = (float)(m_uItemHeight);
                quad.tr.vertices.z = 0.0f;
                //m_pTextureAtlas.updateQuad(quad,(uint)i);
            }
        }

        #region setString getString->StrString
        /// <summary>
        /// setString getString->StrString
        /// </summary>
        #endregion
        public virtual string StrString
        {
            get
            {
                return m_sString;
            }

            set
            {
                uint len = (uint)value.Length;
                if (len > m_pTextureAtlas.getTotalQuads())
                {
                    m_pTextureAtlas.resizeCapacity(len);
                }
                m_sString = "";
                this.updateAtlasValues();

                CCSize s=new CCSize();
                s.width = (float)(len * m_uItemWidth);
                s.height = (float)(m_uItemHeight);
                //this.setContentSizeInPixels(s);

                m_uQuadsToDraw = len;
            }
        }

        //#if CC_LABELATLAS_DEBUG_DRAW
        //            virtual void draw()
        //        {
        //             base.draw();
        //             CCSize s = this.getContentSize();
        //             CCPoint vertices[4]=
        //           {
        //             ccp(0,0),ccp(s.width,0),
        //             ccp(s.width,s.height),ccp(0,s.height),
        //           };
        //             ccDrawPoly(vertices, 4, true);
        //        }
        //#endif

        public virtual CCLabelProtocol convertToLabelProtocol()
        {
            return (CCLabelProtocol)this;
        }

        // string to render
        protected string m_sString;
        // the first char in the charmap
        protected char m_cMapStartChar;

        public void setString(string label)
        {
            throw new NotImplementedException();
        }

        public string getString()
        {
            throw new NotImplementedException();
        }
    }
}
