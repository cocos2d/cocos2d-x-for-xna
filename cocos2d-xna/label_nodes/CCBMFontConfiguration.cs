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
using cocos2d.Framework;
using System.IO;

namespace cocos2d
{
    /// <summary>
    /// CCBMFontConfiguration has parsed configuration of the the .fnt file
    /// @since v0.8
    /// </summary>
    public class CCBMFontConfiguration : CCObject
    {
        #region public

        // Creating a public interface so that the bitmapFontArray[] is accesible
        // The characters building up the font
        public Dictionary<int, ccBMFontDef> m_pBitmapFontArray = new Dictionary<int, ccBMFontDef>();

        // FNTConfig: Common Height
        public int m_uCommonHeight;

        // Padding
        public ccBMFontPadding m_tPadding = new ccBMFontPadding();

        // atlas name
        public string m_sAtlasName;

        // values for kerning
        public Dictionary<int, tKerningHashElement> m_pKerningDictionary = new Dictionary<int, tKerningHashElement>();

        #endregion

        public CCBMFontConfiguration()
        {
            //CCLOGINFO("cocos2d: deallocing CCBMFontConfiguration");
            this.purgeKerningDictionary();
            //m_sAtlasName.clear();
            m_sAtlasName = "";
        }

        public string Description
        {
            get
            {
                string[] ret = new string[100];
                //sprintf(ret, "<CCBMFontConfiguration | Kernings:%d | Image = %s>", HASH_COUNT(m_pKerningDictionary), m_sAtlasName.c_str());
                return ret.ToString();
            }
        }

        /// <summary>
        /// allocates a CCBMFontConfiguration with a FNT file
        /// </summary>
        public static CCBMFontConfiguration configurationWithFNTFile(string FNTfile)
        {
            CCBMFontConfiguration pRet = new CCBMFontConfiguration();
            if (pRet.initWithFNTfile(FNTfile))
            {
                return pRet;
            }

            return null;
        }

        /// <summary>
        /// initializes a BitmapFontConfiguration with a FNT file
        /// </summary>
        public bool initWithFNTfile(string FNTfile)
        {
            Debug.Assert(FNTfile != null && FNTfile.Length != 0);
            m_pKerningDictionary = new Dictionary<int, tKerningHashElement>();
            this.parseConfigFile(FNTfile);
            return true;
        }

        #region Private method

        private void parseConfigFile(string controlFile)
        {
            CCContent data = CCApplication.sharedApplication().content.Load<CCContent>(controlFile);
            string pBuffer = data.Content;
            long nBufSize = data.Content.Length;
            Debug.Assert(pBuffer != null, "CCBMFontConfiguration::parseConfigFile | Open file error.");

            if (string.IsNullOrEmpty(pBuffer))
            {
                return;
            }

            // parse spacing / padding
            string line;
            string strLeft = pBuffer;
            while (strLeft.Length > 0)
            {
                int pos = strLeft.IndexOf('\n');

                if (pos != -1)
                {
                    // the data is more than a line.get one line
                    line = strLeft.Substring(0, pos);
                    strLeft = strLeft.Substring(pos + 1);
                }
                else
                {
                    // get the left data
                    line = strLeft;
                    strLeft = null;
                }

                if (line.StartsWith("info face"))
                {
                    // XXX: info parsing is incomplete
                    // Not needed for the Hiero editors, but needed for the AngelCode editor
                    //			[self parseInfoArguments:line];
                    this.parseInfoArguments(line);
                }
                // Check to see if the start of the line is something we are interested in

                if (line.StartsWith("common lineHeight"))
                {
                    this.parseCommonArguments(line);
                }

                if (line.StartsWith("page id"))
                {
                    this.parseImageFileName(line, controlFile);
                }

                if (line.StartsWith("chars c"))
                {
                    // Ignore this line
                    continue;
                }

                if (line.StartsWith("char"))
                {
                    // Parse the current line and create a new CharDef
                    ccBMFontDef characterDefinition = new ccBMFontDef();
                    this.parseCharacterDefinition(line, characterDefinition);

                    // Add the CharDef returned to the charArray
                    m_pBitmapFontArray.Add(characterDefinition.charID, characterDefinition);
                }

                if (line.StartsWith("kernings count"))
                {
                    this.parseKerningCapacity(line);
                }

                if (line.StartsWith("kerning first"))
                {
                    this.parseKerningEntry(line);
                }
            }
        }

        private void parseCharacterDefinition(string line, ccBMFontDef characterDefinition)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // char id=32   x=0     y=0     width=0     height=0     xoffset=0     yoffset=44    xadvance=14     page=0  chnl=0 
            //////////////////////////////////////////////////////////////////////////

