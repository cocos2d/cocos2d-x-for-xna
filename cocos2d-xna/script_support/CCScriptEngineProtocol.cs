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
    public class CCScriptEngineProtocol
    {
        public CCScriptEngineProtocol() { }

        // functions for excute touch event
        public virtual bool executeTouchEvent(string pszFuncName, CCTouch pTouch)
        {
            return false;
        }
        public virtual bool executeTouchesEvent(string pszFuncName, List<CCTouch> pTouches)
        {
            return false;
        }

        // functions for CCCallFuncX
        public virtual bool executeCallFunc(string pszFuncName)
        {
            return false;
        }
        public virtual bool executeCallFuncN(string pszFuncName, CCNode pNode)
        {
            return false;
        }
        public virtual bool executeCallFuncND(string pszFuncName, CCNode pNode, object pData)
        {
            return false;
        }
        public virtual bool executeCallFunc0(string pszFuncName, CCObject pObject)
        {
            return false;
        }

        // excute a script function without params
        public virtual int executeFuction(string pszFuncName)
        {
            return 0;
        }
        // excute a script file
        public virtual bool executeScriptFile(string pszFileName)
        {
            return false;
        }
        // excute script from string
        public virtual bool executeString(string pszCodes)
        {
            return false;
        }

        // execute a schedule function
        public virtual bool executeSchedule(string pszFuncName, float t)
        {
            return false;
        }
        // add a search path  
        public virtual bool addSearchPath(string pszPath)
        {
            return false;
        }
    }
}
