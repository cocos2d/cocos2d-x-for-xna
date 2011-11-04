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

using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
namespace cocos2d
{
    //
    // CCTimer
    //
    /** @brief Light weight timer */
    public class CCTimer
    {
        public CCTimer() { }

        /** Initializes a timer with a target and a selector. 
         */
        public bool initWithTarget(object target, SEL_SCHEDULE selector)
        {
            return initWithTarget(target, selector, 0);
        }

        /** Initializes a timer with a target, a selector and an interval in seconds. 
         *  Target is not needed in c#, it is just for compatibility.
         */
        public bool initWithTarget(object target, SEL_SCHEDULE selector, float fSeconds)
        {
            m_pTarget = target;
            m_pfnSelector = selector;
            m_fElapsed = -1;
            m_fInterval = fSeconds;

            return true;
        }

        public void update(float dt)
        {
            if (m_fElapsed == -1)
            {
                m_fElapsed = 0;
            }
            else
            {
                m_fElapsed += dt;
            }

            if (m_fElapsed >= m_fInterval)
            {
                if (null == m_pfnSelector)
                {
                    m_pfnSelector(m_fElapsed);
                    m_fElapsed = 0;
                }
            }
        }

        public SEL_SCHEDULE m_pfnSelector;
        public float m_fInterval;

        protected object m_pTarget;

        protected float m_fElapsed;
        public float elapsed
        {
            get
            {
                return m_fElapsed;
            }
            set
            {
                m_fElapsed = value;
            }
        }
    }

    /** @brief Scheduler is responsible of triggering the scheduled callbacks.
    You should not use NSTimer. Instead use this class.

    There are 2 different types of callbacks (selectors):

    - update selector: the 'update' selector will be called every frame. You can customize the priority.
    - custom selector: A custom selector will be called every frame, or with a custom interval of time

    The 'custom selectors' should be avoided when possible. It is faster, and consumes less memory to use the 'update selector'.

    */
    public class CCScheduler
    {
        ~CCScheduler()
        {
            unscheduleAllSelectors();

            m_pHashForSelectors.Clear();
            m_pHashForUpdates.Clear();
            m_pUpdatesNegList.Clear();
            m_pUpdates0List.Clear();
            m_pUpdatesPosList.Clear();

            g_sharedScheduler = null;
        }

        /** 'tick' the scheduler.
	     You should NEVER call this method, unless you know what you are doing.
	     */
        public void tick(float dt)
        {
            m_bUpdateHashLocked = true;

            if (m_fTimeScale != 1.0f)
            {
                dt *= m_fTimeScale;
            }

            // updates with priority < 0
            foreach (tListEntry entry in m_pUpdatesNegList)
            {
                if ((!entry.paused) && (!entry.markedForDeletion))
                {
                    entry.target.update(dt);
                }
            }

            // updates with priority == 0
            foreach (tListEntry entry in m_pUpdates0List)
            {
                if ((!entry.paused) && (!entry.markedForDeletion))
                {
                    entry.target.update(dt);
                }
            }

            // updates with priority > 0
            foreach (tListEntry entry in m_pUpdatesPosList)
            {
                if ((!entry.paused) && (!entry.markedForDeletion))
                {
                    entry.target.update(dt);
                }
            }

            // Interate all over the custom selectors
            foreach (KeyValuePair<SelectorProtocol, tHashSelectorEntry> kvp in m_pHashForSelectors)
            {             
                tHashSelectorEntry elt = kvp.Value;
                m_pCurrentTarget = elt;
                m_bCurrentTargetSalvaged = false;

                if (!m_pCurrentTarget.paused)
                {
                    // The 'timers' array may change while inside this loop
                    foreach (CCTimer timer in elt.timers)
                    {
                        elt.currentTimerSalvaged = false;

                        elt.currentTimer.update(dt);

                        /* not needed
                        if (elt.currentTimerSalvaged)
                        {
                            // The currentTimer told the remove itself. To prevent the timer from
                            // accidentally deallocating itself before finishing its step, we retained
                            // it. Now that step is done, it's safe to release it.
                            elt->currentTimer->release();
                        }
                         * */
                        elt.currentTimer = null;
                    }                    
                }

                // not needed
                // elt = (tHashSelectorEntry*)elt->hh.next;

                // only delete currentTarget if no actions were scheduled during the cycle (issue #481)
                if (m_bCurrentTargetSalvaged && m_pCurrentTarget.timers.Count == 0)
                {
                    removeHashElement(m_pCurrentTarget);
                }
            }

            // delete all updates that are morked for deletion
            // updates with priority < 0
            foreach (tListEntry entry in m_pUpdatesNegList)
            {
                if (entry.markedForDeletion)
                {
                    removeUpdateFromHash(entry);
                }
            }

            // updates with priority == 0
            foreach (tListEntry entry in m_pUpdates0List)
            {
                if (entry.markedForDeletion)
                {
                    removeUpdateFromHash(entry);
                }
            }

            // updates with priority > 0
            foreach (tListEntry entry in m_pUpdatesPosList)
            {
                if (entry.markedForDeletion)
                {
                    removeUpdateFromHash(entry);
                }
            }
        }

