using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCMenuItem : CCNode
    {
        bool m_bIsSelected;
        bool m_bIsEnabled;

        protected SelectorProtocol	m_pListener;
		protected SEL_MenuHandler	m_pfnSelector;
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
                m_tContentSize.width, m_tContentSize.height);
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

                  #warning "Need Support CCScriptEngineManager"
                //if (m_functionName.size() && CCScriptEngineManager.sharedScriptEngineManager().getScriptEngine())
                //{
                //    CCScriptEngineManager.sharedScriptEngineManager().getScriptEngine().executeCallFuncN(m_functionName.c_str(), this);
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
            set { m_bIsEnabled = value;}
        }

        public virtual bool Selected 
        {
            get { return m_bIsSelected; }
        }
    }
}
