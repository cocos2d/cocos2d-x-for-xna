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
        public bool IsSwallowsTouches { get; set; }

        /// <summary>
        /// MutableSet that contains the claimed touches 
        /// </summary>
        public List<CCTouch> ClaimedTouches
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///  initializes a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not
        /// </summary>
        public bool initWithDelegate(CCTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// allocates a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not 
        /// </summary>
        public static CCTargetedTouchHandler handlerWithDelegate(CCTouchDelegate pDelegate, int nPriority, bool bSwallow)
        {
            throw new NotImplementedException();
        }

        protected bool m_bSwallowsTouches;
        protected List<CCTouch> m_pClaimedTouches;
    }
}
