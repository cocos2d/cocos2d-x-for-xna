/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

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
    /// <summary>
    /// A counter colock-wise radial transition to the next scene
    /// </summary>
    public class CCTransitionRadialCCW : CCTransitionScene
    {
        const int kSceneRadial = int.MaxValue;

        public override void onEnter()
        {
            base.onEnter();
            // create a transparent color layer
            // in which we are going to add our rendertextures
            CCSize size = CCDirector.sharedDirector().getWinSize();

            // create the second render texture for outScene
            CCRenderTexture outTexture = CCRenderTexture.renderTextureWithWidthAndHeight((int)size.width, (int)size.height);

            if (outTexture == null)
            {
                return;
            }

            outTexture.Sprite.anchorPoint = new CCPoint(0.5f, 0.5f);
            outTexture.position = new CCPoint(size.width / 2, size.height / 2);
            outTexture.anchorPoint = new CCPoint(0.5f, 0.5f);

            // render outScene to its texturebuffer
            outTexture.clear(0, 0, 0, 1);
            outTexture.begin();
            m_pOutScene.visit();
            outTexture.end();

            //	Since we've passed the outScene to the texture we don't need it.
            this.hideOutShowIn();

            //	We need the texture in RenderTexture.
            CCProgressTimer outNode = CCProgressTimer.progressWithTexture(outTexture.Sprite.Texture);
            // but it's flipped upside down so we flip the sprite
            //outNode.Sprite.->setFlipY(true);
            //	Return the radial type that we want to use
            outNode.Type = radialType();
            outNode.Percentage = 100.0f;
            outNode.position = new CCPoint(size.width / 2, size.height / 2);
            outNode.anchorPoint = new CCPoint(0.5f, 0.5f);

            // create the blend action
            CCAction layerAction = CCSequence.actions
            (
                CCProgressFromTo.actionWithDuration(m_fDuration, 100.0f, 0.0f),
                CCCallFunc.actionWithTarget(this, base.finish)
            );
            // run the blend action
            outNode.runAction(layerAction);

            // add the layer (which contains our two rendertextures) to the scene
            this.addChild(outNode, 2, kSceneRadial);
        }

        /// <summary>
        /// clean up on exit
        /// </summary>
        public override void onExit()
        {
            // remove our layer and release all containing objects 
            this.removeChildByTag(kSceneRadial, false);
            base.onExit();
        }

        public new static CCTransitionRadialCCW transitionWithDuration(float t, CCScene scene)
        {
            CCTransitionRadialCCW pScene = new CCTransitionRadialCCW();
            pScene.initWithDuration(t, scene);

            return pScene;
        }

        protected override void sceneOrder()
        {
            m_bIsInSceneOnTop = false;
        }

        protected virtual CCProgressTimerType radialType()
        {
            return CCProgressTimerType.kCCProgressTimerTypeRadialCCW;
        }
    }
}
