using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCScriptEngineManager
    {
        public static CCScriptEngineManager sharedScriptEngineManager()
        {
            throw new NotImplementedException();
        }

        public CCScriptEngineProtocol ScriptEngine { get; set; }
        public void removeScriptEngine()
        { 
            throw new NotImplementedException();
        }

        private CCScriptEngineManager()
        { }

        CCScriptEngineProtocol m_pScriptEngine;
    }
}
