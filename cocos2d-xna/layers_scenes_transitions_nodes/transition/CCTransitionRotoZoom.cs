/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
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
    public class CCTransitionRotoZoom : CCTransitionScene
    {

        public override void onEnter()
        {
            base.onEnter();

            m_pInScene.scale = 0.001f;
            m_pOutScene.scale = 1.0f;

            m_pInScene.anchorPoint = new CCPoint(0.5f, 0.5f);
            m_pOutScene.anchorPoint = new CCPoint(0.5f, 0.5f);

            CCActionInterval rotozoom = (CCActionInterval)(CCSequence.actions
            (
                CCSpawn.actions
                (
                    CCScaleBy.actionWithDuration(m_fDuration / 2, 0.001f),
                    CCRotateBy.actionWithDuration(m_fDuration / 2, 360 * 2),
                    null
                ),
                CCDelayTime.actionWithDuration(m_fDuration / 2),
                null
            ));

            m_pOutScene.runAction(rotozoom);
            m_pInScene.runAction
            (
                CCSequence.actions
                (
                    rotozoom.reverse(),
                    CCCallFunc.actionWithTarget(this, (base.finish)),
                    null
                )
            );
        }

        //DECLEAR_TRANSITIONWITHDURATION(CCTransitionRotoZoom);
        public static CCTransitionRotoZoom transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionRotoZoom pScene = new CCTransitionRotoZoom();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }
    }
}
