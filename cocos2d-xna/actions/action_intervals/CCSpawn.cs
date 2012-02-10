/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
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

using System.Diagnostics;
using System;
namespace cocos2d
{
    /** @brief Spawn a new action immediately */
    public class CCSpawn : CCActionInterval
    {
        /** helper constructor to create an array of spawned actions */
        public static CCFiniteTimeAction actions(params CCFiniteTimeAction[] actions)
        {
            return actionsWithArray(actions);
        }

        /** helper contructor to create an array of spawned actions given an array */
        public static CCFiniteTimeAction actionsWithArray(CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            for (int i = 1; i < actions.Length; i++)
            {
                prev = actionOneTwo(prev, actions[i]);
            }

            return prev;
        }

        /** creates the Spawn action */
        public static CCSpawn actionOneTwo(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            CCSpawn spawn = new CCSpawn();
            spawn.initOneTwo(action1, action2);

            return spawn;
        }

        public bool initOneTwo(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            Debug.Assert(action1 != null);
            Debug.Assert(action2 != null);

            bool bRet = false;

            float d1 = action1.duration;
            float d2 = action2.duration;

            if (base.initWithDuration(Math.Max(d1, d2)))
            {
                m_pOne = action1;
                m_pTwo = action2;

                if (d1 > d2)
                {
                    m_pTwo = CCSequence.actionOneTwo(action2, CCDelayTime.actionWithDuration(d1 - d2));
                }
                else if (d1 < d2)
                {
                    m_pOne = CCSequence.actionOneTwo(action1, CCDelayTime.actionWithDuration(d2 - d1));
                }
             
                bRet = true;
            }           

            return bRet;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCSpawn ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCSpawn;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCSpawn();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCFiniteTimeAction param1 = m_pOne.copy() as CCFiniteTimeAction;
            CCFiniteTimeAction param2 = m_pTwo.copy() as CCFiniteTimeAction;
            if (param1 == null || param2 == null)
            {
                return null;
            }

            ret.initOneTwo(param1, param2);

            return ret;
        }

        public override void startWithTarget(CCNode target)
        {
            base.startWithTarget(target);
            m_pOne.startWithTarget(target);
            m_pTwo.startWithTarget(target);
        }

        public override void stop()
        {
            m_pOne.stop();
            m_pTwo.stop();
            base.stop();
        }

        public override void update(float dt)
        {
            if (m_pOne != null)
            {
                m_pOne.update(dt);
            }

            if (m_pTwo != null)
            {
                m_pTwo.update(dt);
            }
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCSpawn.actionOneTwo(m_pOne.reverse(), m_pTwo.reverse());
        }

        protected CCFiniteTimeAction m_pOne = new CCFiniteTimeAction();
        protected CCFiniteTimeAction m_pTwo = new CCFiniteTimeAction();
    }
}
