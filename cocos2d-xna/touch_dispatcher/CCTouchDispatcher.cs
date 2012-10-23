/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

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
using System.Collections;

namespace cocos2d
{
    /** @brief CCTouchDispatcher.
 Singleton that handles all the touch events.
 The dispatcher dispatches events to the registered TouchHandlers.
 There are 2 different type of touch handlers:
   - Standard Touch Handlers
   - Targeted Touch Handlers
 
 The Standard Touch Handlers work like the CocoaTouch touch handler: a set of touches is passed to the delegate.
 On the other hand, the Targeted Touch Handlers only receive 1 touch at the time, and they can "swallow" touches (avoid the propagation of the event).
 
 Firstly, the dispatcher sends the received touches to the targeted touches.
 These touches can be swallowed by the Targeted Touch Handlers. If there are still remaining touches, then the remaining touches will be sent
 to the Standard Touch Handlers.

 @since v0.8.0
 */
    public class CCTouchDispatcher : IEGLTouchDelegate
    {
        #region singleton

        static CCTouchDispatcher pSharedDispatcher;

        /// <summary>
        /// singleton of the CCTouchDispatcher
        /// </summary>
        public static CCTouchDispatcher sharedDispatcher()
        {
            // synchronized ??
            if (pSharedDispatcher == null)
            {
                pSharedDispatcher = new CCTouchDispatcher();
                pSharedDispatcher.init();
            }

            return pSharedDispatcher;
        }

        #endregion

        private CCTouchDispatcher()
        { }

        public bool init()
        {
            m_bDispatchEvents = true;
            m_pTargetedHandlers = new List<CCTouchHandler>();
            m_pStandardHandlers = new List<CCTouchHandler>();

            m_pHandlersToAdd = new List<CCTouchHandler>();
            m_pHandlersToRemove = new List<object>();

            m_bToRemove = false;
            m_bToAdd = false;
            m_bToQuit = false;
            m_bLocked = false;

            return true;
        }

        /// <summary>
        /// Whether or not the events are going to be dispatched. Default: true
        /// </summary>
        public bool IsDispatchEvents
        {
            get { return m_bDispatchEvents; }
            set { m_bDispatchEvents = value; }
        }

        /// <summary>
        /// Adds a standard touch delegate to the dispatcher's list.
        /// See StandardTouchDelegate description.
        /// IMPORTANT: The delegate will be retained.
        /// </summary>
        public void addStandardDelegate(ICCStandardTouchDelegate pDelegate, int nPriority)
        {
            CCTouchHandler pHandler = CCStandardTouchHandler.handlerWithDelegate(pDelegate, nPriority);
            if (!m_bLocked)
            {
                forceAddHandler(pHandler, m_pStandardHandlers);
            }
            else
            {
                m_pHandlersToAdd.Add(pHandler);
                m_bToAdd = true;
            }
        }

        /// <summary>
        /// Adds a targeted touch delegate to the dispatcher's list.
        /// See TargetedTouchDelegate description.
        /// IMPORTANT: The delegate will be retained.
        /// </summary>
        public void addTargetedDelegate(ICCTargetedTouchDelegate pDelegate, int nPriority, bool bSwallowsTouches)
        {
            CCTouchHandler pHandler = CCTargetedTouchHandler.handlerWithDelegate(pDelegate, nPriority, bSwallowsTouches);
            if (!m_bLocked)
            {
                forceAddHandler(pHandler, m_pTargetedHandlers);
            }
            else
            {
                m_pHandlersToAdd.Add(pHandler);
                m_bToAdd = true;
            }
        }

        /// <summary>
        /// Removes a touch delegate.
        /// The delegate will be released
        /// </summary>
        public void removeDelegate(ICCTouchDelegate pDelegate)
        {
            if (pDelegate == null)
            {
                return;
            }

            if (!m_bLocked)
            {
                forceRemoveDelegate(pDelegate);
            }
            else
            {
                m_pHandlersToRemove.Add(pDelegate);
                m_bToRemove = true;
            }
        }

        /// <summary>
        /// Removes all touch delegates, releasing all the delegates
        /// </summary>
        public void removeAllDelegates()
        {
            if (!m_bLocked)
            {
                forceRemoveAllDelegates();
            }
            else
            {
                m_bToQuit = true;
            }
        }

        /// <summary>
        /// Changes the priority of a previously added delegate. 
        /// The lower the number, the higher the priority
        /// </summary>
        public void setPriority(int nPriority, ICCTouchDelegate pDelegate)
        {
            CCTouchHandler handler = null;

            handler = this.findHandler(pDelegate);
            handler.Priority = nPriority;

            this.rearrangeHandlers(m_pTargetedHandlers);
            this.rearrangeHandlers(m_pStandardHandlers);
        }

