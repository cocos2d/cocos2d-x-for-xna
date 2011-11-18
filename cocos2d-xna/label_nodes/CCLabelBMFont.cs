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
    public class CCLabelBMFont : CCSpriteBatchNode, CCLabelProtocol, CCRGBAProtocol
    {
        // how many characters are supported
        const uint kCCBMFontMaxChars = 2048;//256,

        //** conforms to CCRGBAProtocol protocol */
        private byte m_cOpacity;
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

        //** conforms to CCRGBAProtocol protocol */
        //CC_PROPERTY_PASS_BY_REF(ccColor3B, m_tColor, Color)
        private ccColor3B m_tColor;
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

        //** conforms to CCRGBAProtocol protocol */
        //CC_PROPERTY(bool, m_bIsOpacityModifyRGB, IsOpacityModifyRGB)
        private bool m_bIsOpacityModifyRGB;
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

        // string to render
        protected string m_sString;
        protected CCBMFontConfiguration m_pConfiguration;

        public CCLabelBMFont()
        {
            m_sString = "";
            //CC_SAFE_RELEASE(m_pConfiguration);
        }

        #region Purges the cached data.
        /// <summary>
        /// Purges the cached data.
        /// Removes from memory the cached configurations and the atlas name dictionary.
        /// @since v0.99.3
        /// </summary>
        #endregion
        public static void purgeCachedData()
        {
            FNTConfigRemoveCache();
        }

        #region creates a bitmap font altas with an initial string and the FNT file
        /// <summary>
        /// creates a bitmap font altas with an initial string and the FNT file
        /// </summary>
        #endregion
        public static CCLabelBMFont labelWithString(string str, string fntFile)
        {
            CCLabelBMFont pRet = new CCLabelBMFont();
            if (pRet != null && pRet.initWithString(str, fntFile))
            {
                //pRet->autorelease();
                return pRet;
            }
            //CC_SAFE_DELETE(pRet)
            return null;
        }

        #region init a bitmap font altas with an initial string and the FNT file
        /// <summary>
        /// init a bitmap font altas with an initial string and the FNT file
        /// </summary>
        #endregion
        public bool initWithString(string theString, string fntFile)
        {
            Debug.Assert(theString != null);
            //CC_SAFE_RELEASE(m_pConfiguration);// allow re-init
            m_pConfiguration = FNTConfigLoadFile(fntFile);

            Debug.Assert(m_pConfiguration != null, "Error creating config for LabelBMFont");
            CCSpriteBatchNode ccspriteBatchNode = new CCSpriteBatchNode();
            if (ccspriteBatchNode.initWithFile(m_pConfiguration.m_sAtlasName, (uint)(theString.Length)))
            {
                m_cOpacity = 255;
                m_tColor = new ccColor3B(255, 255, 255);
                m_tContentSize = new CCSize(0, 0);
                m_bIsOpacityModifyRGB = m_pobTextureAtlas.getTexture().getHasPremultipliedAlpha();
                setAnchorPoint(new CCPoint(0.5f, 0.5f));
                this.setString(theString);
                return true;
            }
            return false;
        }

        #region updates the font chars based on the string to render
        /// <summary>
        /// updates the font chars based on the string to render
        /// </summary>
        #endregion
        public void createFontChars()
        {
            int nextFontPositionX = 0;
            int nextFontPositionY = 0;
            short prev = -1;
            int kerningAmount = 0;

            CCSize tmpSize = new CCSize(0, 0);

            int longestLine = 0;
            uint totalHeight = 0;

            uint quantityOfLines = 1;

            uint stringLen = (uint)m_sString.Length;

            if (0 == stringLen)
            {
                return;
            }

            for (int i = 0; i < stringLen - 1; ++i)
            {
                ushort c = m_sString[i];
                if (c == '\n')
                {
                    quantityOfLines++;
                }
            }

            totalHeight = m_pConfiguration.m_uCommonHeight * quantityOfLines;
            nextFontPositionY = -(int)(m_pConfiguration.m_uCommonHeight - m_pConfiguration.m_uCommonHeight * quantityOfLines);

            for (int i = 0; i < stringLen; i++)
            {
                ushort c = m_sString[i];
                Debug.Assert(c < kCCBMFontMaxChars, "LabelBMFont: character outside bounds");

                if (c == '\n')
                {
                    nextFontPositionX = 0;
                    nextFontPositionY -= (int)m_pConfiguration.m_uCommonHeight;
                    continue;
                }

                //kerningAmount = this.kerningAmountForFirst(prev, c);

                ccBMFontDef fontDef = m_pConfiguration.m_pBitmapFontArray[c];

                CCRect rect = fontDef.rect;

                CCSprite fontChar;

                fontChar = (CCSprite)(this.getChildByTag(i));
                if (fontChar != null)
                {
                    fontChar = new CCSprite();
                    //fontChar.initWithBatchNodeRectInPixels(this, rect);
                    this.addChild(fontChar, 0, i);
                    //fontChar.release();
                }
                else
                {
                    // reusing fonts
                    fontChar.setTextureRectInPixels(rect, false, rect.size);

                    // restore to default in case they were modified
                    fontChar.visible = true;
                    fontChar.Opacity = 255;
                }

                float yOffset = (float)(m_pConfiguration.m_uCommonHeight - fontDef.yOffset);
                fontChar.positionInPixels = (new CCPoint(nextFontPositionX + fontDef.xOffset + fontDef.rect.size.width / 2.0f + kerningAmount,
                    (float)nextFontPositionY + yOffset - rect.size.height / 2.0f));

                //		NSLog(@"position.y: %f", fontChar.position.y);

                // update kerning
                nextFontPositionX += m_pConfiguration.m_pBitmapFontArray[c].xAdvance + kerningAmount;
                //prev = c;

                // Apply label properties
                fontChar.IsOpacityModifyRGB = m_bIsOpacityModifyRGB;
                // Color MUST be set before opacity, since opacity might change color if OpacityModifyRGB is on
                fontChar.Color = m_tColor;

                // only apply opaccity if it is different than 255 )
                // to prevent modifying the color too (issue #610)
                if (m_cOpacity != 255)
                {
                    fontChar.Opacity = m_cOpacity;
                }

                if (longestLine < nextFontPositionX)
                {
                    longestLine = nextFontPositionX;
                }
            }

            tmpSize.width = (float)longestLine;
            tmpSize.height = (float)totalHeight;

            //this.setContentSizeInPixels(tmpSize);
        }

        //public virtual string gsString
        //{
        //    get
        //    {
        //        return m_sString;
        //    }
        //    set
        //    {
        //        m_sString = "";
        //        m_sString = value;

        //        if (m_pChildren != null && m_pChildren.Count > 0)
        //        {
        //            for (int i = 0; i < m_pChildren.Count; i++)
        //            {
        //                CCObject child = m_pChildren[i];
        //                CCNode pNode = (CCNode)child;
        //                if (pNode != null)
        //                {
        //                    // pNode.setIsVisible(false);
        //                    pNode.visible = false;
        //                }
        //            }
        //        }
        //        this.createFontChars();
        //    }
        //}

        public virtual void setString(string label)
        {
            m_sString = "";
            m_sString = label;

            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    CCObject child = m_pChildren[i];
                    CCNode pNode = (CCNode)child;
                    if (pNode != null)
                    {
                        // pNode.setIsVisible(false);
                        pNode.visible = false;
                    }
                }
            }
            this.createFontChars();
        }

        public virtual string getString()
        {
            return m_sString;
        }

        public virtual void setCString(string label)
        {
            setString(label);
        }

        // varPoint=var
        public virtual void setAnchorPoint(CCPoint varPoint)
        {
            if (!CCPoint.CCPointEqualToPoint(varPoint, m_tAnchorPoint))
            {
                //CCSpriteBatchNode.setAnchorPoint(varPoint);
                this.createFontChars();
            }
        }

        public virtual CCRGBAProtocol convertToRGBAProtocol()
        {
            return (CCRGBAProtocol)this;
        }

        public virtual CCLabelProtocol convertToLabelProtocol()
        {
            return (CCLabelProtocol)this;
        }

