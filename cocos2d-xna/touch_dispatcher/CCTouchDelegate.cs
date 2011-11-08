/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
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
    public class CCTouchDelegate
    {
        protected ccTouchDelegateFlag m_eTouchDelegateType;
        protected Dictionary<int, string> m_pEventTypeFuncMap;


        /// <summary>
        /// only CCTouchDispatcher & children can change m_eTouchDelegateType
        /// </summary>
        /// <returns></returns>
        internal ccTouchDelegateFlag getTouchDelegateType()
        {
            return m_eTouchDelegateType;
        }

        //! call the release() in child(layer or menu)
        public virtual void destroy() { }
        //! call the retain() in child (layer or menu)
        public virtual void keep() { }

        public virtual bool ccTouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            return false;
        }
        // optional

        public virtual void ccTouchMoved(CCTouch pTouch, CCEvent pEvent)
        {

        }
        public virtual void ccTouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
        }
        public virtual void ccTouchCancelled(CCTouch pTouch, CCEvent pEvent)
        {
        }

        // optional
        public virtual void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {

        }
        public virtual void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        { }
        public virtual void ccTouchesEnded(List<CCTouch> pTouches, CCEvent pEvent)
        {
        }
        public virtual void ccTouchesCancelled(List<CCTouch> pTouches, CCEvent pEvent)
        {
        }

        // functions for script call back
        public void registerScriptTouchHandler(int eventType, string pszScriptFunctionName)
        {
            if (m_pEventTypeFuncMap == null)
            {
                m_pEventTypeFuncMap = new Dictionary<int, string>();
            }

            (m_pEventTypeFuncMap)[eventType] = pszScriptFunctionName;
        }

        public bool isScriptHandlerExist(int eventType)
        {
            if (m_pEventTypeFuncMap != null)
            {
                return (m_pEventTypeFuncMap)[eventType].Count() != 0;
            }

            return false;
        }

        public void excuteScriptTouchHandler(int eventType, CCTouch pTouch)
        {
            if (m_pEventTypeFuncMap != null && CCScriptEngineManager.sharedScriptEngineManager().ScriptEngine != null)
            {
                CCScriptEngineManager.sharedScriptEngineManager().ScriptEngine.executeTouchEvent((m_pEventTypeFuncMap)[eventType].ToString(),
                                                                                                         pTouch);
            }

        }

        public void excuteScriptTouchesHandler(int eventType, List<CCTouch> pTouches)
        {
            if (m_pEventTypeFuncMap != null && CCScriptEngineManager.sharedScriptEngineManager().ScriptEngine != null)
            {
                CCScriptEngineManager.sharedScriptEngineManager().ScriptEngine.executeTouchesEvent((m_pEventTypeFuncMap)[eventType].ToString(),
                                                                                                            pTouches);
            }
        }
    }
}
