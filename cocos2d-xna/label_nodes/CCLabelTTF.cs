/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
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
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    /// <summary>
    /// CCLabelTTF is a subclass of CCTextureNode that knows how to render text labels
    /// All features from CCTextureNode are valid in CCLabelTTF
    /// CCLabelTTF objects are slow. Consider using CCLabelAtlas or CCLabelBMFont instead.
    /// </summary>
    public class CCLabelTTF : CCSprite, ICCLabelProtocol
    {
        public CCLabelTTF()
        {
            this.m_eAlignment = CCTextAlignment.CCTextAlignmentCenter;
            this.m_pFontName = string.Empty;
            this.m_fFontSize = 0.0f;
        }

        public string description()
        {
            string ret = string.Format("<CCLabelTTF | FontName = {0}, FontSize = {1}>", m_pFontName, m_fFontSize);
            return ret;
        }

        /// <summary>
        /// creates a CCLabelTTF from a fontname, alignment, dimension and font size
        /// </summary>
        public static CCLabelTTF labelWithString(string label, CCSize dimensions, CCTextAlignment alignment, string fontName, float fontSize)
        {
            CCLabelTTF pRet = new CCLabelTTF();
            if (pRet != null && pRet.initWithString(label, dimensions, alignment, fontName, fontSize))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// creates a CCLabelTTF from a fontname and font size
        /// </summary>
        public static CCLabelTTF labelWithString(string label, string fontName, float fontSize)
        {
            CCLabelTTF pRet = new CCLabelTTF();
            if (pRet.initWithString(label, fontName, fontSize))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes the CCLabelTTF with a font name, alignment, dimension and font size
        /// </summary>
        public bool initWithString(string label, CCSize dimensions, CCTextAlignment alignment, string fontName, float fontSize)
        {
            Debug.Assert(label != null);
            if (init())
            {
                m_tDimensions = new CCSize(dimensions.width * CCDirector.sharedDirector().ContentScaleFactor, dimensions.height * CCDirector.sharedDirector().ContentScaleFactor);
                m_eAlignment = alignment;

                m_pFontName = fontName;

                m_fFontSize = fontSize * CCDirector.sharedDirector().ContentScaleFactor;
                this.setString(label);
                return true;
            }
            return false;
        }

        /// <summary>
        /// initializes the CCLabelTTF with a font name and font size
        /// </summary>
        public bool initWithString(string label, string fontName, float fontSize)
        {
            Debug.Assert(label != null);
            if (base.init())
            {
                m_tDimensions = new CCSize(0, 0);
                m_pFontName = fontName;

                m_fFontSize = fontSize * CCDirector.sharedDirector().ContentScaleFactor;
                this.setString(label);
                return true;
            }

            return false;
        }

        /// <summary>
        /// changes the string to render
        /// @warning Changing the string is as expensive as creating a new CCLabelTTF. To obtain better performance use CCLabelAtlas
        /// </summary>
        public void setString(string label)
        {
            m_pString = label;

            CCTexture2D texture;
            if (CCSize.CCSizeEqualToSize(m_tDimensions, new CCSize(0, 0)))
            {
                texture = new CCTexture2D();
                texture.initWithString(label, m_pFontName.ToString(), m_fFontSize);
            }
            else
            {
                texture = new CCTexture2D();
                texture.initWithString(label, m_tDimensions, m_eAlignment, m_pFontName.ToString(), m_fFontSize);
            }
            this.Texture = texture;

            CCRect rect = new CCRect(0, 0, 0, 0);
            rect.size = m_pobTexture.getContentSize();
            this.setTextureRect(rect);
        }

        public string getString()
        {
            return m_pString.ToString();
        }

        public virtual ICCLabelProtocol convertToLabelProtocol()
        {
            return (ICCLabelProtocol)this;
        }

        protected SpriteFont spriteFont;
        protected CCSize m_tDimensions;
        protected CCTextAlignment m_eAlignment;
        protected string m_pFontName;
        protected float m_fFontSize;
        protected string m_pString;
    }
}