        /** The scheduled method will be called every 'interval' seconds.
	     If paused is YES, then it won't be called until it is resumed.
	     If 'interval' is 0, it will be called every frame, but if so, it recommened to use 'scheduleUpdateForTarget:' instead.
	     If the selector is already scheduled, then only the interval parameter will be updated without re-scheduling it again.

	     @since v0.99.3
	     */
        public void scheduleSelector(SEL_SCHEDULE selector, SelectorProtocol target, float fInterval, bool bPaused)
        {
            Debug.Assert(selector != null);
            Debug.Assert(target != null);

            tHashSelectorEntry element = null;
            if (!m_pHashForSelectors.ContainsKey(target))
            {
                element = new tHashSelectorEntry();
                element.target = target;
                m_pHashForSelectors[target] = element;

                // Is this the 1st element ? Then set the pause level to all the selectors of this target
                element.paused = bPaused;
            }
            else
            {
                Debug.Assert(element.paused == bPaused);
            }

            if (element.timers == null)
            {
                element.timers = new List<CCTimer>();
            }
            else
            {
                foreach (CCTimer timer in element.timers)
                {
                    if (selector == timer.m_pfnSelector)
                    {
                        Debug.WriteLine("CCSheduler#scheduleSelector. Selector already scheduled.");
                        timer.m_fInterval = fInterval;
                        return;
                    }
                }
            }

            CCTimer timer2 = new CCTimer();
            timer2.initWithTarget(target, selector, fInterval);
            element.timers.Add(timer2);
        }

        /** Schedules the 'update' selector for a given target with a given priority.
	     The 'update' selector will be called every frame.
	     The lower the priority, the earlier it is called.
	     @since v0.99.3
	     */
        public void scheduleUpdateForTarget(SelectorProtocol targt, int nPriority, bool bPaused)
        {
            tHashUpdateEntry hashElement = null;
            if (m_pHashForUpdates.ContainsKey(targt))
            {
                // TODO: check if priority has changed!

                hashElement = m_pHashForUpdates[targt];
                hashElement.entry.markedForDeletion = false;
            }

            // most of the updates are going to be 0, that's way there
            // is an special list for updates with priority 0
            if (nPriority == 0)
            {
                appendIn(m_pUpdates0List, targt, bPaused);
            }
            else if (nPriority < 0)
            {
                appendIn(m_pUpdatesNegList, targt, bPaused);
            }
            else
            {
                appendIn(m_pUpdatesPosList, targt, bPaused);
            }
        }

        /** Unschedule a selector for a given target.
	     If you want to unschedule the "update", use unscheudleUpdateForTarget.
	     @since v0.99.3
	     */
        public void unscheduleSelector(SEL_SCHEDULE selector, SelectorProtocol target)
        {
            // explicity handle nil arguments when removing an object
            if (selector == null || target == null)
            {
                return;
            }

            tHashSelectorEntry element = null;
            if (m_pHashForSelectors.ContainsKey(target))
            {
                element = m_pHashForSelectors[target];

                foreach (CCTimer timer in element.timers)
                {
                    if (selector == timer.m_pfnSelector)
                    {
                        if (timer == element.currentTimer && (!element.currentTimerSalvaged))
                        {
                            element.currentTimerSalvaged = true;
                        }

                        if (element.timers.Count == 0)
                        {
                            if (m_pCurrentTarget == element)
                            {
                                m_bCurrentTargetSalvaged = true;
                            }
                            else
                            {
                                removeHashElement(element);
                            }
                        }

                        return;
                    }
                }
            }
        }

