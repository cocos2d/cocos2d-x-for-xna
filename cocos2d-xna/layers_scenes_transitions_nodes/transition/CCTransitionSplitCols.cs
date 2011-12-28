/****************************************************************************
Copyright (c) 2010-2011 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

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
    public class CCTransitionSplitCols : CCTransitionScene, ICCTransitionEaseScene
    {

        public virtual CCActionInterval action()
        {
            return CCSplitCols.actionWithCols(3, m_fDuration / 2.0f);
        }

        public override void onEnter()
        {
            base.onEnter();
            m_pInScene.visible = false;

            CCActionInterval split = action();
            CCActionInterval seq = (CCActionInterval)CCSequence.actions
            (
                split,
                CCCallFunc.actionWithTarget(this, (base.hideOutShowIn)),
                split.reverse()
            );

            this.runAction
        (
            CCSequence.actions(
            easeActionWithAction(seq),
            CCCallFunc.actionWithTarget(this, base.finish),
            CCStopGrid.action()
            )
         );

        }

        public virtual CCActionInterval easeActionWithAction(CCActionInterval action)
        {
            return CCEaseInOut.actionWithAction(action, 3.0f);
        }

        //public   DECLEAR_TRANSITIONWITHDURATION(CCTransitionSplitCols);
        public static new CCTransitionSplitCols transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionSplitCols pScene = new CCTransitionSplitCols();
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