        public void touches(List<CCTouch> pTouches, CCEvent pEvent, int uIndex)
        {
            List<CCTouch> pMutableTouches;
            m_bLocked = true;

            // optimization to prevent a mutable copy when it is not necessary
            int uTargetedHandlersCount = m_pTargetedHandlers.Count;
            int uStandardHandlersCount = m_pStandardHandlers.Count;
            bool bNeedsMutableSet = (uTargetedHandlersCount > 0 && uStandardHandlersCount > 0);

            if (bNeedsMutableSet)
            {
                CCTouch[] tempArray = pTouches.ToArray();
                pMutableTouches = tempArray.ToList();
            }
            else
            {
                pMutableTouches = pTouches;
            }

            CCTouchType sHelper = (CCTouchType)uIndex;

            // process the target handlers 1st
            if (uTargetedHandlersCount > 0)
            {
                #region CCTargetedTouchHandler

                foreach (CCTouch pTouch in pTouches)
                {
                    foreach (CCTargetedTouchHandler pHandler in m_pTargetedHandlers)
                    {
                        ICCTargetedTouchDelegate pDelegate = (ICCTargetedTouchDelegate)(pHandler.Delegate);

                        bool bClaimed = false;
                        if (sHelper == CCTouchType.CCTOUCHBEGAN)
                        {
                            bClaimed = pDelegate.ccTouchBegan(pTouch, pEvent);

                            if (bClaimed)
                            {
                                pHandler.ClaimedTouches.Add(pTouch);
                            }
                        }
                        else
                        {
                            if (pHandler.ClaimedTouches.Contains(pTouch))
                            {
                                // moved ended cancelled
                                bClaimed = true;

                                switch (sHelper)
                                {
                                    case CCTouchType.CCTOUCHMOVED:
                                        pDelegate.ccTouchMoved(pTouch, pEvent);
                                        break;
                                    case CCTouchType.CCTOUCHENDED:
                                        pDelegate.ccTouchEnded(pTouch, pEvent);
                                        pHandler.ClaimedTouches.Remove(pTouch);
                                        break;
                                    case CCTouchType.CCTOUCHCANCELLED:
                                        pDelegate.ccTouchCancelled(pTouch, pEvent);
                                        pHandler.ClaimedTouches.Remove(pTouch);
                                        break;
                                }
                            }
                        }

                        if (bClaimed && pHandler.IsSwallowsTouches)
                        {
                            if (bNeedsMutableSet)
                            {
                                pMutableTouches.Remove(pTouch);
                            }

                            break;
                        }
                    }
                }

                #endregion
            }

            // process standard handlers 2nd
            if (uStandardHandlersCount > 0 && pMutableTouches.Count > 0)
            {
                #region CCStandardTouchHandler
                foreach (CCStandardTouchHandler pHandler in m_pStandardHandlers)
                {
                    ICCStandardTouchDelegate pDelegate = (ICCStandardTouchDelegate)pHandler.Delegate;
                    switch (sHelper)
                    {
                        case CCTouchType.CCTOUCHBEGAN:
                            pDelegate.ccTouchesBegan(pMutableTouches, pEvent);
                            break;
                        case CCTouchType.CCTOUCHMOVED:
                            pDelegate.ccTouchesMoved(pMutableTouches, pEvent);
                            break;
                        case CCTouchType.CCTOUCHENDED:
                            pDelegate.ccTouchesEnded(pMutableTouches, pEvent);
                            break;
                        case CCTouchType.CCTOUCHCANCELLED:
                            pDelegate.ccTouchesCancelled(pMutableTouches, pEvent);
                            break;
                    }
                }
                #endregion
            }

            if (bNeedsMutableSet)
            {
                pMutableTouches = null;
            }

            //
            // Optimization. To prevent a [handlers copy] which is expensive
            // the add/removes/quit is done after the iterations
            //
            m_bLocked = false;
            if (m_bToRemove)
            {
                m_bToRemove = false;
                for (int i = 0; i < m_pHandlersToRemove.Count; ++i)
                {
                    forceRemoveDelegate((ICCTouchDelegate)m_pHandlersToRemove[i]);
                }
                m_pHandlersToRemove.Clear();
            }

            if (m_bToAdd)
            {
                m_bToAdd = false;
                foreach (CCTouchHandler pHandler in m_pHandlersToAdd)
                {
                    if (pHandler is CCTargetedTouchHandler && pHandler.Delegate is ICCTargetedTouchDelegate)
                    {
                        forceAddHandler(pHandler, m_pTargetedHandlers);
                    }
                    else if (pHandler is CCStandardTouchHandler && pHandler.Delegate is ICCStandardTouchDelegate)
                    {
                        forceAddHandler(pHandler, m_pStandardHandlers);
                    }
                    else
                    {
                        CCLog.Log("ERROR: inconsistent touch handler and delegate found in m_pHandlersToAdd of CCTouchDispatcher");
                    }
                }

                m_pHandlersToAdd.Clear();
            }

            if (m_bToQuit)
            {
                m_bToQuit = false;
                forceRemoveAllDelegates();
            }
        }

