using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// This is the callback signature used to update the target property managed by this action.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    public delegate void CCActionTweenDelegate(float value, string key);

    /** CCActionTween

     CCActionTween is an action that lets you update any property of an object.
     For example, if you want to modify the "width" property of a target from 200 to 300 in 2 seconds, then:

        id modifyWidth = [CCActionTween actionWithDuration:2 key:@"width" from:200 to:300];
        [target runAction:modifyWidth];


     Another example: CCScaleTo action could be rewriten using CCPropertyAction:

        // scaleA and scaleB are equivalents
        id scaleA = [CCScaleTo actionWithDuration:2 scale:3];
        id scaleB = [CCActionTween actionWithDuration:2 key:@"scale" from:1 to:3];


     @since v0.99.2
     */
    public class CCActionTween : CCActionInterval
    {
        public CCActionTween(CCActionTweenDelegate d)
        {
            m_pDelegate = d;
        }
        public static CCActionTween actionWithDuration(float aDuration, string key, float from, float to, CCActionTweenDelegate d)
        {
            return CCActionTween.create(aDuration, key, from, to, d);
        }
        /** creates an initializes the action with the property name (key), and the from and to parameters. */
        public static CCActionTween create(float aDuration, string key, float from, float to, CCActionTweenDelegate d)
        {
            CCActionTween pRet = new CCActionTween(d);
            pRet.initWithDuration(aDuration, key, from, to);
            return pRet;
        }
        /** initializes the action with the property name (key), and the from and to parameters. */
        public virtual bool initWithDuration(float aDuration, string key, float from, float to)
        {
            if (base.initWithDuration(aDuration))
            {
                m_strKey = key;
                m_fTo = to;
                m_fFrom = from;
                return true;
            }

            return false;
        }

        public override void startWithTarget(CCNode pTarget)
        {
            base.startWithTarget(pTarget);
            m_fDelta = m_fTo - m_fFrom;
        }
        public override void update(float dt)
        {
            m_pDelegate(m_fTo - m_fDelta * (1 - dt), m_strKey);
        }
        public CCActionInterval reverse()
        {
            return CCActionTween.create(m_fDuration, m_strKey, m_fTo, m_fFrom, m_pDelegate);
        }

        protected string m_strKey;
        protected float m_fFrom, m_fTo;
        protected float m_fDelta;
        protected CCActionTweenDelegate m_pDelegate;
    }
}
