/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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
    public class CCTransitionShrinkGrow : CCTransitionScene, ICCTransitionEaseScene
    {
        public override void onEnter()
        {
            base.onEnter();

            m_pInScene.scale = 0.001f;
            m_pOutScene.scale = (1.0f);

            m_pInScene.anchorPoint = new CCPoint(2 / 3.0f, 0.5f);
            m_pOutScene.anchorPoint = new CCPoint(1 / 3.0f, 0.5f);

            CCActionInterval scaleOut = CCScaleTo.actionWithDuration(m_fDuration, 0.01f);
            CCActionInterval scaleIn = CCScaleTo.actionWithDuration(m_fDuration, 1.0f);

            m_pInScene.runAction(this.easeActionWithAction(scaleIn));
            m_pOutScene.runAction
            (
                CCSequence.actions
                (
                    this.easeActionWithAction(scaleOut),
                    CCCallFunc.actionWithTarget(this, (base.finish)),
                    null
                )
            );
        }

        public virtual CCActionInterval easeActionWithAction(CCActionInterval action)
        {
            return CCEaseOut.actionWithAction(action, 2.0f);
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionShrinkGrow);
        public static new CCTransitionShrinkGrow transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionShrinkGrow pScene = new CCTransitionShrinkGrow();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        CCFiniteTimeAction ICCTransitionEaseScene.easeActionWithAction(CCActionInterval action)
        {
            throw new NotImplementedException();
        }
    }
}
