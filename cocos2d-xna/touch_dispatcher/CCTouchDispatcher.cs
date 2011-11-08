/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011      Fulcrum Mobile Network, Inc.

http://www.cocos2d-x.org
http://www.openxlive.com

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
using System.Linq;
using System.Text;

namespace cocos2d
{
    public class CCTouchDispatcher : EGLTouchDelegate
    {

        public bool init()
        {
            throw new NotImplementedException();
        }
        public CCTouchDispatcher()
        { }

        /** Whether or not the events are going to be dispatched. Default: true */
        public bool isDispatchEvents()
        {
            throw new NotImplementedException();
        }
        public void setDispatchEvents(bool bDispatchEvents)
        {
            throw new NotImplementedException();
        }

        /** Adds a standard touch delegate to the dispatcher's list.
         See StandardTouchDelegate description.
         IMPORTANT: The delegate will be retained.
         */
        public void addStandardDelegate(CCTouchDelegate pDelegate, int nPriority)
        {
            throw new NotImplementedException();
        }

        /** Adds a targeted touch delegate to the dispatcher's list.
         See TargetedTouchDelegate description.
         IMPORTANT: The delegate will be retained.
         */
        public void addTargetedDelegate(CCTouchDelegate pDelegate, int nPriority, bool bSwallowsTouches)
        {
            throw new NotImplementedException();
        }

        /** Removes a touch delegate.
         The delegate will be released
         */
        public void removeDelegate(CCTouchDelegate pDelegate)
        {
            throw new NotImplementedException();
        }

        /** Removes all touch delegates, releasing all the delegates */
        public void removeAllDelegates()
        { }

        /** Changes the priority of a previously added delegate. The lower the number,
        the higher the priority */
        public void setPriority(int nPriority, CCTouchDelegate pDelegate)
        {
            throw new NotImplementedException();
        }

        public void touches(List<CCTouch> pTouches, CCEvent pEvent, uint uIndex)
        {
            throw new NotImplementedException();
        }

        public virtual void touchesBegan(List<CCTouch> touches, CCEvent pEvent)
        {
            throw new NotImplementedException();
        }
        public virtual void touchesMoved(List<CCTouch> touches, CCEvent pEvent)
        {
            throw new NotImplementedException();
        }
        public virtual void touchesEnded(List<CCTouch> touches, CCEvent pEvent)
        {
            throw new NotImplementedException();
        }
        public virtual void touchesCancelled(List<CCTouch> touches, CCEvent pEvent)
        {
            throw new NotImplementedException();
        }


        /** singleton of the CCTouchDispatcher */
        public static CCTouchDispatcher sharedDispatcher()
        {
            throw new NotImplementedException();
        }
        public CCTouchHandler findHandler(CCTouchDelegate pDelegate)
        {
            throw new NotImplementedException();
        }


        protected void forceRemoveDelegate(CCTouchDelegate pDelegate)
        {
            throw new NotImplementedException();
        }
        protected void forceAddHandler(CCTouchHandler pHandler, List<CCTouchHandler> pArray)
        {
            throw new NotImplementedException();
        }
        protected void forceRemoveAllDelegates()
        {
            throw new NotImplementedException();
        }
        protected void rearrangeHandlers(List<CCTouchHandler> pArray)
        {
            throw new NotImplementedException();
        }


        protected List<CCTouchHandler> m_pTargetedHandlers;
        protected List<CCTouchHandler> m_pStandardHandlers;

        protected bool m_bLocked;
        protected bool m_bToAdd;
        protected bool m_bToRemove;
        protected List<CCTouchHandler> m_pHandlersToAdd;
        //protected	 _ccCArray m_pHandlersToRemove;
        protected bool m_bToQuit;
        protected bool m_bDispatchEvents;

        // 4, 1 for each type of event
        //protected	struct ccTouchHandlerHelperData m_sHandlerHelperData[ccTouchMax];

    }
}
