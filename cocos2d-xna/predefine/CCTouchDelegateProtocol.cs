/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2009      Valentin Milea

http://www.cocos2d-x.org

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System.Collections.Generic;
namespace cocos2d
{
    public interface CCTargetedTouchDelegate
    {
        /** Return YES to claim the touch.
            @since v0.8
        */
        bool ccTouchBegan(CCTouch touch, CCEvent event_);
        // touch updates
        void ccTouchMoved(CCTouch touch, CCEvent event_);
        void ccTouchEnded(CCTouch touch, CCEvent event_);
        void ccTouchCancelled(CCTouch touch, CCEvent event_);
    }

    /**
     CCStandardTouchDelegate.
 
     This type of delegate is the same one used by CocoaTouch. You will receive all the events (Began,Moved,Ended,Cancelled).
     @since v0.8
    */
    public interface CCStandardTouchDelegate
    {
        void ccTouchesBegan(List<CCTouch> touches, CCEvent event_);
        void ccTouchesMoved(List<CCTouch> touches, CCEvent event_);
        void ccTouchesEnded(List<CCTouch> touches, CCEvent event_);
        void ccTouchesCancelled(List<CCTouch> touches, CCEvent event_);
    }
}
