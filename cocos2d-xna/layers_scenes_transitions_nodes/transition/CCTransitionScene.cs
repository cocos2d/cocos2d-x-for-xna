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
using System.Diagnostics;

namespace cocos2d
{
    public class CCTransitionScene : CCScene
    {
        protected CCScene m_pInScene;
        protected CCScene m_pOutScene;
        protected float m_fDuration;
        protected bool m_bIsInSceneOnTop;
        protected bool m_bIsSendCleanupToScene;

        public CCTransitionScene()
        {
        }

        public override void draw()
        {
            base.draw();

            if (m_bIsInSceneOnTop)
            {
                m_pOutScene.visit();
                m_pInScene.visit();
            }
            else
            {
                m_pInScene.visit();
                m_pOutScene.visit();
            }
        }

        public override void onEnter()
        {
            base.onEnter();
            m_pInScene.onEnter();
        }

        public override void onExit()
        {
            base.onExit();
            m_pOutScene.onExit();

            // inScene should not receive the onExit callback
            // only the onEnterTransitionDidFinish
            m_pInScene.onEnterTransitionDidFinish();
        }

        public override void cleanup()
        {
            base.cleanup();

            if (m_bIsSendCleanupToScene)
                m_pOutScene.cleanup();
        }

        /// <summary>
        /// creates a base transition with duration and incoming scene 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public static CCTransitionScene transitionWithDuration(float t, CCScene scene)
        {

            CCTransitionScene pScene = new CCTransitionScene();
            if (pScene != null && pScene.initWithDuration(t, scene))
            {
                return pScene;
            }
            pScene = null;
            return null;
        }

        /// <summary>
        ///  initializes a transition with duration and incoming scene
        /// </summary>
        /// <param name="t"></param>
        /// <param name="scene"></param>
        /// <returns></returns>
        public virtual bool initWithDuration(float t, CCScene scene)
        {
            Debug.Assert(scene != null, "Argument scene must be non-nil");

            if (base.init())
            {
                m_fDuration = t;

                // retain
                m_pInScene = scene;
                m_pOutScene = CCDirector.sharedDirector().runningScene;
                m_eSceneType = ccSceneFlag.ccTransitionScene;

                Debug.Assert(m_pInScene != m_pOutScene, "Incoming scene must be different from the outgoing scene");

                // disable events while transitions
                CCTouchDispatcher.sharedDispatcher().IsDispatchEvents = false;
                this.sceneOrder();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// called after the transition finishes
        /// </summary>
        public void finish()
        {
            // clean up 	
            m_pInScene.visible = true;
            m_pInScene.position = new CCPoint(0, 0);
            m_pInScene.scale = 1.0f;
            m_pInScene.rotation = 0.0f;
            m_pInScene.Camera.restore();

            m_pOutScene.visible = false;
            m_pOutScene.position = new CCPoint(0, 0);
            m_pOutScene.scale = 1.0f;
            m_pOutScene.rotation = 0.0f;
            m_pOutScene.Camera.restore();

            //[self schedule:@selector(setNewScene:) interval:0];
            this.schedule(this.setNewScene, 0);
        }

        /// <summary>
        ///  used by some transitions to hide the outter scene
        /// </summary>
        public void hideOutShowIn()
        {
            m_pInScene.visible = true;
            m_pOutScene.visible = false;
        }

        protected virtual void sceneOrder()
        {
            m_bIsInSceneOnTop = true;
        }

        private void setNewScene(float dt)
        {
            // [self unschedule:_cmd]; 
            // "_cmd" is a local variable automatically defined in a method 
            // that contains the selector for the method
            this.unschedule(this.setNewScene);
            CCDirector director = CCDirector.sharedDirector();
            // Before replacing, save the "send cleanup to scene"
            m_bIsSendCleanupToScene = director.isSendCleanupToScene();
            director.replaceScene(m_pInScene);
            // enable events while transitions
            CCTouchDispatcher.sharedDispatcher().IsDispatchEvents = true;
            // issue #267
            m_pOutScene.visible = true;
        }

    }
}
