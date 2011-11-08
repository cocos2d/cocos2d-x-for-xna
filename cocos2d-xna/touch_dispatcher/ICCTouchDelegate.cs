using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public interface ICCTouchDelegate
    {
        bool ccTouchBegan(CCTouch pTouch, CCEvent pEvent);
        // optional

        void ccTouchMoved(CCTouch pTouch, CCEvent pEvent);
        void ccTouchEnded(CCTouch pTouch, CCEvent pEvent);
        void ccTouchCancelled(CCTouch pTouch, CCEvent pEvent);

        // optional
        void ccTouchesBegan(List<CCTouch> pTouches, CCEvent pEvent);
        void ccTouchesMoved(List<CCTouch> pTouches, CCEvent pEvent);
        void ccTouchesEnded(List<CCTouch> pTouches, CCEvent pEvent);
        void ccTouchesCancelled(List<CCTouch> pTouches, CCEvent pEvent);
    }
}
