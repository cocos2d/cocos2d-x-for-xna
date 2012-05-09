/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
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
using System.Diagnostics;
namespace cocos2d
{
    public enum ActionTag
    {
        //! Default tag
        kCCActionTagInvalid = -1,
    }

    /// <summary>
    /// @brief Base class for CCAction objects.
    /// </summary>
    public class CCAction : CCObject
    {
        public CCAction()
        {
            m_nTag = (int)ActionTag.kCCActionTagInvalid;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCAction ret = null;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCAction)tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCAction();
            }

            ret.m_nTag = m_nTag;
            return ret;
        }

        /// <summary>
        /// return true if the action has finished
        /// </summary>
        public virtual bool isDone()
        {
            return true;
        }

        /// <summary>
        /// called before the action start. It will also set the target.
        /// </summary>
        public virtual void startWithTarget(CCNode target)
        {
            m_pOriginalTarget = m_pTarget = target;
        }

        /// <summary>
        /// called after the action has finished. It will set the 'target' to nil.
        /// IMPORTANT: You should never call "[action stop]" manually. Instead, use: "target->stopAction(action);"
        /// </summary>
        public virtual void stop()
        {
            m_pTarget = null;
        }

        /// <summary>
        /// called every frame with it's delta time. DON'T override unless you know what you are doing.
        /// </summary>
        public virtual void step(float dt)
        {
            CCLog.Log("[Action step]. override me");
        }

        /// <summary>
        /// called once per frame. time a value between 0 and 1
        /// For example: 
        /// - 0 means that the action just started
        /// - 0.5 means that the action is in the middle
        /// - 1 means that the action is over
        /// </summary>
        public virtual void update(float dt)
        {
            CCLog.Log("[Action update]. override me");
        }

        /// <summary>
        /// Allocates and initializes the action
        /// </summary>
        public static CCAction action()
        {
            return new CCAction();
        }

        // Properties

        protected CCNode m_pTarget;
        /// <summary>
        /// Set the original target, since target can be nil.
        /// Is the target that were used to run the action. Unless you are doing something complex, like CCActionManager, you should NOT call this method.
        /// The target is 'assigned', it is not 'retained'.
        /// @since v0.8.2
        /// </summary>
        public CCNode target
        {
            get
            {
                return m_pTarget;
            }
            set
            {
                m_pTarget = value;
            }
        }

        protected CCNode m_pOriginalTarget;
        /// <summary>
        /// The "target".
        /// The target will be set with the 'startWithTarget' method.
        /// When the 'stop' method is called, target will be set to nil.
        /// The target is 'assigned', it is not 'retained'.
        /// </summary>
        public CCNode originalTarget
        {
            get
            {
                return m_pOriginalTarget;
            }
            set
            {
                m_pOriginalTarget = value;
            }
        }

        /// <summary>
        /// The action tag. An identifier of the action
        /// </summary>
        protected int m_nTag;
        public int tag
        {
            get
            {
                return m_nTag;
            }
            set
            {
                m_nTag = value;
            }
        }
    }
}
