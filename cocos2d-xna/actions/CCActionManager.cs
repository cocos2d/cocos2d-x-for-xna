/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2009      Valentin Milea
Copyright (c) 2011      Zynga Inc.
 

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
using System.Diagnostics;
namespace cocos2d
{
    /** 
     @brief CCActionManager is a singleton that manages all the actions.
     Normally you won't need to use this singleton directly. 99% of the cases you will use the CCNode interface,
     which uses this singleton.
     But there are some cases where you might need to use this singleton.
     Examples:
	    - When you want to run an action where the target is different from a CCNode. 
	    - When you want to pause / resume the actions
 
     @since v0.8
     */
    public class CCActionManager : CCObject, SelectorProtocol
    {
        private CCActionManager()
        {
            Debug.Assert(g_sharedActionManager == null);
        }

        ~CCActionManager()
        {
            removeAllActions();
            m_pTargets.Clear();
            g_sharedActionManager = null;
        }

        public bool init()
        {
            m_pTargets = new Dictionary<CCObject, tHashElement>();
            CCScheduler.sharedScheduler().scheduleUpdateForTarget(this, 0, false);

            return true;
        }

        // actions

        /** Adds an action with a target. 
         If the target is already present, then the action will be added to the existing target.
         If the target is not present, a new instance of this target will be created either paused or not, and the action will be added to the newly created target.
         When the target is paused, the queued actions won't be 'ticked'.
         */
        public void addAction(CCAction action, CCNode target, bool paused)
        {
            Debug.Assert(action != null);
            Debug.Assert(target != null);

            tHashElement element = null;
            if (!m_pTargets.ContainsKey(target))
            {
                element = new tHashElement();
                element.paused = paused;
                element.target = target;
                m_pTargets.Add(target, element);
            }
            else
            {
                element = m_pTargets[target];
            }

            actionAllocWithHashElement(element);

            Debug.Assert(!element.actions.Contains(action));
            element.actions.Add(action);

            action.startWithTarget(target);
        }

        /** Removes all actions from all the targets.
        */
        public void removeAllActions()
        {
            CCObject[] targets = new CCObject[m_pTargets.Keys.Count];
            m_pTargets.Keys.CopyTo(targets, 0);
            for (int i = 0; i < targets.Length; i++)
            {
                removeAllActionsFromTarget(targets[i]);
            }
        }

        /** Removes all actions from a certain target.
	     All the actions that belongs to the target will be removed.
	     */
        public void removeAllActionsFromTarget(CCObject target)
        {
            if (target == null)
            {
                return;
            }

            if (m_pTargets.ContainsKey(target))
            {
                tHashElement element = m_pTargets[target];

                if (element.actions.Contains(element.currentAction) && (!element.currentActionSalvaged))
                {
                    element.currentActionSalvaged = true;
                }

                element.actions.Clear();
                if (m_pCurrentTarget == element)
                {
                    m_bCurrentTargetSalvaged = true;
                }
                else
                {
                    deleteHashElement(element);
                }
            }
            else
            {
                Debug.WriteLine("cocos2d: removeAllActionsFromTarget: Target not found");
            }
        }

        /** Removes an action given an action reference.
        */
        public void removeAction(CCAction action)
        {
            if (action == null)
            {
                return;
            }

            CCObject target = action.originalTarget;

            if (m_pTargets.ContainsKey(target))
            {
                tHashElement element = m_pTargets[target];
                int i = element.actions.IndexOf(action);

                if (i != -1)
                {
                    removeActionAtIndex(i, element);
                }
            }
            else
            {
                Debug.WriteLine("cocos2d: removeAction: Target not found");
            }
        }

        /** Removes an action given its tag and the target */
        public void removeActionByTag(int tag, CCObject target)
        {
            Debug.Assert((tag != (int)ActionTag.kCCActionTagInvalid));
            Debug.Assert(target != null);

            if (m_pTargets.ContainsKey(target))
            {
                tHashElement element = m_pTargets[target];

                int limit = element.actions.Count;
                for (int i = 0; i < limit; i++)
                {
                    CCAction action = element.actions[i];

                    if (action.tag == (int)tag && action.originalTarget == target)
                    {
                        removeActionAtIndex(i, element);
                        break;
                    }
                }
            }
        }

