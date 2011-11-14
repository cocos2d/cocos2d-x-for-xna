using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// It forwardes each event to the delegate.
    /// </summary>
    public class CCStandardTouchHandler : CCTouchHandler
    {
        /// <summary>
        ///  initializes a TouchHandler with a delegate and a priority
        /// </summary>
        public virtual bool initWithDelegate(ICCTouchDelegate pDelegate, int nPriority)
        {
            if (base.initWithDelegate(pDelegate, nPriority))
            {
                /*
                 * we can not do this in c++
                if( [del respondsToSelector:@selector(ccTouchesBegan:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorBeganBit;
                if( [del respondsToSelector:@selector(ccTouchesMoved:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorMovedBit;
                if( [del respondsToSelector:@selector(ccTouchesEnded:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorEndedBit;
                if( [del respondsToSelector:@selector(ccTouchesCancelled:withEvent:)] )
                    enabledSelectors_ |= ccTouchSelectorCancelledBit;
                */

                return true;
            }

            return false;
        }

        /// <summary>
        /// allocates a TouchHandler with a delegate and a priority
        /// </summary>
        public static CCStandardTouchHandler handlerWithDelegate(ICCTouchDelegate pDelegate, int nPriority)
        {
            CCStandardTouchHandler pHandler = new CCStandardTouchHandler();

            if (pHandler.initWithDelegate(pDelegate, nPriority))
            {
                pHandler = null;
            }
            else
            {
                pHandler = null;
            }

            return pHandler;
        }
    }
}
