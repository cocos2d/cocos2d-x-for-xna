/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.

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

using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace cocos2d
{
    //
    // CCLayer
    //
    /** @brief CCLayer is a subclass of CCNode that implements the TouchEventsDelegate protocol.

    All features from CCNode are valid, plus the following new features:
    - It can receive iPhone Touches
    - It can receive Accelerometer input
    */
    public class CCLayer : CCNode, CCTargetedTouchDelegate, CCStandardTouchDelegate
    {
        public CCLayer()
        {
            ///@todo
            throw new NotImplementedException();
        }

        ~CCLayer()
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static new CCLayer node()
        {
            ///@todo
            throw new NotImplementedException();
        }

        public override void onEnter()
        {
            ///@todo
            throw new NotImplementedException();
        }
        public override void onExit()
        {
            ///@todo
            throw new NotImplementedException();
        }
        public override void onEnterTransitionDidFinish()
        {
            ///@todo
            throw new NotImplementedException();
        }

        // touches

        public virtual bool ccTouchBegan(CCTouch touch, CCEvent event_)
        {
            Debug.Assert(false, "Layer#ccTouchBegan override me");
            return true;
        }
        public virtual void ccTouchMoved(CCTouch touch, CCEvent event_)
        {
        }
        public virtual void ccTouchEnded(CCTouch touch, CCEvent event_)
        {
        }
        public virtual void ccTouchCancelled(CCTouch touch, CCEvent event_)
        {
        }
        public virtual void ccTouchesBegan(List<CCTouch> touches, CCEvent event_)
        {
        }
        public virtual void ccTouchesMoved(List<CCTouch> touches, CCEvent event_)
        {
        }
        public virtual void ccTouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
        }
        public virtual void ccTouchesCancelled(List<CCTouch> touches, CCEvent event_)
        {
        }

        /// <summary>
        /// @todo
        /// </summary>
        //virtual void didAccelerate(CCAcceleration* pAccelerationValue) {CC_UNUSED_PARAM(pAccelerationValue);}

        /** If isTouchEnabled, this method is called onEnter. Override it to change the
	    way CCLayer receives touch events.
	    ( Default: CCTouchDispatcher::sharedDispatcher()->addStandardDelegate(this,0); )
	    Example:
	    void CCLayer::registerWithTouchDispatcher()
	    {
	    CCTouchDispatcher::sharedDispatcher()->addTargetedDelegate(this,INT_MIN+1,true);
	    }
	    @since v0.8.0
	    */
        public virtual void registerWithTouchDispatcher()
        {
            ///@todo
            throw new NotImplementedException();
        }

        // Properties

        /** whether or not it will receive Touch events.
	    You can enable / disable touch events with this property.
	    Only the touches of this node will be affected. This "method" is not propagated to it's children.
	    @since v0.8.1
	    */
        private bool m_bIsTouchEnabled;
        public bool isTouchEnabled
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }

        /** whether or not it will receive Accelerometer events
	    You can enable / disable accelerometer events with this property.
	    @since v0.8.1
	    */
        private bool m_bIsAccelerometerEnabled;
        public bool isAccelerometerEnabled
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }

        /** whether or not it will receive keypad events
        You can enable / disable accelerometer events with this property.
        it's new in cocos2d-x
        */
        private bool m_bIsKeypadEnabled;
        public bool isKeypadEnabled
        {
            get
            {
                ///@todo
                throw new NotImplementedException();
            }
            set
            {
                ///@todo
                throw new NotImplementedException();
            }
        }
    }
}
