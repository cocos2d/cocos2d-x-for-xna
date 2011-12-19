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
    /// @brief CCMenuItem base class
    /// Subclass CCMenuItem (or any subclass) to create your custom CCMenuItem objects.
    /// </summary>
    public class CCMenuItem : CCNode
    {
        protected static uint _fontSize = 32;
        protected static string _fontName = "Arial";
        protected static bool _fontNameRelease = false;

        public const int kCurrentItem = 32767;
        public const uint kZoomActionTag = 0xc0c05002;

        protected bool m_bIsSelected;
        protected bool m_bIsEnabled;

        protected SelectorProtocol m_pListener;
        protected SEL_MenuHandler m_pfnSelector;
        protected string m_functionName;

        public CCMenuItem()
        {
            m_bIsSelected = false;
            m_bIsEnabled = false;
            m_pListener = null;
            m_pfnSelector = null;
        }

        /// <summary>
        /// Creates a CCMenuItem with a target/selector
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static CCMenuItem itemWithTarget(SelectorProtocol rec, SEL_MenuHandler selector)
        {
            CCMenuItem pRet = new CCMenuItem();
            pRet.initWithTarget(rec, selector);

            return pRet;
        }

        /// <summary>
        /// Initializes a CCMenuItem with a target/selector
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool initWithTarget(SelectorProtocol rec, SEL_MenuHandler selector)
        {
            anchorPoint = new CCPoint(0.5f, 0.5f);
            m_pListener = rec;
            m_pfnSelector = selector;
            m_bIsEnabled = true;
            m_bIsSelected = false;
            return true;
        }

        /// <summary>
        /// Returns the outside box
        /// </summary>
        /// <returns></returns>
        public CCRect rect()
        {
            return new CCRect(m_tPosition.x - m_tContentSize.width * m_tAnchorPoint.x,
                m_tPosition.y - m_tContentSize.height * m_tAnchorPoint.y,
                m_tContentSize.width,
                m_tContentSize.height);
        }

        /// <summary>
        /// Activate the item
        /// </summary>
        public virtual void activate()
        {
            if (m_bIsEnabled)
            {
                if (m_pListener != null)
                {
                    //(m_pListener.m_pfnSelector)(this);
                }
                m_pfnSelector(this);
#warning "Need Support CCScriptEngineManager"
                //if (m_functionName.size() && CCScriptEngineManager.sharedScriptEngineManager().getScriptEngine())
                //{
                //CCScriptEngineManager.sharedScriptEngineManager().getScriptEngine().executeCallFuncN(m_functionName.c_str(), this);
                //}
            }
        }

        /// <summary>
        /// The item was selected (not activated), similar to "mouse-over"
        /// </summary>
        public virtual void selected()
        {
            m_bIsSelected = true;
        }

        /// <summary>
        /// The item was unselected
        /// </summary>
        public virtual void unselected()
        {
            m_bIsSelected = false;
        }

        /// <summary>
        /// Register a script function, the function is called in activete
        /// If pszFunctionName is NULL, then unregister it.
        /// </summary>
        /// <param name="pszFunctionName"></param>
        public virtual void registerScriptHandler(string pszFunctionName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// set the target/selector of the menu item
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="selector"></param>
        public virtual void setTarget(SelectorProtocol rec, SEL_MenuHandler selector)
        {
            m_pListener = rec;
            m_pfnSelector = selector;
        }

        public virtual bool Enabled
        {
            get { return m_bIsEnabled; }
            set { m_bIsEnabled = value; }
        }

        public virtual bool Selected
        {
            get { return m_bIsSelected; }
        }
    }
}