        /** Gets an action given its tag an a target
	     @return the Action the with the given tag
	     */
        public CCAction getActionByTag(uint tag, CCObject target)
        {
            Debug.Assert((int)tag != (int)ActionTag.kCCActionTagInvalid);

            if (m_pTargets.ContainsKey(target))
            {
                tHashElement element = m_pTargets[target];

                if (element.actions != null)
                {
                    int limit = element.actions.Count;
                    for (int i = 0; i < limit; i++)
                    {
                        CCAction action = element.actions[i];

                        if (action.tag == (int)tag)
                        {
                            return action;
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("cocos2d : getActionByTag: Target not found");
                }
            }
            else
            {
                Debug.WriteLine("cocos2d : getActionByTag: Target not found");
            }

            return null;
        }

        /** Returns the numbers of actions that are running in a certain target. 
	     * Composable actions are counted as 1 action. Example:
	     * - If you are running 1 Sequence of 7 actions, it will return 1.
	     * - If you are running 7 Sequences of 2 actions, it will return 7.
	     */
        public uint numberOfRunningActionsInTarget(CCObject target)
        {
            if (m_pTargets.ContainsKey(target))
            {
                tHashElement element = m_pTargets[target];
                return (element.actions != null) ? (uint)element.actions.Count : 0;
            }

            return 0;
        }

        /** Pauses the target: all running actions and newly added actions will be paused.
	    */
        public void pauseTarget(CCObject target)
        {
            if (m_pTargets.ContainsKey(target))
            {
                m_pTargets[target].paused = true;
            }
        }

        /** Resumes the target. All queued actions will be resumed.
	    */
        public void resumeTarget(CCObject target)
        {
            if (m_pTargets.ContainsKey(target))
            {
                m_pTargets[target].paused = false;
            }
        }

        /** returns a shared instance of the CCActionManager */
        public static CCActionManager sharedManager()
        {
            CCActionManager ret = g_sharedActionManager;

            if (ret == null)
            {
                ret = g_sharedActionManager = new CCActionManager();

                if (!g_sharedActionManager.init())
                {
                    ret = g_sharedActionManager = null;
                }
            }

            return ret;
        }

        /** purges the shared action manager. It releases the retained instance.
	     * because it uses this, so it can not be static
	     @since v0.99.0
	     */
        public void purgeSharedManager()
        {
            CCScheduler.sharedScheduler().unscheduleAllSelectorsForTarget(this);
            g_sharedActionManager = null;
        }

        protected void removeActionAtIndex(int index, tHashElement element)
        {
            CCAction action = element.actions[index];

            if (action == element.currentAction && (!element.currentActionSalvaged))
            {
                element.currentActionSalvaged = true;
            }

            element.actions.RemoveAt(index);

            // update actionIndex in case we are in tick. looping over the actions
            if (element.actionIndex >= index)
            {
                element.actionIndex--;
            }

            if (element.actions.Count == 0)
            {
                if (m_pCurrentTarget == element)
                {
                    m_bCurrentTargetSalvaged = true;
                }
                else
                {
                    deleteHashElement(element);
                }
            }
        }

        protected void deleteHashElement(tHashElement element)
        {
            element.actions.Clear();
            element.target = null;
            CCObject keyToDelete = null;
            foreach (KeyValuePair<CCObject, tHashElement> kvp in m_pTargets)
            {
                if (element == kvp.Value)
                {
                    keyToDelete = kvp.Key;
                    break;
                }
            }

            if (keyToDelete != null)
            {
                m_pTargets.Remove(keyToDelete);
            }
        }

        protected void actionAllocWithHashElement(tHashElement element)
        {
            if (element.actions == null)
            {
                element.actions = new List<CCAction>();
            }
        }

        public void update(float dt)
        {
            CCObject[] keys = new CCObject[m_pTargets.Keys.Count];
            m_pTargets.Keys.CopyTo(keys, 0);

            for (int i = 0; i < keys.Length; i++)
            {
                tHashElement elt = m_pTargets[keys[i]];
                m_pCurrentTarget = elt;
                m_bCurrentTargetSalvaged = false;

                if (!m_pCurrentTarget.paused)
                {
                    // The 'actions' may change while inside this loop.
                    for (m_pCurrentTarget.actionIndex = 0; m_pCurrentTarget.actionIndex < m_pCurrentTarget.actions.Count;
                        m_pCurrentTarget.actionIndex++)
                    {
                        m_pCurrentTarget.currentAction = m_pCurrentTarget.actions[m_pCurrentTarget.actionIndex];
                        if (m_pCurrentTarget.currentAction == null)
                        {
                            continue;
                        }

                        m_pCurrentTarget.currentActionSalvaged = false;

                        m_pCurrentTarget.currentAction.step(dt);

                        if (m_pCurrentTarget.currentAction.isDone())
                        {
                            m_pCurrentTarget.currentAction.stop();

                            CCAction action = m_pCurrentTarget.currentAction;
                            // Make currentAction nil to prevent removeAction from salvaging it.
                            m_pCurrentTarget.currentAction = null;
                            removeAction(action);
                        }

                        m_pCurrentTarget.currentAction = null;
                    }
                }

                // only delete currentTarget if no actions were scheduled during the cycle (issue #481)
                if (m_bCurrentTargetSalvaged && m_pCurrentTarget.actions.Count == 0)
                {
                    deleteHashElement(m_pCurrentTarget);
                }
            }

            m_pCurrentTarget = null;
        }

        protected Dictionary<CCObject, tHashElement> m_pTargets;
        protected tHashElement m_pCurrentTarget;
        protected bool m_bCurrentTargetSalvaged;

        private static CCActionManager g_sharedActionManager;
    }

    public class tHashElement
    {
        public List<CCAction> actions;
        public CCObject target;
        public int actionIndex;
        public CCAction currentAction;
        public bool currentActionSalvaged;
        public bool paused;
    }
}
