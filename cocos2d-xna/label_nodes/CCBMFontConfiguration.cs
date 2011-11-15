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
    public class CCBMFontConfiguration : CCObject
    {
        // XXX: Creating a public interface so that the bitmapFontArray[] is accesible
        //@public
        // The characters building up the font
        public List<ccBMFontDef> m_pBitmapFontArray;

        // FNTConfig: Common Height
        public uint m_uCommonHeight;

        // Padding
        public ccBMFontPadding m_tPadding;

        // atlas name
        public string m_sAtlasName;

        // values for kerning
        public tKerningHashElement m_pKerningDictionary;

        public CCBMFontConfiguration()
        {
            //CCLOGINFO("cocos2d: deallocing CCBMFontConfiguration");
            this.purgeKerningDictionary();
            //m_sAtlasName.clear();
            m_sAtlasName = "";
        }

        public string description()
        {
            string[] ret = new string[100];
            //sprintf(ret, "<CCBMFontConfiguration | Kernings:%d | Image = %s>", HASH_COUNT(m_pKerningDictionary), m_sAtlasName.c_str());
            return ret.ToString();
        }

        /// <summary>
        /// allocates a CCBMFontConfiguration with a FNT file
        /// </summary>
        public static CCBMFontConfiguration configurationWithFNTFile(string FNTfile)
        {
            CCBMFontConfiguration pRet = new CCBMFontConfiguration();
            if (pRet.initWithFNTfile(FNTfile))
            {
                //pRet->autorelease();
                return pRet;
            }
            //CC_SAFE_DELETE(pRet);
            return null;
        }

        /// <summary>
        /// initializes a BitmapFontConfiguration with a FNT file
        /// </summary>
        public bool initWithFNTfile(string FNTfile)
        {
            Debug.Assert(FNTfile != null && FNTfile.Length != 0);
            m_pKerningDictionary = null;
            this.parseConfigFile(FNTfile);
            return true;
        }

        private void parseConfigFile(string controlFile)
        {
        //string fullpath =CCFileUtils.fullPathFromRelativePath(controlFile);

        //CCFileData data(fullpath, "rb");
        //ulong nBufSize = data.getSize();
        //string pBuffer = (string) data.getBuffer();

        //Debug.Assert(pBuffer!=null, "CCBMFontConfiguration::parseConfigFile | Open file error.");

        //if (!pBuffer)
        //{
        //    return;
        //}

        //// parse spacing / padding
        //string line;
        //string strLeft(pBuffer, nBufSize);
        //while (strLeft.Length> 0)        //{
        //    int pos = strLeft.IndexOf('\n');

        //    if (pos != (int)std::string::npos)
        //    {
        //        // the data is more than a line.get one line
        //        line = strLeft.Substring(0, pos);
        //        strLeft = strLeft.Substring(pos + 1);
        //    }
        //    else
        //    {
        //        // get the left data
        //        line = strLeft;
        //        strLeft.erase();
        //    }

        //    if(line.Substring(0,("info face").Length) == "info face") 
        //    {
        //        // XXX: info parsing is incomplete
        //        // Not needed for the Hiero editors, but needed for the AngelCode editor
        //        //			[self parseInfoArguments:line];
        //        this.parseInfoArguments(line);
        //    }
        //    // Check to see if the start of the line is something we are interested in
        //    else if(line.Substring(0,("common lineHeight").Length) == "common lineHeight")
        //    {
        //        this.parseCommonArguments(line);
        //    }
        //    else if(line.Substring(0,("page id").Length) == "page id")
        //    {
        //        this.parseImageFileName(line, controlFile);
        //    }
        //    else if(line.Substring(0,("chars c").Length) == "chars c")
        //    {
        //        // Ignore this line
        //    }
        //    else if(line.Substring(0,("char").Length) == "char")
        //    {
        //        // Parse the current line and create a new CharDef
        //        ccBMFontDef characterDefinition;
        //        this.parseCharacterDefinition(line,characterDefinition);

        //        // Add the CharDef returned to the charArray

        //        //m_pBitmapFontArray[ characterDefinition.charID ] = characterDefinition;
        //        m_pBitmapFontArray.Add(characterDefinition);
        //    }
        //    else if(line.Substring(0,("kernings count").Length) == "kernings count")
        //    {
        //        this.parseKerningCapacity(line);
        //    }
        //    else if(line.Substring(0,("kerning first").Length) == "kerning first")
        //    {
        //        this.parseKerningEntry(line);
        //    }
        //}
            throw new NotImplementedException();
        }

        private void parseCharacterDefinition(string line, ccBMFontDef characterDefinition)
        {
            throw new NotImplementedException();
        }

        private void parseInfoArguments(string line)
        {
            throw new NotImplementedException();
        }

        private void parseCommonArguments(string line)
        {
            throw new NotImplementedException();
        }

        private void parseImageFileName(string line, string fntFile)
        {
        //    //////////////////////////////////////////////////////////////////////////
        //// line to parse:
        //// page id=0 file="bitmapFontTest.png"
        ////////////////////////////////////////////////////////////////////////////

        //// page ID. Sanity check
        //int index = line.IndexOf('=')+1;
        //int index2 = line.IndexOf(' ', index);
        //string value = line.Substring(index, index2-index);
        //Debug.Assert(Convert.ToInt32(value) == 0, "LabelBMFont file could not be found");
        //// file 
        //index = line.IndexOf('"')+1;
        //index2 = line.IndexOf('"', index);
        //value = line.Substring(index, index2-index);

        //m_sAtlasName = CCFileUtils::fullPathFromRelativeFile(value, fntFile);
            throw new NotImplementedException();
        }

        private void parseKerningCapacity(string line)
        {
            throw new NotImplementedException();
        }

        private void parseKerningEntry(string line)
        {
            throw new NotImplementedException();
        }

        private void purgeKerningDictionary()
        {
            //tKerningHashElement current;
            //while (m_pKerningDictionary != null)
            //{
            //    current = m_pKerningDictionary;
            //    HASH_DEL(m_pKerningDictionary, current);
            //    free(current);
            //}
        }
    }
}
