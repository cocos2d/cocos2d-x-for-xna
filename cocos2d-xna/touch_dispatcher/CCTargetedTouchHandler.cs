using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTargetedTouchHandler : CCTouchHandler
    {
        /// <summary>
        /// whether or not the touches are swallowed
        /// </summary>
        public bool IsSwallowsTouches 
        {
            get { return m_bSwallowsTouches; }
            set { m_bSwallowsTouches = value; }
        }

        /// <summary>
        /// MutableSet that contains the claimed touches 
        /// </summary>
        public List<CCTouch> ClaimedTouches
        {
            get
            {
                return m_pClaimedTouches;
            }
        }

        /// <summary>
        ///  initializes a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not
        /// </summary>
        public bool initWithDelegate(CCTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            if (base.initWithDelegate(pDelegate, nPriority))
            {
                m_pClaimedTouches = new List<CCTouch>();
                m_bSwallowsTouches = bSwallow;

                /*
                if( [aDelegate respondsToSelector:@selector(ccTouchBegan:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorBeganBit;
                if( [aDelegate respondsToSelector:@selector(ccTouchMoved:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorMovedBit;
                if( [aDelegate respondsToSelector:@selector(ccTouchEnded:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorEndedBit;
                if( [aDelegate respondsToSelector:@selector(ccTouchCancelled:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorCancelledBit;
                */
                return true;
            }

            return false;
        }

        /// <summary>
        /// allocates a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not 
        /// </summary>
        public static CCTargetedTouchHandler handlerWithDelegate(CCTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            CCTargetedTouchHandler pHandler = new CCTargetedTouchHandler();

            if (pHandler.initWithDelegate(pDelegate, nPriority, bSwallow))
            {
                pHandler = null;
            }
            else
            {
                pHandler = null;
            }

            return pHandler;
        }

        protected bool m_bSwallowsTouches;
        protected List<CCTouch> m_pClaimedTouches;
    }
}
