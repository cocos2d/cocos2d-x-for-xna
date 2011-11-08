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
        public virtual bool initWithDelegate(CCTouchDelegate pDelegate, int nPriority)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// allocates a TouchHandler with a delegate and a priority
        /// </summary>
        public static CCStandardTouchHandler handlerWithDelegate(CCTouchDelegate pDelegate, int nPriority) 
        {
            throw new NotImplementedException();
        }
    }
}
