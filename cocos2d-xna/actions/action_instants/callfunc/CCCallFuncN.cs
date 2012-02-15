/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.


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
    /** 
    @brief Calls a 'callback' with the node as the first argument
    N means Node
    */
    public class CCCallFuncN : CCCallFunc
    {

        public CCCallFuncN()
        {
            m_pCallFuncN = null;
        }
		
        ~CCCallFuncN()
        {
        }
		
        public static CCCallFuncN actionWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFuncN selector) 
        {
	        CCCallFuncN pRet = new CCCallFuncN();

	        if (pRet != null && pRet.initWithTarget(pSelectorTarget, selector)) 
            {
		        return pRet;
	        }

	        return null;
        }

        // todo
        //public static CCCallFuncN actionWithScriptFuncName(string pszFuncName) 
        //{
        //    CCCallFuncN pRet = new CCCallFuncN();

        //    if (pRet && pRet->initWithScriptFuncName(pszFuncName)) {
        //        pRet->autorelease();
        //        return pRet;
        //    }

        //    CC_SAFE_DELETE(pRet);
        //    return NULL;
        //}

        public bool initWithTarget(SelectorProtocol pSelectorTarget, SEL_CallFuncN selector) 
        {
	        if (base.initWithTarget(pSelectorTarget)) 
            {
		        m_pCallFuncN = selector;
		        return true;
	        }

	        return false;
        }

		// super methods

        public override CCObject copyWithZone(CCZone zone) 
        {
	        CCZone pNewZone = null;
	        CCCallFuncN pRet = null;

	        if (zone != null && zone.m_pCopyObject != null)
            {
		        //in case of being called at sub class
		        pRet = (CCCallFuncN) (zone.m_pCopyObject);
	        } else {
		        pRet = new CCCallFuncN();
		        zone = pNewZone = new CCZone(pRet);
	        }

	        base.copyWithZone(zone);
	        pRet.initWithTarget(m_pSelectorTarget, m_pCallFuncN);
	        return pRet;
        }
        
        public override void execute() 
        {
            if (null != m_pCallFuncN) 
            {
                m_pCallFuncN(m_pTarget);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFuncN(
            //            m_scriptFuncName.c_str(), m_pTarget);
            //}
        }

        private SEL_CallFuncN m_pCallFuncN;
    }
}