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
    /** 
    @brief Calls a 'callback' with the node as the first argument and the 2nd argument is data
    * ND means: Node and Data. Data is void *, so it could be anything.
    */
    public class CCCallFuncND : CCCallFuncN
    {
         public static CCCallFuncND actionWithTarget(SelectorProtocol pSelectorTarget,
		        SEL_CallFuncND selector, object d) 
         {
	        CCCallFuncND pRet = new CCCallFuncND();

	        if (pRet != null && pRet.initWithTarget(pSelectorTarget, selector, d)) 
            {
		        return pRet;
	        }

	        return null;
        }

        // todo
        //public static CCCallFuncND actionWithScriptFuncName(string pszFuncName, object d) 
        //{
        //    CCCallFuncND pRet = new CCCallFuncND();

        //    if (pRet != null && pRet.initWithScriptFuncName(pszFuncName)) 
        //    {
        //        pRet.m_pData = d;
        //        return pRet;
        //    }
	        
        //    return null;
        //}


        public bool initWithTarget(SelectorProtocol pSelectorTarget,
		        SEL_CallFuncND selector, object d) 
        {
	        if (base.initWithTarget(pSelectorTarget)) 
            {
		        m_pData = d;
		        m_pCallFuncND = selector;
		        return true;
	        }

	        return false;
        }

        public override CCObject copyWithZone(CCZone zone) 
        {
	        CCZone pNewZone = null;
	        CCCallFuncND pRet = null;

	        if (zone != null && zone.m_pCopyObject != null) 
            {
		        //in case of being called at sub class
		        pRet = (CCCallFuncND) (zone.m_pCopyObject);
	        } 
            else 
            {
		        pRet = new CCCallFuncND();
		        zone = pNewZone = new CCZone(pRet);
	        }

	        base.copyWithZone(zone);
	        pRet.initWithTarget(m_pSelectorTarget, m_pCallFuncND, m_pData);
	        return pRet;
        }

        public override void execute() 
        {
            if (null != m_pCallFuncND) 
            {
                m_pCallFuncND(m_pTarget, m_pData);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFuncND(
            //            m_scriptFuncName.c_str(), m_pTarget, m_pData);
            //}
        }

    	protected object m_pData;

        protected SEL_CallFuncND m_pCallFuncND;
    
    }
}