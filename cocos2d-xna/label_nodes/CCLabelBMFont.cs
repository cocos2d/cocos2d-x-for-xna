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
    public class CCLabelBMFont : CCSpriteBatchNode, ICCLabelProtocol, ICCRGBAProtocol
    {
        // how many characters are supported
        const uint kCCBMFontMaxChars = 2048;//256,

        public static Dictionary<string, CCBMFontConfiguration> configurations;

        #region CCRGBAProtocol members

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

        #endregion

        // string to render
        protected string m_sString = "";
        protected CCBMFontConfiguration m_pConfiguration;

        public CCLabelBMFont()
        {

        }

        /// <summary>
        /// creates a bitmap font altas with an initial string and the FNT file
        /// </summary>
        public static CCLabelBMFont labelWithString(string str, string fntFile)
        {
            CCLabelBMFont pRet = new CCLabelBMFont();
            if (pRet.initWithString(str, fntFile))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// init a bitmap font altas with an initial string and the FNT file
        /// </summary>
        public bool initWithString(string theString, string fntFile)
        {
            Debug.Assert(theString != null);
            //CC_SAFE_RELEASE(m_pConfiguration);// allow re-init
            m_pConfiguration = FNTConfigLoadFile(fntFile);

            Debug.Assert(m_pConfiguration != null, "Error creating config for LabelBMFont");
            if (base.initWithFile(m_pConfiguration.m_sAtlasName, theString.Length))
            {
                m_cOpacity = 255;
                m_tColor = new ccColor3B(255, 255, 255);
                m_tContentSize = new CCSize(0, 0);
                m_bIsOpacityModifyRGB = m_pobTextureAtlas.Texture.HasPremultipliedAlpha;
                anchorPoint = new CCPoint(0.5f, 0.5f);
                this.setString(theString);
                return true;
            }
            return false;
        }

        public virtual void setString(string label)
        {
            m_sString = label;

            if (m_pChildren != null && m_pChildren.Count != 0)
            {
                for (int i = 0; i < m_pChildren.Count; i++)
                {
                    CCNode pNode = m_pChildren[i];
                    if (pNode != null)
                    {
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

        /// <summary>
        /// updates the font chars based on the string to render
        /// </summary>
        public void createFontChars()
        {
            int nextFontPositionX = 0;
            int nextFontPositionY = 0;
            int prev = -1;
            int kerningAmount = 0;

            CCSize tmpSize = new CCSize(0, 0);

            int longestLine = 0;
            int totalHeight = 0;

            int quantityOfLines = 1;

            int stringLen = m_sString.Length;

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
            nextFontPositionY = -(m_pConfiguration.m_uCommonHeight - m_pConfiguration.m_uCommonHeight * quantityOfLines);

            for (int i = 0; i < stringLen; i++)
            {
                int c = m_sString[i];
                Debug.Assert(c < kCCBMFontMaxChars, "LabelBMFont: character outside bounds");

                if (c == '\n')
                {
                    nextFontPositionX = 0;
                    nextFontPositionY -= (int)m_pConfiguration.m_uCommonHeight;
                    continue;
                }

                kerningAmount = this.kerningAmountForFirst(prev, c);

                ccBMFontDef fontDef = m_pConfiguration.m_pBitmapFontArray[c];

                CCRect rect = fontDef.rect;

                CCSprite fontChar = (CCSprite)(this.getChildByTag(i));
                if (fontChar == null)
                {
                    fontChar = new CCSprite();
                    fontChar.initWithBatchNodeRectInPixels(this, rect);
                    this.addChild(fontChar, 0, i);
                }
                else
                {
                    // reusing fonts
                    //fontChar = new CCSprite();
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
                prev = c;

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

            this.contentSizeInPixels = tmpSize;
        }

        public override CCPoint anchorPoint
        {
            get
            {
                return base.anchorPoint;
            }
            set
            {
                if (!CCPoint.CCPointEqualToPoint(value, m_tAnchorPoint))
                {
                    base.anchorPoint = value;
                    this.createFontChars();
                }
            }
        }

        public virtual ICCRGBAProtocol convertToRGBAProtocol()
        {
            return (ICCRGBAProtocol)this;
        }

        public virtual ICCLabelProtocol convertToLabelProtocol()
        {
            return (ICCLabelProtocol)this;
        }

#if CC_LABELBMFONT_DEBUG_DRAW
		virtual void draw();
#endif // CC_LABELBMFONT_DEBUG_DRAW

        #region generate CCBMFontConfiguration

        private char atlasNameFromFntFile(string fntFile)
        {
            throw new NotImplementedException();
        }

        private int kerningAmountForFirst(int first, int second)
        {
            int ret = 0;
            int key = (first << 16) | (second & 0xffff);

            if (m_pConfiguration.m_pKerningDictionary != null)
            {
                if (m_pConfiguration.m_pKerningDictionary.ContainsKey(key))
                {
                    tKerningHashElement element = m_pConfiguration.m_pKerningDictionary[key];
                    if (element != null)
                        ret = element.amount;
                }
            }
            return ret;
        }

        private static CCBMFontConfiguration FNTConfigLoadFile(string file)
        {
            CCBMFontConfiguration pRet = null;

            if (configurations == null)
            {
                configurations = new Dictionary<string, CCBMFontConfiguration>();
            }

            if (!configurations.Keys.Contains(file))
            {
                pRet = CCBMFontConfiguration.configurationWithFNTFile(file);
                configurations.Add(file, pRet);
            }
            else
            {
                pRet = configurations[file];
            }

            return pRet;
        }

        public static void FNTConfigRemoveCache()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Purges the cached data.
        /// Removes from memory the cached configurations and the atlas name dictionary.
        /// @since v0.99.3
        /// </summary>
        public static void purgeCachedData()
        {
            FNTConfigRemoveCache();
        }

        #endregion
    }
}

