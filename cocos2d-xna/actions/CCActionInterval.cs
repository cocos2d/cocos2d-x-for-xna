/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2011 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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
namespace cocos2d
{
    /** 
    @brief An interval action is an action that takes place within a certain period of time.
    It has an start time, and a finish time. The finish time is the parameter
    duration plus the start time.

    These CCActionInterval actions have some interesting properties, like:
    - They can run normally (default)
    - They can run reversed with the reverse method
    - They can run with the time altered with the Accelerate, AccelDeccel and Speed actions.

    For example, you can simulate a Ping Pong effect running the action normally and
    then running it again in Reverse mode.

    Example:

    CCAction *pingPongAction = CCSequence::actions(action, action->reverse(), NULL);
    */
    public class CCActionInterval : CCFiniteTimeAction
    {
        /** initializes the action */
        public bool initWithDuration(float d)
        {
            ///@todo
            throw new NotImplementedException();
        }

        /** returns true if the action has finished */
        public override bool isDone()
        {
            ///@todo
            throw new NotImplementedException();
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public override void step(float dt)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public override void startWithTarget(CCNode target)
        {
            ///@todo
            throw new NotImplementedException();
        }

        /// <summary>
        /// C# cannot return type of sub class if it override father function.
        /// In c++ this function return CCActionInterval, I don't know if it
        /// will be a problem. 
        /// Fix me if needed.
        /// </summary>
        /// <returns></returns>
        public override CCFiniteTimeAction reverse()
        {
            ///@todo
            throw new NotImplementedException();
        }

        public void setAmplitudeRate(float amp)
        {
            ///@todo
            throw new NotImplementedException();
        }

        public float getAmplitudeRate()
        {
            ///@todo
            throw new NotImplementedException();
        }

        public static CCActionInterval actionWithDuration(float d)
        {
            ///@todo
            throw new NotImplementedException();
        }

        #region properties

        /** how many seconds had elapsed since the actions started to run. */
        protected float m_elapsed;
        public float elapsed
        {
            // read only
            get
            {
                return m_elapsed;
            }
        }

        #endregion

        protected bool m_bFirstTick;
    }
}