        public virtual void touchesBegan(List<CCTouch> touches, CCEvent pEvent)
        {
            if (m_bDispatchEvents)
            {
                this.touches(touches, pEvent, (int)CCTouchType.CCTOUCHBEGAN);
            }
        }
        public virtual void touchesMoved(List<CCTouch> touches, CCEvent pEvent)
        {
            if (m_bDispatchEvents)
            {
                this.touches(touches, pEvent, (int)CCTouchType.CCTOUCHMOVED);
            }
        }
        public virtual void touchesEnded(List<CCTouch> touches, CCEvent pEvent)
        {
            if (m_bDispatchEvents)
            {
                this.touches(touches, pEvent, (int)CCTouchType.CCTOUCHENDED);
            }
        }
        public virtual void touchesCancelled(List<CCTouch> touches, CCEvent pEvent)
        {
            if (m_bDispatchEvents)
            {
                this.touches(touches, pEvent, (int)CCTouchType.CCTOUCHCANCELLED);
            }
        }

        public CCTouchHandler findHandler(ICCTouchDelegate pDelegate)
        {
            foreach (CCTouchHandler handler in m_pTargetedHandlers)
            {
                if (handler.Delegate == pDelegate)
                {
                    return handler;
                }
            }

            foreach (CCTouchHandler handler in m_pStandardHandlers)
            {
                if (handler.Delegate == pDelegate)
                {
                    return handler;
                }
            }

            return null;
        }

        protected void forceRemoveDelegate(ICCTouchDelegate pDelegate)
        {
            // remove handler from m_pStandardHandlers
            foreach (CCTouchHandler pHandler in m_pStandardHandlers)
            {
                if (pHandler != null && pHandler.Delegate == pDelegate)
                {
                    m_pStandardHandlers.Remove(pHandler);
                    break;
                }
            }

            // remove handler from m_pTargetedHandlers
            foreach (CCTouchHandler pHandler in m_pTargetedHandlers)
            {
                if (pHandler != null && pHandler.Delegate == pDelegate)
                {
                    m_pTargetedHandlers.Remove(pHandler);
                    break;
                }
            }
        }
        protected void forceAddHandler(CCTouchHandler pHandler, List<CCTouchHandler> pArray)
        {
            int u = 0;
            for (int i = 0; i < pArray.Count; i++)
            {
                CCTouchHandler h = pArray[i];

                if (h != null)
                {
                    if (h.Priority < pHandler.Priority)
                    {
                        ++u;
                    }

                    if (h.Delegate == pHandler.Delegate)
                    {
                        return;
                    }
                }
            }

            pArray.Insert(u, pHandler);
        }
        protected void forceRemoveAllDelegates()
        {
            m_pStandardHandlers.Clear();
            m_pTargetedHandlers.Clear();
        }
        protected void rearrangeHandlers(List<CCTouchHandler> pArray)
        {
            pArray.Sort(less);
        }

        /// <summary>
        /// Used for sort
        /// </summary>
        int less(CCTouchHandler p1, CCTouchHandler p2)
        {
            return p1.Priority - p2.Priority;
        }

        protected List<CCTouchHandler> m_pTargetedHandlers;
        protected List<CCTouchHandler> m_pStandardHandlers;

        bool m_bLocked;
        bool m_bToAdd;
        bool m_bToRemove;
        List<CCTouchHandler> m_pHandlersToAdd;
        List<object> m_pHandlersToRemove;
        bool m_bToQuit;
        bool m_bDispatchEvents;
    }

    public enum CCTouchType
    {
        CCTOUCHBEGAN = 0,
        CCTOUCHMOVED = 1,
        CCTOUCHENDED = 2,
        CCTOUCHCANCELLED = 3,
        ccTouchMax = 4
    }
}
