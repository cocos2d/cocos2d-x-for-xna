using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /** @brief
 This type of delegate is the same one used by CocoaTouch. You will receive all the events (Began,Moved,Ended,Cancelled).
 @since v0.8
 */
    public interface ICCStandardTouchDelegate
    {       
        // optional
        void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent);
        void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent);
        void ccTouchesEnded(List<CCTouch> pTouches, CCEvent pEvent);
        void ccTouchesCancelled(List<CCTouch> pTouches, CCEvent pEvent);
    }
}