#if CC_LABELBMFONT_DEBUG_DRAW
		virtual void draw();
#endif // CC_LABELBMFONT_DEBUG_DRAW

        private char atlasNameFromFntFile(string fntFile)
        {
            throw new NotImplementedException();
        }

        private int kerningAmountForFirst(ushort first, ushort second)
        {
            int ret = 0;
            int key = (first << 16) | (second & 0xffff);

            if (m_pConfiguration.m_pKerningDictionary!=null)
            {
                tKerningHashElement element = null;
                //HASH_FIND_INT(m_pConfiguration.m_pKerningDictionary, key, element);		
                if (element != null)
                    ret = element.amount;
            }
            return ret;
        }

        private static CCBMFontConfiguration FNTConfigLoadFile(string file)
        {
            throw new NotImplementedException();
        }

        public static void FNTConfigRemoveCache()
        {
            throw new NotImplementedException();
        }

        //public ccColor3B Color
        //{
        //    get
        //    {
        //        return m_tColor;
        //    }
        //    set
        //    {
        //        m_tColor = value;
        //        if (m_pChildren != null && m_pChildren.Count > 0)
        //        {
        //            for (int i = 0; i < m_pChildren.Count; i++)
        //            {
        //                CCObject child = m_pChildren[i];
        //                CCSprite pNode = (CCSprite)child;
        //                if (pNode != null)
        //                {
        //                    //pNode.setColor(m_tColor);
        //                    pNode.Color = m_tColor;
        //                }
        //            }
        //        }
        //    }
        //}

        public void setColor(ccColor3B var)
        {
            m_tColor = var;
            if (m_pChildren != null && m_pChildren.Count > 0)
            {
                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    CCObject child = m_pChildren[i];
                    CCSprite pNode = (CCSprite)child;
                    if (pNode != null)
                    {
                        //pNode.setColor(m_tColor);
                        pNode.Color = m_tColor;
                    }
                }
            }
        }

        public ccColor3B getColor()
        {
            return m_tColor;
        }

        //public byte Opacity
        //{
        //    get
        //    {
        //        return m_cOpacity;
        //    }
        //    set
        //    {
        //        m_cOpacity = value;

        //        if (m_pChildren != null && m_pChildren.Count != 0)
        //        {
        //            for (int i = 0; i < m_pChildren.Count; i++)
        //            {
        //                CCObject child = m_pChildren[i];
        //                CCNode pNode = (CCNode)child;
        //                if (pNode != null)
        //                {
        //                    CCRGBAProtocol pRGBAProtocol = pNode.convertToRGBAProtocol();
        //                    if (pRGBAProtocol != null)
        //                    {
        //                        pRGBAProtocol.Opacity = m_cOpacity;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public void setOpacity(byte var)
        {
            m_cOpacity = var;

            if (m_pChildren != null && m_pChildren.Count != 0)
            {
                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    CCObject child = m_pChildren[i];
                    CCNode pNode = (CCNode)child;
                    if (pNode != null)
                    {
                        //CCRGBAProtocol pRGBAProtocol = pNode.convertToRGBAProtocol();
                        //if (pRGBAProtocol != null)
                        //{
                        //    pRGBAProtocol.Opacity = m_cOpacity;
                        //}
                    }
                }
            }
        }

        public byte getOpacity()
        {
            return m_cOpacity;
        }

        //public bool IsOpacityModifyRGB
        //{
        //    get
        //    {
        //        return m_bIsOpacityModifyRGB;
        //    }
        //    set
        //    {
        //        m_bIsOpacityModifyRGB = value;
        //        if (m_pChildren != null && m_pChildren.Count != 0)
        //        {
        //            for (int i = 0; i < m_pChildren.Count; i++)
        //            {
        //                CCObject child = m_pChildren[i];
        //                CCNode pNode = (CCNode)child;
        //                if (pNode != null)
        //                {
        //                    CCRGBAProtocol pRGBAProtocol = pNode.convertToRGBAProtocol();
        //                    if (pRGBAProtocol != null)
        //                    {
        //                        pRGBAProtocol.IsOpacityModifyRGB = m_bIsOpacityModifyRGB;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public void setIsOpacityModifyRGB(bool var)
        {
            m_bIsOpacityModifyRGB = var;
            if (m_pChildren != null && m_pChildren.Count != 0)
            {
                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    CCObject child = m_pChildren[i];
                    CCNode pNode = (CCNode)child;
                    if (pNode != null)
                    {
                        //CCRGBAProtocol pRGBAProtocol = pNode.convertToRGBAProtocol();
                        //if (pRGBAProtocol != null)
                        //{
                        //    pRGBAProtocol.IsOpacityModifyRGB = m_bIsOpacityModifyRGB;
                        //}
                    }
                }
            }
        }

        public bool getIsOpacityModifyRGB() 
        {
            return m_bIsOpacityModifyRGB;
        }

    }
}

