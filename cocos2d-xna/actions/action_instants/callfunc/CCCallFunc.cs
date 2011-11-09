/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

http://www.cocos2d-x.org

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
namespace cocos2d
{
    /** @brief Calls a 'callback'
    */
    public class CCCallFunc : CCActionInstant
    {
        public CCCallFunc()
        {
            m_pSelectorTarget = null;
            m_scriptFuncName = "";
            m_pCallFunc = null;
		}

		~CCCallFunc()
		{
		}

        public static CCCallFunc actionWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFunc selector)
        {
	        CCCallFunc pRet = new CCCallFunc();

	        if (pRet != null && pRet.initWithTarget(pSelectorTarget)) 
            {
		        pRet.m_pCallFunc = selector;
		        return pRet;
	        }

	        return null;
        }

        // todo 
        //public static CCCallFunc actionWithScriptFuncName(string pszFuncName)
        //{
        //    CCCallFunc pRet = new CCCallFunc();

        //    if (pRet != null && pRet.initWithScriptFuncName(pszFuncName)) 
        //    {
        //        return pRet;
        //    }

        //    return null;
        //}


		public virtual bool initWithTarget(SelectorProtocol pSelectorTarget)
        {
	        m_pSelectorTarget = pSelectorTarget;
	        return true;
        }

        // todo
        //public override bool initWithScriptFuncName(string pszFuncName)
        //{
        //    m_scriptFuncName = pszFuncName;
        //    return true;
        //}

		/** executes the callback */
        public virtual void execute() 
        {
            throw new NotImplementedException();

            // todo
            //if (m_pCallFunc) {
            //    (m_pSelectorTarget->*m_pCallFunc)();
            //}

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFunc(
            //            m_scriptFuncName.c_str());
            //}
        }

        //super methods
		public override void startWithTarget(CCNode pTarget)
        {
	        base.startWithTarget(pTarget);
	        execute();
        }

        public override CCObject copyWithZone(CCZone pZone) 
        {
	        CCZone pNewZone = null;
	        CCCallFunc pRet = null;

	        if (pZone != null && pZone.m_pCopyObject != null) 
            {
		        //in case of being called at sub class
		        pRet = (CCCallFunc) (pZone.m_pCopyObject);
	        } else {
		        pRet = new CCCallFunc();
		        pZone = pNewZone = new CCZone(pRet);
	        }

	        base.copyWithZone(pZone);
	        pRet.initWithTarget(m_pSelectorTarget);
	        pRet.m_pCallFunc = m_pCallFunc;
	        pRet.m_scriptFuncName = m_scriptFuncName;
	        return pRet;
        }

		// void registerScriptFunction(const char* pszFunctionName);

		public SelectorProtocol getTargetCallback()
		{
			return m_pSelectorTarget;
		}

		public void setTargetCallback(SelectorProtocol pSel)
		{
			if (pSel != m_pSelectorTarget)
			{
				m_pSelectorTarget = pSel;

			}
		}

		/** Target that will be called */
		protected SelectorProtocol m_pSelectorTarget;
		/** the script function name to call back */
	    protected string m_scriptFuncName;

        private SEL_CallFunc m_pCallFunc;
    }
}