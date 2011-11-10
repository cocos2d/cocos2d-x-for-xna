using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /**
 @brief
 Using this type of delegate results in two benefits:
 - 1. You don't need to deal with CCSets, the dispatcher does the job of splitting
 them. You get exactly one UITouch per call.
 - 2. You can *claim* a UITouch by returning YES in ccTouchBegan. Updates of claimed
 touches are sent only to the delegate(s) that claimed them. So if you get a move/
 ended/cancelled update you're sure it's your touch. This frees you from doing a
 lot of checks when doing multi-touch. 

 (The name TargetedTouchDelegate relates to updates "targeting" their specific
 handler, without bothering the other handlers.)
 @since v0.8
 */
    public interface ICCTargetedTouchDelegate : ICCTouchDelegate
    {
        bool ccTouchBegan(CCTouch pTouch, CCEvent pEvent);

        // optional
        void ccTouchMoved(CCTouch pTouch, CCEvent pEvent);
        void ccTouchEnded(CCTouch pTouch, CCEvent pEvent);
        void ccTouchCancelled(CCTouch pTouch, CCEvent pEvent);
    }
}
