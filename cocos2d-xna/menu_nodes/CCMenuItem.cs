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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a CCMenuItem with a target/selector
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public bool initWithTarget(SelectorProtocol rec, SEL_MenuHandler selector)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the outside box
        /// </summary>
        /// <returns></returns>
        public CCRect rect() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Activate the item
        /// </summary>
        public virtual void activate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The item was selected (not activated), similar to "mouse-over"
        /// </summary>
        public virtual void selected()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The item was unselected
        /// </summary>
        public virtual void unselected() 
        {
            throw new NotImplementedException();
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
        public void setTarget(SelectorProtocol rec, SEL_MenuHandler selector)
        {
            throw new NotImplementedException();
        }

    }
}