        /** Unschedules all selectors for a given target.
	     This also includes the "update" selector.
	     @since v0.99.3
	     */
        public void unscheduleAllSelectorsForTarget(SelectorProtocol target)
        {
            // explicit NULL handling
            if (target == null)
            {
                return;
            }

            // custom selectors
            tHashSelectorEntry element = m_pHashForSelectors[target];
            if (element != null)
            {
                if (element.timers.Contains(element.currentTimer))
                {
                    element.currentTimerSalvaged = true;
                }
                element.timers.Clear();

                if (m_pCurrentTarget == element)
                {
                    m_bCurrentTargetSalvaged = true;
                }
                else
                {
                    removeHashElement(element);
                }
            }

            // update selector
            unscheduleUpdateForTarget(target);
        }

        /** Unschedules the update selector for a given target
	     @since v0.99.3
	     */
        public void unscheduleUpdateForTarget(SelectorProtocol target)
        {
            if (target == null)
            {
                return;
            }

            tHashUpdateEntry element = null;
            if (m_pHashForSelectors.ContainsKey(target))
            {
                element = m_pHashForUpdates[target];
                if (m_bUpdateHashLocked)
                {
                    element.entry.markedForDeletion = true;
                }
                else
                {
                    removeUpdateFromHash(element.entry);
                }
            }
        }

        /** Unschedules all selectors for a given target.
	     This also includes the "update" selector.
	     @since v0.99.3
	     */
        public void unscheduleAllSelectors()
        {
            foreach (KeyValuePair<SelectorProtocol, tHashSelectorEntry> kvp in m_pHashForSelectors)
            {
                unscheduleAllSelectorsForTarget(kvp.Value.target);
            }

            // Updates selectors
            foreach (tListEntry entry in m_pUpdates0List)
            {
                unscheduleAllSelectorsForTarget(entry.target);
            }
            foreach (tListEntry entry in m_pUpdatesNegList)
            {
                unscheduleAllSelectorsForTarget(entry.target);
            }
            foreach (tListEntry entry in m_pUpdatesPosList)
            {
                unscheduleAllSelectorsForTarget(entry.target);
            }
        }

        /** Pauses the target.
	     All scheduled selectors/update for a given target won't be 'ticked' until the target is resumed.
	     If the target is not present, nothing happens.
	     @since v0.99.3
	     */
        public void pauseTarget(SelectorProtocol target)
        {
            Debug.Assert(target != null);

            // custom selectors
            if (m_pHashForSelectors.ContainsKey(target))
            {
                m_pHashForSelectors[target].paused = true;
            }

            // Update selector
            if (m_pHashForUpdates.ContainsKey(target))
            {
                tHashUpdateEntry element = m_pHashForUpdates[target];
                Debug.Assert(element != null);
                element.entry.paused = true;
            }
        }

        /** Resumes the target.
	     The 'target' will be unpaused, so all schedule selectors/update will be 'ticked' again.
	     If the target is not present, nothing happens.
	     @since v0.99.3
	     */
        public void resumeTarget(SelectorProtocol target)
        {
            Debug.Assert(target != null);

            // custom selectors
            if (m_pHashForSelectors.ContainsKey(target))
            {
                m_pHashForSelectors[target].paused = false;
            }

            // Update selector
            if (m_pHashForUpdates.ContainsKey(target))
            {
                tHashUpdateEntry element = m_pHashForUpdates[target];
                Debug.Assert(element != null);
                element.entry.paused = false;
            }
        }

        /** Returns whether or not the target is paused
        @since v1.0.0
        */
        public bool isTargetPaused(SelectorProtocol target)
        {
            Debug.Assert(target != null, "target must be non nil");

            // Custom selectors
            tHashSelectorEntry element = m_pHashForSelectors[target];
            if (element != null)
            {
                return element.paused;
            }

            return false; // should never get here
        }

        /** returns a shared instance of the Scheduler */
        public static CCScheduler sharedScheduler()
        {
            if (g_sharedScheduler == null)
            {
                g_sharedScheduler = new CCScheduler();
                g_sharedScheduler.init();
            }

            return g_sharedScheduler;
        }

        /** purges the shared scheduler. It releases the retained instance.
	     @since v0.99.0
	     */
        public static void purgeSharedScheduler()
        {
            g_sharedScheduler = null;
        }

        private void removeHashElement(tHashSelectorEntry element)
        {
            element.timers.Clear();
            element.target = null;
            SelectorProtocol target = null;
            foreach (KeyValuePair<SelectorProtocol, tHashSelectorEntry> kvp in m_pHashForSelectors)
            {
                if (element == kvp.Value)
                {
                    target = kvp.Key;
                    break;
                }
            }
            m_pHashForSelectors.Remove(target);
        }