            // Character ID
            int index = line.IndexOf("id=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            characterDefinition.charID = int.Parse(value.Replace("id=", ""));
            //CCAssert(characterDefinition->charID < kCCBMFontMaxChars, "BitmpaFontAtlas: CharID bigger than supported");

            // Character x
            index = line.IndexOf("x=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.origin.x = float.Parse(value.Replace("x=", ""));

            // Character y
            index = line.IndexOf("y=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.origin.y = float.Parse(value.Replace("y=", ""));

            // Character width
            index = line.IndexOf("width=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.size.width = float.Parse(value.Replace("width=", ""));

            // Character height
            index = line.IndexOf("height=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.rect.size.height = float.Parse(value.Replace("height=", ""));

            // Character xoffset
            index = line.IndexOf("xoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.xOffset = int.Parse(value.Replace("xoffset=", ""));

            // Character yoffset
            index = line.IndexOf("yoffset=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.yOffset = int.Parse(value.Replace("yoffset=", ""));

            // Character xadvance
            index = line.IndexOf("xadvance=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            characterDefinition.xAdvance = int.Parse(value.Replace("xadvance=", ""));
        }

        // info face
        private void parseInfoArguments(string line)
        {
            //////////////////////////////////////////////////////////////////////////
            // possible lines to parse:
            // info face="Script" size=32 bold=0 italic=0 charset="" unicode=1 stretchH=100 smooth=1 aa=1 padding=1,4,3,2 spacing=0,0 outline=0
            // info face="Cracked" size=36 bold=0 italic=0 charset="" unicode=0 stretchH=100 smooth=1 aa=1 padding=0,0,0,0 spacing=1,1
            //////////////////////////////////////////////////////////////////////////

            // padding
            int index = line.IndexOf("padding=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);

            value = value.Replace("padding=", "");
            string[] temp = value.Split(',');
            m_tPadding.top = int.Parse(temp[0]);
            m_tPadding.right = int.Parse(temp[1]);
            m_tPadding.bottom = int.Parse(temp[2]);
            m_tPadding.left = int.Parse(temp[3]);

            //CCLOG("cocos2d: padding: %d,%d,%d,%d", m_tPadding.left, m_tPadding.top, m_tPadding.right, m_tPadding.bottom);
        }

        // common
        private void parseCommonArguments(string line)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // common lineHeight=104 base=26 scaleW=1024 scaleH=512 pages=1 packed=0
            //////////////////////////////////////////////////////////////////////////

            // Height
            int index = line.IndexOf("lineHeight=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            m_uCommonHeight = int.Parse(value.Replace("lineHeight=", ""));

            // scaleW. sanity check
            index = line.IndexOf("scaleW=") + "scaleW=".Length;
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            //CCAssert(atoi(value.c_str()) <= CCConfiguration::sharedConfiguration()->getMaxTextureSize(), "CCLabelBMFont: page can't be larger than supported");
            // scaleH. sanity check
            index = line.IndexOf("scaleH=") + "scaleH=".Length;
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            //CCAssert(atoi(value.c_str()) <= CCConfiguration::sharedConfiguration()->getMaxTextureSize(), "CCLabelBMFont: page can't be larger than supported");
            // pages. sanity check
            index = line.IndexOf("pages=") + "pages=".Length;
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            //CCAssert(atoi(value.c_str()) == 1, "CCBitfontAtlas: only supports 1 page");

            // packed (ignore) What does this mean ??
        }

        //page file
        private void parseImageFileName(string line, string fntFile)
        {
            //    //////////////////////////////////////////////////////////////////////////
            //// line to parse:
            //// page id=0 file="bitmapFontTest.png"
            ////////////////////////////////////////////////////////////////////////////

            // page ID. Sanity check
            int index = line.IndexOf('=') + 1;
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            Debug.Assert(Convert.ToInt32(value) == 0, "LabelBMFont file could not be found");
            // file 
            index = line.IndexOf('"') + 1;
            index2 = line.IndexOf('"', index);
            value = line.Substring(index, index2 - index);

            string name = value.Substring(0, value.LastIndexOf('.'));

            m_sAtlasName = fntFile.Substring(0, fntFile.LastIndexOf("/")) + "/images/" + name;
        }

        private void parseKerningCapacity(string line)
        {
            // When using uthash there is not need to parse the capacity.

            //	CCAssert(!kerningDictionary, @"dictionary already initialized");
            //	
            //	// Break the values for this line up using =
            //	CCMutableArray *values = [line componentsSeparatedByString:@"="];
            //	NSEnumerator *nse = [values objectEnumerator];	
            //	CCString *propertyValue;
            //	
            //	// We need to move past the first entry in the array before we start assigning values
            //	[nse nextObject];
            //	
            //	// count
            //	propertyValue = [nse nextObject];
            //	int capacity = [propertyValue intValue];
            //	
            //	if( capacity != -1 )
            //		kerningDictionary = ccHashSetNew(capacity, targetSetEql);
        }

        private void parseKerningEntry(string line)
        {
            //////////////////////////////////////////////////////////////////////////
            // line to parse:
            // kerning first=121  second=44  amount=-7
            //////////////////////////////////////////////////////////////////////////

            // first
            int first;
            int index = line.IndexOf("first=");
            int index2 = line.IndexOf(' ', index);
            string value = line.Substring(index, index2 - index);
            first = int.Parse(value.Replace("first=", ""));

            // second
            int second;
            index = line.IndexOf("second=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index, index2 - index);
            second = int.Parse(value.Replace("second=", ""));

            // amount
            int amount;
            index = line.IndexOf("amount=");
            index2 = line.IndexOf(' ', index);
            value = line.Substring(index);
            amount = int.Parse(value.Replace("amount=", ""));

            tKerningHashElement element = new tKerningHashElement();
            element.amount = amount;
            element.key = (first << 16) | (second & 0xffff);
            m_pKerningDictionary.Add(element.key, element);
        }

        private void purgeKerningDictionary()
        {
            m_pKerningDictionary.Clear();
        }

        #endregion
    }
}
