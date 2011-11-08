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