        private void removeUpdateFromHash(tListEntry entry)
        {
            tHashUpdateEntry element = null;
            if (m_pHashForUpdates.ContainsKey(entry.target))
            {
                // list entry
                element = m_pHashForUpdates[entry.target];
                element.list.Clear();
                element.entry = null;

                // hash entry
                element.target = null;
                SelectorProtocol target = null;
                foreach (KeyValuePair<SelectorProtocol, tHashUpdateEntry> kvp in m_pHashForUpdates)
                {
                    if (element == kvp.Value)
                    {
                        target = kvp.Key;
                        break;
                    }
                }
                m_pHashForUpdates.Remove(target);
            }
        }

        private CCScheduler()
        {
            Debug.Assert(g_sharedScheduler == null);          
        }

        private bool init()
        {
            m_fTimeScale = 1.0f;

            m_pUpdatesNegList = new List<tListEntry>();
            m_pUpdates0List = new List<tListEntry>();
            m_pUpdatesPosList = new List<tListEntry>();
            m_pHashForUpdates = new Dictionary<SelectorProtocol, tHashUpdateEntry>();
            m_pHashForSelectors = new Dictionary<SelectorProtocol, tHashSelectorEntry>();

            return true;
        }

        // update specific

        private void priorityIn(List<tListEntry> list, SelectorProtocol target, int nPriority, bool bPaused)
        {
            tListEntry listElement = new tListEntry();

            listElement.target = target;
            listElement.priority = nPriority;
            listElement.paused = bPaused;
            listElement.markedForDeletion = false;

            // list will not be empty, because it is new in init()
            int index = 0;
            foreach (tListEntry element in list)
            {
                if (nPriority < element.priority)
                {
                    break;                  
                }

                ++index;
            }

            // Can not throw exception, because index in [0, count]
            list.Insert(index, listElement);

            // update hash entry for quick access
            tHashUpdateEntry hashElement = new tHashUpdateEntry();
            hashElement.target = target;
            hashElement.list = list;
            hashElement.entry = listElement;
            m_pHashForUpdates.Add(target, hashElement);
        }

        private void appendIn(List<tListEntry> list, SelectorProtocol target, bool bPaused)
        {
            tListEntry listElement = new tListEntry();

            listElement.target = target;
            listElement.paused = bPaused;
            listElement.markedForDeletion = false;

            list.Add(listElement);

            // update hash entry for quicker access
            tHashUpdateEntry hashElement = new tHashUpdateEntry();
            hashElement.target = target;
            hashElement.list = list;
            hashElement.entry = listElement;
            m_pHashForUpdates.Add(target, hashElement);
        }

        private static CCScheduler g_sharedScheduler;

        /** Modifies the time of all scheduled callbacks.
	    You can use this property to create a 'slow motion' or 'fast forward' effect.
	    Default is 1.0. To create a 'slow motion' effect, use values below 1.0.
	    To create a 'fast forward' effect, use values higher than 1.0.
	    @since v0.8
	    @warning It will affect EVERY scheduled selector / action.
	    */
        protected float m_fTimeScale;
        public float timeScale
        {
            get
            {
                return m_fTimeScale;
            }
            set
            {
                m_fTimeScale = value;
            }
        }

        //
        // "updates with priority" stuff
        //
        protected List<tListEntry> m_pUpdatesNegList;                       // list of priority < 0
        protected List<tListEntry> m_pUpdates0List;                         // list priority == 0
        protected List<tListEntry> m_pUpdatesPosList;                       // list priority > 0
        protected Dictionary<SelectorProtocol, tHashUpdateEntry>  m_pHashForUpdates;  // hash used to fetch quickly the list entries for pause,delete,etc

        // Used for "selectors with interval"
        protected Dictionary<SelectorProtocol, tHashSelectorEntry> m_pHashForSelectors;
        protected tHashSelectorEntry m_pCurrentTarget;
        protected bool m_bCurrentTargetSalvaged;
        // If true unschedule will not remove anything from a hash. Elements will only be marked for deletion.
        protected bool m_bUpdateHashLocked;
    }

    public class tListEntry
    {
        public SelectorProtocol    target;
        public int                 priority;
        public bool                paused;
        public bool                markedForDeletion;
    }

    public class tHashUpdateEntry
    {
        public List<tListEntry>     list;  // Which list does it belong to ?
        public tListEntry           entry; // entry in the list
        public SelectorProtocol     target;// hash key
    }

    public class tHashSelectorEntry
    {
        public List<CCTimer>    timers;
        public SelectorProtocol target;
        public CCTimer          currentTimer;
        public bool             currentTimerSalvaged;
        public bool             paused;
    }
}
