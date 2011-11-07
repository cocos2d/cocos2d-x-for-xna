using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public enum tCCMenuState
	{
        kCCMenuStateWaiting,
        kCCMenuStateTrackingTouch
    };

    public class CCMenu : CCLayer, CCRGBAProtocol
    {
		//protected tCCMenuState m_eState;
		protected CCMenuItem m_pSelectedItem;		

        public CCMenu()
		{
            m_cOpacity = 0;
			m_pSelectedItem = null;
        }

        /** creates an empty CCMenu */
        public static CCMenu node()
        {
            throw new NotImplementedException();
        }

        ///** creates a CCMenu with it's items */
        public static CCMenu menuWithItems(params CCMenuItem[] item)
        {
            throw new NotImplementedException();
        }

		/** creates a CCMenu with it's item, then use addChild() to add 
		  * other items. It is used for script, it can't init with undetermined
		  * number of variables.
		*/
		public static CCMenu menuWithItem(CCMenuItem item)
        {
            throw new NotImplementedException();
        }

        /** initializes an empty CCMenu */
        public bool init()
        {
            throw new NotImplementedException();
        }

        ///** initializes a CCMenu with it's items */
        //bool initWithItems(CCMenuItem* item, va_list args);

		/** align items vertically */
		public void alignItemsVertically()
        {
            throw new NotImplementedException();
        }

		/** align items vertically with padding
		@since v0.7.2
		*/
		public void alignItemsVerticallyWithPadding(float padding)
        {
            throw new NotImplementedException();
        }

		/** align items horizontally */
		public void alignItemsHorizontally()
        {
            throw new NotImplementedException();
        }

		/** align items horizontally with padding
		@since v0.7.2
		*/
		public void alignItemsHorizontallyWithPadding(float padding)
        {
            throw new NotImplementedException();
        }

		/** align items in rows of columns */
        //void alignItemsInColumns(unsigned int columns, ...);
        //void alignItemsInColumns(unsigned int columns, va_list args);

		/** align items in columns of rows */
        //void alignItemsInRows(unsigned int rows, ...);
        //void alignItemsInRows(unsigned int rows, va_list args);

		//super methods
		public virtual void addChild(CCNode child, int zOrder)
        {
            throw new NotImplementedException();
        }

		public virtual void addChild(CCNode child, int zOrder, int tag)
        {
            throw new NotImplementedException();
        }

		public virtual void registerWithTouchDispatcher()
        {
            throw new NotImplementedException();
        }

        /**
        @brief For phone event handle functions
        */
        public virtual bool ccTouchBegan(CCTouch touch, CCEvent ccevent)
        {
            throw new NotImplementedException();
        }

        public virtual void ccTouchEnded(CCTouch touch, CCEvent ccevent)
        {
            throw new NotImplementedException();
        }

        public virtual void ccTouchCancelled(CCTouch touch, CCEvent ccevent)
        {
            throw new NotImplementedException();
        }

        public virtual void ccTouchMoved(CCTouch touch, CCEvent ccevent)
        {
            throw new NotImplementedException();
        }

		public virtual void destroy()
        {
            throw new NotImplementedException();
        }

		public virtual void keep()
        {
            throw new NotImplementedException();
        }

        /**
        @since v0.99.5
        override onExit
        */
        public virtual void onExit()
        {
            throw new NotImplementedException();
        }

		public virtual CCRGBAProtocol convertToRGBAProtocol() 
        { 
            return (CCRGBAProtocol)this; 
        }

        protected CCMenuItem itemForTouch(CCTouch touch)
        {
            throw new NotImplementedException();
        }

        #region CCRGBAProtocol Interface

        protected ccColor3B m_tColor;
        protected byte m_cOpacity;

        public ccColor3B Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public byte Opacity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsOpacityModifyRGB
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
