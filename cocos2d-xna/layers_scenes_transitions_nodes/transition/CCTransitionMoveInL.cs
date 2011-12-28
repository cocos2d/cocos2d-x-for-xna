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
    public class CCTransitionMoveInL : CCTransitionScene, ICCTransitionEaseScene
    {
        /// <summary>
        /// initializes the scenes
        /// </summary>
        public virtual void initScenes()
        {
            CCSize s = CCDirector.sharedDirector().getWinSize();
            m_pInScene.position = new CCPoint(-s.width, 0);
        }

        /// <summary>
        /// returns the action that will be performed
        /// </summary>
        /// <returns></returns>
        public virtual CCActionInterval action()
        {
            return CCMoveTo.actionWithDuration(m_fDuration, new CCPoint(0, 0));
        }

        public override void onEnter()
        {
            base.onEnter();
            this.initScenes();

            CCActionInterval a = this.action();

            m_pInScene.runAction
            (
                CCSequence.actions
                (
                    this.easeActionWithAction(a),
                    CCCallFunc.actionWithTarget(this, base.finish),
                    null
                )
            );
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionMoveInL);
        public static new CCTransitionMoveInL transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionMoveInL pScene = new CCTransitionMoveInL();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        public CCFiniteTimeAction easeActionWithAction(CCActionInterval action)
        {
            return CCEaseOut.actionWithAction(action, 2.0f);
        }
    }
}
