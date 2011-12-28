/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
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

namespace cocos2d
{
    /// <summary>
    /// A CCMenuItemFont
    /// Helper class that creates a CCMenuItemLabel class with a Label
    /// </summary>
    public class CCMenuItemFont : CCMenuItemLabel
    {
        public CCMenuItemFont()
            : base()
        { }
        /// <summary>
        /// default font size 
        /// </summary>
        public static uint FontSize
        {
            get { return CCMenuItem._fontSize; }
            set { CCMenuItem._fontSize = value; }
        }

        /// <summary>
        /// default font name
        /// </summary>
        public static string FontName
        {
            get { return CCMenuItem._fontName; }
            set { CCMenuItem._fontName = value; }
        }

        /// <summary>
        /// creates a menu item from a string without target/selector. To be used with CCMenuItemToggle
        /// </summary>
        public static CCMenuItemFont itemFromString(string value)
        {
            CCMenuItemFont pRet = new CCMenuItemFont();
            pRet.initFromString(value, null, null);
            //pRet->autorelease();
            return pRet;
        }
        /// <summary>
        /// creates a menu item from a string with a target/selector
        /// </summary>
        public static CCMenuItemFont itemFromString(string value, SelectorProtocol target, SEL_MenuHandler selector)
        {
            CCMenuItemFont pRet = new CCMenuItemFont();
            pRet.initFromString(value, target, selector);
            //pRet->autorelease();
            return pRet;
        }
        /** initializes a menu item from a string with a target/selector */
        public bool initFromString(string value, SelectorProtocol target, SEL_MenuHandler selector)
        {
            //CCAssert( value != NULL && strlen(value) != 0, "Value length must be greater than 0");

            m_strFontName = _fontName;
            m_uFontSize = _fontSize;

            CCLabelTTF label = CCLabelTTF.labelWithString(value, m_strFontName, (float)m_uFontSize);
            if (base.initWithLabel(label, target, selector))
            {
                // do something ?
            }

            return true;
        }

        /** set font size
          * c++ can not overload static and non-static member functions with the same parameter types
          * so change the name to setFontSizeObj
          */
        public uint FontSizeObj
        {
            set
            {
                m_uFontSize = value;
                recreateLabel();
            }
            get { return m_uFontSize; }
        }


        /** set the font name 
         * c++ can not overload static and non-static member functions with the same parameter types
         * so change the name to setFontNameObj
         */
        public string FontNameObj
        {
            set
            {
                m_strFontName = value;
                recreateLabel();
            }
            get
            {
                return m_strFontName;
            }
        }

        protected void recreateLabel()
        {
            //    CCLabelTTF label = CCLabelTTF.labelWithString(m_pLabel.convertToLabelProtocol().getString(),
            //        m_strFontName, (float)m_uFontSize);
            //    this.Label = label;
        }

        protected uint m_uFontSize;
        protected string m_strFontName;
    }
}
