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
namespace cocos2d
{
    /** @brief Runs actions sequentially, one after another */
    public class CCSequence : CCActionInterval
    {
        public CCSequence()
        {
            m_pActions = new CCFiniteTimeAction[2];
        }

        public bool initOneTwo(CCFiniteTimeAction actionOne, CCFiniteTimeAction aciontTwo)
        {
            Debug.Assert(actionOne != null);
            Debug.Assert(aciontTwo != null);

            float d = actionOne.duration + aciontTwo.duration;
            base.initWithDuration(d);

            m_pActions[0] = actionOne;
            m_pActions[1] = aciontTwo;

            return true;
        }

        public override CCObject copyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCSequence ret = null;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCSequence;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCSequence();
                tmpZone = new CCZone(ret);
            }

            base.copyWithZone(tmpZone);

            CCFiniteTimeAction param1 = m_pActions[0].copy() as CCFiniteTimeAction;
            CCFiniteTimeAction param2 = m_pActions[1].copy() as CCFiniteTimeAction;

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
            m_split = m_pActions[0].duration / m_fDuration;
            m_last = -1;
        }

        public override void stop()
        {
            m_pActions[0].stop();
            m_pActions[1].stop();
            base.stop();
        }

        public override void update(float dt)
        {
            int found = 0;
            float new_t = 0.0f;

            if (dt >= m_split)
            {
                found = 1;
                if (m_split == 1)
                {
                    new_t = 1;
                }
                else
                {
                    new_t = (dt - m_split) / (1 - m_split);
                }
            }
            else
            {
                found = 0;
                if (m_split != 0)
                {
                    new_t = dt / m_split;
                }
                else
                {
                    new_t = 1;
                }
            }

            if (m_last == -1 && found == 1)
            {
                m_pActions[0].startWithTarget(m_pTarget);
                m_pActions[0].update(1.0f);
                m_pActions[0].stop();
            }

            if (m_last != found)
            {
                if (m_last != -1)
                {
                    m_pActions[m_last].update(1.0f);
                    m_pActions[m_last].stop();
                }

                m_pActions[found].startWithTarget(m_pTarget);
            }

            m_pActions[found].update(new_t);
            m_last = found;
        }

        public override CCFiniteTimeAction reverse()
        {
            return CCSequence.actionOneTwo(m_pActions[1].reverse(), m_pActions[0].reverse());
        }

        /** helper constructor to create an array of sequenceable actions */
        public static CCFiniteTimeAction actions(params CCFiniteTimeAction[] actions)
        {
            return actionsWithArray(actions);
        }

        /** helper contructor to create an array of sequenceable actions given an array */
        public static CCFiniteTimeAction actionsWithArray(CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];

            for (int i = 1; i < actions.Length; i++)
            {
                prev = actionOneTwo(prev, actions[i]);
            }

            return prev;
        }

        public static CCSequence actionOneTwo(CCFiniteTimeAction actionOne, CCFiniteTimeAction actionTwo)
        {
            CCSequence sequence = new CCSequence();
            sequence.initOneTwo(actionOne, actionTwo);

            return sequence;
        }

        protected CCFiniteTimeAction[] m_pActions;
        protected float m_split;
        protected int m_last;
    }
}
